// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming.Serialization
{
	public class LayerSerialize: ISerializationSurrogate
	{
		public virtual void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			Layer layer = (Layer) obj;

            info.AddValue("Opacity", layer.Opacity);
            info.AddValue("Visible", layer.Visible);
            info.AddValue("Name", layer.Name);
            info.AddValue("DrawShadows", layer.DrawShadows);
            info.AddValue("ShadowOffset", Serialize.AddPointF(layer.ShadowOffset));
            info.AddValue("ShadowColor", layer.ShadowColor.ToArgb().ToString());
            info.AddValue("SoftShadows", layer.SoftShadows);
		}

		public virtual object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
            Layer layer = (Layer) obj;
            SerializationInfoEnumerator enumerator = info.GetEnumerator();

            //Enumerate the info object and apply to the appropriate properties
            while (enumerator.MoveNext())
            {
                if (enumerator.Name == "Opacity") layer.Opacity = info.GetByte("Opacity");
                else if (enumerator.Name == "Visible") layer.Visible = info.GetBoolean("Visible");
                else if (enumerator.Name == "Name") layer.Name = info.GetString("Name");
                else if (enumerator.Name == "DrawShadows") layer.DrawShadows = info.GetBoolean("DrawShadows");
                else if (enumerator.Name == "ShadowOffset") layer.ShadowOffset = Serialize.GetPointF(info.GetString("ShadowOffset"));
                else if (enumerator.Name == "ShadowColor") layer.ShadowColor = Color.FromArgb(Convert.ToInt32(info.GetString("ShadowColor")));
                else if (enumerator.Name == "SoftShadows") info.GetBoolean("SoftShadows");
            }

            return layer;
		}
	}
}
