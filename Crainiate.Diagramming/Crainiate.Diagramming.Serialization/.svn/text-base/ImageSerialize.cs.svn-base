// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming.Serialization
{
	public class ImageSerialize: ISerializationSurrogate
	{
		public virtual void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			Crainiate.Diagramming.Image image = (Crainiate.Diagramming.Image) obj;

            info.AddValue("Location", Serialization.Serialize.AddPointF(image.Location));
            info.AddValue("InterpolationMode", Convert.ToInt32(image.InterpolationMode).ToString());

            if (image.Path != null) info.AddValue("Path", image.Path);
            if (image.Resource != null) info.AddValue("Resource", image.Resource);
            if (image.Assembly != null) info.AddValue("Assembly", image.Assembly);

            //Serialize the actual image as an object
            info.AddValue("Image", image.Bitmap, typeof(System.Drawing.Image));
		}

		public virtual object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
            Crainiate.Diagramming.Image image = (Crainiate.Diagramming.Image) obj;
            SerializationInfoEnumerator enumerator = info.GetEnumerator();

            //Enumerate the info object and apply to the appropriate properties
            while (enumerator.MoveNext())
            {
                if (enumerator.Name == "Location") image.Location = Serialize.GetPointF(info.GetString("Location"));
                else if (enumerator.Name == "InterpolationMode") image.InterpolationMode = (InterpolationMode)Enum.Parse(typeof(InterpolationMode), info.GetString("InterpolationMode"));
                else if (enumerator.Name == "Path") image.SetPath(info.GetString("Path"));
                else if (enumerator.Name == "Resource") image.SetResource(info.GetString("Resource"));
                else if (enumerator.Name == "Assembly") image.SetAssembly(info.GetString("Assembly"));

                else if (enumerator.Name == "Image") image.SetImage((System.Drawing.Image) info.GetValue("Image", typeof(System.Drawing.Image)));
            }

            return image;
		}
	}
}
