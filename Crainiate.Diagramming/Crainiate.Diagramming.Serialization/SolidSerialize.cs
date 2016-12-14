// (c) Copyright Crainiate Software 2010

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming.Serialization
{
	public class SolidSerialize: ElementSerialize
	{
		public override void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
            base.GetObjectData(obj, info, context);

			Solid solid = (Solid) obj;

            info.AddValue("BackColor", solid.BackColor.ToArgb().ToString());
            info.AddValue("Clip", solid.Clip);
            info.AddValue("GradientMode", Convert.ToInt32(solid.GradientMode).ToString());
            info.AddValue("GradientColor", solid.GradientColor.ToArgb().ToString());
            info.AddValue("DrawGradient", solid.DrawGradient);
            info.AddValue("DrawBorder", solid.DrawBorder);
            info.AddValue("DrawBackground", solid.DrawBackground);
            info.AddValue("Location", Serialize.AddPointF(solid.Location));
            info.AddValue("InternalRectangle", Serialize.AddRectangleF(solid.InternalRectangle));
            info.AddValue("Rotation", solid.Rotation);

            info.AddValue("Label", solid.Label,typeof(Label));
            info.AddValue("Image", solid.Image, typeof(Image));
            info.AddValue("StencilItem", solid.StencilItem, typeof(StencilItem));
		}

		public override object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
            base.SetObjectData(obj, info, context, selector);

            Solid solid = (Solid) obj;
            SerializationInfoEnumerator enumerator = info.GetEnumerator();            

            //Enumerate the info object and apply to the appropriate properties
            while (enumerator.MoveNext())
            {
                if (enumerator.Name == "BackColor") solid.BackColor = Color.FromArgb(Convert.ToInt32(info.GetString("BackColor")));
                else if (enumerator.Name == "Clip") solid.Clip = info.GetBoolean("Clip");

                else if (enumerator.Name == "GradientMode") solid.GradientMode = (LinearGradientMode)Enum.Parse(typeof(LinearGradientMode), info.GetString("GradientMode"), true);
                else if (enumerator.Name == "GradientColor") solid.GradientColor = Color.FromArgb(Convert.ToInt32(info.GetString("GradientColor")));

                else if (enumerator.Name == "DrawGradient") solid.DrawGradient = info.GetBoolean("DrawGradient");
                else if (enumerator.Name == "DrawBorder") solid.DrawBorder = info.GetBoolean("DrawBorder");
                else if (enumerator.Name == "DrawBackground") solid.DrawBackground = info.GetBoolean("DrawBackground");
                else if (enumerator.Name == "Location") solid.Location = Serialize.GetPointF(info.GetString("Location"));
                else if (enumerator.Name == "InternalRectangle") solid.SetInternalRectangle(Serialize.GetRectangleF(info.GetString("InternalRectangle")));

                else if (enumerator.Name == "Rotation") solid.Rotation = info.GetSingle("Rotation");

                else if (enumerator.Name == "Label") solid.Label = (Label) info.GetValue("Label", typeof(Label));
                else if (enumerator.Name == "Image") solid.Image = (Image) info.GetValue("Image", typeof(Image));
                else if (enumerator.Name == "StencilItem") solid.StencilItem = (StencilItem) info.GetValue("StencilItem", typeof(StencilItem));
            }

            return solid;
		}
	}
}
