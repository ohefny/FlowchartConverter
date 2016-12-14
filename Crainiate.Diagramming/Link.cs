// (c) Copyright Crainiate Software 2010



#define DEBUG

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using Crainiate.Diagramming.Collections;

namespace Crainiate.Diagramming
{
	[Serializable]
	public class Link: Line, IPortContainer, ICloneable
	{
		//Property variables
		private LineJoin _lineJoin;
		private bool _allowMove;

		private Origin _startOrigin;
		private Origin _endOrigin;
		
		private bool _drawSelected;
		private bool _selected;
		private UserInteraction _interaction;
        private Ports _ports;

        private Label _label;
        private Image _image;

		//Working variables
		private List<PointF> _points;

		//Events
		public event EventHandler SelectedChanged;
		public event EventHandler OriginInvalid;

		//Constructors
		public Link(): base()
		{
			Start = new Origin();
			End = new Origin();
            Ports = new Ports(Model);
		}

        public Link(Origin start, Origin end): base()
        {
            Start = start;
            End = end;
            Ports = new Ports(Model);
        }
		
		public Link(PointF start, PointF end): base()
		{
			Start = new Origin(start);
			End = new Origin(end);
			Ports = new Ports(Model);
		}

		public Link(Shape start,Shape end):base()
		{
			Start = new Origin(start);
			End = new Origin(end);
			Ports = new Ports(Model);
		}

		public Link(Port start,Port end):base()
		{
			Start = new Origin(start);
			End = new Origin(end);
			Ports = new Ports(Model);
		}

		public Link(Link prototype)
		{
			AllowMove = prototype.AllowMove;
			LineJoin = prototype.LineJoin;
			DrawSelected = prototype.DrawSelected;
			Interaction = prototype.Interaction;
			
			//Set up new origins
			Start = new Origin(prototype.FirstPoint);
			End = new Origin(prototype.LastPoint);

			Start.Marker = prototype.Start.Marker;
			End.Marker = prototype.End.Marker;

			List<PointF> points = new List<PointF>();
            points.AddRange(prototype.Points);
            SetPoints(points);

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

            if (prototype.Label != null) Label = (Label) prototype.Label.Clone();
            if (prototype.Image != null) Image = (Image) prototype.Image.Clone();

			DrawPath();
		}

		//Properties

		//Sets the start location
		public virtual Origin Start
		{
			get
			{
				return _startOrigin;				
			}
			set
			{
                if (value == null) throw new ArgumentNullException();

				_startOrigin = value;

                if (_startOrigin != null)
                {
                    _startOrigin.OriginInvalid += new EventHandler(Origin_OriginInvalid);
                    _startOrigin.SetParent(this);
                }

				//Draw the path and relocate the ports
				DrawPath();
				LocatePorts();
                CreateHandles();

				OnElementInvalid();
			}
		}

		//Sets the End location
		public virtual Origin End
		{
			get
			{
				return _endOrigin;				
			}
			set
			{
                if (value == null) throw new ArgumentNullException();

    			_endOrigin = value;

                if (_endOrigin != null)
                {
                    _endOrigin.OriginInvalid += new EventHandler(Origin_OriginInvalid);
                    _endOrigin.SetParent(this);
                }

				//Redraw the internal path;
				DrawPath();
				LocatePorts();
                CreateHandles();

				OnElementInvalid();
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
                if (value == null) throw new ArgumentNullException("Ports");
				
				_ports = value;
				_ports.InsertElement +=new ElementsEventHandler(Ports_InsertElement);
				
				//Set the back references for the ports
				foreach (Port port in _ports.Values)
				{
					port.SetParent(this);
					port.ElementInvalid +=new EventHandler(Port_ElementInvalid);
				}
				
				OnElementInvalid();
			}
		}

        //Returns the label for this link
        public virtual Label Label
        {
            get
            {
                return _label;
            }
            set
            {
                //Remove any existing handlers
                _label = value;
                if (_label != null)
                {
                    _label.LabelInvalid += new EventHandler(Label_LabelInvalid);
                    _label.SetParent(this);
                }
                OnElementInvalid();
            }
        }

