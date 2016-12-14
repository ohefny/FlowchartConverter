// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Collections;
using System.Text;

using Crainiate.Diagramming.Collections;

namespace Crainiate.Diagramming
{
    public class ScaleCommand: TransformCommand 
    {
        //Property variables
        private float _dx;
        private float _dy;
        private bool _keepAspect;

        //Constructors
        public ScaleCommand(Controller controller): base(controller)
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

        public bool KeepAspect
        {
            get
            {
                return _keepAspect;
            }
            set
            {
                _keepAspect = value;
            }
        }

        //Methods
        public virtual void Scale()
        {
            float sx = 1; //scale
            float sy = 1;
            float mx = 0; //movement as a result of scale
            float my = 0;

            foreach (Element element in Elements)
            {
                if (element.Visible)
                {
                    //Scale shapes
                    if (element is Shape)
                    {
                        Shape shape = element as Shape; //a clone of the original shape, contained in the list
                        Shape actionshape = shape.ActionElement as Shape; //the actual shape being moved

                        if (actionshape.AllowScale)
                        {
                            if (Controller.Model.Route != null) Controller.Model.Route.Reform();

                            PointF saveLocation = shape.Location;
                            SizeF saveSize = shape.Size;

                            //Reset the ports
                            foreach (Port port in shape.Ports.Values)
                            {
                                Port actionPort = (Port)actionshape.Ports[port.Key];
                                port.SuspendValidation();
                                port.Location = actionPort.Location;
                                port.ResumeValidation();
                            }

                            //Reset shape location and size
                            shape.Location = actionshape.Location; //reset location
                            shape.SetSize(actionshape.Size, actionshape.InternalRectangle); //reset to original size

                            //Reset children of a complex shape
                            if (shape is ComplexShape)
                            {
                                ComplexShape complex = (ComplexShape)shape;

                                foreach (Solid solid in complex.Children.Values)
                                {
                                    Solid actionSolid = (Solid)solid.ActionElement;

                                    solid.Location = actionSolid.Location; //reset location
                                    solid.SetSize(actionSolid.Size, actionSolid.InternalRectangle); //reset to original size
                                }
                            }

                            //Scale Right x				
                            if (MouseElements.MouseHandle.Type == HandleType.TopRight || MouseElements.MouseHandle.Type == HandleType.Right || MouseElements.MouseHandle.Type == HandleType.BottomRight)
                            {
                                sx = ((Dx) / shape.ActionElement.Bounds.Width) + 1;
                            }
                            //Scale Bottom y
                            if (MouseElements.MouseHandle.Type == HandleType.BottomLeft || MouseElements.MouseHandle.Type == HandleType.Bottom || MouseElements.MouseHandle.Type == HandleType.BottomRight)
                            {
                                sy = ((Dy) / shape.ActionElement.Bounds.Height) + 1;
                            }
                            //Scale Left x
                            if (MouseElements.MouseHandle.Type == HandleType.TopLeft || MouseElements.MouseHandle.Type == HandleType.Left || MouseElements.MouseHandle.Type == HandleType.BottomLeft)
                            {
                                sx = ((-Dx) / shape.ActionElement.Bounds.Width) + 1;
                                mx = Dx;
                                if (shape.Bounds.Width * sx < shape.MinimumSize.Width) mx = (shape.ActionElement.Bounds.Width - shape.MinimumSize.Width);
                                if (shape.Bounds.Width * sx > shape.MaximumSize.Width) mx = (shape.ActionElement.Bounds.Width - shape.MaximumSize.Width);
                            }
                            //Scale Top y
                            if (MouseElements.MouseHandle.Type == HandleType.TopLeft || MouseElements.MouseHandle.Type == HandleType.Top || MouseElements.MouseHandle.Type == HandleType.TopRight)
                            {
                                sy = ((-Dy) / shape.ActionElement.Bounds.Height) + 1;
                                my = Dy;
                                if (shape.Bounds.Height * sy < shape.MinimumSize.Height) my = (shape.ActionElement.Bounds.Height - shape.MinimumSize.Height);
                                if (shape.Bounds.Height * sy > shape.MaximumSize.Height) my = (shape.ActionElement.Bounds.Height - shape.MaximumSize.Height);
                            }

                            shape.Scale(sx, sy, mx, my, KeepAspect || shape.KeepAspect);

                            //Restore shape bounds if not correct
                            if (!Controller.BoundsCheck(shape.Location,0,0))
                            {
                                shape.Location = saveLocation;
                                shape.Size = saveSize;
                            }
                        }
                    }

                    //Move line origins
                    if (element is Link)
                    {
                        if (element is ComplexLine)
                        {
                            ComplexLine line = (ComplexLine)element;
                            ComplexLine actionline = (ComplexLine)line.ActionElement;
                            Segment segment;
                            Segment actionSegment;

                            if (MouseElements.MouseHandle.Type == HandleType.Origin)
                            {
                                for (int i2 = 0; i2 < line.Segments.Count; i2++)
                                {
                                    segment = line.Segments[i2];
                                    actionSegment = actionline.Segments[i2];

                                    if (actionSegment.Start == MouseElements.MouseStartOrigin)
                                    {
                                        if (Controller.BoundsCheck(actionSegment.Start.Location, Dx, Dy))
                                        {
                                            segment.Start.Location = (PointF)actionline.Points[i2]; //Resets the location
                                            segment.Start.Move(Dx, Dy);
                                            line.DrawPath();
                                        }
                                        break;
                                    }

                                    if (actionSegment.End == MouseElements.MouseStartOrigin)
                                    {
                                        if (Controller.BoundsCheck(actionSegment.End.Location, Dx, Dy))
                                        {
                                            segment.End.Location = (PointF)actionline.Points[i2 + 1]; //Resets the location
                                            segment.End.Move(Dx, Dy);
                                            line.DrawPath();
                                        }
                                        break;
                                    }
                                }
                            }

                            //Add the segment and reset the handle to an origin handle
                            if (MouseElements.MouseHandle.Type == HandleType.Expand)
                            {
                                //Find the segment
                                ExpandHandle expand = MouseElements.MouseHandle as ExpandHandle;

                                //Get origin locations
                                PointF start = line.GetOriginLocation(expand.Segment.Start, expand.Segment.End);
                                PointF end = line.GetOriginLocation(expand.Segment.End, expand.Segment.Start);

                                Origin origin = new Origin(new PointF(start.X + ((end.X - start.X) / 2), start.Y + ((end.Y - start.Y) / 2)));
                                Origin actionOrigin = new Origin(new PointF(origin.Location.X, origin.Location.Y));

                                line.AddSegment(expand.Index + 1, origin);
                                actionline.AddSegment(expand.Index + 1, actionOrigin);

                                MouseElements.MouseHandle = new Handle(HandleType.Origin);

                                //Set up mouse elements
                                MouseElements = new MouseElements(MouseElements);
                                MouseElements.MouseStartOrigin = actionOrigin;
                                //diagram.SetMouseElements(mouseElements);
                            }
                        }
                        else if (element is Connector)
                        {
                            Connector line = element as Connector;
                            Connector actionLine = element.ActionElement as Connector;

                            //Move start or end of connector
                            if (MouseElements.MouseHandle.Type == HandleType.Origin)
                            {
                                Origin origin = null;
                                PointF point = new PointF();

                                //Get the origin point
                                if (actionLine.Start == MouseElements.MouseStartOrigin && actionLine.Start.AllowMove)
                                {
                                    origin = line.Start;
                                    point = (PointF)actionLine.Points[0];
                                }
                                if (actionLine.End == MouseElements.MouseStartOrigin && actionLine.End.AllowMove)
                                {
                                    origin = line.End;
                                    point = (PointF)actionLine.Points[actionLine.Points.Count - 1];
                                }

                                if (origin != null)
                                {
                                    if (Controller.BoundsCheck(point, Dx, Dy))
                                    {
                                        //Offset the origin point
                                        origin.Location = new PointF(point.X + Dx, point.Y + Dy);

                                        //Set to shape if current mouse element is shape
                                        if (MouseElements.IsDockable() && Controller.CanDock(InteractiveMode, MouseElements))
                                        {
                                            if (MouseElements.MouseMoveElement is Shape)
                                            {
                                                origin.Shape = MouseElements.MouseMoveElement as Shape;
                                            }
                                            else if (MouseElements.MouseMoveElement is Port)
                                            {
                                                origin.Port = MouseElements.MouseMoveElement as Port;
                                            }
                                        }

                                        line.CalculateRoute();
                                    }
                                }
                            }
                            //Move a connector segment
                            else if (MouseElements.MouseHandle.Type == HandleType.UpDown || MouseElements.MouseHandle.Type == HandleType.LeftRight)
                            {
                                ConnectorHandle handle = MouseElements.MouseHandle as ConnectorHandle;

                                if (handle != null)
                                {
                                    PointF start = (PointF)actionLine.Points[handle.Index - 1];
                                    PointF end = (PointF)actionLine.Points[handle.Index];

                                    //Move the two segment points and place them back in the correct place
                                    if (MouseElements.MouseHandle.Type == HandleType.UpDown)
                                    {
                                        if (Controller.BoundsCheck(start, 0, Dy) && Controller.BoundsCheck(end, 0, Dy))
                                        {
                                            start.Y += Dy;
                                            end.Y += Dy;

                                            //Update the line
                                            line.Points[handle.Index - 1] = start;
                                            line.Points[handle.Index] = end;
                                        }
                                    }
                                    else if (MouseElements.MouseHandle.Type == HandleType.LeftRight)
                                    {
                                        if (Controller.BoundsCheck(end, Dx, 0) && Controller.BoundsCheck(end, Dx, 0))
                                        {
                                            start.X += Dx;
                                            end.X += Dx;

                                            //Update the line
                                            line.Points[handle.Index - 1] = start;
                                            line.Points[handle.Index] = end;
                                        }
                                    }
                                }
                            }
                        }
                        else if (element is Curve)
                        {
                            Curve curve = (Curve)element;
                            Curve actionCurve = (Curve)curve.ActionElement;

                            if (MouseElements.MouseStartOrigin == actionCurve.Start && actionCurve.Start.AllowMove)
                            {
                                if (Controller.BoundsCheck(actionCurve.FirstPoint, Dx, Dy))
                                {
                                    curve.Start.Location = actionCurve.FirstPoint; //Resets the location
                                    curve.Start.Move(Dx, Dy);
                                }
                            }
                            else if (MouseElements.MouseStartOrigin == actionCurve.End && actionCurve.End.AllowMove)
                            {
                                if (Controller.BoundsCheck(actionCurve.LastPoint, Dx, Dy))
                                {
                                    curve.End.Location = actionCurve.LastPoint; //Resets the location
                                    curve.End.Move(Dx, Dy);
                                }
                            }
                            else
                            {
                                //Move control points
                                int index = 0;
                                foreach (PointF point in actionCurve.ControlPoints)
                                {
                                    PointF location = new PointF(point.X - actionCurve.Bounds.X, point.Y - actionCurve.Bounds.Y);

                                    if (MouseElements.MouseHandle != null && MouseElements.MouseHandle.Path != null && MouseElements.MouseHandle.Path.IsVisible(location))
                                    {
                                        curve.ControlPoints[index] = new PointF(actionCurve.ControlPoints[index].X + Dx, actionCurve.ControlPoints[index].Y + Dy);
                                        break;
                                    }
                                    index++;
                                }
                            }
                        }
                        else if (element is Link)
                        {
                            Link line = (Link)element;
                            Link actionline = (Link)line.ActionElement;

                            if (MouseElements.MouseStartOrigin == actionline.Start && actionline.Start.AllowMove)
                            {
                                if (Controller.BoundsCheck(actionline.FirstPoint, Dx, Dy))
                                {
                                    line.Start.Location = actionline.FirstPoint; //Resets the location
                                    line.Start.Move(Dx, Dy);
                                }
                            }
                            if (MouseElements.MouseStartOrigin == actionline.End && actionline.End.AllowMove)
                            {
                                if (Controller.BoundsCheck(actionline.LastPoint, Dx, Dy))
                                {
                                    line.End.Location = actionline.LastPoint; //Resets the location
                                    line.End.Move(Dx, Dy);
                                }
                            }
                        }

                        //Update docking
                        if (MouseElements.MouseMoveElement != null && MouseElements.IsDockable() && Controller.CanDock(InteractiveMode, MouseElements))
                        {
                            Link line = (Link)element;
                            Link actionline = (Link)line.ActionElement;

                            if (MouseElements.MouseMoveElement is Shape)
                            {
                                if (MouseElements.MouseStartOrigin == actionline.Start && actionline.Start.AllowMove)
                                {
                                    line.Start.Shape = MouseElements.MouseMoveElement as Shape;
                                }
                                if (MouseElements.MouseStartOrigin == actionline.End && actionline.End.AllowMove)
                                {
                                    line.End.Shape = MouseElements.MouseMoveElement as Shape;
                                }
                            }
                            else if (MouseElements.MouseMoveElement is Port)
                            {
                                if (MouseElements.MouseStartOrigin == actionline.Start && actionline.Start.AllowMove)
                                {
                                    line.Start.Port = MouseElements.MouseMoveElement as Port;
                                }
                                if (MouseElements.MouseStartOrigin == actionline.End && actionline.End.AllowMove)
                                {
                                    line.End.Port = MouseElements.MouseMoveElement as Port;
                                }
                            }
                        }

                        Link clone = element as Link;
                        clone.DrawPath(); //Update the action path
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
                Scale();
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
            Scale();
            Execute();
        }
    }
}
