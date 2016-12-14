// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Runtime.Serialization;

using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	public class Origin
	{
		//Property variables
		private PointF _location;
		private Shape _shape;
		private Port _port;
        private Link _line;

		private MarkerBase _marker;
		private bool _docked;
		private bool _allowMove;

		private Link _parent;

		#region Interface

		public event EventHandler OriginInvalid;
		public event EventHandler DockChanged;

		//Constructors
		protected internal Origin()
		{
			AllowMove = true;
		}

		public Origin(PointF location)
		{	
			Location = location;
			AllowMove = true;
		}

		public Origin(Shape shape)
		{
			Shape = shape;
			AllowMove = true;
		}

		public Origin(Port port)
		{
			Port = port;
			AllowMove = true;
		}

        public Origin(Link line)
        {
            Line = line;
            AllowMove = true;
        }

		//Properties
		//Sets a location as the origin
		public virtual PointF Location
		{
			get
			{
				return _location;
			}
			set
			{
				if (! _location.Equals(value))
				{
					//Store if the dock has changed
					bool changed = (_shape != null || _port != null || _line != null);

					_location = value;
					_shape = null;
					_port = null;
					_docked = false;
                    _line = null;

					OnOriginInvalid();

					//Check to see if must fire dock changed event
					if (changed) OnDockChanged();
				}
			}
		}

		//Sets a shape as the origin
		public virtual Shape Shape
		{
			get
			{
				return _shape;
			}
			set
			{
				if (_shape != value)
				{
					_shape = value;
					_port = null;
					_location = new PointF();
					_docked = true;
                    _line = null;

					_shape.LayoutChanged += new LayoutChangedEventHandler(Element_LayoutChanged);

					OnOriginInvalid();
					OnDockChanged();
				}
			}
		}

		//Sets a port as the origin
		public virtual Port Port
		{
			get
			{
				return _port;
			}
			set
			{
				if (_port != value)
				{

					_port = value;
					_shape = null;
					_location = new PointF();
					_docked = true;
                    _line = null;

					_port.LayoutChanged += new LayoutChangedEventHandler(Element_LayoutChanged);

					OnOriginInvalid();
					OnDockChanged();
				}
			}
		}

        //Sets a shape as the origin
        public virtual Link Line
        {
            get
            {
                return _line;
            }
            set
            {
                if (_line != value)
                {
                    _line = value;
                    _shape = null;
                    _port = null;
                    _location = new PointF();
                    _docked = true;

                    OnOriginInvalid();
                    OnDockChanged();
                }
            }
        }
		
		//sets the marker at the start of the line
		public virtual MarkerBase Marker
		{
			get
			{
				return _marker;
			}
			set
			{
				if (_marker != null) _marker.ElementInvalid -=new EventHandler(Marker_ElementInvalid);

				_marker = value;
				if (value != null) _marker.ElementInvalid +=new EventHandler(Marker_ElementInvalid);
				
				OnOriginInvalid();
			}
		}

		//Determines whether the origin is docked to a shape or port
		public virtual bool Docked
		{
			get
			{
				return _docked;
			}
		}

		//Returns the current directly docked shape or port docked shape
		public virtual Element DockedElement
		{
			get
			{
				//Can be a line or a shape, or null
				if (Port != null) return Port.Parent as Element;
                if (Line != null) return Line;
				if (Shape != null) return Shape;

                return null;
			}
		}

		//Indicates whether the origin can be moved at runtime
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

		//Returns the parent line for this origin
		public virtual Link Parent
		{
			get
			{
				return _parent;
			}
		}

		//Methods
		public virtual void Move(float dx, float dy)
		{
			Location = new PointF(Location.X+dx,Location.Y+dy);	
		}

		public void SetParent(Link parent)
		{
			_parent = parent;
		}

		//Raises the OriginInvalid event
		protected virtual void OnOriginInvalid()
		{
			if (OriginInvalid != null) OriginInvalid(this,EventArgs.Empty);
		}

		//Raises the DockChanged event
		protected virtual void OnDockChanged()
		{
			if (DockChanged != null) DockChanged(this,EventArgs.Empty);
		}

		#endregion

		#region Events

		//Handles marker invalid events
		private void Marker_ElementInvalid(object sender, EventArgs e)
		{
			OnOriginInvalid();
		}

		private void Element_LayoutChanged(object sender, LayoutChangedEventArgs e)
		{
			OnOriginInvalid();
		}

		#endregion

	}
}
