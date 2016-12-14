// (c) Copyright Crainiate Software 2010

using System;
using System.Drawing;
using System.Collections;

using Crainiate.Diagramming.Collections;

namespace Crainiate.Diagramming
{
    public class ClipboardCommand: Command
    {
        //Property variables
        private string _commandText;
        private PointF _location;
        private Shapes _shapes;
        private Lines _lines;

        //Working variables
        private ModelMomento _modelMomento;
                
        //Constructors
        public ClipboardCommand(Controller controller): base(controller)
        {
            if (controller == null) throw new ArgumentNullException();
            _modelMomento = new ModelMomento(controller.Model);
            _modelMomento.WriteItem(controller.Model);
        }

        public virtual string CommandText
        {
            get
            {
                return _commandText;
            }
            set
            {
                _commandText = value;
            }
        }

        public virtual Shapes Shapes
        {
            get
            {
                return _shapes;
            }
            set
            {
                _shapes = value;
            }
        }

        public virtual Lines Lines
        {
            get
            {
                return _lines;
            }
            set
            {
                _lines = value;
            }
        }

        public PointF Location
        {
            get 
            { 
                return _location; 
            }
            set 
            { 
                _location = value; 
            }
        }

        //Implementation
        public override void Execute()
        {
            //Make comand lower case
            string command = CommandText.ToLower();

            switch (command)
            {
                case "cut":
                    DoCutCopyDelete(true, true);
                    Controller.Clipboard.IsCopy = false;
                    break;

                case "copy":
                    DoCutCopyDelete(true, false);
                    Controller.Clipboard.IsCopy = true;
                    break;

                case "delete":
                    DoCutCopyDelete(false, true);
                    Controller.Refresh();
                    break;

                case "paste":
                    Controller.Suspend();
                    Controller.SelectElements(false);
                    Controller.Clipboard.Read();
                    Controller.Clipboard.ResolveItems();
                    DoPaste(Location);
                    Controller.Clipboard.IsCopy = true;
                    Controller.Resume();
                    Controller.Refresh();
                    break;
            }
        }

        public override void Undo()
        {
            _modelMomento.WriteItem(Controller.Model);
        }

        public override void Redo()
        {
            _modelMomento.WriteItem(Controller.Model);
        }

        public virtual bool GetCommand()
        {
            //Make comand lower case
            string command = CommandText.ToLower();

            switch (command)
            {
                case "cut":
                    return (Shapes.Count > 0 || Lines.Count > 0);
                    break;
                case "copy":
                    return (Shapes.Count > 0 || Lines.Count > 0);
                    break;
                case "delete":
                    return (Shapes.Count > 0 || Lines.Count > 0);
                    break;
                case "paste":
                    return (Controller.Clipboard.Elements != null);
                    break;
            }

            return false;
        }

        //Loop through and add if cut or copy  selected shapes, lines with either shape selected, selected lines 
        //Remove if cut or delete 
        //Do not rewrite keys in copy buffer
        private void DoCutCopyDelete(bool add, bool remove)
        {
            ElementList removeShapes = new ElementList(false);
            ElementList removeLines = new ElementList(false);

            Controller.Clipboard.Elements = new Elements();
            Controller.Suspend();

            //Add any cut/copied shapes to a remove list
            DoCutCopyDeleteSelectedElement<Shape>(add, remove, Shapes, removeShapes);
            DoCutCopyDeleteSelectedElement<Line>(add, remove, Lines, removeLines);

            //Write the items to the clipboard before they are removed and change the origins etc
            Controller.Clipboard.Write();

            //Remove any shapes from the collection
            if (remove)
            {
                foreach (Shape shape in removeShapes)
                {
                    shape.Model.Shapes.Remove(shape.Key);
                }

                foreach (Link line in removeLines)
                {
                    line.Model.Lines.Remove(line.Key);
                }
            }

            Controller.Resume();
            Controller.Refresh();
        }

