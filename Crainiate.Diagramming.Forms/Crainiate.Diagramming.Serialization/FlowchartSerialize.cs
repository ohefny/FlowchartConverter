// (c) Copyright Crainiate Software 2010




using System;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Flowcharting;

namespace Crainiate.Diagramming.Serialization
{
	public class FlowchartSerialize: DiagramSerialize
	{
		public FlowchartSerialize()
		{
		}

		public override void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(obj,info,context);
			
			Flowchart flowchart = (Flowchart) obj;

			info.AddValue("Orientation",Convert.ToInt32(flowchart.Orientation).ToString());
			info.AddValue("Spacing",Serialize.AddSizeF(flowchart.Spacing));
		}

		public override object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			base.SetObjectData(obj,info,context,selector);

			Flowchart flowchart = (Flowchart) obj;
			
			flowchart.Orientation = (FlowchartOrientation) Enum.Parse(typeof(FlowchartOrientation), info.GetString("Orientation"));
			flowchart.Spacing = Serialize.GetSizeF(info.GetString("Spacing"));

			return flowchart;
		}
	}
}
