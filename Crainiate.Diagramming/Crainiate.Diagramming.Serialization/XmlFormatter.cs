// (c) Copyright Crainiate Software 2010




//
//Based on XmlFormatter library by Patrick Boom / CodeProject http://www.codeproject.com/KB/XML/XMLFormatter.aspx
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Collections;

//Implements a custom XmlFormatter which uses surrogates to serialize and deserialize objects 
namespace Crainiate.Diagramming.Serialization
{
    public class XmlFormatter: IFormatter
    {
        //Property variables
        private SerializationBinder _binder;
        private StreamingContext _context;
        private ISurrogateSelector _selector = null;
        
        private object _target;
        private bool _shallow;

        //Working variables
        private Dictionary<object, int> _objectIds;
        private Dictionary<int, object> _idObjects;
        private Dictionary<string, ResourceEntry> _resources;

        //Constructors
        public XmlFormatter()
        {
            _binder = new CustomBinder();
            _context = new StreamingContext(StreamingContextStates.All);
            _resources = new Dictionary<string, ResourceEntry>();

            AddDefaultSurrogates();
        }

        //Properties
        //Gets or sets the type binder.
        public SerializationBinder Binder
        {
            get
            {
                return _binder;
            }
            set
            {
                _binder = value;
            }
        }

        //Gets or sets the StreamingContext.
        public StreamingContext Context
        {
            get
            {
                return _context;
            }
            set
            {
                _context = value;
            }
        }

        //Gets or sets the SurrogateSelector.
        public ISurrogateSelector SurrogateSelector
        {
            get
            {
                return _selector;
            }
            set
            { 
                _selector = value;
            }
        }

        public bool Shallow
        {
            get
            {
                return _shallow;
            }
            set
            {
                _shallow = value;
            }
        }

        //An existing object that will be the target of a shallow deserialization
        public object Target
        {
            get 
            { 
                return _target; 
            }
            set 
            { 
                _target = value; 
            }
        }

        //The resources mde available when serializing the object graph.
        public Dictionary<string, ResourceEntry> Resources
        {
            get
            {
                return _resources;
            }
        }

        //Methods
        //Serializes the passed object to the passed stream.
        public virtual void Serialize(Stream serializationStream, object objectToSerialize)
        {
            if (objectToSerialize == null) return;
            if (serializationStream == null) throw new ArgumentNullException("serializationStream");

            //Create a writer with an empty namespace
            XmlTextWriter writer = new XmlTextWriter(serializationStream, Encoding.UTF8);
            writer.Namespaces = false;

            //Initialise the dictionary to hold a record of object and resource references
            _objectIds = new Dictionary<object, int>();
            _resources.Clear();
            
            //Serialize the object graph to xml and a resource dictionary
            SerializeObject(writer, new FormatterConverter(), objectToSerialize.GetType().Name, null, objectToSerialize, objectToSerialize.GetType());
            
            //Release object references
            _objectIds.Clear();
        }

        //Deserializes an object from the passed stream.
        public virtual object Deserialize(Stream serializationStream)
        {
            //Create the collection containing deserialized objects
            if (_idObjects == null) _idObjects= new Dictionary<int, object>(); 

            object result = DeserializeObject(serializationStream);

            //Clear the collection so that we dont hold onto a reference
            _idObjects.Clear();

            return result;
        }

        //Determine if the object type should be serialized as a resource
        protected virtual bool IsResource(Type type)
        {
            return typeof(System.Drawing.Image).IsAssignableFrom(type);
        }

