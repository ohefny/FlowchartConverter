// (c) Copyright Crainiate Software 2010

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

namespace Crainiate.Diagramming
{
    public abstract class TransformCommand: Command 
    {
        //Property variables
        private ElementList _elements;
        private MouseElements _mouseElements;

        private RectangleF[] _vectors;

        private InteractiveMode _interactiveMode;

        //Constructors
        public TransformCommand(Controller controller): base(controller)
        {
            if (controller == null) throw new ArgumentNullException();
        }

        //Properties
        public InteractiveMode InteractiveMode
        {
            get
            {
                return _interactiveMode;
            }
            set
            {
                _interactiveMode = value;
            }
        }

        public ElementList Elements
        {
            get
            {
                return _elements;
            }
            set
            {
                _elements = value;
            }
        }

        public MouseElements MouseElements
        {
            get
            {
                return _mouseElements;
            }
            set
            {
                _mouseElements = value;
            }
        }

        public virtual RectangleF[] Vectors
        {
            get
            {
                return _vectors;
            }
        }

        public virtual bool Executed {get; set;}

        public override void Execute()
        {
            if (Elements == null) return;

            MouseElements mouseElements = MouseElements;

            foreach (Element element in Elements)
            {
                if (element.Visible)
                {
                    if (element.ActionElement == null) throw new ComponentException("Element action may not be null.");
                    if (element is Shape)
                    {
                        Shape shape = (Shape) element;
                        Shape actionShape = (Shape) element.ActionElement;

                        //Round values if appropriate
                        if (Controller.RoundPixels)
                        {
                            shape.Location = Point.Round(shape.Location);
                            shape.Size = System.Drawing.Size.Round(shape.Size);
                        }

                        //Move and scale. Shape size property does not check equality
                        if (!actionShape.Location.Equals(shape.Location)) actionShape.Location = shape.Location; //new PointF(shape.X,shape.Y);
                        if (!actionShape.Size.Equals(shape.Size)) actionShape.Size = shape.Size;

                        //Update children of a complex shape
                        if (shape is ComplexShape)
                        {
                            ComplexShape complex = (ComplexShape)shape;

                            foreach (Solid solid in complex.Children.Values)
                            {
                                Solid actionSolid = (Solid)solid.ActionElement;
                                actionSolid.SetPath(solid.GetPath());
                                actionSolid.SetRectangle(solid.Location);
                                actionSolid.SetTransformRectangle(solid.Location);
                            }
                        }
                    }

                    //Update rotation
                    if (element is ITransformable)
                    {
                        ITransformable transform = element as ITransformable;
                        ITransformable actionTransform = element.ActionElement as ITransformable;

                        if (actionTransform.Rotation != transform.Rotation) actionTransform.Rotation = transform.Rotation;
                    }

                    if (element is Port)
                    {
                        Port port = (Port)element;
                        Port actionPort = (Port)element.ActionElement;

                        //Move and scale. Port size property does not check equality
                        if (!actionPort.Location.Equals(port.Location)) actionPort.Location = port.Location; //new PointF(port.X,port.Y);

                        //Update the port percentage
                        IPortContainer ports = (IPortContainer)actionPort.Parent;
                        ports.GetPortPercentage(actionPort, actionPort.Location);
                    }

                    //Update the locations of the line origins
                    if (element is Link)
                    {
                        Link clone = (Link)element;

                        //Undock any origins
                        if (mouseElements.MouseStartOrigin != null && mouseElements.MouseStartOrigin.Docked && mouseElements.MouseStartOrigin.AllowMove)
                        {
                            Origin origin = mouseElements.MouseStartOrigin;
                            if (origin == origin.Parent.Start) origin.Location = origin.Parent.FirstPoint;
                            if (origin == origin.Parent.End) origin.Location = origin.Parent.LastPoint;
                        }

                        if (element is ComplexLine)
                        {
                            ComplexLine complexLine = (ComplexLine)element;
                            ComplexLine actionLine = (ComplexLine)element.ActionElement;
                            Segment segment = null;
                            Segment actionSegment = null;

                            for (int i = 0; i < complexLine.Segments.Count; i++)
                            {
                                segment = complexLine.Segments[i];
                                actionSegment = actionLine.Segments[i];
                                if (!actionSegment.Start.Docked) actionSegment.Start.Location = segment.Start.Location;
                            }

                            //Update end of last segment
                            if (segment != null && actionSegment != null && !actionSegment.End.Docked) actionSegment.End.Location = segment.End.Location;

                            actionLine.DrawPath();
                            actionLine.LocatePorts();
                        }
                        else if (element is Curve)
                        {
                            Curve curve = (Curve)element;
                            Curve actionCurve = (Curve)element.ActionElement;

                            if (!actionCurve.Start.Docked) actionCurve.Start.Location = curve.Start.Location;
                            if (!actionCurve.End.Docked) actionCurve.End.Location = curve.End.Location;

                            actionCurve.ControlPoints = curve.ControlPoints;
                            actionCurve.DrawPath();
                            actionCurve.LocatePorts();
                        }
                        else if (element is Connector)
                        {
                            //Update connector oblong handle
                            if (mouseElements.MouseHandle.Type == HandleType.UpDown || mouseElements.MouseHandle.Type == HandleType.LeftRight)
                            {
                                Connector connectorLine = element as Connector;
                                Connector actionLine = element.ActionElement as Connector;

                                //Get the two points of the segment
                                ConnectorHandle handle = mouseElements.MouseHandle as ConnectorHandle;
                                if (handle != null)
                                {
                                    actionLine.Points[handle.Index - 1] = (PointF)connectorLine.Points[handle.Index - 1];
                                    actionLine.Points[handle.Index] = (PointF)connectorLine.Points[handle.Index];
                                    actionLine.RefinePoints();
                                    actionLine.DrawPath();
                                    actionLine.LocatePorts();
                                    actionLine.CreateHandles();
                                }
                            }
                            //Update start or end of connector
                            else if (mouseElements.MouseHandle.Type == HandleType.Origin)
                            {
                                Connector connectorLine = element as Connector;
                                Connector actionLine = element.ActionElement as Connector;

                                actionLine.SetPoints(connectorLine.Points);
                                actionLine.RefinePoints();

                                //Set origins
                                if (!actionLine.Start.Docked) actionLine.Start.Location = connectorLine.FirstPoint;
                                if (!actionLine.End.Docked) actionLine.End.Location = connectorLine.LastPoint;

                                actionLine.GetPortPercentages();
                                actionLine.DrawPath();
                                actionLine.LocatePorts();
                            }
                            //Move all points if connector is not connected
                            else if (mouseElements.MouseHandle.Type == HandleType.Move)
                            {
                                Connector connectorLine = element as Connector;
                                Connector actionLine = element.ActionElement as Connector;

                                if (actionLine.AllowMove && !actionLine.Start.Docked && !actionLine.End.Docked)
                                {
                                    actionLine.Points.Clear();

                                    foreach (PointF point in connectorLine.Points)
                                    {
                                        actionLine.Points.Add(point);
                                    }

                                    actionLine.DrawPath();
                                    actionLine.LocatePorts();
                                }
                            }
                        }
                        else
                        {
                            Link line = (Link)element;
                            Link actionLine = (Link)element.ActionElement;

                            //Round values if appropriate
                            if (Controller.RoundPixels)
                            {
                                line.Start.Location = Point.Round(line.Start.Location);
                                line.End.Location = Point.Round(line.End.Location);
                            }

                            if (!actionLine.Start.Docked) actionLine.Start.Location = line.Start.Location;
                            if (!actionLine.End.Docked) actionLine.End.Location = line.End.Location;

                            actionLine.DrawPath();
                            actionLine.LocatePorts();
                        }
                    }

                    if (element is Port)
                    {
                        Port actionPort = element as Port;
                        Port port = actionPort.ActionElement as Port;

                        port.Location = actionPort.Location;
                    }
                }
            }

            //Update the line docking
            if (mouseElements != null && mouseElements.MouseStartOrigin != null && mouseElements.MouseStartOrigin.AllowMove && mouseElements.MouseMoveElement != null && mouseElements.IsDockable() && Controller.CanDock(InteractiveMode, mouseElements))
            {
                Link line = mouseElements.MouseStartElement as Link;

                //Dock start to shape
                if (mouseElements.MouseStartOrigin == line.Start && mouseElements.MouseMoveElement is Shape)
                {
                    line.Start.Shape = mouseElements.MouseMoveElement as Shape;
                }
                //Dock end to shape
                if (mouseElements.MouseStartOrigin == line.End && mouseElements.MouseMoveElement is Shape)
                {
                    line.End.Shape = mouseElements.MouseMoveElement as Shape;
                }
                //Dock start to port
                if (mouseElements.MouseStartOrigin == line.Start && mouseElements.MouseMoveElement is Port)
                {
                    line.Start.Port = mouseElements.MouseMoveElement as Port;
                }
                //Dock end to port
                if (mouseElements.MouseStartOrigin == line.End && mouseElements.MouseMoveElement is Port)
                {
                    line.End.Port = mouseElements.MouseMoveElement as Port;
                }
            }

            Executed = true;
        }
        
        protected internal virtual void SetVectors(RectangleF[] vectors)
        {
            _vectors = vectors;
        }
    }
}
