// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Crainiate.Diagramming
{
	public class Arrow: MarkerBase, ICloneable
	{
		private float _inset;

		//Constructor
		public Arrow()
		{
			Width = 10;
			Inset = 6;
			DrawPath(Width,Height,_inset);
		}

		public Arrow(bool drawBackground)
		{
			Width = 10;
			Inset = 6;
			DrawBackground = drawBackground;
			DrawPath(Width,Height,_inset);
		}

		public Arrow(Arrow prototype): base(prototype)
		{
			_inset = prototype.Inset;
		}

		//Properties
		//Sets or gets the height of the marker
		public virtual float Inset
		{
			get
			{
				return _inset;
			}
			set
			{
				_inset = value;
				DrawPath(Width,Height,_inset);
				OnElementInvalid();
			}
		}

		public override float Width
		{
			get
			{
				return base.Width;
			}
			set
			{
				base.Width = value;
				DrawPath(value,Height,_inset);;
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
				DrawPath(Width,value,_inset);
				OnElementInvalid();
			}
		}

		public override object Clone()
		{
			return new Arrow(this);
		}

		//Draws an arrow
		protected virtual void DrawPath(float width,float height,float inset)
		{
			GraphicsPath path = new GraphicsPath();
			float middle = width / 2;
			
			path.AddLine(middle,0,width,height);
			path.AddLine(width,height,middle,inset);
			path.AddLine(middle,inset,0,height);
			path.CloseFigure();
			
			SetPath(path);
		}
	}
}
