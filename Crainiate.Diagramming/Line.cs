// (c) Copyright Crainiate Software 2010

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using Crainiate.Diagramming.Collections;

namespace Crainiate.Diagramming
{
	[Serializable]
	public abstract class Line: Element, ISelectable, IUserInteractive
	{
        //Extended
        OnShapeClickListener onShapeSelectedListener;
		//Property variables
		private LineJoin _lineJoin;
		private bool _allowMove;
		
		private bool _drawSelected;
		private bool _selected;
		private UserInteraction _interaction;

		//Working variables
		private List<PointF> _points;

		#region Interface

		//Events
		public event EventHandler SelectedChanged;
		public event EventHandler OriginInvalid;

		//Constructors
		public Line():base()
		{
			AllowMove = true;
			DrawSelected = true;
			Interaction = UserInteraction.BringToFront;
			SmoothingMode = SmoothingMode.HighQuality;
		}

		//Properties
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
		
		//Indicates whether the line can be moved as an element.
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

		//Gets or sets the join style for the ends of two consecutive line segements.
		public virtual LineJoin LineJoin
		{
			get
			{
				return _lineJoin;
			}
			set
			{
				if (_lineJoin != value)
				{
					_lineJoin = value;
					DrawPath();
					OnElementInvalid();
				}
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

		//Returns the points that make up this line
		public virtual List<PointF> Points
		{
			get
			{
				return _points;
			}
		}

		//Returns the first point of this line
		public virtual PointF FirstPoint
		{
			get
			{
				return (PointF) _points[0];
			}
		}

		//Returns the last point of this line
		public virtual PointF LastPoint
		{
			get
			{
				return (PointF) _points[_points.Count-1];
			}
		}

        public OnShapeClickListener OnShapeSelectedListener
        {
            get
            {
                return onShapeSelectedListener;
            }

            set
            {
                onShapeSelectedListener = value;
            }
        }

        //Methods
       protected  internal virtual void SetPoints(List<PointF> points)
		{
			_points = points;
		}
       /* protected virtual void SetPoints(List<PointF> points)
        {
            _points = points;
        }*/

        //Raises the element SelectedChanged event.
        protected virtual void OnSelectedChanged()
		{
			if (SelectedChanged != null) SelectedChanged(this,EventArgs.Empty);
            onShapeSelectedListener.onShapeClicked();
           
		}

		#endregion

		#region Overrides

		public override bool Intersects(RectangleF rectangle)
		{
			return LineIntersects(rectangle);
		}

		//Returns the type of cursor from this point
		public override Handle Handle(PointF location)
		{
			return GetLineHandle(location);
		}

		#endregion

		#region Implementation

		protected internal abstract void CreateHandles();
        public abstract void DrawPath();
		
		//Returns a marker matrix from a diagram matrix
		public static Matrix GetMarkerTransform(MarkerBase marker, PointF markerPoint, PointF referencePoint, Matrix initialMatrix)
		{
			//Get the angle between the start and end points of the line
			Double rotation = Geometry.DegreesFromRadians(Geometry.GetAngle(markerPoint.X,markerPoint.Y,referencePoint.X,referencePoint.Y));
			
			//Save the graphics state and translate and transform to the marker origin.
			initialMatrix.Translate(markerPoint.X - marker.Bounds.X, markerPoint.Y - marker.Bounds.Y);
			initialMatrix.Rotate(Convert.ToSingle(rotation-90));
			if (marker.Centered) 
			{
				initialMatrix.Translate(marker.Bounds.Width / 2 * -1,marker.Bounds.Height / 2 * -1);
			}
			else
			{
				initialMatrix.Translate(marker.Bounds.Width / 2 * -1,1);
			}
			
			return initialMatrix;
		}

		//Returns the location if not docked, or the center if docked
		internal PointF GetSourceLocation(Origin source)
		{
			//Set up the source location (point or element center)
			if (source.DockedElement == null) return source.Location;

			return source.DockedElement.Center;
		}

		private bool LineIntersects(RectangleF rectangle)
		{
			//If the rectangle contains the whole line rectangle then return true
			if (rectangle.Contains(Bounds)) return true;

			PointF startLocation = (PointF) _points[0];
			PointF endLocation = (PointF) _points[_points.Count-1];

			//Return the intersection of the line with the selection rectangle
			return !Geometry.RectangleIntersection(startLocation,endLocation,rectangle).IsEmpty;
		}

		//Gets the cursor from the diagram point
		private Handle GetLineHandle(PointF location)
		{
			if (!Selected || Handles == null) return Singleton.Instance.DefaultHandle;

			//Offset location to local co-ordinates
			location = new PointF(location.X - Bounds.X, location.Y - Bounds.Y);

			//Check each handle
			foreach (Handle handle in Handles)
			{
				if (handle.Path.IsVisible(location)) return handle;
			}

			return Singleton.Instance.DefaultHandle;
		}

		#endregion

	}

}