        //Serializes the object using the passed XmlWriter.
        private void SerializeObject(XmlTextWriter writer, FormatterConverter converter, string elementName, string key, object objectToSerialize, Type objectType)
        {
            //Write the opening tag
            writer.WriteStartElement(elementName);

            //Include type information.
            WriteAttributes(writer, objectType, key);

            //Only serialize object if not null
            if (objectToSerialize != null)
            {
                //Write an object reference if the object has already been serialized
                if (!Shallow && _objectIds.ContainsKey(objectToSerialize))
                {
                    writer.WriteStartAttribute("ref");
                    writer.WriteString(_objectIds[objectToSerialize].ToString());
                    writer.WriteEndAttribute();

                    writer.WriteEndElement();
                    writer.Flush();

                    return;
                }

                int id = _objectIds.Count + 1;
                _objectIds.Add(objectToSerialize, id);

                //Write the id attribute
                writer.WriteStartAttribute("id");
                writer.WriteString(id.ToString());
                writer.WriteEndAttribute();

                //for each serializable item in this object
                foreach (SerializationEntry entry in GetMemberInfo(objectToSerialize, objectType, converter))
                {
                    //Simple type, directly write the value.
                    if (entry.ObjectType.IsPrimitive || entry.ObjectType == typeof(string) || entry.ObjectType.IsEnum || entry.ObjectType == typeof(DateTime))
                    {
                        WriteValueElement(writer, entry);
                    }
                    //Write resource
                    else if (IsResource(entry.ObjectType))
                    {
                        ResourceEntry resource = new ResourceEntry(entry.Name, entry.ObjectType, entry.Value);

                        //Get a unique uri for the resource
                        string uri = String.Format("/Resource/{0}/{1}.{2}",resource.Name,id,resource.GetResourceType());

                        //Write the uri reference to the document
                        WriteResourceElement(entry.Name, writer, entry.ObjectType, uri);

                        //Add to the resources dictionary
                        if (!_resources.ContainsKey(uri)) _resources.Add(uri, resource);
                    }
                    //Serialize the object (recursive call), if not shallow
                    else if (!Shallow)
                    {
                        SerializeObject(writer, converter, entry.Name, null, entry.Value, entry.ObjectType);
                    }
                }

                //Process list and dictionary members
                bool isList = typeof(IList).IsAssignableFrom(objectType);
                bool isDictionary = typeof(IDictionary).IsAssignableFrom(objectType);

                //An IList collection
                if (isList)
                {
                    //write the opening tag.
                    writer.WriteStartElement("Collection");

                    //Indicate that this is a collection
                    writer.WriteStartAttribute("type");
                    writer.WriteString("IList");
                    writer.WriteEndAttribute();

                    IList list = objectToSerialize as IList;
                    if (list != null)
                    {
                        IEnumerator enumerator = list.GetEnumerator();

                        while (enumerator.MoveNext())
                        {
                            Type enumeratedType = enumerator.Current.GetType();

                            //serialize the object (recursive call)
                            SerializeObject(writer, converter, enumeratedType.Name, null, enumerator.Current, enumeratedType);
                        }
                    }

                    //Write closing tag
                    writer.WriteEndElement();
                }

                //An IDictionary collection
                else if (isDictionary)
                {
                    //write the opening tag.
                    writer.WriteStartElement("Collection");

                    //Indicate that this is a collection
                    writer.WriteStartAttribute("type");
                    writer.WriteString("IDictionary");
                    writer.WriteEndAttribute();

                    IDictionary dictionary = objectToSerialize as IDictionary;
                    if (dictionary != null)
                    {
                        IDictionaryEnumerator enumerator = dictionary.GetEnumerator();

                        while (enumerator.MoveNext())
                        {
                            Type enumeratedType = enumerator.Value.GetType();

                            //serialize the object (recursive call)
                            SerializeObject(writer, converter, enumeratedType.Name, enumerator.Key.ToString(), enumerator.Value, enumeratedType);
                        }
                    }

                    //Close the collection element
                    writer.WriteEndElement();
                }
            }
           
            //Write the closing tag and flush the contents of the writer to the stream
            writer.WriteEndElement();
            writer.Flush();
        }

        //Writes the Type and key attributes to the element
        private void WriteAttributes(XmlTextWriter writer, Type objectType, string key)
        {
            writer.WriteStartAttribute("type");
            writer.WriteString(objectType.FullName);
            writer.WriteEndAttribute();

            //Optionally, write out the key (if in a Dictionary)
            if (key != null && key.Length > 0)
            {
                writer.WriteStartAttribute("key");
                writer.WriteString(key);
                writer.WriteEndAttribute();
            }
        }

        //Writes a simple element to the writer. The name of the element is the name of the object type.
        private void WriteValueElement(XmlTextWriter writer, SerializationEntry entry)
        {
            WriteValueElement(entry.Name, writer, entry.ObjectType, entry.Value);
        }

        //Writes a simple element to the writer. 
        private void WriteValueElement(string tagName, XmlTextWriter writer, SerializationEntry entry)
        {
            WriteValueElement(tagName, writer, entry.ObjectType, entry.Value);
        }

        //Writes a simple element to the writer. 
        private void WriteValueElement(string tagName, XmlTextWriter writer,Type objectType, object value)
        {
            //write opening tag
            writer.WriteStartElement(tagName);

            if (objectType.IsEnum)
            {
                writer.WriteValue(((int) value).ToString());
            }
            else
            {
                writer.WriteValue(value);
            }

            //write closing tag
            writer.WriteEndElement();
        }

