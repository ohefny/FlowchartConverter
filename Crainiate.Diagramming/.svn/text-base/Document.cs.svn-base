// (c) Copyright Crainiate Software 2010

using System;
using System.IO;
using System.IO.Packaging;
using System.Runtime.Serialization;

using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
    public class Document
    {
        //Property variables
        private Model _model;
        private XmlFormatter _formatter;

        public Document()
        {
            _formatter = Singleton.Instance.XmlFormatter;
        }

        public Document(Model model): this()
        {
            _model = model;
        }

        public virtual Model Model
        {
            get
            {
                return _model;
            }
            set
            {
                if (value == null) throw new ArgumentNullException();
                _model = value;
            }
        }

        public virtual XmlFormatter Formatter
        {
            get
            {
                return _formatter;
            }
            set
            {
                if (value == null) throw new ArgumentNullException();
                _formatter = value;
            }
        }

        //Loads a diagram from the filename provided
        public virtual void Open(string path)
        {
            using (Stream stream = new FileStream(path, FileMode.Open))
            {
                OpenPackage(stream);
                stream.Close();
            }
        }

        //Loads a diagram from the stream provided.
        public virtual void Open(Stream stream)
        {
            OpenPackage(stream);
        }

        //Saves a diagram in binary format to the filename provided.
        public virtual void Save(string path)
        {
            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                CreatePackage(stream);
                stream.Close();
            }
        }

        //Saves a diagram in binary format to the stream provided.
        public virtual void Save(Stream stream)
        {
            CreatePackage(stream);
        }

        protected virtual void CreatePackage(Stream stream)
        {
            using (Package package = ZipPackage.Open(stream, FileMode.Create))
            {
                Uri collectionUri = new Uri("/Content/Collection.xml", UriKind.Relative);
                PackagePart part = package.CreatePart(collectionUri, System.Net.Mime.MediaTypeNames.Text.Xml, CompressionOption.Maximum);

                //Serialize the model into the part stream
                XmlFormatter formatter = Formatter;
                formatter.Shallow = false;

                formatter.Serialize(part.GetStream(), Model);

                package.CreateRelationship(part.Uri, TargetMode.Internal, "Main Document");

                //Loop through all resources and create parts
                foreach (string key in Formatter.Resources.Keys)
                {
                    Uri resourceUri = new Uri(key, UriKind.Relative);
                    ResourceEntry entry = Formatter.Resources[key];

                    //Need to use a valid mime type otherwise a null reference error occurs
                    PackagePart resourcePart = package.CreatePart(resourceUri, entry.GetMimeType(), CompressionOption.Maximum);
                    entry.SaveResourceStream(resourcePart.GetStream());
                    PackageRelationship relationship = package.CreateRelationship(resourcePart.Uri, TargetMode.Internal, "Resource");
                }

                //Close the package and save all the underlying streams
                package.Close();
            }
        }

        protected virtual void OpenPackage(Stream stream)
        {
            //Clear out any existing resources
            Formatter.Resources.Clear();

            using (Package package = ZipPackage.Open(stream, FileMode.Open))
            {
                PackagePart xmlPart = null;

                //Loop through all the parts that are resources
                foreach (PackagePart part in package.GetParts())
                {
                    if (part.Uri.OriginalString == "/Content/Collection.xml")
                    {
                        xmlPart = part;
                    }
                    else
                    {
                        //Create the object from the part stream
                        object resource = CreateObject(part.ContentType, part.GetStream());

                        //Create a resource entry and add the object to the entry if an object was created
                        if (resource != null)
                        {
                            string name = "Image";

                            ResourceEntry entry = new ResourceEntry(name, resource.GetType(), resource);

                            //Add to the resources collection
                            Formatter.Resources.Add(part.Uri.OriginalString, entry);
                        }
                    }
                }

                if (xmlPart == null) throw new SerializationException("Xml Part not found in document.");

                Model = (Model) Formatter.Deserialize(xmlPart.GetStream());
            }
        }

        //Creates a new instance of the specific object type which takes a single stream parameter in its constructor
        protected virtual object CreateObject(string contentType, Stream data)
        {
            Type type = null;

            if (contentType == "image/bmp") type = typeof(System.Drawing.Bitmap);
            if (type == null) return null;

            return Activator.CreateInstance(type, new object[] { data });
        }
    }
}
