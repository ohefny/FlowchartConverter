// (c) Copyright Crainiate Software 2010

using System.Drawing;

using Crainiate.Diagramming.Collections;

namespace Crainiate.Diagramming
{
    public class TranslateCommand: TransformCommand 
    {
        //Property variables
        private float _dx;
        private float _dy;

        //Constructors
        public TranslateCommand(Controller controller): base(controller)
        {

        }

        //Properties
        public float Dx
        {
            get
            {
                return _dx;
            }
            set
            {
                _dx = value;
            }
        }

        public float Dy
        {
            get
            {
                return _dy;
            }
            set
            {
                _dy = value;
            }
        }

        //Methods

        //Translates an action, typically in response to mouse movement
        public virtual void Translate()
        {
            SizeF delta = new SizeF(Dx, Dy);
            delta = Controller.BoundsCheck(Elements, new SizeF(Dx, Dy));
            if (delta.IsEmpty) return;

            bool snapped = false;
                            
            //Reset any vectors
            SetVectors(null);

            //Initial loop to check for location snapping   
            foreach (Element element in Elements)
            {
                if (element.Visible)
                {
                    //Offset shapes
                    if (element is Shape)
                    {
                        Shape shape = element as Shape;
                        Shape actionshape = shape.ActionElement as Shape; //the actual shape being moved

                        if (shape.AllowMove && shape.AllowSnap)
                        {
                            PointF location = Geometry.CombinePoint(actionshape.Location, new PointF(Dx, Dy));
                            PointF snap = Controller.SnapToLocation(location, shape.Size, this); //Sets the vector

                            //If a shape is snapped to a location then readjust the dx and dy values
                            if (!snapped && !location.Equals(snap))
                            {
                                Dx = Dx + snap.X - location.X;
                                Dy = Dy + snap.Y - location.Y;
                                snapped = true;
                            }
                        }
                    }
                }
            }
            
            //Change position of each element
            foreach (Element element in Elements)
            {
                if (element.Visible)
                {
                    //Offset shapes
                    if (element is Shape)
                    {
                        Shape shape = element as Shape;
                        Shape actionshape = shape.ActionElement as Shape; //the actual shape being moved

                        if (shape.AllowMove)
                        {
                            PointF location = Geometry.CombinePoint(actionshape.Location, new PointF(Dx, Dy));
                            shape.Location = location;
                        }

                        if (Controller.Model.Route != null) Controller.Model.Route.Reform();
                    }

                    //Offset ports
                    if (element is Port)
                    {
                        Port port = element as Port;
                        Port actionPort = element.ActionElement as Port;

                        if (port.AllowMove)
                        {
                            PointF location = new PointF(actionPort.X + Dx, actionPort.Y + Dy);
                            
                            //Call a method on the container to adjust the point so that it lies outside the path
                            if (port.Parent.Contains(location)) location = port.Parent.Forward(location);

                            //Port is outside the path
                            port.Location = port.Parent.Intercept(location);
                        }

                        if (Controller.Model.Route != null) Controller.Model.Route.Reform();
                    }

                    //Offset line
                    if (element is Link)
                    {
                        //Offset all segments in complex line
                        if (element is ComplexLine)
                        {
                            ComplexLine complexLine = element as ComplexLine;
                            ComplexLine actionLine = element.ActionElement as ComplexLine;
                            Segment segment = null;
                            Segment actionSegment = null;

                            if (actionLine.AllowMove)
                            {
                                for (int i = 0; i < complexLine.Segments.Count; i++)
                                {
                                    segment = complexLine.Segments[i];
                                    actionSegment = actionLine.Segments[i];

                                    if (!actionSegment.Start.Docked)
                                    {
                                        segment.Start.Move(Dx, Dy);
                                    }
                                }
                                if (!actionSegment.End.Docked) segment.End.Move(Dx, Dy);
                            }
                        }
                        else if (element is Curve)
                        {
                            Curve curve = (Curve)element;
                            Curve actionCurve = (Curve)curve.ActionElement;
                            if (actionCurve.AllowMove)
                            {
                                if (!actionCurve.Start.Docked || (actionCurve.Start.Shape != null && actionCurve.Start.Shape.Selected) || (actionCurve.Start.Port != null && ((ISelectable)actionCurve.Start.Port.Parent).Selected)) curve.Start.Move(Dx, Dy);
                                if (!actionCurve.End.Docked || (actionCurve.End.Shape != null && actionCurve.End.Shape.Selected) || (actionCurve.End.Port != null && ((ISelectable)actionCurve.End.Port.Parent).Selected)) curve.End.Move(Dx, Dy);

                                PointF[] newPoints = new PointF[actionCurve.ControlPoints.GetUpperBound(0) + 1];
                                for (int i = 0; i <= actionCurve.ControlPoints.GetUpperBound(0); i++)
                                {
                                    newPoints[i] = new PointF(curve.ControlPoints[i].X + Dx, curve.ControlPoints[i].Y + Dy);
                                }
                                curve.ControlPoints = newPoints;
                            }
                        }
                        else if (element is Connector)
                        {
                            Connector connector = (Connector)element;
                            Connector actionConnector = (Connector)connector.ActionElement;

                            if (actionConnector.AllowMove && !actionConnector.Start.Docked && !actionConnector.End.Docked)
                            {
                                List<PointF> newPoints = new List<PointF>();

                                foreach (PointF point in actionConnector.Points)
                                {
                                    newPoints.Add(new PointF(point.X + Dx, point.Y + Dy));
                                }
                                connector.SetPoints(newPoints);
                                connector.DrawPath();
                            }
                        }
                        else
                        {
                            Link line = (Link) element;
                            Link actionLine = (Link) line.ActionElement;

                            if (actionLine.AllowMove)
                            {
                                if (!actionLine.Start.Docked) line.Start.Location = Geometry.CombinePoint(actionLine.Start.Location, new PointF(Dx, Dy));
                                if (!actionLine.End.Docked) line.End.Location = Geometry.CombinePoint(actionLine.End.Location, new PointF(Dx, Dy));

                                line.DrawPath();
                            }
                        }
                    }

                    //Offset stand-alone port
                    if (element is Port)
                    {
                        Port port = (Port)element;
                        if (port.AllowMove) port.Move(Dx, Dy);
                    }
                }
            }
        }

        public override void Undo()
        {
            Dx = -Dx;
            Dy = -Dy;

            try
            {
                Translate();
                Execute();
            }
            finally
            {
                Dx = -Dx;
                Dy = -Dy;
            }
        }

        public override void Redo()
        {
            Translate();
            Execute();
        }
    }
}
