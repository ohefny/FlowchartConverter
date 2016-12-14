// (c) Copyright Crainiate Software 2010

using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming.Serialization
{
    public class ModelSerialize : ISerializationSurrogate
	{
		//Implement ISerializable
		public virtual void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
            Model model = obj as Model;

            info.AddValue("Shapes", model.Shapes);
            info.AddValue("Lines", model.Lines);
            info.AddValue("Elements", model.Elements);
            info.AddValue("Layers", model.Layers);
		}

		public virtual object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
            Model model = obj as Model;

            //Reset reference properties
            model.SetShapes(null);
            model.SetLines(null);
            model.SetElements(null);

            SerializationInfoEnumerator enumerator = info.GetEnumerator();

            //Enumerate the info object and apply to the appropriate properties
            while (enumerator.MoveNext())
            {
                if (enumerator.Name == "Shapes") model.SetShapes(info.GetValue("Shapes", typeof(Shapes)) as Shapes);
                else if (enumerator.Name == "Lines") model.SetLines(info.GetValue("Lines", typeof(Lines)) as Lines);
                else if (enumerator.Name == "Elements") model.SetElements(info.GetValue("Elements", typeof(ElementList)) as ElementList);
                else if (enumerator.Name == "Layers") model.Layers = (Layers) info.GetValue("Layers", typeof(Layers));
            }

            return model;
		}
	}
}
