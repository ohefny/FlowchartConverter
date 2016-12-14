// (c) Copyright Crainiate Software 2010

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

using Crainiate.Diagramming.Collections;

namespace Crainiate.Diagramming.Serialization
{
    //Serializes or deserializes any Elements<T> based collection
	public class ElementsSerialize<T>: ISerializationSurrogate
        where T : Element
	{
		public virtual void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
            Elements<T> elements = obj as Elements<T>;

            if (elements == null) throw new SerializationException(String.Format("Error serializing Elements collection of {1}.",typeof(T).ToString()));

            //Add a reference to the container
            if (elements.Model != null) info.AddValue("Model", elements.Model, elements.Model.GetType());
		}

		public virtual object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
            Elements<T> elements = obj as Elements<T>;
            SerializationInfoEnumerator enumerator = info.GetEnumerator();

            //Reset nullable properties
            elements.SetModel(null);

            //Enumerate the info object and apply to the appropriate properties
            while (enumerator.MoveNext())
            {
                if (enumerator.Name == "Model") elements.SetModel(info.GetValue("Model",typeof(Model)) as Model);
            }

            return elements;
		}
	}
}
