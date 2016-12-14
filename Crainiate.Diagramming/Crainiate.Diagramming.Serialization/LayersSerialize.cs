// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

using Crainiate.Diagramming.Collections;

namespace Crainiate.Diagramming.Serialization
{
    //Serializes or deserializes any Elements<T> based collection
	public class LayersSerialize: ISerializationSurrogate
	{
		public virtual void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
            Layers layers = obj as Layers;

            //Add a reference to the container
            info.AddValue("CurrentLayer", layers.CurrentLayer);
            info.AddValue("Model", layers.Model);
		}

		public virtual object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
            Layers layers = obj as Layers;
            SerializationInfoEnumerator enumerator = info.GetEnumerator();

            //Reset nullable properties
            layers.SetCurrentLayer(null);

            //Enumerate the info object and apply to the appropriate properties
            while (enumerator.MoveNext())
            {
                if (enumerator.Name == "CurrentLayer") layers.SetCurrentLayer(info.GetValue("CurrentLayer", typeof(Layer)) as Layer);
                else if (enumerator.Name == "Model") layers.SetModel(info.GetValue("Model", typeof(Model)) as Model);
            }

            return layers;
		}
	}
}
