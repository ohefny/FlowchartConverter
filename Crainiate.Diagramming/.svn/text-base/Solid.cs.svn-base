// (c) Copyright Crainiate Software 2010

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	[Serializable]
	public class Solid: Element, ILabelContainer, ITransformable
	{
		//Properties
		private Color _backColor;
		private Color _gradientColor;
		private LinearGradientMode _gradientMode;

		private Brush _customBrush;
		private bool _clip;

		private bool _drawGradient;
		private bool _drawBorder;

		private Label _label;
		private Image _image;
		private StencilItem _stencilItem;

		private float _rotation;

		//Working variables
		private RectangleF _internalRectangle;
		internal bool _drawBackground; // internal for print renderer to set
		private GraphicsPath _transformPath;
		private RectangleF _transformRectangle;
		private RectangleF _transformInternalRectangle;

		#region Interface

		//Constructor
		//Creates a new solid element
		public Solid()
		{
			BackColor = System.Drawing.Color.White;
			GradientColor = System.Drawing.Color.White;
			GradientMode = LinearGradientMode.ForwardDiagonal;
			Clip = true;
			DrawGradient = false;
			DrawBorder = true;
			DrawBackground = true;

			SetRectangle(Singleton.Instance.DefaultSize);
			StencilItem = Singleton.Instance.DefaultStencilItem;
		}

		public Solid(Solid prototype): base(prototype)
		{
			_customBrush = prototype.CustomBrush;
			_drawBackground = prototype.DrawBackground;
			_drawBorder = prototype.DrawBorder;
			_drawGradient = prototype.DrawGradient;
			_backColor = prototype.BackColor;
			_gradientColor = prototype.GradientColor;
			_gradientMode = prototype.GradientMode;
			_rotation = prototype.Rotation;

			_transformPath = prototype.TransformPath;
			_transformRectangle = prototype.TransformRectangle;
			_transformInternalRectangle = prototype.TransformInternalRectangle;
			
			if (prototype.Label != null) Label = (Label) prototype.Label.Clone();
			if (prototype.Image != null) Image = (Image) prototype.Image.Clone();
			
			_stencilItem = prototype.StencilItem;
			//if (prototype.StencilItem != null) _stencilItem = (StencilItem) prototype.StencilItem.Clone();

			_internalRectangle = prototype.InternalRectangle;
            _clip = prototype.Clip;
		}

		//Properties

		//Returns the shape Label which defines the text for this shape.
		public Label Label
		{
			get
			{
				return _label;
			}
			set
			{
				if (_label != null) 
				{
					_label.LabelInvalid -= new EventHandler(Label_LabelInvalid);
				}

				_label = value;
				if (_label != null) 
				{
					_label.LabelInvalid += new EventHandler(Label_LabelInvalid);
					_label.SetParent(this);
				}
				OnElementInvalid();
			}
		}

		//Returns the Image object which which displays an image for this shape.
		public Image Image
		{
			get
			{
				return _image;
			}
			set
			{
				if (_image != null) 
				{
					_image.ImageInvalid -= new EventHandler(Image_ImageInvalid);
				}

				_image = value;
				if (_image != null) 
				{
					_image.ImageInvalid += new EventHandler(Image_ImageInvalid);
					_image.SetParent(this);
				}

				OnElementInvalid();
			}
		}

		//Gets or sets the stencil used to draw this shape
		public StencilItem StencilItem
		{
			get
			{
				return _stencilItem;
			}
			set
			{
				_stencilItem = value;
				if (value != null) 
				{
					SetPath(value.Draw(Width,Height),value.InternalRectangle(Width,Height));
					value.CopyTo(this);
				}
			}
		}

		//Gets or sets the size of the shape.
		//Doesnt do equality check becuase min or max size may have changed
		public virtual SizeF Size
		{
			get
			{
				return Bounds.Size;
			}
			set
			{
				SetSizeInternal(value.Width, value.Height);
				OnElementInvalid();
			}
		}

		//Gets or sets the Width of the shape.
		public virtual float Width
		{
			get
			{
				return Bounds.Width;
			}
			set
			{
				SetSizeInternal(value, Bounds.Height);
				OnElementInvalid();
			}
		}

		//Gets or sets the Height of the shape.
		public virtual float Height
		{
			get
			{
				return Bounds.Height;
			}
			set
			{
				SetSizeInternal(Bounds.Width, value);
				OnElementInvalid();
			}
		}

		//Specifys the clockwise direction of the shape in degrees.
		public virtual float Rotation
		{
			get
			{
				return _rotation;
			}
			set
			{
				if (_rotation != value)
				{
					_rotation = value;

					_transformPath = Geometry.RotatePath(GetPath(),Location, value);
					_transformRectangle = _transformPath.GetBounds();
					
					_transformPath = Geometry.RotatePath(GetPath(), value);
					_transformInternalRectangle = Geometry.GetInternalRectangle(_transformPath);

					OnElementInvalid();
				}
			}
		}

		//Determines whether the gradient color is used to render a shape gradient.
		public virtual bool DrawGradient
		{
			get
			{
				return _drawGradient;
			}
			set
			{
				if (_drawGradient != value)
				{
					_drawGradient = value;
					OnElementInvalid();
				}
			}
		}

		//Determines whether the outline determined by the graphicspath is shown for this shape.
		public virtual bool DrawBorder
		{
			get
			{
				return _drawBorder;
			}
			set
			{
				if (_drawBorder != value)
				{
					_drawBorder = value;
					OnElementInvalid();
				}
			}
		}

		//Determines whether the outline determined by the graphicspath is shown for this shape.
		public virtual bool DrawBackground
		{
			get
			{
				return _drawBackground ;
			}
			set
			{
				if (_drawBackground != value)
				{
					_drawBackground = value;
					OnElementInvalid();
				}
			}
		}

		//Determines whether the image and annotation are confined to the shape's outline.
		public virtual bool Clip
		{
			get
			{
				return _clip;
			}
			set
			{
				if (_clip != value)
				{
					_clip = value;
					OnElementInvalid();
				}
			}
		}

		//The color used to draw the shape's background.
		public virtual Color BackColor
		{
			get
			{
				return _backColor;
			}
			set
			{
				if (! _backColor.Equals(value))
				{
					_backColor = value;
					OnElementInvalid();
				}
			}
		}

		//The color used to combine the shape's background color when drawing a gradient effect.
		public virtual Color GradientColor
		{
			get
			{
				return _gradientColor;
			}
			set
			{
				if (!_gradientColor.Equals(value))
				{
					_gradientColor = value;
					OnElementInvalid();
				}
			}
		}

		//Determines how gradients are drawn for this shape.
		public virtual LinearGradientMode GradientMode
		{
			get
			{
				return _gradientMode;
			}
			set
			{
				if (! _gradientMode.Equals(value))
				{
					_gradientMode = value;
					OnElementInvalid();
				}
			}
		}

		//Gets or sets a custom brush used for drawing this shape.
		public virtual Brush CustomBrush
		{
			get
			{
				return _customBrush;
			}
			set
			{
				_customBrush = value;

				OnElementInvalid();
			}
		}

		//Gets or sets the x co-ordinate of the position of the shape in pixels.
		public virtual float X
		{
			get
			{
				return Bounds.X;
			}
			set
			{
				if (Bounds.X != value)
				{
					//Store values before updating underlying rectangle
					float dx = value - Bounds.X;
					
					//Update control rectangle
					SetTransformRectangle(new PointF(TransformRectangle.X + dx, TransformRectangle.Y));
					SetRectangle(new PointF(value,Bounds.Y));
					OnElementInvalid();
				}
			}
		}

		//Gets or sets y co-ordinate of the position of the shape in pixels.
		public virtual float Y
		{
			get
			{
				return Bounds.Y;
			}
			set
			{
				if (Bounds.Y != value)
				{
					//Store values before updating underlying rectangle
					float dy = value - Bounds.Y;
					
					//Update control rectangle
					SetTransformRectangle(new PointF(TransformRectangle.X, TransformRectangle.Y + dy));
					SetRectangle(new PointF(Bounds.X,value));
					OnElementInvalid();
				}
			}
		}

		//Gets or sets the location of the solid element
		public virtual PointF Location
		{
			get
			{
				return Bounds.Location;
			}
			set
			{
				if (! Bounds.Location.Equals(value))
				{
					//Store values before updating underlying rectangle
					float dx = value.X - Bounds.X;
					float dy = value.Y - Bounds.Y;
					
					//Update control rectangle
					SetTransformRectangle(new PointF(TransformRectangle.X + dx, TransformRectangle.Y + dy));
					SetRectangle(value);
					OnElementInvalid();
				}
			}
		}

		//Returns the internal rectangle
		public virtual RectangleF InternalRectangle
		{
			get
			{
				return _internalRectangle;
			}
		}

		//Returns the current path with the current transformation
		public virtual GraphicsPath TransformPath
		{
			get
			{
				return _transformPath;
			}
		}

		//Returns the rectangle bounding the current transformation
		public virtual RectangleF TransformRectangle
		{
			get
			{
				return _transformRectangle;
			}
		}

		//Returns the rectangle inside the current transformation
		public virtual RectangleF TransformInternalRectangle
		{
			get
			{
				return _transformInternalRectangle;
			}
		}

		//Methods
		//Returns the intercept of a line drawn from the point provided to the centre of this shape.
		public override PointF Intercept(PointF location)
		{
			return GetIntercept(location);
		}

		//Moves a shape by the offset values supplied.
		public virtual void Move(float dx, float dy)
		{
			SetTransformRectangle(new PointF(TransformRectangle.X + dx, TransformRectangle.Y + dy));
			SetRectangle(new PointF(Bounds.X + dx, Bounds.Y + dy));
			OnElementInvalid();
		}

        public void SetTransformRectangle(RectangleF rectangle)
        {
            _transformRectangle = rectangle;
        }

        public void SetTransformRectangle(PointF location)
        {
            _transformRectangle = new RectangleF(location, _transformRectangle.Size);
        }

		public void SetInternalRectangle(RectangleF rectangle)
		{
			_internalRectangle = rectangle;
		}

		public void SetSize(float width, float height, RectangleF internalRectangle)
		{
			SetSizeInternal(width,height,internalRectangle);
		}

		public void SetSize(SizeF size, RectangleF internalRectangle)
		{
			SetSizeInternal(size.Width,size.Height,internalRectangle);
		}

		public void SetTransformPath(GraphicsPath path)
		{
			_transformPath = path;
		}

		#endregion

		#region Overrides

		public override object Clone()
		{
			return new Solid(this);
		}

        public override void ApplyTheme(Theme theme)
        {
            _backColor = theme.BackColor;
            _gradientColor = theme.GradientColor;
            _gradientMode = theme.GradientMode;
            _customBrush = theme.CustomBrush;

            //Will raise the elementinvalid event
            base.ApplyTheme(theme);
        }

		//Adds a vector path to this element.
		public override void AddPath(GraphicsPath path, bool connect)
		{
			if (path.PointCount == 0) return;
			
			base.AddPath(path,connect);
		}

		//Set the rotated path
		public override void SetPath(GraphicsPath path)
		{
			base.SetPath(path);

			if (Rotation == 0)
			{
				SetTransformRectangle(Bounds);
				SetTransformPath(path);
				_transformInternalRectangle = InternalRectangle;
			}
			else
			{
				SetTransformRectangle(Geometry.RotatePath(path, Location, Rotation).GetBounds());
				SetTransformPath(Geometry.RotatePath(path, Rotation));
				_transformInternalRectangle = Geometry.GetInternalRectangle(TransformPath);
			}
		}

		public virtual void SetPath(GraphicsPath path, RectangleF internalRectangle)
		{
			base.SetPath(path);
			SetInternalRectangle(internalRectangle);
			
			if (Rotation == 0)
			{
				SetTransformRectangle(Bounds);
				SetTransformPath(path);
				_transformInternalRectangle = internalRectangle;
			}
			else
			{
				SetTransformRectangle(Geometry.RotatePath(path, Location, Rotation).GetBounds()); //Bounds with location
				SetTransformPath(Geometry.RotatePath(path, Rotation)); //Transformed without location
				_transformInternalRectangle = Geometry.GetInternalRectangle(TransformPath); //only used for path docking optimization
			}
		}

		//Sets the vector path for this element.
		public override void ResetPath()
		{
			SetInternalRectangle(new RectangleF());
			base.ResetPath();
		}

		public override void ScalePath(float x, float y, float dx, float dy)
		{
			base.ScalePath (x, y, dx, dy);

			GraphicsPath path = GetPath(); 
			SetTransformRectangle(Geometry.RotatePath(path, Location, Rotation).GetBounds());
			SetTransformPath(Geometry.RotatePath(path, Rotation));
		}

		public virtual void ScalePath(float x, float y, float dx, float dy, RectangleF internalRectangle)
		{
			base.ScalePath(x,y,dx,dy);

			SetInternalRectangle(internalRectangle);

			GraphicsPath path = GetPath(); 
			SetTransformRectangle(Geometry.RotatePath(path, Location, Rotation).GetBounds());
			SetTransformPath(Geometry.RotatePath(path, Rotation));

			if (Rotation == 0) _transformInternalRectangle = internalRectangle;
		}

		public virtual void RotatePath(float degrees)
		{
			SetPath(Geometry.RotatePath(GetPath(), degrees));
		}

		//Determines whether this solid element contains the location
		public override bool Contains(PointF location)
		{
			if (DrawBackground)
			{
				return SolidContains(location);
			}
			else
			{
				return base.Contains(location);
			}
		}

		#endregion

		#region Events
		
		//Handles annotation invalid events
		private void Label_LabelInvalid(object sender, EventArgs e)
		{
			OnElementInvalid();
		}

		//Handles image invalid events
		private void Image_ImageInvalid(object sender, EventArgs e)
		{
			OnElementInvalid();
		}

		#endregion

		#region Implementation

		private PointF GetIntercept(PointF location)
		{
			//Cache the location properties
			float x = X;
			float y = Y;
			PointF center = Center;

			//Create transform location moved to the path origin and check if inside path
			PointF transform = new PointF(location.X - x,location.Y - y);
			if (TransformPath.IsVisible(transform)) return center;

			//Get the bounding rectangle intercept and move it to the origin
			//because all path measurements are from the origin
			location = Geometry.RectangleIntersection(location, center, TransformRectangle);
			location = new PointF(location.X - x,location.Y - y);
			
			//Get the center of the shape offset to the path origin
			center = new PointF(center.X - x,center.Y - y);
			
			location = Geometry.GetPathIntercept(location, center, TransformPath, Singleton.Instance.DefaultPen);
			location = Geometry.RoundPoint(location,1);

			return new PointF(location.X + x, location.Y + y);
		}

		private void SetSizeInternal(float width, float height)
		{
			if (StencilItem != null && StencilItem.Redraw)
			{
				SetPath(StencilItem.Draw(width,height),StencilItem.InternalRectangle(width,height));
			}
			else
			{
				float scaleX = Convert.ToSingle(width / Bounds.Width);
				float scaleY = Convert.ToSingle(height / Bounds.Height);
				
				//Scale rectangle
				RectangleF original = InternalRectangle;
				RectangleF rect = new RectangleF(original.X * scaleX,original.Y * scaleY,original.Width * scaleX,original.Height * scaleY);
				ScalePath(scaleX,scaleY,0,0,rect);
			}
		}

		private void SetSizeInternal(float width, float height, RectangleF internalRectangle)
		{
			if (StencilItem != null && StencilItem.Redraw)
			{
				SetPath(StencilItem.Draw(width,height),internalRectangle);
			}
			else
			{
				float scaleX = Convert.ToSingle(width / Bounds.Width);
				float scaleY = Convert.ToSingle(height / Bounds.Height);
				ScalePath(scaleX,scaleY,0,0,internalRectangle);
			}
		}

		private bool SolidContains(PointF location)
		{
			//Get boundary
			RectangleF bounds = TransformRectangle;

			//If inside inflate boundary
			if (bounds.Contains(location))
			{
				//Check the outline offset to the path (0,0)
				location.X -= TransformRectangle.X;
				location.Y -= TransformRectangle.Y;
				
				//Can return in use error
				try
				{
					if (TransformPath.IsVisible(location)) return true;
				}
				catch
				{
					
				}
			}
			
			return false;
		}

		public virtual PointF GetLabelLocation()
		{
			if (Label.Size.IsEmpty || Label.Offset.IsEmpty) return InternalRectangle.Location;
			return Label.Offset;
		}

		public virtual SizeF GetLabelSize()
		{
			if (Label.Size.IsEmpty || Label.Offset.IsEmpty) return InternalRectangle.Size;
			return Label.Size;
		}

		#endregion

	}
}