        //Loop through each item in the copycut buffer and add it back to the diagram
        //Paste into centre of container
        private void DoPaste(PointF location)
        {
            if (Controller.Clipboard.Elements == null) return;

            //Create a renderlist and add the elements
            ElementList list = new ElementList();
            foreach (Element element in Controller.Clipboard.Elements.Values)
            {
                list.Add(element);
            }

            //Get the bounds
            RectangleF bounds = list.GetBounds();

            //Calculate
            float dx = -bounds.X + ((Controller.Model.Size.Width - bounds.Width) / 2);
            float dy = -bounds.Y + ((Controller.Model.Size.Height - bounds.Height) / 2);

            dx = Convert.ToSingle(Math.Round(dx, 0));
            dy = Convert.ToSingle(Math.Round(dy, 0));

            //Loop through each element and add
            if (Controller.Clipboard.Elements != null)
            {
                //Add Shapes
                foreach (Element element in Controller.Clipboard.Elements.Values)
                {
                    if (element is Shape)
                    {
                        Shape shape = (Shape) element;
                        Shape newShape = (Shape) shape.Clone();
                        string key = null;

                        //Set key
                        key = (Controller.Model.Shapes.ContainsKey(shape.Key)) ? Controller.Model.Shapes.CreateKey() : shape.Key;

                        //Set temporary action element
                        shape.ActionElement = newShape;

                        //Change any settings required
                        newShape.SetLayer(null);
                        newShape.Selected = true;

                        //Offset by container offset
                        newShape.Move(dx, dy);

                        //Round values if appropriate
                        if (Controller.RoundPixels)
                        {
                            newShape.Location = Point.Round(newShape.Location);
                            newShape.Size = System.Drawing.Size.Round(newShape.Size);
                        }

                        //Add if allowed by the runtime
                        if (Controller.CanAdd(newShape)) Controller.Model.Shapes.Add(key, newShape);
                    }
                }

                //Add Lines
                foreach (Element element in Controller.Clipboard.Elements.Values)
                {
                    if (element is Link)
                    {
                        Link line = (Link)element;
                        Link newLine = (Link)line.Clone();
                        string key = null;

                        //Define the key
                        key = (Controller.Model.Lines.ContainsKey(line.Key)) ? Controller.Model.Lines.CreateKey() : key = line.Key;

                        //Change any settings required
                        newLine.SetLayer(null);
                        newLine.Selected = true;

                        if (element is ComplexLine)
                        {
                            ComplexLine complexLine = (ComplexLine)newLine;
                            Segment segment = null;

                            for (int i = 0; i < complexLine.Segments.Count; i++)
                            {
                                segment = complexLine.Segments[i];
                                segment.Start.Move(dx, dy);
                            }
                        }
                        else if (element is Curve)
                        {
                            Curve curve = (Curve)newLine;

                            curve.Start.Move(dx, dy);
                            curve.End.Move(dx, dy);

                            PointF[] newPoints = new PointF[curve.ControlPoints.GetUpperBound(0) + 1];
                            for (int i = 0; i <= curve.ControlPoints.GetUpperBound(0); i++)
                            {
                                newPoints[i] = new PointF(curve.ControlPoints[i].X + dx, curve.ControlPoints[i].Y + dy);
                            }
                            curve.ControlPoints = newPoints;
                        }
                        else if (element is Connector) //Connectors cannot be moved in this way
                        {
                            Connector conn = (Connector)newLine;
                            List<PointF> points = new List<PointF>();

                            foreach (PointF point in conn.Points)
                            {
                                points.Add(new PointF(point.X + dx, point.Y + dy));
                            }
                            conn.SetPoints(points);
                            conn.DrawPath();
                        }
                        else
                        {
                            newLine.Start.Move(dx, dy);
                            newLine.End.Move(dx, dy);
                            newLine.DrawPath();
                        }

                        //Reconnect start origins
                        if (line.Start.DockedElement != null)
                        {
                            if (line.Start.Shape != null && line.Start.Shape.ActionElement != null)
                            {
                                newLine.Start.Shape = (Shape)line.Start.Shape.ActionElement;
                            }
                            else if (line.Start.Port != null && line.Start.Port.Parent is Shape)
                            {
                                Shape shape = (Shape)line.Start.Port.Parent;
                                Shape newShape = (Shape)shape.ActionElement;

                                newLine.Start.Port = (Port)newShape.Ports[line.Start.Port.Key];
                            }
                        }

                        //Reconnect end origins
                        if (line.End.DockedElement != null)
                        {
                            if (line.End.Shape != null && line.End.Shape.ActionElement != null)
                            {
                                newLine.End.Shape = (Shape)line.End.Shape.ActionElement;
                            }
                            else if (line.End.Port != null && line.End.Port.Parent is Shape)
                            {
                                Shape shape = (Shape)line.End.Port.Parent;
                                Shape newShape = (Shape)shape.ActionElement;

                                newLine.End.Port = (Port)newShape.Ports[line.End.Port.Key];
                            }
                        }

                        //Create and add to lines collection
                        if (Controller.CanAdd(newLine)) Controller.Model.Lines.Add(key, newLine);
                    }
                }

                //Remove any temporary action elements
                foreach (Element element in Controller.Clipboard.Elements.Values)
                {
                    element.ActionElement = null;
                }
            }
        }

        private void DoCutCopyDeleteSelectedElement<T>(bool add, bool remove, Elements<T> collection, ElementList results)
            where T:Element
        {
            //Add any cut/copied lines 
            foreach (Element element in collection.Values)
            {
                if (element is ISelectable)
                {
                    ISelectable selectable = element as ISelectable;

                    if (selectable.Selected)
                    {
                        if (remove && Controller.CanDelete(element)) results.Add(element);
                        if (add) Controller.Clipboard.Elements.Add(element.Key, element);
                    }
                }
            }
        }

        private void DoCutCopyDeleteSelectedLines(bool add, bool remove, Lines collection, ElementList results)
        {
            //Add any cut/copied lines 
            foreach (Link line in collection.Values)
            {
                if (line.Selected)
                {
                    if (remove && Controller.CanDelete(line)) results.Add(line);
                    if (add) Controller.Clipboard.Elements.Add(line.Key, line);
                }
            }
        }
    }
}
