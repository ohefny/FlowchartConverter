// (c) Copyright Crainiate Software 2010

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming.Serialization
{
	public class LinkSerialize: ElementSerialize
	{
		public override void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
            base.GetObjectData(obj, info, context);

			Link line = (Link) obj;

            info.AddValue("AllowMove", line.AllowMove);
            info.AddValue("DrawSelected", line.DrawSelected);
            info.AddValue("Selected", line.Selected);
            info.AddValue("LineJoin", Convert.ToInt32(line.LineJoin).ToString());
            info.AddValue("Interaction", Convert.ToInt32(line.Interaction).ToString());

            //Add property values using special function?
            info.AddValue("Start", line.Start); 
            info.AddValue("End", line.End);

            info.AddValue("Ports", line.Ports);
		}

		public override object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
            base.SetObjectData(obj, info, context, selector);

            Link line = (Link) obj;
            SerializationInfoEnumerator enumerator = info.GetEnumerator();            

            //Enumerate the info object and apply to the appropriate properties
            while (enumerator.MoveNext())
            {
                if (enumerator.Name == "AllowMove") line.AllowMove = info.GetBoolean("AllowMove");
                else if (enumerator.Name == "DrawSelected") line.DrawSelected = info.GetBoolean("DrawSelected");
                else if (enumerator.Name == "Selected") line.Selected = info.GetBoolean("Selected");
                else if (enumerator.Name == "LineJoin") line.LineJoin = (LineJoin)Enum.Parse(typeof(LineJoin), info.GetString("LineJoin"), true);
                else if (enumerator.Name == "Interaction") line.Interaction = (UserInteraction)Enum.Parse(typeof(UserInteraction), info.GetString("Interaction"), true);
                else if (enumerator.Name == "Start") line.Start = (Origin)info.GetValue("Start", typeof(Origin));
                else if (enumerator.Name == "End") line.End = (Origin)info.GetValue("End", typeof(Origin));
                else if (enumerator.Name == "Ports") line.Ports = (Ports) info.GetValue("Ports", typeof(Ports));
            }

            return line;
		}
	}
}
