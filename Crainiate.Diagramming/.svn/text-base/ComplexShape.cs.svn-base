// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

using Crainiate.Diagramming.Collections;

namespace Crainiate.Diagramming
{
	public class ComplexShape: Shape, ICloneable
	{
		//Property variables
		private Solids _children;

		//Working variables
		private ElementList _renderList;

		#region Interface

		//Constructors
		public ComplexShape()
		{
			Children = new Solids(Model);
			KeepAspect = true;
		}

		public ComplexShape(ComplexShape prototype): base(prototype)
		{
			Children = new Solids(Model);
            KeepAspect = true;

			foreach (Solid solid in prototype.Children.Values)
			{
				_children.Add(solid.Key,(Solid) solid.Clone());
			}
		}

		//Properties
		public virtual Solids Children
		{
			get
			{
				return _children;
			}
			set
			{
				if (value == null) throw new ArgumentNullException();

				_children = value;	

				_children.InsertElement += new ElementsEventHandler(Element_Insert);
				_children.RemoveElement += new ElementsEventHandler(Element_Remove);

				CreateRenderList();
				OnElementInvalid(); 
			}
		}

		//Returns the internal renderlist
		public virtual ElementList RenderList
		{
			get
			{
				return _renderList;
			}
		}

		#endregion

		#region Overrides

		public override object Clone()
		{
			return new ComplexShape(this);
		}

		public override bool Contains(PointF location)
		{
			//Check if parent shape contains the point
			if (base.Contains (location)) return true;

			PointF point = new PointF(location.X - Bounds.X,location.Y - Bounds.Y);
			
			foreach (Solid solid in RenderList)
			{
				if (solid.Contains(point)) return true;
			}

			return false;
		}

		public override void Scale(float x, float y, float dx, float dy, bool maintainAspect)
		{
			//Store current rectangle
			RectangleF rect = Bounds;
			
			//Scale the parent shape
			base.Scale(x, y, dx, dy, KeepAspect);
			
			//Check the values
			if (Width < (rect.Width * x)) x = Convert.ToSingle(Width / rect.Width);
			if (Height < (rect.Height * y)) y = Convert.ToSingle(Height / rect.Height);
			if (Width > (rect.Width * x)) x = Convert.ToSingle(Width / rect.Width);
			if (Height > (rect.Height * y)) y = Convert.ToSingle(Height / rect.Height);

			ScaleChildren(x, y, dx, dy, KeepAspect);

			Invalidate();
		}

		public override SizeF Size
		{
			get
			{
				return base.Size;
			}
			set
			{
				if (!value.Equals(Size))
				{
					SizeF newsize = ValidateSize(value.Width, value.Height); 
					
					float scalex = (newsize.Width / Size.Width);
					float scaley = (newsize.Height / Size.Height);

					ScaleChildren(scalex, scaley, 0, 0, false);

					base.Size = newsize;
				}
			}
		}


		#endregion

		#region Events

		//Occurs when an element is added to the elements collection
		private void Element_Insert(object sender, ElementsEventArgs e)
		{
			Element element = e.Value;

			//Set the layer
			element.SetLayer(Layer);

			//Set handlers
			element.ElementInvalid +=new EventHandler(Element_ElementInvalid);

			//Set the container
			element.SetModel(Model);

			CreateRenderList();
			OnElementInvalid();
		}

		//Occurs when an element is added to the elements collection
		private void Element_Remove(object sender, ElementsEventArgs e)
		{
			Element element = e.Value;
			element.ElementInvalid -=new EventHandler(Element_ElementInvalid);

			CreateRenderList();
			OnElementInvalid();
		}

		//Occurs when an element raises an invalid event
		private void Element_ElementInvalid(object sender, EventArgs e)
		{
			OnElementInvalid();
		}

		#endregion

		#region Implementation	
	
		private void CreateRenderList()
		{
			_renderList = new ElementList();

			//Check for intersections with the zoomed rectangle;
			foreach (Solid solid in Children.Values)
			{
				if (solid.Visible) _renderList.Add(solid);
			}
			_renderList.Sort();
            _renderList.SetModifiable(false);
		}

		protected virtual void ScaleChildren(float scaleX, float scaleY, float dx, float dy, bool maintainAspect)
		{
			foreach (Solid solid in Children.Values)
			{
				RectangleF rect = solid.InternalRectangle;
				rect = new RectangleF(rect.X * scaleX,rect.Y * scaleY,rect.Width * scaleX,rect.Height * scaleY);

				float mx = solid.Location.X * (scaleX - 1);
				float my = solid.Location.Y * (scaleY - 1);
				solid.ScalePath(scaleX,scaleY,mx,my,rect);
			}
		}

		#endregion
	}
}
