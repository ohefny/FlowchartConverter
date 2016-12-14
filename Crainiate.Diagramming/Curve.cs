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
	public class Curve: Link, ICloneable
	{
		//Property variables
		private float _tension;
		private CurveType _curveType;
		private PointF[] _controlPoints;

		#region Interface

		//Constructors
		public Curve(): base()
		{
			_curveType = CurveType.Spline;			
			_tension = 0.5F;
			CreateControlPoints();
		}

		public Curve(PointF start,PointF end): base(start,end)
		{
			_curveType = CurveType.Spline;
			_tension = 0.5F;
			CreateControlPoints();
		}

		public Curve(Shape start,Shape end): base(start,end)
		{
			_curveType = CurveType.Spline;
			_tension = 0.5F;
			CreateControlPoints();
		}

		public Curve(Port start,Port end): base(start,end)
		{
			_curveType = CurveType.Spline;
			_tension = 0.5F;
			CreateControlPoints();
		}

		public Curve(Curve prototype): base(prototype)
		{
			_curveType = prototype.CurveType;
			_tension = prototype.Tension;
			_controlPoints = (PointF[]) prototype.ControlPoints.Clone();
			DrawPath();
		}
	
		//Properties
		public virtual PointF[] ControlPoints
		{
			get
			{
				return _controlPoints;
			}
			set
			{
				_controlPoints = value;

				DrawPath();
				OnElementInvalid();
			}
		}

		public virtual float Tension
		{
			get
			{
				return _tension;
			}
			set
			{
				if (_tension != value)
				{
					_tension = value;

					DrawPath();
					OnElementInvalid();
				}
			}
		}

		public virtual CurveType CurveType
		{
			get
			{
				return _curveType;
			}
			set
			{
				if (_curveType != value)
				{
					_curveType = value;

					DrawPath();
					OnElementInvalid();
				}
			}
		}

		protected virtual void SetControlPoints(PointF[] points)
		{
			_controlPoints = points;
		}

		#endregion

		#region Overrides

		public override object Clone()
		{
			return new Curve(this);
		}

		//Code to draw path for the line
		public override void DrawPath()
		{
			if (Model == null) return;
			if (ControlPoints == null) return;

			//Get the start and end location depending on start shapes etc
			PointF startLocation = GetOriginLocation(Start,End);
			PointF endLocation = GetOriginLocation(End,Start);

			//Add the points to the solution
			List<PointF> points = new List<PointF>();
			points.Add(startLocation);
			
			//Add the control points
			//If bezier must be 2,5,8 control points etc
			PointF[] controlPoints;

			//Set up control points
			if (CurveType == CurveType.Bezier)
			{
				//Must be 2, 5, 8 etc
				if (_controlPoints.GetUpperBound(0) < 1) throw new CurveException("Bezier must contain at least 2 control points.");
				
				int intMax = (((int) (_controlPoints.GetUpperBound(0) - 1) / 3) * 3) + 2;
				controlPoints = new PointF[intMax];

				for (int i = 0; i < intMax; i++ )
				{
					controlPoints[i] = _controlPoints[i];
				}
			}
			else
			{
				controlPoints = _controlPoints;
			}
			points.InsertRange(1,controlPoints);
			
			//Add the end points
			points.Add(endLocation);

			//Draw the path
			GraphicsPath path = new GraphicsPath();
			
			if (CurveType == CurveType.Bezier)
			{
				path.AddBeziers(points.ToArray());
			}
			else
			{
				path.AddCurve(points.ToArray(),Tension);
			}

			SetPoints(points);

			//Calculate path rectangle
			RectangleF rect = path.GetBounds();
			SetRectangle(rect); //Sets the bounding rectangle
			SetPath(path); //setpath moves the line to 0,0
		}

		public override bool Intersects(RectangleF rectangle)
		{
			return CurveIntersects(rectangle);
		}

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

			//Loop through each point and add an offset handle
			GraphicsPath path;

			foreach (PointF point in Points)
			{
				path = (GraphicsPath) defaultPath.Clone();
				matrix.Reset();
				matrix.Translate(point.X - Bounds.X - halfRectangle.Width,point.Y - Bounds.Y - halfRectangle.Height);
				path.Transform(matrix);
				Handles.Add(new Handle(path, HandleType.Origin));
			}

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

		#endregion

		#region Implementation	
	
		private bool CurveIntersects(RectangleF rectangle)
		{
			//Translate rectangle to local co-ordinates
			rectangle.Location = new PointF(rectangle.Location.X - Bounds.Location.X,rectangle.Location.Y - Bounds.Location.Y);
	
			//If the rectangle contains the whole line rectangle then return true
			if (rectangle.Contains(Bounds)) return true;

			Region region = new Region(GetPath());

			return region.IsVisible(rectangle);
		}

		private void CreateControlPoints()
		{
			PointF start = GetOriginLocation(Start, End);
			PointF end = GetOriginLocation(Start, End);
			PointF mid = new PointF(start.X + ((end.X - start.X)/2), end.Y + ((end.Y - start.Y) /2));
			
			ControlPoints = new PointF[] {mid};
		}

		#endregion
	}
}
