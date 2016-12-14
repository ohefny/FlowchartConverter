// (c) Copyright Crainiate Software 2010




using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

using Crainiate.Diagramming.Collections;
using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	public class ComplexLine: Link, ICloneable
	{
		//Property variables
		private Segments _segments;
		private bool _allowExpand;

		#region Interface

		//Constructors
		public ComplexLine(): base()
		{
			_segments = new Segments();
			Segment segment = new Segment(Start,End);
			Segments.Add(segment);
			segment.SegmentInvalid += new EventHandler(segment_SegmentInvalid);

			AllowExpand = true;
		}

		public ComplexLine(PointF start,PointF end): base(start,end)
		{
			_segments = new Segments();
			Segment segment = new Segment(Start,End);
			Segments.Add(segment);
			segment.SegmentInvalid += new EventHandler(segment_SegmentInvalid);

			AllowExpand = true;
		}

		public ComplexLine(Shape start,Shape end): base(start,end)
		{
			_segments = new Segments();
			Segment segment = new Segment(Start,End);
			Segments.Add(segment);
			segment.SegmentInvalid += new EventHandler(segment_SegmentInvalid);

			AllowExpand = true;
		}

		public ComplexLine(Port start,Port end): base(start,end)
		{
			_segments = new Segments();
			Segment segment = new Segment(Start,End);
			Segments.Add(segment);
			segment.SegmentInvalid += new EventHandler(segment_SegmentInvalid);

			AllowExpand = true;
		}

		public ComplexLine(ComplexLine prototype): base (prototype)
		{
			_segments = new Segments();
			Segment segment = new Segment(Start, End);
			Segments.Add(segment);
			segment.SegmentInvalid += new EventHandler(segment_SegmentInvalid);

			//Set up segments
			for (int i = 0; i < prototype.Segments.Count-1; i++)
			{
				segment = AddSegment(i+1,new Origin((PointF) prototype.Points[i+1])); 
				segment.Start.Marker = prototype.Segments[i+1].Start.Marker;
			}
			DrawPath();

			AllowExpand = prototype.AllowExpand;
		}

		//Properties
		public virtual Segments Segments
		{
			get
			{
				return _segments;
			}
		}

		public virtual bool AllowExpand
		{
			get
			{
				return _allowExpand;
			}
			set
			{ 
				_allowExpand = value;
			}
		}

		//Methods
		public virtual Segment AddSegment(int position, Origin origin)
		{
			//Valid the position
			if (position < 1) throw new ArgumentException("Position must be greater than zero.","position");
			if (position > Segments.Count) throw new ArgumentException("Position cannot be greater than the total number of segments.","position");
			if (origin == null) throw new ArgumentNullException("origin","Origin may not be null.");
			
			//Create new segment
			Segment segment = new Segment(origin,Segments[position-1].End);

			//Set the previous end to the new origin
			Segments[position-1].SetEnd(origin);

			Segments.Insert(position,segment);
			
			origin.OriginInvalid +=new EventHandler(Origin_OriginInvalid);
			segment.SegmentInvalid += new EventHandler(segment_SegmentInvalid);
			origin.SetParent(this);
			
			DrawPath();
			OnElementInvalid();

			return segment;
		}

		public virtual void RemoveSegment(int position)
		{
			//Valid the position
			if (position < 1) throw new ArgumentException("Position must be greater than zero.","position");
			if (position > Segments.Count) throw new ArgumentException("Position can be greater than the total number of segments.","position");

			Segments.RemoveAt(position);
			Segments[position-1].SetEnd(Segments[position].Start);
		}

		public void SetSegments(Segments segments)
		{
			if (_segments != null)
			{
				foreach (Segment segment in _segments)
				{
					segment.SegmentInvalid -= new EventHandler(segment_SegmentInvalid);
				
					if (segment.End != End) segment.End.OriginInvalid -=new EventHandler(Origin_OriginInvalid);
				}
			}

			_segments = segments;
			foreach (Segment segment in segments)
			{
				segment.SegmentInvalid += new EventHandler(segment_SegmentInvalid);
				
				if (segment.End != End) segment.End.OriginInvalid +=new EventHandler(Origin_OriginInvalid);
			}
		}

		#endregion

		#region Overrides

		public override object Clone()
		{
			return new ComplexLine(this);
		}

		public override Origin Start
		{
			get
			{
				return base.Start;
			}
			set
			{
				base.Start = value;
				if (Segments != null) Segments[0].SetStart(value);
			}
		}

		public override Origin End
		{
			get
			{
				return base.End;
			}
			set
			{
				base.End = value;
				if (Segments != null) Segments[Segments.Count-1].SetEnd(value);
			}
		}

		

		public override Origin OriginFromLocation(PointF location)
		{
			return GetOriginFromLocation(location);
		}

		public override bool Intersects(RectangleF rectangle)
		{
			return ComplexIntersects(rectangle);
		}

        public override void LocatePort(Port port)
        {
            if (Points == null) return;

            //Work out total length of line
            PointF lastPoint = new Point();
            double totalLength = 0;

            //Loop through and add each length to total
            foreach (PointF point in Points)
            {
                if (!lastPoint.IsEmpty)
                {
                    RectangleF bounds = Geometry.CreateRectangle(point, lastPoint);
                    totalLength += Geometry.DistancefromOrigin(new PointF(bounds.Width, bounds.Height));
                }
                lastPoint = point;
            }

            //Find position by 
            double length = 0;
            double lengthPercent = totalLength * port.Percent / 100;

            PointF result = new PointF();
            lastPoint = new PointF();

            foreach (PointF point in Points)
            {
                if (!lastPoint.IsEmpty)
                {
                    double start = length;

                    RectangleF bounds = Geometry.CreateRectangle(point, lastPoint);

                    length += Geometry.DistancefromOrigin(new PointF(bounds.Width, bounds.Height));

                    //Check if we are in the right segment
                    if (length > lengthPercent)
                    {
                        //Work out the degrees between the last points
                        double rad = Geometry.GetAngle(lastPoint.X, lastPoint.Y, point.X, point.Y);

                        //Now work out the sides from the angle and H
                        double side1 = Math.Cos(rad) * (lengthPercent - start);
                        double side2 = Math.Sin(rad) * (lengthPercent - start);

                        result = new PointF(Convert.ToSingle(side1) + lastPoint.X, Convert.ToSingle(side2) + lastPoint.Y);
                        break;
                    }
                }
                lastPoint = point;
            }

            port.Validate = false;
            port.Location = result;
            port.Validate = true;
        }

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
			int count = 0;
			PointF previous = new PointF();

			foreach (PointF point in Points)
			{
				//Add the split segment 
				if (count > 0 && AllowExpand)
				{
					path = (GraphicsPath) defaultPath.Clone();
					matrix.Reset();
					matrix.Translate(previous.X - Bounds.X - halfRectangle.Width + ((point.X - previous.X) / 2), previous.Y - Bounds.Y - halfRectangle.Height+ ((point.Y - previous.Y) / 2));
					path.Transform(matrix);

					//Create a new expand handle
					ExpandHandle expand = new ExpandHandle(Segments[count-1]);
					expand.Path = path;
					expand.Index = count-1;
					expand.CanDock = false;
					Handles.Add(expand);
				}

				path = (GraphicsPath) defaultPath.Clone();
				matrix.Reset();
				matrix.Translate(point.X - Bounds.X - halfRectangle.Width,point.Y - Bounds.Y - halfRectangle.Height);
				path.Transform(matrix);
				
				//Create handle
				Handle handle = new Handle(path,HandleType.Origin);
				handle.CanDock = false;
				Handles.Add(handle);

				count ++;
				previous = point;
			}
			
			//Set up the docking
			Handles[0].CanDock = true;
			Handles[Handles.Count-1].CanDock = true;
		}

		#endregion

		#region Events

		//Handles invalid origin events
		private void Origin_OriginInvalid(object sender, EventArgs e)
		{
			DrawPath();
			OnElementInvalid();
		}

		private void segment_SegmentInvalid(object sender, EventArgs e)
		{
			OnElementInvalid();
		}

		#endregion

		#region Implementation
	
		private Origin GetOriginFromLocation(PointF location)
		{
			if (Handles == null) CreateHandles();
			location = new PointF(location.X - Bounds.X, location.Y - Bounds.Y);
		
			int count = 0;
			foreach (Handle handle in Handles)
			{
				if (handle.Type == HandleType.Origin) count ++;
				if (handle.Path.IsVisible(location))
				{
					if (count > Segments.Count) return Segments[count-2].End;
					return Segments[count-1].Start;
				}
			}

			return null;
		}

		private bool ComplexIntersects(RectangleF rectangle)
		{
			//If the rectangle contains the whole line rectangle then return true
			if (rectangle.Contains(Bounds)) return true;

			PointF startLocation = new PointF();
			PointF endLocation = (PointF) Points[1];

			foreach (PointF point in Points)
			{
				if (!startLocation.IsEmpty)
				{
					if (! Geometry.RectangleIntersection(startLocation,endLocation,rectangle).IsEmpty) return true;
				}
				startLocation = endLocation;
				endLocation = point;
			}
			
			return !Geometry.RectangleIntersection(startLocation,endLocation,rectangle).IsEmpty;

			return true;
		}

        //Code to draw path for the line
        public override void DrawPath()
        {
            if (Model == null) return;
            if (Segments == null) return;

            List<PointF> points = new List<PointF>();
            GraphicsPath path = new GraphicsPath();
            PointF startLocation;
            PointF endLocation;

            foreach (Segment segment in Segments)
            {
                //Get the start and end location depending on start shapes etc
                startLocation = GetOriginLocation(segment.Start, segment.End);
                endLocation = GetOriginLocation(segment.End, segment.Start);

                //Add the points to the solution
                if (points.Count == 0) points.Add(startLocation);
                points.Add(endLocation);

                //Draw the path
                path.AddLine(startLocation, endLocation);
            }

            SetPoints(points);

            //Calculate path rectangle
            RectangleF rect = path.GetBounds();
            SetRectangle(rect); //Sets the bounding rectangle
            SetPath(path); //setpath moves the line to 0,0
        }

		//Returns a marker matrix from a diagram matrix
		public Matrix GetSegmentTransform(PointF targetPoint, PointF referencePoint, Matrix initialMatrix)
		{
			//Get the angle between the start and end points of the line
			Double rotation = Geometry.DegreesFromRadians(Geometry.GetAngle(targetPoint.X,targetPoint.Y,referencePoint.X,referencePoint.Y));
			
			//Save the graphics state and translate and transform to the marker origin.
			initialMatrix.Translate(targetPoint.X-Bounds.X,targetPoint.Y-Bounds.Y);
			initialMatrix.Rotate(Convert.ToSingle(rotation));
			
			return initialMatrix;
		}

		#endregion

	}
}
