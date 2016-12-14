// (c) Copyright Crainiate Software 2010




#define DEBUG

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using Crainiate.Diagramming.Collections;

namespace Crainiate.Diagramming
{
	public class Shape:Solid, ICloneable, ISelectable, IUserInteractive, IPortContainer
	{
		//Properties
		private bool _drawSelected;
		private bool _selected;
		private bool _keepAspect;

		private Ports _ports;
		
		//Events
		public event LayoutChangedEventHandler LayoutChanged;
		public event EventHandler SelectedChanged;

		//Constructors
		public Shape()
		{
			AllowMove = true;
			AllowScale = true;
			AllowRotate = false;
			DrawSelected = true;
			Direction = Direction.Both;
			Interaction = UserInteraction.BringToFront;

            MinimumSize = Singleton.Instance.DefaultMinimumSize;
            MaximumSize = Singleton.Instance.DefaultMaximumSize;

			Ports = new Ports(Model);
		}

		public Shape(StencilItem stencil)
		{
			AllowMove = true;
			AllowScale = true;
			AllowRotate = false;
			DrawSelected = true;
			Direction = Direction.Both;
			Interaction = UserInteraction.BringToFront;

			MinimumSize = new SizeF(32, 32);
			MaximumSize = new SizeF(320, 320);

			//Set up stencil
			StencilItem = stencil;
			stencil.CopyTo(this);

            Ports = new Ports(Model);
		}

		public Shape(Shape prototype): base(prototype)
		{
			AllowMove = prototype.AllowMove;
			AllowScale = prototype.AllowScale;
			AllowRotate = prototype.AllowRotate;
			_drawSelected = prototype.DrawSelected;
			Direction = prototype.Direction;
			Interaction = prototype.Interaction;
			
			MaximumSize = prototype.MaximumSize;
			MinimumSize = prototype.MinimumSize;
			_keepAspect = prototype.KeepAspect;

			//Copy ports
            Ports = new Ports(Model);
			foreach (Port port in prototype.Ports.Values)
			{
				Port clone = (Port) port.Clone();
				Ports.Add(port.Key,clone);
				
				clone.SuspendValidation();
				clone.Location = port.Location;
				clone.ResumeValidation();
			}
		}

		//Properties
		//Determines whether this shape can be moved by a diagram move action.
		public virtual bool AllowMove {get; set;}

        //Determines whether this shape can be moved by a diagram move action.
        public virtual bool AllowSnap {get; set;}

		//Determines whether this shape can be scaled by a diagram size action.
		public virtual bool AllowScale {get; set;}

		//Determines whether this shape can be scaled by a diagram size action.
		public virtual bool AllowRotate {get; set;}

		public virtual Direction Direction {get; set;}

		public virtual UserInteraction Interaction {get; set;}

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
                    if (_selected) CreateHandles();
                    if (Group != null) Group.Selected = value;

