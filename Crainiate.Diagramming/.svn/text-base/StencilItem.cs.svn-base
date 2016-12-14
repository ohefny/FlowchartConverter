// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	public class StencilItem: ICloneable	
	{
		//Property variables
		private bool _redraw;
		internal string _key; //internal for assignment from collection

		private Color _borderColor;
		private DashStyle _borderStyle;
		private SmoothingMode _smoothingMode;
		private Color _backColor;
		private Color _gradientColor;
		private LinearGradientMode _gradientMode;
		private bool _drawGradient;
		private Label _label;
		private Image _image;
		private StencilItemOptions _stencilItemOptions;
		private bool _keepAspect;

		//Working variables
		private SizeF _baseSize;
		private SizeF _lastRequest;
		private RectangleF _baseInternalRectangle;
		private GraphicsPath _basePath;
		private float _aspectRatio;

		#region Interface

		//Events
		public event DrawShapeEventHandler DrawShape;

		//Constructors
		public StencilItem()
		{
			_drawGradient = true;
			_borderColor = Color.FromArgb(66,65,66);
			_backColor = Color.White;
			_gradientMode = LinearGradientMode.ForwardDiagonal;
			_gradientColor = Color.White;
			_smoothingMode = SmoothingMode.HighQuality;
			_stencilItemOptions = StencilItemOptions.InnerRectangleFull | StencilItemOptions.SoftShadow;
			_aspectRatio = 1.0F;
		}

		//Properties
		public virtual bool Redraw
		{
			get
			{
				return _redraw;
			}
			set
			{
				_redraw = value;
			}
		}

		public virtual string Key
		{
			get
			{
				return _key;
			}
			set
			{
				_key = value;
			}
		}

		public virtual Label Label
		{
			get
			{
				return _label;
			}
			set
			{
				_label = value;
			}
		}

		public virtual Image Image
		{
			get
			{
				return _image;
			}
			set
			{
				_image = value;
			}
		}

		public GraphicsPath BasePath 
		{
			get
			{
				return _basePath;
			}
		}

		public RectangleF BaseInternalRectangle
		{
			get
			{
				return _baseInternalRectangle;
			}
		}

		public SizeF BaseSize
		{
			get
			{
				return _baseSize;
			}
		}

		public float AspectRatio
		{
			get
			{
				return _aspectRatio;
			}
		}

		//The color used to draw the shape's borders.
		public virtual Color BorderColor
		{
			get
			{
				return _borderColor;
			}
			set
			{
				_borderColor = value;
			}
		}

		//Sets or retrieves the dash style used to draw the shape's border.
		public virtual DashStyle BorderStyle
		{
			get
			{
				return _borderStyle;
			}
			set
			{
				_borderStyle = value;
			}
		}

		//gets or sets the value determining how anti aliasing is performed
		public virtual SmoothingMode SmoothingMode
		{
			get
			{
				return _smoothingMode;
			}
			set
			{
				_smoothingMode = value;
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
				}
			}
		}

		//Determines whether items drawn with the stencil item are intended to keep their height to width ratio the same
		public virtual bool KeepAspect
		{
			get
			{
				return _keepAspect;
			}
			set
			{
				_keepAspect = value;
			}
		}

		public virtual StencilItemOptions Options
		{
			get
			{
				return _stencilItemOptions;
			}
			set
			{
				_stencilItemOptions = value;
			}
		}

		//Methods
		//Draw the graphics path by raising the draw event, and set the internal rectangle
		//The internal rectangle should be set by the draw method to prevent it from being calculated
		public virtual GraphicsPath Draw(float width, float height)
		{
			return GetPath(width,height);
		}

		//Returns the internal rectangle for this stencil item
		public virtual RectangleF InternalRectangle(float width, float height)
		{
			if (BasePath == null) return new RectangleF();
			if (Redraw && width == _lastRequest.Width && height == _lastRequest.Height) return _baseInternalRectangle;

			//Cache the rectangle for next time
			_lastRequest = new SizeF(width,height);

			//Recalculate if first time or a redraw for a different size
			if (_baseInternalRectangle.IsEmpty || Redraw)
			{
				//Get internal rectangle
				_baseInternalRectangle = Geometry.GetInternalRectangle(GetPath(width,height));

				//Shrink by one pixel
				_baseInternalRectangle.Inflate(-1,-1);

				_baseInternalRectangle = Rectangle.Round(_baseInternalRectangle);
				return _baseInternalRectangle;
			}

			return GetInternalRectangle(width,height);
		}

		//Copies the stencil default values to the element supplied
		public virtual void CopyTo(Solid element)
		{
			//Set the element values
			element.BorderColor = BorderColor;
			element.BorderStyle = BorderStyle;
			element.SmoothingMode = SmoothingMode;
			element.BackColor = BackColor;
			element.GradientColor = GradientColor;
			element.GradientMode = GradientMode;
			element.DrawGradient = DrawGradient;
			element.Label = Label;
			element.Image = Image;
			
			if (element is Shape)
			{
				Shape shape = (Shape) element;
				shape.KeepAspect = KeepAspect;

				//Make sure shape is resized to correct aspect
				if (KeepAspect && AspectRatio != 1)
				{
					//If width greater than height
					if (AspectRatio > 1)
					{
						shape.Height = shape.Width / AspectRatio;
					}
					else
					{
						shape.Width = shape.Height * AspectRatio;
					}
				}
			}
		}

		//Sets the base graphics path manually
		protected internal virtual void SetBasePath(GraphicsPath path)
		{
			if (path == null) return;
			_baseSize = path.GetBounds().Size;
			_basePath = path;
			Geometry.MovePathToOrigin(_basePath);
		}

		//Sets the base internal rectangle manually
		protected internal virtual void SetBaseInternalRectangle(RectangleF rectangle, float width, float height)
		{
			_baseInternalRectangle = rectangle;
			_lastRequest = new SizeF(width,height);
		}

		//Raises the DrawShape event.
		protected virtual void OnDrawShape(GraphicsPath path,float width, float height)
		{
			//If there are no event handlers then draw the default
			if (DrawShape == null) 
			{
				DrawDefault(path,width,height);
			}
			else
			{
				DrawShape(this,new DrawShapeEventArgs(path,width,height));
			}
		}

		#endregion

		#region Implementation

		public object Clone()
		{
			StencilItem item = new StencilItem();
			item.Redraw = _redraw;
		
			item.BorderColor = _borderColor;
			item.BorderStyle =  _borderStyle;
			item.SmoothingMode = _smoothingMode;
			item.BackColor = _backColor;
			item.GradientColor =  _gradientColor;
			item.GradientMode = _gradientMode;
			item.DrawGradient = _drawGradient;
			item.Options = _stencilItemOptions;

			item.SetBasePath(_basePath);
			item.SetBaseInternalRectangle(_baseInternalRectangle,_baseSize.Width,_baseSize.Height);

			return item;
		}

		private void DrawDefault(GraphicsPath path,float width, float height)
		{
			path.AddArc(0, 0, 20, 20, 180, 90);
			path.AddArc(width - 20, 0, 20, 20, 270, 90);
			path.AddArc(width - 20, height - 20, 20, 20, 0, 90);
			path.AddArc(0, height - 20, 20, 20, 90, 90);
			path.CloseFigure();

			SetBaseInternalRectangle(new RectangleF(5, 5, width - 10, height - 10), width, height);
		}

		private GraphicsPath GetPath(float width, float height)
		{
			GraphicsPath path = new GraphicsPath();

			if (Redraw || BasePath == null) 
			{
				//Raise the draw event in which the path is drawn
				OnDrawShape(path,width,height);

				//Scale the path to correct size, calculating ration drawn
				RectangleF rect = path.GetBounds();
				path = Geometry.ScalePath(path, width / rect.Width, height / rect.Height);
				_aspectRatio = (width / rect.Width * height / rect.Height);
				
				//Cache the path for later use, or to calculate internal rectangle
				_baseSize = new SizeF(width,height); // do not measure size because of rounding issues
				_basePath = path;
				Geometry.MovePathToOrigin(_basePath);
			}
			else
			{
				float sx = width / BaseSize.Width;
				float sy = height / BaseSize.Height;
			
				path = Geometry.ScalePath(BasePath,sx,sy);
			}
			
			return path;
		}

		//Return a scaled verion of the cached rectangle
		private RectangleF GetInternalRectangle(float width,float height)
		{
			float sx = width / BaseSize.Width;
			float sy = height / BaseSize.Height;
			return Geometry.ScaleRectangle(BaseInternalRectangle,sx,sy);
		}

		#endregion


	}
}
