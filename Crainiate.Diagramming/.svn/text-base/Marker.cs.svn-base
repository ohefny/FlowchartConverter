// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Crainiate.Diagramming
{
	public class Marker: MarkerBase, ICloneable
	{
		//Property variables
		private MarkerStyle _markerStyle;
		
		#region Interface

		//Constructor
		public Marker()
		{
			_markerStyle = MarkerStyle.Ellipse;
			DrawPath();
		}

		public Marker(MarkerStyle style)
		{
			_markerStyle = style;
			DrawPath();
		}

		public Marker(Marker prototype): base(prototype)
		{
			_markerStyle = prototype.Style;
			DrawPath();
		}

		//Properties
		public override float Width
		{
			get
			{
				return base.Width;
			}
			set
			{
				base.Width = value;
				DrawPath();
				OnElementInvalid();
			}
		}

		public override float Height
		{
			get
			{
				return base.Height;
			}
			set
			{
				base.Height = value;
				DrawPath();
				OnElementInvalid();
			}
		}


		//Sets or gets the marker style
		public virtual MarkerStyle Style
		{
			get
			{
				return _markerStyle;
			}
			set
			{
				_markerStyle = value;
				DrawPath();
				OnElementInvalid();
			}
		}

		#endregion

		#region Overrides

		public override object Clone()
		{
			return new Marker(this);
		}

		#endregion

		#region Implementation

		//Draws an BasicMarker
		protected virtual void DrawPath()
		{
			GraphicsPath path = new GraphicsPath();
			PointF middle = new PointF(Width / 2, Height /2);

			switch (_markerStyle)
			{
				case MarkerStyle.Diamond:
					path.AddLine(middle.X,0,Width,middle.Y);
					path.AddLine(Width,middle.Y,middle.X,Height);
					path.AddLine(middle.X,Height,0,middle.Y);
					break;

				case MarkerStyle.Ellipse:
					path.AddEllipse(0,0,Width,Height);
					break;

				case MarkerStyle.Rectangle:
					path.AddRectangle(new RectangleF(0,0,Width,Height));
					break;
			}
			
			SetPath(path);
		}

		#endregion
	}
}
