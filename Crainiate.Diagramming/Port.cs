// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming
{
	public class Port: Solid, ISelectable, IUserInteractive
	{
		//Property variables
		private PortAlignment _alignment;
		
		private bool _validate;
		private PointF _offset;
		private bool _allowMove;
		private bool _allowRotate;
		private bool _drawSelected;
		private bool _selected;
		private Direction _direction;
		private UserInteraction _interaction;
		private PortStyle _portStyle;
        private bool _fixed;

		//Working variables
		internal IPortContainer _parent;
		private float _percent = 50F;
		private PortOrientation _orientation;
		private bool _suspendValidation;

		//Events
		public event LayoutChangedEventHandler LayoutChanged;
		public event EventHandler SelectedChanged;

		//Constructors
		//Sets the inital orientation
        public Port(): this(PortOrientation.None, 50F)
        {
        }

		public Port(PortOrientation orientation): this(orientation, 50F)
		{
		}
        
        //Sets the inital orientation and percentage
		public Port(float percent): this(PortOrientation.Top, percent)
		{
		}

		//Sets the inital orientation and percentage
		public Port(PortOrientation orientation, float percent)
		{
			Label = null;
			StencilItem = null;

			_validate  = true;
			_allowMove = true;
			_allowRotate = true;
			_offset = new PointF();
			_orientation = orientation;
			_percent = percent;
			Alignment =  PortAlignment.Center;
			Direction = Direction.Both;
			Interaction = UserInteraction.BringToFront;
			Style = PortStyle.Default;
		}

		public Port(Port prototype): base(prototype)
		{
			Label = null;
			StencilItem = null;
				
			_offset = prototype.Offset;
			_allowMove = prototype.AllowMove;
			_allowRotate = prototype.AllowRotate;
			_direction = prototype.Direction;
			_interaction = prototype.Interaction;
			Label = null;
			_portStyle = prototype.Style;
			Cursor = prototype.Cursor;

			_percent = prototype.Percent;
			_orientation = prototype.Orientation;
			
			//Needed for action move
			_parent = prototype.Parent;
            Alignment = prototype.Alignment;
		}

		//Properties
		public virtual PortStyle Style
		{
			get
			{
				return _portStyle;
			}
			set
			{
				_portStyle = value;
				DrawPortStyle();
				
				if (Orientation == PortOrientation.Right) RotatePath(90);
				if (Orientation == PortOrientation.Bottom) RotatePath(180);
				if (Orientation == PortOrientation.Left) RotatePath(270);

				OnElementInvalid();
			}
		}

        public virtual bool Fixed
        {
            get
            {
                return _fixed;
            }
            set
            {
                _fixed = value;
            }
        }

		public virtual Direction Direction
		{
			get
			{
				return _direction;
			}
			set
			{
				_direction = value;
			}
		}

		public virtual UserInteraction Interaction
		{
			get
			{
				return _interaction;
			}
			set
			{
				_interaction = value;
			}
		}

		//The starting orientation of this port
		public virtual PortOrientation Orientation
		{
			get
			{
				return _orientation;
			}
			set
			{
				if (_orientation != value)
				{
					int rotation = GetPortRotation(value) - GetPortRotation(_orientation);
					RotatePath(rotation);
				
					_orientation = value;
				}
			}
		}

		//The starting percentage of this port
		public virtual float Percent
		{
			get
			{
				return _percent;
			}
			set
			{
				if (_percent != value)
				{
					_percent = value;
					if (Parent != null) Parent.LocatePort(this);
				}
			}
		}

		public virtual PointF Offset
		{
			get
			{
				return _offset;
			}
		}

		//Returns the port's parent (shape or line)
		public virtual IPortContainer Parent
		{
			get
			{
				return _parent;
			}
		}

		public virtual PortAlignment Alignment
		{
			get
			{
				return _alignment;
			}
			set
			{
				if (_alignment != value)
				{
					_alignment = value;
					SetOffset(CalculateOffset());
					OnElementInvalid();
				}
			}
		}

		//Indicates whether the port can be moved at runtime
		public virtual bool AllowMove
		{
			get
			{
				return _allowMove;
			}

			set
			{
				_allowMove = value;
			}
		}

		//Determines whether ports can be moved from one orientation to another by the user
		public virtual bool AllowRotate
		{
			get
			{
				return _allowRotate;
			}

			set
			{
				_allowRotate = value;
			}
		}

		//Indicates whether the selection path decoration is shown when the element is selected.
		public virtual bool DrawSelected
		{
			get
			{
				return _drawSelected;
			}
			set
			{
				if (_drawSelected != value)
				{
					_drawSelected = value;
					OnElementInvalid();
				}
			}
		}

		//Indicates whether or the shape is currently selected.
		public virtual bool Selected
		{
			get
			{
				return _selected;
			}
			set
			{
				if (_selected != value)
				{
					_selected = value;
					OnSelectedChanged();
					OnElementInvalid();
				}
			}
		}

        public virtual void SuspendValidation()
        {
            _suspendValidation = true;
        }

        public virtual void ResumeValidation()
        {
            _suspendValidation = false;
        }

		public virtual bool Validate
		{
			get
			{
				return _validate;
			}
			set
			{
				_validate = value;
			}
		}

		protected internal virtual void SetParent(IPortContainer parent)
		{
			_parent = parent;
		}
		
		protected internal virtual void SetOffset(PointF offset)
		{
			_offset = offset;
		}

		protected internal virtual void SetPercent(float percent)
		{
			_percent = percent;
		}
		
		//Raises the shape move event.
		protected virtual void OnLayoutChanged(System.Drawing.RectangleF rectangle)
		{
			if (LayoutChanged != null)  LayoutChanged(this, new LayoutChangedEventArgs(rectangle));
		}

		//Raises the element SelectedChanged event.
		protected virtual void OnSelectedChanged()
		{
			if (SelectedChanged != null) SelectedChanged(this,EventArgs.Empty);
		}

		public override object Clone()
		{
			return new Port(this);
		}

		public override float X
		{
			get
			{
				return Bounds.X; 
			}
			set
			{
                if (value == Location.X) return;
                Move(value - Location.X, 0);
			}
		}

		public override float Y
		{
			get
			{
				return Bounds.Y; 
			}
			set
			{
                if (value == Location.Y) return;
                Move(0, value - Location.Y);
			}
		}

		public override PointF Location
		{
			get
			{
				return Bounds.Location;
			}
			set
			{
                if (value.Equals(Location)) return;
                Move(value.X - Location.X, value.Y - Location.Y);
			}
		}

		//Moves the port along the path of the parent shape
		public override void Move(float dx, float dy)
		{
			if (dx == 0 && dy == 0) return;

			if (!Validate  || _suspendValidation || Parent.ValidatePortLocation(this,new PointF(Location.X + dx,Location.Y + dy)))
			{
				SetTransformRectangle(new PointF(TransformRectangle.X + dx, TransformRectangle.Y + dy));
				SetRectangle(new PointF(Bounds.X + dx, Bounds.Y + dy));

				if (Parent != null)
				{
					Orientation = Parent.GetPortOrientation(this,Location);
					SetOffset(CalculateOffset());
				}

				OnLayoutChanged(Bounds);
				OnElementInvalid();
			}
		}

		//Adjust the intercept for the port offset
		public override PointF Intercept(PointF location)
		{
			PointF intercept;
			
			//If default style then do normal intercept else return center
			if (Style == PortStyle.Default)
			{
				intercept = base.Intercept(new PointF(location.X - _offset.X,location.Y - _offset.Y));
			}
			else
			{
				intercept = Center;
			}

			//Offset depending on the port offset
			return new PointF(intercept.X + _offset.X,intercept.Y + _offset.Y);
		}

		
		//Adjust for offset and return
		public override bool Contains(PointF location)
		{
			location.X -= Offset.X;
			location.Y -= Offset.Y;
			
			//If default style then return default else just use rectangle
			if (Style == PortStyle.Simple)
			{
				return Bounds.Contains(location);
			}
			else
			{
				return base.Contains(location);				
			}
		}

		protected internal PointF CalculateOffset()
		{	
			float width = Bounds.Width;
			float height = Bounds.Height;
			float halfwidth = width / 2;
			float halfheight = height / 2;

			if (Alignment == PortAlignment.Center || Parent ==null) return new PointF(- halfwidth, - halfheight);

			//Return outset or inset values
			if (Alignment == PortAlignment.Outset)
			{
				if (_orientation == PortOrientation.Top) return new PointF(- halfwidth,-height);
				if (_orientation == PortOrientation.Bottom) return new PointF(- halfwidth,0);
				if (_orientation == PortOrientation.Left) return new PointF(-width,-halfheight);
				return new PointF(0,-halfheight);
			}
			else
			{
				if (_orientation == PortOrientation.Top) return new PointF(- halfwidth,0);
				if (_orientation == PortOrientation.Bottom) return new PointF(- halfwidth,-height);
				if (_orientation == PortOrientation.Left) return new PointF(0,-halfheight);
				return new PointF(-width,-halfheight);
			}
		}

		private int GetPortRotation(PortOrientation orientation)
		{
			if (orientation == PortOrientation.Right) return 90;
			if (orientation == PortOrientation.Bottom) return 180;
			if (orientation == PortOrientation.Left) return 270;
			return 0;
		}

		private void DrawPortStyle()
		{
			GraphicsPath path = new GraphicsPath();
			RectangleF inner = new RectangleF();
			
			//Default rectangle
			if (Style == PortStyle.Default)
			{
				path.AddRectangle(new Rectangle(0,0,10,10));
				inner = new RectangleF(1,1,8,8);
				SmoothingMode = SmoothingMode.None;
			}
				//Input
			else if (Style == PortStyle.Input)
			{
				path.AddLine(0,0,2,10);
				path.AddLine(2,10,10,10);
				path.AddLine(10,10,12,0);
				path.CloseFigure();
				inner = new RectangleF(1,1,8,8);
				SmoothingMode = SmoothingMode.HighQuality;
			}
				//Output
			else if (Style == PortStyle.Output)
			{
				path.AddLine(2,0,0,10);
				path.AddLine(0,10,12,10);
				path.AddLine(12,10,10,0);
				path.CloseFigure();
				inner = new RectangleF(1,1,8,8);
				SmoothingMode = SmoothingMode.HighQuality;
			}

				//Cross
			else if (Style == PortStyle.Simple)
			{
				path.AddLine(0,0,9,9);
				path.CloseFigure();
				path.AddLine(0,9,9,0);
				path.CloseFigure();
				SmoothingMode = SmoothingMode.None;
			}
			SetPath(path,inner);
		}
	}
}
