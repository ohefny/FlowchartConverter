// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Runtime.Serialization;

using Crainiate.Diagramming.Forms;
using Crainiate.Diagramming.Layouts;
using Crainiate.Diagramming.Serialization;
using Crainiate.Diagramming.Forms.Rendering;

namespace Crainiate.Diagramming.Serialization
{
	public class ViewSerialize: ISerializationSurrogate
	{
		private Shapes _shapes;
		private Lines _lines;
		private Layers _layers;

		//Constructors
		public ViewSerialize()
		{

		}

		//Properties
		public virtual Shapes Shapes
		{
			get
			{
				return _shapes;
			}
		}

		public virtual Lines Lines
		{
			get
			{
				return _lines;
			}
		}

		public virtual Layers Layers
		{
			get
			{
				return _layers;
			}
		}

		//Implement ISerializeSurrogate
		public virtual void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			View diagram = (View) obj;
			
			info.AddValue("Shapes",diagram.Model.Shapes,typeof(Elements));
			info.AddValue("Lines",diagram.Model.Lines,typeof(Elements));
			info.AddValue("Layers",diagram.Model.Layers,typeof(Layers));
			info.AddValue("Zoom",diagram.Zoom);
			info.AddValue("ShowTooltips",diagram.ShowTooltips);
            info.AddValue("Paged", diagram.Paging.Enabled);
			info.AddValue("Margin",diagram.Margin);
			info.AddValue("WorkspaceColor",diagram.Paging.WorkspaceColor.ToArgb().ToString());
		}

		public virtual object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			View diagram = (View) obj;

			diagram.Suspend();
			
			_shapes = (Shapes) info.GetValue("Shapes",typeof(Shapes));
			_lines = (Lines) info.GetValue("Lines",typeof(Lines));
			_layers = (Layers) info.GetValue("Layers",typeof(Layers));
			
			//Diagram is created without a constructor, so need to do some initialization
			diagram.Render = new ControlRender();

			diagram.Zoom = info.GetSingle("Zoom");
			diagram.ShowTooltips = info.GetBoolean("ShowTooltips");
            diagram.Paging.Enabled = info.GetBoolean("Paged");
            diagram.Paging.WorkspaceColor = Color.FromArgb(Convert.ToInt32(info.GetString("WorkspaceColor")));

			diagram.Resume();
			return diagram;
		}

		public virtual void UpdateObjectReferences()
		{
            //##
            ////Relink layers
            //foreach (Layer layer in _layers)
            //{	
            //    foreach (Element element in layer.Elements.Values)
            //    {
            //        element.SetLayer(layer);
            //    }
            //}
		}
	}
}