					OnSelectedChanged();
					OnElementInvalid();
				}
			}
		}

		//Determines if the shape maintains it's ratio of width to height when sized.
		public virtual bool KeepAspect
		{
			get
			{
				return _keepAspect;
			}
			set
			{
				if (value != _keepAspect)
				{
					_keepAspect = value;
					CreateHandles();
				}
			}
		}

		//Determines the minimum width and height the shape can be resized to through the diagram interface.
		public virtual SizeF MinimumSize  {get; set;}

		//Determines the maximum width and height the shape can be resized to.
		public virtual SizeF MaximumSize {get; set;}

		//Gets or sets the size of the shape.
		//Doesnt do equality check becuase min or max size may have changed
		public virtual SizeF Size
		{
			get
			{
				return base.Size;
			}
			set
			{
				//Rotate ports to zero for scaling
				if (Rotation != 0) RotatePorts(-Rotation);			

				//Use old rectangle size to calculate port offset from new size
				SizeF existing = Size;
				SizeF size = ValidateSize(value.Width, value.Height); 
				
				base.Size = size;

				ScalePorts(size.Width / existing.Width, size.Height / existing.Height, 0 ,0);
                CreateHandles();

				OnLayoutChanged(Bounds);

				//Rotate ports back to original location
				if (Rotation != 0) RotatePorts(Rotation);
			}
		}

		public virtual Ports Ports
		{
			get
			{
				return _ports;
			}
			set
			{
				//Reset handlers
				if (_ports != null)
				{
					_ports.InsertElement -=new ElementsEventHandler(Ports_InsertElement);
					
					foreach (Port port in _ports.Values)
					{
						port.ElementInvalid -=new EventHandler(Port_ElementInvalid);
					}
				}

				if (value == null) 
				{
                    _ports = new Ports(Model);
				}
				else
				{					
					_ports = value;
					_ports.InsertElement +=new ElementsEventHandler(Ports_InsertElement);
					
					//Set the back references for the ports
					foreach (Port port in _ports.Values)
					{
						port.SetParent(this);
						port.ElementInvalid +=new EventHandler(Port_ElementInvalid);
					}
				}
				OnElementInvalid();
			}
		}

		//Methods

		//Scales and moves a shape by the new supplied ratios and changes.
		public virtual void Scale(float scaleX, float scaleY, float dx, float dy, bool maintainAspect)
		{
			//Rotate ports to zero for scaling
			if (Rotation != 0) RotatePorts(-Rotation);

			ScaleShape(scaleX, scaleY, dx, dy, maintainAspect);
			OnLayoutChanged(Bounds);

			//Rotate ports to zero for scaling
			if (Rotation != 0) RotatePorts(Rotation);
		}

		//Rotates the shape clockwise by the specified number in degrees.
		public virtual void Rotate(float degrees)
		{
			//Call the property so that the correct code is run
			Rotation += degrees;
		}
		
		//Raises the layout changed event.
		protected virtual void OnLayoutChanged(RectangleF rect)
		{
			if (LayoutChanged != null) LayoutChanged(this, new LayoutChangedEventArgs(rect));
		}

		//Raises the element SelectedChanged event.
		protected virtual void OnSelectedChanged()
		{
			if (SelectedChanged != null) SelectedChanged(this,EventArgs.Empty);
		}

		public override object Clone()
		{
			return new Shape(this);
		}

        public override void SetLayer(Layer layer)
        {
            base.SetLayer(layer);

            //Set the back references for the ports
            foreach (Port port in _ports.Values)
            {
                port.SetLayer(layer);
            }
        }

        public override void SetModel(Model model)
        {
            base.SetModel(model);

            //Set the back references for the ports
            foreach (Port port in _ports.Values)
            {
                port.SetModel(model);
            }
        }

		//Change the x co ordinate of the shape's location
		public override float X
		{
			get
			{
				return Bounds.X;
			}
			set
			{
				if (Bounds.X != value)
				{
					//Move ports by change in x and y
					foreach(Port port in Ports.Values)
					{
						port.SuspendValidation();
						port.Move(value - Bounds.X,0);
						port.ResumeValidation();
					}

					//Adjust the location rectangle and transform rectangle
					SetTransformRectangle(new PointF(TransformRectangle.X + (value - Bounds.X), TransformRectangle.Y));
					SetRectangle(new PointF(value,Bounds.Y));

                    OnLayoutChanged(Bounds);
					OnElementInvalid();
				}
			}
		}

		//Change the y co ordinate of the shape's location
		public override float Y
		{
			get
			{
				return base.Y;
			}
			set
			{
				if (Bounds.Y != value)
				{
					//Move ports by change in x and y
					foreach(Port port in Ports.Values)
					{
						port.SuspendValidation();
						port.Move(0,value - Bounds.Y);
						port.ResumeValidation();
					}

					//Adjust the location rectangle and transform rectangle
					SetTransformRectangle(new PointF(TransformRectangle.X, TransformRectangle.Y + (value - Bounds.Y)));
					SetRectangle(new PointF(Bounds.X,value));

					//Raise the appropriate events
					OnLayoutChanged(Bounds);
					OnElementInvalid();
				}
			}
		}

		//Changes the location by using the move method
		public override PointF Location
		{
			get
			{
				return base.Location;
			}
			set
			{
				if (!Bounds.Location.Equals(value))
				{
					//Store values before updating underlying rectangle
					float dx = value.X - Bounds.X;
					float dy = value.Y - Bounds.Y;
					
					//Update control rectangle
					SetTransformRectangle(new PointF(TransformRectangle.X + dx, TransformRectangle.Y + dy));
					SetRectangle(value);

					//Move ports by change in x and y
					if (Ports != null)
					{
						foreach(Port port in Ports.Values)
						{
							port.SuspendValidation();
							port.Move(dx,dy);
							port.ResumeValidation();
						}
					}

					OnLayoutChanged(Bounds);
					OnElementInvalid();
				}
			}
		}
		
		//Moves the shape and the ports
		public override void Move(float dx, float dy)
		{
			//Move each of the ports
			foreach(Port port in Ports.Values)
			{
				port.SuspendValidation();
				port.Move(dx, dy);
				port.ResumeValidation();
			}

			//Update the shape rectangle
			SetTransformRectangle(new PointF(TransformRectangle.X + dx, TransformRectangle.Y + dy));
			SetRectangle(new PointF(Bounds.X + dx, Bounds.Y + dy));

			//Raise the appropriate events
			OnLayoutChanged(Bounds);
			OnElementInvalid();
		}

		//Gets or sets the Width of the shape.
		public override float Width
		{
			get
			{
				return Bounds.Width;
			}
			set
			{
				//Rotate ports to zero for scaling
				if (Rotation != 0) RotatePorts(-Rotation);

				//Use the old values to validate size and scale ports
				float existing = Width;
				float width = ValidateSize(value, Bounds.Height).Width;
				
				base.Width = width;

				ScalePorts(width / existing, 1, 0 ,0);
                CreateHandles();

				OnLayoutChanged(Bounds);

				//Rotate ports to zero for scaling
				if (Rotation != 0) RotatePorts(Rotation);
			}
		}

		//Gets or sets the Height of the shape.
		public override float Height
		{
			get
			{
				return Bounds.Height;
			}
			set
			{
				//Rotate ports to zero for scaling
				if (Rotation != 0) RotatePorts(-Rotation);

				//Validate the new height
				float existing = Height;
				float height = ValidateSize(Bounds.Width, value).Height;
				
				base.Height = height;

				ScalePorts(1, height / existing, 0 ,0);
                CreateHandles();
				
				OnLayoutChanged(Bounds);

				//Rotate ports to zero for scaling
				if (Rotation != 0) RotatePorts(Rotation);
			}
		}

		//Raises the roation changed event when the rotation is changed
		public override float Rotation
		{
			get
			{
				return base.Rotation;
			}
			set
			{
				if (base.Rotation != value)
				{
					//Get the change in rotation
					float dr = value - Rotation;

					base.Rotation = value;
					RotatePorts(dr);

					OnLayoutChanged(Bounds);
				}
			}
		}

		//Returns the type of cursor from this point
		public override Handle Handle(PointF location)
		{
			return GetShapeHandle(location);
		}

		//Create a list of handles 
		protected internal override void CreateHandles()
		{
			if (Model == null) return;
			SetHandles(new Handles());

			//Get the default graphics path and scale it
			//Render render = RenderFromContainer();
			GraphicsPath defaultPath = (GraphicsPath) Singleton.Instance.DefaultHandlePath.Clone();
			Matrix matrix = new Matrix();
			//matrix.Scale(render.ZoomFactor,render.ZoomFactor);
			//defaultPath.Transform(matrix);

			RectangleF pathRectangle = defaultPath.GetBounds();
			RectangleF halfRectangle = new RectangleF(0,0,pathRectangle.Width /2, pathRectangle.Height /2);
            float onePixel = 1; //* render.ZoomFactor;

			//Add top left
			GraphicsPath path = (GraphicsPath) defaultPath.Clone();
			matrix.Reset();
			matrix.Translate(-pathRectangle.Width - onePixel,-pathRectangle.Height - onePixel);
			path.Transform(matrix);
			Handles.Add(new Handle(path, HandleType.TopLeft));

			//Add top right
			path = (GraphicsPath) defaultPath.Clone();
			matrix.Reset();
			matrix.Translate(TransformRectangle.Width, -pathRectangle.Height - onePixel);
			path.Transform(matrix);
			Handles.Add(new Handle(path,HandleType.TopRight));

			//Add bottom left
			path = (GraphicsPath) defaultPath.Clone();
			matrix.Reset();
			matrix.Translate(- pathRectangle.Width - onePixel, TransformRectangle.Height + onePixel);
			path.Transform(matrix);
			Handles.Add(new Handle(path,HandleType.BottomLeft));

			//Add bottom right
			path = (GraphicsPath) defaultPath.Clone();
			matrix.Reset();
			matrix.Translate(TransformRectangle.Width + onePixel, TransformRectangle.Height + onePixel);
			path.Transform(matrix);
			Handles.Add(new Handle(path,HandleType.BottomRight));

			if (!KeepAspect)
			{
				//Add top middle
				path = (GraphicsPath) defaultPath.Clone();
				matrix.Reset();
				matrix.Translate((TransformRectangle.Width / 2) - halfRectangle.Width, -pathRectangle.Height - onePixel);
				path.Transform(matrix);
				Handles.Add(new Handle(path,HandleType.Top));

				//Add left
				path = (GraphicsPath) defaultPath.Clone();
				matrix.Reset();
				matrix.Translate(- pathRectangle.Width  -onePixel , (TransformRectangle.Height / 2) - halfRectangle.Height);
				path.Transform(matrix);
				Handles.Add(new Handle(path,HandleType.Left));

				//Add right
				path = (GraphicsPath) defaultPath.Clone();
				matrix.Reset();
				matrix.Translate(TransformRectangle.Width + onePixel, (TransformRectangle.Height / 2) - halfRectangle.Height);
				path.Transform(matrix);
				Handles.Add(new Handle(path,HandleType.Right));

				//Add bottom
				path = (GraphicsPath) defaultPath.Clone();
				matrix.Reset();
				matrix.Translate((TransformRectangle.Width / 2) - halfRectangle.Width, TransformRectangle.Height + onePixel);
				path.Transform(matrix);
				Handles.Add(new Handle(path,HandleType.Bottom));
			}

			//Add rotation handle
			if (AllowRotate)
			{
				//Add top middle
				path = (GraphicsPath) defaultPath.Clone();
				matrix.Reset();
				matrix.Translate((TransformRectangle.Width / 2) - halfRectangle.Width, (TransformRectangle.Height / 2) - halfRectangle.Height);
				path.Transform(matrix);
				Handles.Add(new Handle(path,HandleType.Rotate));
			}
		}

		protected SizeF ValidateSize(float width, float height)
		{
			if (!KeepAspect || width > height)
			{
				if (width < MinimumSize.Width) width = MinimumSize.Width;
				if (width > MaximumSize.Width) width = MaximumSize.Width;
			}
			
			if (!KeepAspect || height > width)
			{
				if (height < MinimumSize.Height) height = MinimumSize.Height;
				if (height > MaximumSize.Height) height = MaximumSize.Height;
			}
			
			return new SizeF(width,height);
		}

		public override bool Contains(PointF location)
		{
			return ShapeContains(location);
		}
		
		private void Ports_InsertElement(object sender, ElementsEventArgs e)
		{
			//Sets the shape of the port
			Port port = (Port) e.Value;
			port.SetParent(this);
			port.SetLayer(Layer);
			port.ElementInvalid +=new EventHandler(Port_ElementInvalid);
			port.SetModel(Model);
			port.SetOrder(_ports.Count -1);

			//If not deserializing then locate port
			if (port.Location.IsEmpty) 
			{
				LocatePort(port);
			}
			//Else just set orientation and offset
			else
			{
				port.Orientation = GetPortOrientation(port, port.Location);
				port.SetOffset(port.CalculateOffset());
			}
		}

		//Occurs when a port becomes invalid
		private void Port_ElementInvalid(object sender, EventArgs e)
		{
			OnElementInvalid();
		}
	
		//Performs hit testing for an element from a location
		//if a valid diagram provided, hit testing is performed using current transform
		private bool ShapeContains(PointF location)
		{
			//Inflate rectangle to include selection handles
			RectangleF bound = TransformRectangle;

            float handle = 6;
			bound.Inflate(handle,handle);

			//If inside inflate boundary
			if (bound.Contains(location))
			{
				//Return true if clicked in selection rectangle but not path rectangle
				if (Selected && !TransformRectangle.Contains(location)) return true;

				//Check the outline offset to the path (0,0)
				location.X -= Bounds.X;
				location.Y -= Bounds.Y;

			
				if (DrawBackground)
				{
					if (TransformPath.IsVisible(location)) return true;
				}
				else
				{
					//Get bounding rect
					float width = BorderWidth + 2;

					if (TransformPath.IsOutlineVisible(location,new Pen(Color.Black, width))) return true;
				}
			}
			
			return false;
		}

		//Scale a shape using ratios
		private void ScaleShape(float sx, float sy, float dx, float dy, bool maintainAspect)
		{
			//Check min and max sizes
			if (!KeepAspect || Width >= Height)
			{
				if (MaximumSize.Width < (Bounds.Width * sx)) sx = Convert.ToSingle(MaximumSize.Width / Bounds.Width);
				if (MinimumSize.Width > (Bounds.Width * sx)) sx = Convert.ToSingle(MinimumSize.Width / Bounds.Width);
			}
			
			if (!KeepAspect || Height >= Width)
			{
				if (MaximumSize.Height < (Bounds.Height * sy)) sy = Convert.ToSingle(MaximumSize.Height / Bounds.Height);
				if (MinimumSize.Height > (Bounds.Height * sy)) sy = Convert.ToSingle(MinimumSize.Height / Bounds.Height);
			}

			if (maintainAspect)
			{
				if (sx < sy)
				{
					sy = sx;
				}
				else
				{
					sx = sy;
				}
			}

			//Get the new size
			float width = Width * sx;
			float height = Height * sy;

			if (StencilItem != null && StencilItem.Redraw)
			{
				SetPath(StencilItem.Draw(width,height),StencilItem.InternalRectangle(width,height));
				SetRectangle(new PointF(Location.X + dx,Location.Y + dy));
			}
			else
			{
				if (StencilItem == null)
				{
					RectangleF original = InternalRectangle;
					RectangleF rect = new RectangleF(original.X * sx,original.Y * sy,original.Width * sx,original.Height * sy);
					ScalePath(sx,sy,dx,dy,rect);
				}
				else
				{
					ScalePath(sx,sy,dx,dy,StencilItem.InternalRectangle(width,height));
				}
			}
			
			//Offset the ports before the new rectangle is updated
			ScalePorts(sx,sy,dx,dy);
            CreateHandles();
		}

		private void ScalePorts(float sx, float sy, float dx, float dy)
		{
			//Change positions of ports
			SizeF offset;

			if (Ports != null)
			{
				foreach (Port port in Ports.Values)
				{
                    if (!port.Fixed)
                    {
                        Matrix matrix = new Matrix();

                        //Set the origin to the rectangle location
                        matrix.Translate(Bounds.X, Bounds.Y);

                        //Scale the matrix so that the offset is updated
                        matrix.Scale(sx, sy);

                        //Offset the port using the difference between the port and the rectangle
                        matrix.Translate(port.Location.X - Bounds.X, port.Location.Y - Bounds.Y);

                        //Reset the scale
                        matrix.Scale(1 / sx, 1 / sy);

                        //Once the matrix transforms have been reset, move by the dx and dy values
                        matrix.Translate(dx, dy);

                        port.SuspendValidation();
                        port.Location = new PointF(matrix.OffsetX, matrix.OffsetY);
                        port.ResumeValidation();
                    }
				}
			}
		}

		private void RotatePorts(float degrees)
		{
			if (Ports != null)
			{
				PointF rotateat = new PointF(Center.X - Bounds.X, Center.Y - Bounds.Y);
				
				foreach (Port port in Ports.Values)
				{
					Matrix matrix = new Matrix();

					//Set the origin to the rectangle location
					matrix.Translate(Bounds.X, Bounds.Y);

					//Rotate around the center 
					matrix.RotateAt(degrees, rotateat);

					//Offset the port using the difference between the port and the rectangle
					matrix.Translate(port.Location.X - Bounds.X, port.Location.Y - Bounds.Y);
		
					port.SuspendValidation();
					port.Location = new PointF(matrix.OffsetX, matrix.OffsetY);			
					port.Rotation = Rotation;
					port.ResumeValidation();
				}
			}
		}

		//Locate a port based on the orientation(side) of the parent and the percentage
		public virtual void LocatePort(Port port)
		{
			RectangleF shapeRect = TransformRectangle;
			PointF start = new PointF();
			float ratio = port.Percent / 100;

			switch (port.Orientation)
			{
				case PortOrientation.Top:
					start = new PointF(shapeRect.X +(shapeRect.Width * ratio),shapeRect.Y-1);
					break;
				case PortOrientation.Bottom:
					start = new PointF(shapeRect.X +(shapeRect.Width * ratio),shapeRect.Y+shapeRect.Height+1);
					break;
				case PortOrientation.Left:
					start = new PointF(shapeRect.X-1,shapeRect.Y +(shapeRect.Height * ratio));
					break;
				case PortOrientation.Right:
					start = new PointF(shapeRect.X + shapeRect.Width +1,shapeRect.Y +(shapeRect.Height * ratio));
					break;
				default:
					break;
			}
			
			port.Validate = false;
			port.Location = Intercept(start);
			port.Validate = true;
		}

		public virtual PortOrientation GetPortOrientation(Port port,PointF location)
		{
			return Geometry.GetOrientation(location,Center,Bounds);
		}

		public virtual float GetPortPercentage(Port port,PointF location)
		{
			float ratio = 0;

			if (port.Orientation == PortOrientation.Top || port.Orientation == PortOrientation.Bottom)
			{
				ratio = (location.X-Bounds.X) / (Bounds.Right - Bounds.Left);
			}
			else
			{
				ratio = (location.Y-Bounds.Y) / (Bounds.Bottom - Bounds.Top);
			}

			return Convert.ToSingle(Math.Round(ratio * 100,1));
		}

		//Takes the port and validates its location against the shape's path
		public bool ValidatePortLocation(Port port,PointF location)
		{
			//Check for switch changes
			if (!port.AllowRotate)
			{
				PortOrientation orientation = Geometry.GetOrientation(location,Center,Bounds);
				if (port.Orientation != orientation) return false;
			}

			//Offset location to local co-ordinates and check outline
			location.X -= Bounds.X;
			location.Y -= Bounds.Y;

			return TransformPath.IsOutlineVisible(location,new Pen(Color.Black,5));
		}

        //Opposite of intercept. Places a point in a logical location outside of the shape's path
        public PointF Forward(PointF location)
        {
            //Cache the location properties
            float x = X;
            float y = Y;
            
            //Get the center of the shape offset to the path origin
            PointF center = Center;
            center = new PointF(center.X - x, center.Y - y);

            //Create transform location moved to the path origin and check if inside path
            PointF transform = new PointF(location.X - x, location.Y - y);
            
            //Get the angle between the center and the location
            double angle = Geometry.GetAngle(center, transform);
            float length = Width > Height ? Width : Height;

            location = Geometry.SetAngle(transform, angle, length);
            location = new PointF(location.X + x, location.Y + y);

            return location;
        }

		//Gets the cursor from the diagram point
		private Handle GetShapeHandle(PointF location)
		{
			if (!Selected || Handles == null) return Singleton.Instance.DefaultHandle;

			//Offset location to local co-ordinates
			location = new PointF(location.X - TransformRectangle.X, location.Y - TransformRectangle.Y);

			//Check each handle
			foreach (Handle handle in Handles)
			{
				if (handle.Path.IsVisible(location)) return handle;
			}

			return Singleton.Instance.DefaultHandle;
		}
    }
}