        //Returns the Image object which which displays an image for this link
        public virtual Image Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                if (_image != null)
                {
                    _image.ImageInvalid += new EventHandler(Image_ImageInvalid);
                }
                OnElementInvalid();
            }
        }

		//Methods
		public virtual Origin OriginFromLocation(PointF location)
		{
			return GetOriginFromLocation(location);
		}

		//Raises the OriginInvalid event
		protected virtual void OnOriginInvalid(object sender)
		{
			if (OriginInvalid != null) OriginInvalid(sender,EventArgs.Empty);
		}

        //Overrides
		public override object Clone()
		{
			return new Link(this);
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

        public override void ApplyTheme(Theme theme)
        {
            if (Start.Marker != null)
            {
                Start.Marker.BorderColor = theme.BorderColor;
                Start.Marker.BackColor = theme.BackColor;
            }

            if (End.Marker != null)
            {
                End.Marker.BorderColor = theme.BorderColor;
                End.Marker.BackColor = theme.BackColor;
            }

            base.ApplyTheme(theme);
        }

        public override bool Contains(PointF location)
        {
            return LineContains(location);
        }

		//Create a list of handles 
		protected internal override void CreateHandles()
		{
			if (Model == null) return;
            if (Points == null) return;
			SetHandles(new Handles());

			//Get the default graphics path and scale it
			//Render render = RenderFromContainer();
			GraphicsPath defaultPath = (GraphicsPath) Singleton.Instance.DefaultHandlePath.Clone();
			Matrix matrix = new Matrix();
			//matrix.Scale(render.ZoomFactor,render.ZoomFactor);
			//defaultPath.Transform(matrix);
			RectangleF pathRectangle = defaultPath.GetBounds();
			RectangleF halfRectangle = new RectangleF(0,0,pathRectangle.Width /2, pathRectangle.Height /2);

			//Loop through each point and add an offset handle
			GraphicsPath path;

			PointF point = (PointF) Points[0];
			path = (GraphicsPath) defaultPath.Clone();
			matrix.Reset();
			matrix.Translate(point.X - Bounds.X - halfRectangle.Width,point.Y - Bounds.Y - halfRectangle.Height);
			path.Transform(matrix);
			Handles.Add(new Handle(path,HandleType.Origin, true));

			point = (PointF) Points[Points.Count-1];
			path = (GraphicsPath) defaultPath.Clone();
			matrix.Reset();
			matrix.Translate(point.X - Bounds.X - halfRectangle.Width,point.Y - Bounds.Y - halfRectangle.Height);
			path.Transform(matrix);
			Handles.Add(new Handle(path,HandleType.Origin, true));
		}
		
		//Handles invalid origin events
		private void Origin_OriginInvalid(object sender, EventArgs e)
		{
			OnOriginInvalid(sender);
			
			CheckSameOrigin(sender);
			DrawPath();
			LocatePorts();
            CreateHandles();

			OnElementInvalid();
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

			//Locate if not deserializing
			if (port.Location.IsEmpty) LocatePort(port);
		}

		//Occurs when a port becomes invalid
		private void Port_ElementInvalid(object sender, EventArgs e)
		{
			OnElementInvalid();
		}

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

		//Code to draw path for the line
		public override void DrawPath()
		{
			if (Model == null) return; //Reserializing
			if (Start == null || End == null) return; //Cloning line

			//Get the start and end location depending on start shapes etc
			PointF startLocation = GetOriginLocation(Start,End);
			PointF endLocation = GetOriginLocation(End,Start);

			//Add the points to the solution
            List<PointF> points = new List<PointF>();
			points.Add(startLocation);
			points.Add(endLocation);
			SetPoints(points);

			//Draw the path
			GraphicsPath path = new GraphicsPath();
			path.AddLine(startLocation,endLocation);
			
			//Calculate path rectangle
			RectangleF rect = path.GetBounds();
			SetRectangle(rect); //Sets the location rectangle
			SetPath(path); //setpath moves the line to 0,0
		}

		//Returns the end point of a line depending on the starting shape and location
		internal PointF GetOriginLocation(Origin target,Origin source)
		{
            //Just return the location if not docked
			if (target.DockedElement == null) return target.Location;

			//Set up location from source location, shape or marker
			Element targetElement;
			PointF sourceLocation;

			//Set up the target element (shape, port or line)
			targetElement = target.DockedElement;
			sourceLocation = GetSourceLocation(source);
            
			//Get the intercept
			PointF result = targetElement.Intercept(sourceLocation);

			//Readjust the location for different containers
			return result;
		}

		//Returns the location if not docked, or the center if docked
		internal PointF GetSourceLocation(Origin source)
		{
			//Set up the source location (point or element center)
			if (source.DockedElement == null) return source.Location;

			return source.DockedElement.Center;
		}

        //Determines whether a line and markers contains the supplied point
        private bool LineContains(PointF location)
        {
            if (base.Contains(location)) return true;

            //Check markers
            if (_points != null && Bounds.Contains(location))
            {
                //Translate the location to line co-ordinates
                PointF startLocation = (PointF)_points[0];
                PointF startReference = (PointF)_points[1];
                PointF endLocation = (PointF)_points[_points.Count - 1];
                PointF endReference = (PointF)_points[_points.Count - 2];

                if (MarkerContains(Start.Marker, startLocation, startReference, location)) return true;
                if (MarkerContains(End.Marker, endLocation, endReference, location)) return true;
            }

            return false;
        }

        //Determines if a marker contains the supplied point
        private bool MarkerContains(MarkerBase marker, PointF markerPoint, PointF referencePoint, PointF location)
        {
            if (marker == null) return false;

            //Create a new matrix at the line location
            Matrix matrix = new Matrix();
            matrix.Translate(Bounds.X, Bounds.Y);

            GraphicsPath path = marker.GetPath();
            path.Transform(GetMarkerTransform(marker, markerPoint, referencePoint, matrix));

            return path.IsVisible(location);
        }

		private Origin GetOriginFromLocation(PointF location)
		{
			location = new PointF(location.X - Bounds.X,location.Y - Bounds.Y);
			
			if (Handles == null) CreateHandles();
			if (Handles[0].Path.IsVisible(location)) return Start;
			if (Handles[Handles.Count-1].Path.IsVisible(location)) return End;
			
			return null;
		}

		//Locate a port based on the percentage
		public virtual void LocatePort(Port port)
		{
			if (_points == null) return;

			PointF startPoint = (PointF) _points[0];
			PointF endPoint = (PointF) _points[_points.Count-1];
			PointF result = new PointF();

			float ratio = 100 / port.Percent;

			float dx = (endPoint.X - startPoint.X) / ratio;
			float dy = (endPoint.Y - startPoint.Y) / ratio;

			result.X = startPoint.X + dx;
			result.Y = startPoint.Y + dy;

			port.Validate = false;
			port.Location = result;
			port.Validate = true;
		}

		//Always face up for now
		public virtual PortOrientation GetPortOrientation(Port port,PointF location)
		{
			return PortOrientation.None;
		}

		//Takes the port and validates its location against the shape's path
		public bool ValidatePortLocation(Port port,PointF location)
		{
			//Offset location to local co-ordinates and check outline
			location.X -= Bounds.X;
			location.Y -= Bounds.Y;

			return GetPath().IsOutlineVisible(location,new Pen(Color.Black,1));
		}

		public virtual float GetPortPercentage(Port port,PointF location)
		{
			GetPortPercentages();
			return port.Percent;
		}

        public PointF Forward(PointF location)
        {
            return location;
        }

        public virtual PointF GetLabelLocation()
        {
            //Determine central point
            //If even number of points, then position half way on segment
            PointF center;

            PointF start = (PointF)Points[0];
            PointF end = (PointF)Points[1];
            center = new PointF(start.X + ((end.X - start.X) / 2), start.Y + ((end.Y - start.Y) / 2));

            //Offset to line co-ordinates
            return new PointF(center.X - Bounds.X, center.Y - Bounds.Y);
        }

        public virtual SizeF GetLabelSize()
        {
            Graphics graphics = Singleton.Instance.CreateGraphics();
            return graphics.MeasureString(Label.Text, Label.Font);
        }

		//Loop through and calculate the port percentage
		internal virtual void GetPortPercentages()
		{
			if (_points == null) return;
			if (_ports == null) return;

			PointF startPoint = (PointF) _points[0];
			PointF endPoint = (PointF) _points[_points.Count-1];
			float dx = (endPoint.X - startPoint.X);
			float dy = (endPoint.Y - startPoint.Y);
			
			float percent;

			foreach (Port port in Ports.Values)
			{
				percent = 0;

				//can use either x or y, unless x are equal
				if (startPoint.X != endPoint.X)
				{
					percent = 100 / (dx / (port.X - startPoint.X));
				}
				else if (startPoint.Y != endPoint.Y)
				{
					percent = 100 / (dy / (port.Y - startPoint.Y));
				}

				port.SetPercent(percent);
			}
		}

		//reposition the port based on the port percentage
		internal void LocatePorts()
		{
			if (Ports == null) return;

			foreach (Port port in Ports.Values)
			{
				LocatePort(port);
			}
		}

		private void CheckSameOrigin(object sender)
		{
			Origin origin = (Origin) sender;

			//Check for same shapes
			if (origin.Shape != null && End.Shape != null && End.Shape == Start.Shape)
			{
				//If the recent update was the start then undock the end, else the start
				if (origin == Start)
				{
					End.Location = GetOriginLocation(End,Start);
				}
				else
				{
					Start.Location = GetOriginLocation(Start,End);						
				}

			}
		}
	}
}