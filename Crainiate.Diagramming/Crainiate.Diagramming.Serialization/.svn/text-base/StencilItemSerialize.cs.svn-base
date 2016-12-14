// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming.Serialization
{
	public class StencilItemSerialize: ISerializationSurrogate
	{
		public virtual void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			StencilItem item = (StencilItem) obj;

            info.AddValue("Key", item.Key);
            info.AddValue("Redraw", item.Redraw);
            info.AddValue("BasePath", Serialize.AddPath(item.BasePath));
            //info.AddValue("BaseSize", Serialize.AddSizeF(item.BaseSize));
            info.AddValue("BaseInternalRectangle", Serialize.AddRectangleF(item.BaseInternalRectangle));

            info.AddValue("BorderColor", item.BorderColor.ToArgb().ToString());
            info.AddValue("BorderStyle", Convert.ToInt32(item.BorderStyle).ToString());
            info.AddValue("SmoothingMode", Convert.ToInt32(item.SmoothingMode).ToString());
            info.AddValue("BackColor", item.BackColor.ToArgb().ToString());
            info.AddValue("GradientColor", item.GradientColor.ToArgb().ToString());
            info.AddValue("GradientMode", Convert.ToInt32(item.GradientMode).ToString());
            info.AddValue("DrawGradient", item.DrawGradient);
            info.AddValue("Options", Convert.ToInt32(item.Options).ToString());
		}

		public virtual object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
            StencilItem item = (StencilItem) obj;
            SerializationInfoEnumerator enumerator = info.GetEnumerator();

            //Enumerate the info object and apply to the appropriate properties
            while (enumerator.MoveNext())
            {
                if (enumerator.Name == "Key") item.Key = info.GetString("Key");
                else if (enumerator.Name == "Redraw") item.Redraw = info.GetBoolean("Redraw");
                else if (enumerator.Name == "BasePath") item.SetBasePath(Serialize.GetPath(info.GetString("BasePath")));
                //else if (enumerator.Name == "BaseSize") item.SetBaseSize(Serialize.GetSizeF(info.GetString("BaseSize")));
                else if (enumerator.Name == "BaseInternalRectangle") item.SetBaseInternalRectangle(Serialize.GetRectangleF(info.GetString("BaseInternalRectangle")), 0, 0);
                else if (enumerator.Name == "BorderColor") item.BorderColor = Color.FromArgb(Convert.ToInt32(info.GetString("BorderColor")));
                else if (enumerator.Name == "BorderStyle") item.BorderStyle = (DashStyle)Enum.Parse(typeof(DashStyle), info.GetString("BorderStyle"));
                else if (enumerator.Name == "SmoothingMode") item.SmoothingMode = (SmoothingMode)Enum.Parse(typeof(SmoothingMode), info.GetString("SmoothingMode"));

                else if (enumerator.Name == "BackColor") item.BackColor = Color.FromArgb(Convert.ToInt32(info.GetString("BackColor")));
                else if (enumerator.Name == "GradientColor ") item.GradientColor  = Color.FromArgb(Convert.ToInt32(info.GetString("GradientColor")));
                else if (enumerator.Name == "GradientMode") item.GradientMode = (LinearGradientMode)Enum.Parse(typeof(LinearGradientMode), info.GetString("GradientMode"), true);
                else if (enumerator.Name == "DrawGradient") item.DrawGradient = info.GetBoolean("DrawGradient");
            }

            return item;
		}
	}
}
