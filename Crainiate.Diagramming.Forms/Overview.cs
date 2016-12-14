// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;	
using System.Windows.Forms;
using System.ComponentModel;

using Crainiate.Diagramming.Collections;
using Crainiate.Diagramming.Forms.Rendering;

namespace Crainiate.Diagramming.Forms
{
	[ToolboxBitmap(typeof(Overview), "Resource.overview.bmp")]
	public class Overview: View
	{
		//Property variables
		private View _diagram;

		#region Interface
		
		//Constructors
		public Overview(): base()
		{
			Render = new OverviewRender();
			Zoom = 10;
		}

		//Properties
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual View Diagram
		{
			get
			{
				return _diagram;
			}
			set
			{
				if (_diagram != null)
				{
					_diagram.ElementInserted -=new ElementsEventHandler(Diagram_ElementsChanged);
					_diagram.ElementRemoved -=new ElementsEventHandler(Diagram_ElementsChanged);

					if (_diagram is Diagram)
					{
						Diagram diagram = (Diagram) _diagram;
						diagram.UpdateActions -= new UserActionEventHandler(Diagram_UpdateActions);
					}
				}

				if (value == null)
				{
					_diagram = null;
                    //Model.Shapes = new Shapes(Model);
                    //Model.Lines = new Lines(Model);
					Model.Layers = new Layers(Model);
				}
				else
				{
					_diagram = value;
                    //SetShapes(_diagram.Model.Shapes);
                    //SetLines(_diagram.Model.Lines);
                    //SetLayers(_diagram.Model.Layers);
					
					//Set events
					_diagram.ElementInserted +=new ElementsEventHandler(Diagram_ElementsChanged);
					_diagram.ElementRemoved +=new ElementsEventHandler(Diagram_ElementsChanged);
					
					if (_diagram is Diagram)
					{
						Diagram diagram = (Diagram) _diagram;
						diagram.UpdateActions += new UserActionEventHandler(Diagram_UpdateActions);
					}
				}
			}
		}

		//Methods
		public virtual void PositionDiagram(MouseEventArgs e)
		{
			PositionDiagramImplementation(e.X,e.Y);
		}

		public virtual void PositionDiagram(int x, int y)
		{
			PositionDiagramImplementation(x,y);
		}

		#endregion

		#region Events

		private void Diagram_UpdateActions(object sender, UserActionEventArgs e)
		{
			Invalidate();
		}

		#endregion

		#region Implementation

		private void PositionDiagramImplementation(int x, int y)
		{
			if (_diagram == null) return;

			View diagram = (View) _diagram;

			PointF diagra_point = PointToDiagram(x,y);
			int intX = 0;
			int intY = 0;
			double zoom = diagram.Zoom / 100;

			intX = Convert.ToInt32(diagra_point.X - ((diagram.Width / 2) * zoom));
			intY = Convert.ToInt32(diagra_point.Y - ((diagram.Height / 2) * zoom));

			if (intX < 0) intX = 0;
			if (intY < 0) intY = 0;

			diagram.AutoScrollPosition = new Point(intX, intY);
		}

		#endregion

		//Handles elements being added or removed from the target diagram
		private void Diagram_ElementsChanged(object sender, ElementsEventArgs e)
		{
			Invalidate();
		}
	}
}
