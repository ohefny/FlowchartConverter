// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Crainiate.Diagramming
{
	// Base class for an object contained in diagram
	public class Element: ICloneable, IRenderable
	{
		//Property variables
		private int _zOrder; 
		private string _key;

		private GraphicsPath _path;
		private Layer _currentLayer;
		private Model _model;
		private Pen _customPen;

		private object _tag;
		private bool _visible;
		private byte _opacity;
		private string _tooltip;
		private bool _drawShadow;

		private Color _borderColor;
		private DashStyle _borderStyle;
		private float _borderWidth;
		private SmoothingMode _smoothingMode;

		private Handles _handles;
		private Cursor _cursor;
        private Group _group;
		
		//Working variables
		private RectangleF _rectF; //The size of the element
		private Element _actionElement; //associates an action element with this element
		private Element _updateElement; //the cloned element for an action

		//Events
		public event EventHandler ElementInvalid;
		public event EventHandler VisibleChanged;

		//Constructors
		public Element()
		{			
			_key = string.Empty;
			Visible = true;
			SmoothingMode = SmoothingMode.AntiAlias;
			Opacity = Singleton.Instance.DefaultOpacity;
			DrawShadow = true;

			BorderColor = System.Drawing.Color.Black;
			BorderStyle = DashStyle.Solid;
			BorderWidth = Singleton.Instance.DefaultBorderWidth;
			
			_path = new GraphicsPath();
			_cursor = null;
		}

		//Creates a new element by copying an existing element
		//Layer should be assigned by the system and should not be copied
		public Element(Element prototype)
		{
			_key = string.Empty;

			_borderColor = prototype.BorderColor;
			_borderStyle = prototype.BorderStyle;
			_borderWidth = prototype.BorderWidth;
			_smoothingMode = prototype.SmoothingMode;
			_customPen = prototype.CustomPen;
			_drawShadow = prototype.DrawShadow;
			_opacity = prototype.Opacity;
			_tooltip = prototype.Tooltip;
			_visible = prototype.Visible;
			_cursor = prototype.Cursor;

			_path = prototype.GetPath();
			_rectF = prototype.Bounds;
			_model = prototype.Model;
			
			_tag = prototype.Tag;
		}

		//Properties
		public virtual Model Model
		{
			get
			{
				return _model;
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
				OnElementInvalid();
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
				OnElementInvalid();
			}
		}

		//Sets or retrieves the width used to draw the shape's border.
		public virtual float BorderWidth
		{
			get
			{
				return _borderWidth;
			}
			set
			{
				_borderWidth = value;
				OnElementInvalid();
			}
		}

		//Gets or sets the value determining how anti aliasing is performed
		public virtual SmoothingMode SmoothingMode
		{
			get
			{
				return _smoothingMode;
			}
			set
			{
				if (_smoothingMode != value)
				{
					_smoothingMode = value;
					OnElementInvalid();
				}
			}
		}

		//Returns the key for this element when contained in a collection.
		public string Key
		{
			get
			{
				return _key;
			}
		}

		//Sets or gets the tag for this object.
		public virtual object Tag
		{
			get
			{
				return _tag;
			}
			set
			{
				_tag = value;
			}
		}

		public virtual Cursor Cursor
		{
			get
			{
				return _cursor;
			}
			set
			{
				_cursor = value;
			}
		}

        public virtual Group Group
        {
            get
            {
                return _group;
            }
        }

		//Returns the zorder for this shape.
		public int ZOrder
		{
			get
			{
				return _zOrder;
			}
		}

		//Gets or sets the tooltip for this shape.
		public virtual string Tooltip
		{
			get
			{
				return _tooltip;
			}
			set
			{
				_tooltip = value;
			}
		}

		//Defines the percentage opacity for the background of this shape.
		public virtual byte Opacity
		{
			get
			{
				return _opacity;
			}
			set
			{
				if (_opacity != value)
				{
					_opacity = value;
					OnElementInvalid();
				}
			}
		}

		//The rectangle which completely bounds the exterior of this shape.
		public virtual RectangleF Bounds
		{
			get
			{
				return _rectF;
			}
		}

        public virtual PointF Center
        {
            get
            {
                return new PointF(Bounds.X + Bounds.Width / 2, Bounds.Y + Bounds.Height / 2);
            }
        }

		//Indicates whether the shape is currently visible and rendered during drawing operations.
		public virtual bool Visible
		{
			get
			{
				return _visible;
			}
			set
			{
				if (_visible != value)
				{
					_visible = value;
					OnVisibleChanged();
					OnElementInvalid();
				}
			}
		}

		//Indicates whether the selection path decoration is shown when the element is selected.
		public virtual bool DrawShadow
		{
			get
			{
				return _drawShadow;
			}
			set
			{
				if (_drawShadow != value)
				{
					_drawShadow = value;
					OnElementInvalid();
				}
			}
		}

		public virtual Pen CustomPen
		{
			get
			{
				return _customPen;
			}
			set
			{
				_customPen = value;
				OnElementInvalid();
			}
		}

		public virtual Layer Layer
		{
			get
			{
				return _currentLayer;
			}
		}

		//Returns the collection of handles this shape holds.
		public virtual Handles Handles
		{
			get
			{
				return _handles;
			}
		}

		//Sets the element on which an action is being performed
		//Must remain public
		public Element ActionElement
		{
			get
			{
				return _actionElement;
			}
			set
			{
				if (value == null && _actionElement != null) _actionElement.UpdateElement = null;
				_actionElement = value;
				if (_actionElement != null) _actionElement.UpdateElement = this;

			}
		}

		//Sets the element on which an action is being performed
		public Element UpdateElement
		{
			get
			{
				return _updateElement;
			}
			set
			{
				_updateElement = value;
			}
		}

        //Contains a reference the current renderer being used to render the element
        public IRenderer Renderer { get; set; }

		//Methods
		//Returns the vector path for this element.
		public virtual GraphicsPath GetPath()
		{
            return _path;
		}

        public virtual PointF Intercept(PointF location)
        {
            return Center;
        }

		//Adds a vector path to this element.
		public virtual void AddPath(GraphicsPath path,bool connect)
		{
			_path.AddPath(path,connect);
			Geometry.MovePathToOrigin(_path);
			SetRectangle(GetBoundingRectangle().Size);
			OnElementInvalid();
		}

		//Sets the vector path for this element.
		public virtual void SetPath(GraphicsPath path)
		{
			_path = path;

            Geometry.MovePathToOrigin(_path);
            SetRectangle(GetBoundingRectangle().Size);
            OnElementInvalid();
		}	
	
		//Sets the vector path for this element.
		public virtual void ResetPath()
		{
			_path = new GraphicsPath();
			SetRectangle(new Rectangle(0,0,0,0));
			SetHandles(new Handles());
			OnElementInvalid();
		}

		//Scales the path
		public virtual void ScalePath(float sx, float sy, float dx, float dy)
		{
			GraphicsPath path = Geometry.ScalePath(GetPath(), sx,sy);

			//Calculate path rectangle
			RectangleF rect = GetBoundingRectangle();
			rect.Location = Bounds.Location;
			rect.Offset(dx,dy);

			SetRectangle(rect); //Sets the bounding rectangle
			SetPath(path); //setpath moves the path to 0,0;
		}

		//Sends a message to the Model saying that the elment is invalid
		public virtual void Invalidate()
		{
			OnElementInvalid();
		}
	
		//Returns a string representation of this class
		public virtual string ToString()
		{
			return this.Key;
		}

		//Determines whether the supplied co-ordinates are over the element
		public virtual bool Contains(PointF location)
		{
			return ElementContains(location);
		}

		//Determines whether this element intersects with the rectangle provided
		public virtual bool Intersects(RectangleF rectangle)
		{
			return ElementIntersects(rectangle);
		}

		//Returns the type of cursor from this point
		public virtual Handle Handle(PointF location)
		{
			return Singleton.Instance.DefaultHandle;
		}

        public virtual void ApplyTheme(Theme theme)
        {
            _customPen = theme.CustomPen;
            _opacity = theme.Opacity;
		    _borderColor = theme.BorderColor;
            _borderStyle = theme.BorderStyle;
            _borderWidth = theme.BorderWidth;
            _smoothingMode = theme.SmoothingMode;

            OnElementInvalid();
        }

        //Return an element point from a diagram point
        public virtual PointF PointToElement(PointF location)
        {
            return new PointF(location.X - Bounds.X, location.Y - Bounds.Y);
        }

        //Updates the rectangle used to store location and size for shapes and lines
        public virtual void SetRectangle(RectangleF rect)
        {
            if (rect.Width <= 0) rect.Width = 1;
            if (rect.Height <= 0) rect.Height = 1;
            _rectF = rect;
        }

        //Updates the rectangle used to store location and size for shapes and lines
        public virtual void SetRectangle(PointF location)
        {
            _rectF.Location = location;
        }

        //Updates the rectangle used to store location and size for shapes and lines
        public virtual void SetRectangle(SizeF size)
        {
            if (size.Width <= 0) size.Width = 1;
            if (size.Height <= 0) size.Height = 1;

            _rectF.Size = size;
        }

		//Sets the current layer the element is in
		public virtual void SetLayer(Layer layer)
		{
			_currentLayer = layer;
		}

		public virtual void SetModel(Model model)
		{
			_model = model;
		}

		public virtual void SetHandles(Handles handles)
		{
			_handles = handles;
		}

		//Used to set the key value when the element is contained in a collection
		public virtual void SetKey(string key)
		{
			if (_key == string.Empty) _key = key;
		}

		public virtual void SetOrder(int zorder)
		{
			_zOrder = zorder;
		}

        public virtual void SetGroup(Group group)
        {
            _group = group;
        }

		//Event Methods
		
		//Raises the element invalid event.
		protected virtual void OnElementInvalid()
		{
			if (ElementInvalid != null) ElementInvalid(this, EventArgs.Empty);
		}

		//Raises the visible event.
		protected virtual void OnVisibleChanged()
		{
			if (VisibleChanged != null) VisibleChanged(this, EventArgs.Empty);
		}

		//Clones an element
		public virtual object Clone()
		{
			return new Element(this);
		}

        protected internal virtual void CreateHandles()
        {
            SetHandles(new Handles());
        }

        //Performs hit testing for an element from a location
        //if a valid diagram provided, hit testing is performed using current transform
        private bool ElementContains(PointF location)
        {
            //Get bounding rect
            float width = BorderWidth + 2;
            RectangleF bound = Bounds;
            bound.Inflate(width, width);

            //If inside boundary
            if (bound.Contains(location) || bound.Height <= width || bound.Width <= width)
            {
                //Check the outline offset to the path (0,0)
                location.X -= Bounds.X;
                location.Y -= Bounds.Y;
                if (GetPath().IsOutlineVisible(location, new Pen(Color.Black, width))) return true;
            }

            return false;
        }

		//Determines whether this element intersects with the rectangle provided
		private bool ElementIntersects(RectangleF rectangle)
		{
			return rectangle.IntersectsWith(Bounds);
		}

		internal virtual RectangleF GetBoundingRectangle()
		{
			RectangleF rect = _path.GetBounds();
			DiagramUnit unit = DiagramUnit.Pixel;

			return Geometry.RoundRectangleF(rect, 1);
		}
	}
}
