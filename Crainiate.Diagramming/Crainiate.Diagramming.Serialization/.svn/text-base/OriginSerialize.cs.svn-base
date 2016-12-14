// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming.Serialization
{
	public class OriginSerialize: ISerializationSurrogate
	{
		public virtual void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			Origin origin = (Origin) obj;

            info.AddValue("Location", Serialize.AddPointF(origin.Location));
            info.AddValue("AllowMove", origin.AllowMove);

            if (origin.Marker != null) info.AddValue("Marker", origin.Marker);
            if (origin.Shape != null) info.AddValue("Shape", origin.Shape);
            if (origin.Port != null) info.AddValue("Port", origin.Port);
		}

		public virtual object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
            Origin origin = (Origin)obj;

            origin.Location = Serialize.GetPointF(info.GetString("Location"));
            origin.AllowMove = info.GetBoolean("AllowMove");

            if (Serialize.Contains(info, "Marker", typeof(MarkerBase))) origin.Marker = (MarkerBase) info.GetValue("Marker", typeof(MarkerBase));
            if (Serialize.Contains(info, "Shape", typeof(Shape))) origin.Shape = (Shape) info.GetValue("Shape", typeof(Shape));
            if (Serialize.Contains(info, "Port", typeof(Port))) origin.Port = (Port) info.GetValue("Port", typeof(Port));

            return origin;
		}
	}
}
