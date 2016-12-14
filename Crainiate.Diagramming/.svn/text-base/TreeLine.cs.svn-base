// (c) Copyright Crainiate Software 2010

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

using Crainiate.Diagramming.Collections;

namespace Crainiate.Diagramming
{
    public class TreeLine: Line, ICloneable
    {
        private List<Origin> _startOrigins;
        private List<Origin> _endOrigins;

        //Constructors
        public TreeLine()
        {
            _startOrigins = new List<Origin>();
            _endOrigins = new List<Origin>();
        }

        public TreeLine(TreeLine prototype)
        {
            AllowMove = prototype.AllowMove;
            LineJoin = prototype.LineJoin;
            DrawSelected = prototype.DrawSelected;
            Interaction = prototype.Interaction;

            List<PointF> points = new List<PointF>();
            points.AddRange(prototype.Points);
            SetPoints(points);

            _startOrigins = new List<Origin>();
            _endOrigins = new List<Origin>();
            
            DrawPath();
        }

        //Properties
        protected virtual List<Origin> StartOrigins
        {
            get
            {
                return _startOrigins;
            }
        }

        protected virtual List<Origin> EndOrigins
        {
            get
            {
                return _endOrigins;
            }
        }

        //Methods
        //Add to the collection of starting origins
        public virtual void AddStart(Origin origin)
        {
            origin.OriginInvalid += new EventHandler(Origin_OriginInvalid);
            _startOrigins.Add(origin);
        }

        public virtual void AddStart(Shape shape)
        {
            Origin origin = new Origin(shape);
            origin.OriginInvalid += new EventHandler(Origin_OriginInvalid);
            _startOrigins.Add(origin);
        }

        //Add to the collection of ending origins
        public virtual void AddEnd(Origin origin)
        {
            origin.OriginInvalid += new EventHandler(Origin_OriginInvalid);
            _endOrigins.Add(origin);
        }

        public virtual void AddEnd(Shape shape)
        {
            Origin origin = new Origin(shape);
            origin.OriginInvalid += new EventHandler(Origin_OriginInvalid);
            _endOrigins.Add(origin);
        }

        public TreeLine Clone()
        {
            return new TreeLine(this);
        }

        protected internal override void CreateHandles()
        {
            
        }

        public override void DrawPath()
        {
            if (StartOrigins == null | EndOrigins == null) return;

            //Get the bounding rectangles of the starting and ending origins
            RectangleF startRectangle = GetBoundingRectangle(StartOrigins);
            RectangleF endRectangle = GetBoundingRectangle(EndOrigins);

            //Swap rectangles around if end is higher than start
            if (endRectangle.Bottom < startRectangle.Top)
            {
                RectangleF tempRectangle = startRectangle;
                startRectangle = endRectangle;
                endRectangle = tempRectangle;
            }

            //Get the mid point between the starting and ending rectangle boundaries
            float y = Convert.ToInt32(startRectangle.Bottom + ((endRectangle.Top - startRectangle.Bottom) / 2));

            //Draw the path
            GraphicsPath path = new GraphicsPath();

            //Draw down and up to the cross line from each start and end origin
            DrawFromOrigins(path, StartOrigins, y);
            DrawFromOrigins(path, EndOrigins, y);

            RectangleF finalBounds = path.GetBounds();

            //Draw the cross line
            path.AddLine(finalBounds.X, y, finalBounds.Right, y);
            path.CloseFigure();

            //Calculate path rectangle
            RectangleF rect = path.GetBounds();
            SetRectangle(rect); //Sets the location rectangle
            SetPath(path); //setpath moves the line to 0,0
            SetPoints(new List<PointF>() { new PointF(finalBounds.X, y), new PointF(finalBounds.Right, y) });
        }

        //Implementation
        //Occurs when an origin changes
        private void Origin_OriginInvalid(object sender, EventArgs e)
        {
            DrawPath();
        }

        private void DrawFromOrigins(GraphicsPath path, List<Origin> origins, float y)
        {
            //Draw down from each start origin to the branch (cross) line
            foreach (Origin origin in origins)
            {
                PointF end = PointF.Empty;
                PointF start = PointF.Empty;

                if (origin.Docked)
                {
                    Solid targetElement = (origin.Shape != null) ? (Solid)origin.Shape : (Solid)origin.Port;
                    end = new PointF(GetSourceLocation(origin).X, y);
                    start = targetElement.Intercept(end);
                }
                else
                {
                    start = origin.Location;
                    end = new PointF(start.X, y);
                }

                path.AddLine(start, end);
                path.CloseFigure();
            }
        }

        public RectangleF GetBoundingRectangle(List<Origin> origins)
        {
            RectangleF rect = RectangleF.Empty;

            foreach (Origin origin in origins)
            {
                if (origin.Docked)
                {
                    if (origin.Shape != null)
                    {
                        rect = rect.IsEmpty ? origin.Shape.Bounds: Geometry.ComplementRectangle(rect, origin.Shape.Bounds);
                    }
                    else if (origin.Port != null)
                    {
                        rect = rect.IsEmpty ? origin.Port.Bounds : Geometry.ComplementRectangle(rect, origin.Port.Bounds);
                    }
                }
                else
                {
                    rect = rect.IsEmpty ? new RectangleF(origin.Location, new SizeF(1, 1)) : Geometry.ComplementRectangle(rect, origin.Location);
                }
            }

            return rect;
        }
    }
}