        //Writes a simple element to the writer. 
        private void WriteResourceElement(string tagName, XmlTextWriter writer, Type objectType, string uri)
        {
            //write opening tag
            writer.WriteStartElement(tagName);

            //Write out the type attribute
            writer.WriteStartAttribute("type");
            writer.WriteString(objectType.FullName);
            writer.WriteEndAttribute();

            //Write out the uri attribute
            writer.WriteStartAttribute("uri");
            writer.WriteString(uri);
            writer.WriteEndAttribute();

            //write closing tag
            writer.WriteEndElement();
        }

        //Gets all the serializable members of an object and return an enumarable collection.
        private IEnumerable<SerializationEntry> GetMemberInfo(object objectToSerialize, Type objectToSerializeType, FormatterConverter converter)
        {
            ISurrogateSelector selector1 = null;
            ISerializationSurrogate serializationSurrogate = null;
            SerializationInfo info = null;
            Type objectType = objectToSerializeType;

            //If the passed object is null, break the iteration.
            if (objectToSerialize == null) yield break;

            //Get the serialization surrogate
            if (SurrogateSelector == null) throw new NullReferenceException("An error occurred serializing an object. The SurrogateSelector property may not be null.");
            serializationSurrogate = SurrogateSelector.GetSurrogate(objectType, Context, out selector1);
            if (serializationSurrogate == null) throw new NullReferenceException(string.Format("An error occurred serializing an object. A surrogate was not found for type {0}",objectType.Name));

            //Use the surrogate to get the members.
            info = new SerializationInfo(objectType, converter);

            //Get the data from the surrogate.
            if (!objectType.IsPrimitive) serializationSurrogate.GetObjectData(objectToSerialize, info, Context);

            //Create the custom entries collection by copying all the members
            foreach (System.Runtime.Serialization.SerializationEntry member in info)
            {
                //SerializationEntry entry = new SerializationEntry(member.Name, member.ObjectType, member.Value);

                //yield return will return the entry now and return to this point when
                //the enclosing loop (the one that contains the call to this method)
                //request the next item.
                //yield return entry;
                yield return member;
            }
        }

        //Deserializes an object from the given stream for the given type. If graph is not null, it is used to receive data
        private object DeserializeObject(Stream serializationStream)
        {
            object deserialized = null;

            //Create xml reader from stream
            using (XmlTextReader reader = new XmlTextReader(serializationStream))
            {                
                //Check that we have a starting xml element
                if (!reader.IsStartElement()) throw new SerializationException("A starting element was not found when deserializing an object stream.");

                deserialized = InitializeObject(reader, new FormatterConverter());
            }

            return deserialized;
        }

        //Reads an object from the XML and initializes the object.
        private object InitializeObject(XmlTextReader reader, FormatterConverter converter)
        {
            Type actualType;
            int id;

            //Check if a type or ref attribute is present
            if (!reader.HasAttributes) throw new SerializationException("A non-primitive element was found without attributes.");

            //Check for a ref attribute
            string reference = reader.GetAttribute("ref"); //, "http://www.w3.org/2001/XMLSchema-instance");

            //References require a previously deserialized object
            if (reference != null)
            {
                if (!int.TryParse(reference, out id)) throw new SerializationException("Non numeric reference id found.");

                object existing = null;
                if (!_idObjects.TryGetValue(id, out existing)) throw new SerializationException(string.Format("An object reference with id {0} was not previously deserialized.", reference));

                return existing;
            }
            
            //Get the type name
            string actualTypeName = reader.GetAttribute("type"); // "http://www.w3.org/2001/XMLSchema-instance");
            actualType = Binder.BindToType("", actualTypeName);

            //Get the id attribute
            string objectId = reader.GetAttribute("id"); //, "http://www.w3.org/2001/XMLSchema-instance");

            //If the id is null then the object reference is null so return null
            if (objectId == null) return null;

            //Convert to an integer value
            if (!int.TryParse(objectId, out id)) throw new SerializationException("An object id could not be converted to an integer value.");
            
            ISurrogateSelector selector1 = null;
            ISerializationSurrogate serializationSurrogate = null;
            SerializationInfo info = null;
            
            if (SurrogateSelector == null) throw new NullReferenceException("An error occurred deserializing an object. The SurrogateSelector property may not be null.");
            serializationSurrogate = SurrogateSelector.GetSurrogate(actualType, Context, out selector1);
            if (serializationSurrogate == null) throw new NullReferenceException(string.Format("An error occurred deserializing an object. A surrogate was not found for type {0}", actualType.Name));

            //Use surrogate
            info = new SerializationInfo(actualType, converter);

            //Create a instance of the type, or use the existing object graph
            object initializedObject;

            if (Target == null)
            {
                initializedObject = FormatterServices.GetUninitializedObject(actualType);
                
                //Call the default constructor
                //Formatter could be expanded to use non default constructors at some later stage
                ConstructorInfo ci = actualType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null,System.Type.EmptyTypes, null);
                if (ci == null) throw new SerializationException(string.Format("Type {0} must implement a parameterless constructor.", actualType.FullName));
                ci.Invoke(initializedObject, null);
            }
            else
            {
                initializedObject = Target;
                Target = null; //Reset target so that it cannot be used recursively
            }

            //Add the object to the list of id objects
            _idObjects.Add(id, initializedObject);

            //Determine if a list or collection
            IDictionary dictionary = null;
            IList list = null;
            int index = 0;
            
            //Read the first element
            reader.ReadStartElement();

            while (reader.IsStartElement())
            {
                //Determine type
                string typeName = reader.GetAttribute("type"); //, "http://www.w3.org/2001/XMLSchema-instance");
                Type childType = Binder.BindToType("", typeName);

                //Get a key, if any
                string key = reader.GetAttribute("key");

                //Check for a uri attribute
                string uri = reader.GetAttribute("uri");

                //Check if a collection
                if (reader.Name == "Collection" && reader.IsStartElement())
                {
                    if (typeName == "IDictionary")
                    {
                        dictionary = initializedObject as IDictionary;
                        list = null;
                    }
                    else if (typeName == "IList")
                    {
                        list = initializedObject as IList;
                        dictionary = null;
                    }

                    reader.ReadStartElement();
                }
                //Check for a resource entry
                else if (uri != null)
                {
                    //Uris require a resource preloaded in the _resources collection
                    ResourceEntry resourceEntry;
                    if (!_resources.TryGetValue(uri, out resourceEntry)) throw new SerializationException(string.Format("A resource with uri {0} was not found in the resources collection.", uri));

                    info.AddValue(resourceEntry.Name, resourceEntry.Value);

                    reader.Read();
                }
                //Process all other elements
                else
                {
                    object parsedObject = null;

                    //Check if the value can be directly determined or that the type is a complex type.
                    if (childType.IsPrimitive || childType == typeof(string) || childType.IsEnum || childType == typeof(DateTime) || childType == typeof(object))
                    {
                        //Directly parse
                        parsedObject = converter.Convert(reader.ReadString(), childType);
                    }
                    else
                    {
                        //Recurse down the object graph
                        parsedObject = InitializeObject(reader, converter);
                    }

                    //Add to parent collection or add the key value pair to the info object for the serialization surrogate to use
                    if (dictionary != null && key != null)
                    {
                        dictionary.Add(key, parsedObject);
                    }
                    else if (list != null)
                    {
                        list.Add(parsedObject);
                    }
                    else
                    {
                        info.AddValue(reader.Name, parsedObject); //Use info object
                    }

                    //Move to next element
                    reader.Read();

                    //Read past collection
                    if (reader.Name == "Collection" && !reader.IsStartElement()) reader.Read();
                }
            }
            
            //Use the surrogate to populate the instance
            initializedObject = serializationSurrogate.SetObjectData(initializedObject, info, Context, SurrogateSelector);

            return initializedObject;
        }

        private void AddDefaultSurrogates()
        {
            SurrogateSelector selector = new SurrogateSelector();
            StreamingContext context = new StreamingContext(StreamingContextStates.All); //Need to ensure a context is supplied

            //Elements
            selector.AddSurrogate(typeof(Element), context, new ElementSerialize());
            selector.AddSurrogate(typeof(Solid), context, new SolidSerialize());
            selector.AddSurrogate(typeof(Shape), context, new ShapeSerialize());
            selector.AddSurrogate(typeof(Link), context, new LinkSerialize());
            selector.AddSurrogate(typeof(Origin), context, new OriginSerialize());
            
            //Container
            selector.AddSurrogate(typeof(Model), context, new ModelSerialize());

            //Collections
            selector.AddSurrogate(typeof(Elements), context, new ElementsSerialize<Element>());
            selector.AddSurrogate(typeof(Shapes), context, new ElementsSerialize<Shape>());
            selector.AddSurrogate(typeof(Lines), context, new ElementsSerialize<Line>());
            selector.AddSurrogate(typeof(Ports), context, new ElementsSerialize<Port>());
            selector.AddSurrogate(typeof(ElementList), context, new ElementListSerialize());
            selector.AddSurrogate(typeof(Layers), context, new LayersSerialize());

            //Additional
            selector.AddSurrogate(typeof(StencilItem), context, new StencilItemSerialize());
            selector.AddSurrogate(typeof(Label), context, new LabelSerialize());
            selector.AddSurrogate(typeof(Layer), context, new LayerSerialize());
            selector.AddSurrogate(typeof(Image), context, new ImageSerialize());

            _selector = selector;
        }
    }
}
