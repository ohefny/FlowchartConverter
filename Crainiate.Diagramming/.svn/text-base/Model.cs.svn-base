// (c) Copyright Crainiate Software 2010




using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using Crainiate.Diagramming.Layouts;
using Crainiate.Diagramming.Collections;

namespace Crainiate.Diagramming
{
    //Model is part of the Model-View-Controller design pattern
    public class Model
    {
        private Shapes _shapes;
        private Lines _lines;
        private ElementList _elements;
        private Route _route;
        private SizeF _size;

        private Layers _layers;
        private Elements _selectedElements;
        private Shapes _selectedShapes;
        private Lines _selectedLines;

        //Events
        public event ElementsEventHandler ElementInserted;
        public event ElementsEventHandler ElementRemoved;
        public event ElementEventHandler ElementInvalid;
        public event EventHandler ModelInvalid;

        //Constructors
        public Model()
        {
            Clear();
            Layers = new Layers(this);

            //Set initial size to a4 
            SetSize(Geometry.GetPaperSize(DiagramUnit.Pixel, Crainiate.Diagramming.Printing.PaperSizes.Iso.A4));
        }

        //Properties
        public virtual Shapes Shapes
        {
            get
            {
                return _shapes;
            }
        }

        public virtual Lines Lines
        {
            get
            {
                return _lines;
            }
        }

        //A list of shape and line elements in the correct order to render
        public virtual ElementList Elements
        {
            get
            {
                return _elements;
            }
        }

        public virtual SizeF Size
        {
            get
            {
                return _size;
            }
        }

        public virtual Route Route
        {
            get
            {
                return _route;
            }
            set
            {
                if (value == null) throw new ArgumentNullException();
                _route = value;
            }
        }

        public virtual Layers Layers
        {
            get
            {
                return _layers;
            }
            set
            {
                if (value == null) throw new ArgumentNullException();

                _layers = value;
            }
        }

        public PointF GetOffset()
        {
            return new Point(0, 0);
        }

        public bool Contains(PointF location)
        {
            SizeF size = Size;

            RectangleF rectangle = new RectangleF(0, 0, size.Width, size.Height);
            return rectangle.Contains(location);
        }

        public virtual void Clear()
        {
            SetShapes(new Shapes(this));
            SetLines(new Lines(this));

            _elements = new ElementList(false);
            Route = new Route(this);
        }

        public virtual void SetSize(SizeF size)
        {
            _size = size;
        }

        public void SetLines(Lines lines)
        {
            _lines = lines;

            if (_lines != null)
            {
                _lines.InsertElement += new ElementsEventHandler(Element_Insert);
                _lines.RemoveElement += new ElementsEventHandler(Element_Remove);
                _lines.Cleared += new EventHandler(Lines_Cleared);
            }
        }

        public void SetElements(ElementList elements)
        {
            _elements = elements;
        }

        public virtual Elements SelectedElements()
        {
            if (_selectedElements == null) GetSelectedElements();
            return _selectedElements;
        }

        public virtual Elements SelectedElements(System.Type type)
        {
            return GetSelectedElements(type);
        }

        public virtual Shapes SelectedShapes()
        {
            if (_selectedShapes == null) GetSelectedElements();
            return _selectedShapes;
        }

        public virtual Lines SelectedLines()
        {
            if (_selectedLines == null) GetSelectedElements();
            return _selectedLines;
        }

        //Apply a theme to all elements in the model
        public virtual void ApplyTheme(Theme theme)
        {
            SetTheme(theme);
        }

        public virtual void ApplyTheme(Themes themes)
        {
            SetTheme(Singleton.Instance.GetTheme(themes));
        }

        //Clears the selected element collection
        public void ResetSelectedElements()
        {
            _selectedElements = null;
            _selectedShapes = null;
            _selectedLines = null;
        }

        protected internal virtual void SetShapes(Shapes shapes)
        {
            _shapes = shapes;

            if (_shapes != null)
            {
                _shapes.InsertElement += new ElementsEventHandler(Element_Insert);
                _shapes.RemoveElement += new ElementsEventHandler(Element_Remove);
                _shapes.Cleared += new EventHandler(Shapes_Cleared);
            }
        }

        protected virtual void OnElementInserted(Element element)
        {
            if (ElementInserted != null) ElementInserted(this, new ElementsEventArgs(element));
        }

        protected virtual void OnElementRemoved(Element element)
        {
            if (ElementRemoved != null) ElementRemoved(this, new ElementsEventArgs(element));
        }

        protected virtual void OnElementInvalid(Element element)
        {
            if (ElementInvalid != null) ElementInvalid(this, new ElementEventArgs(element));
        }

        protected virtual void OnModelInvalid()
        {
            Route.Reform();
            if (ModelInvalid != null) ModelInvalid(this, EventArgs.Empty);
        }

        //Occurs when an element is added to the elements collection
        private void Element_Insert(object sender, ElementsEventArgs e)
        {
            Element element = e.Value;

            //Set handlers
            element.ElementInvalid += new EventHandler(Element_ElementInvalid);

            //Add to the renderlist
            Elements.SetModifiable(true);
            Elements.Add(element);
            Elements.SetModifiable(false);

            //Raise the ElementInserted event
            OnElementInserted(element);
            OnModelInvalid();
        }

        //Occurs when an element is removed from the elements collection
        private void Element_Remove(object sender, ElementsEventArgs e)
        {
            if (e.Value is Shape) ResetLines((Shape)e.Value);

            //Remove from the renderlist
            Elements.Remove(e.Value);

            //Raise the ElementRemovedEvent
            OnElementRemoved(e.Value);
            OnModelInvalid();
        }

        //Occurs when an element raises an invalid event
        private void Element_ElementInvalid(object sender, EventArgs e)
        {
            OnModelInvalid();
        }

        //Remove all Shape references from layers and rerender
        private void Shapes_Cleared(object sender, EventArgs e)
        {
            //Undo line references
            foreach (Link line in Lines.Values)
            {
                if (line.Start.DockedElement != null && line.Start.DockedElement is Shape) ResetLines((Shape)line.Start.DockedElement);
                if (line.End.DockedElement != null && line.End.DockedElement is Shape) ResetLines((Shape)line.End.DockedElement);
            }
        }

        //Remove all Line references from layers and rerender
        private void Lines_Cleared(object sender, EventArgs e)
        {

        }

        private void ResetLines(Shape shape)
        {
            //Loop through each line and create a remove list
            foreach (Link line in Lines.Values)
            {
                if (line.Start.DockedElement != null && line.Start.DockedElement == shape) line.Start.Location = line.FirstPoint;
                if (line.End.DockedElement != null && line.End.DockedElement == shape) line.End.Location = line.LastPoint;
            }
        }

        //Sets the selected working elements
        private void GetSelectedElements()
        {
            _selectedElements = new Elements();
            _selectedShapes = new Shapes(this);
            _selectedLines = new Lines(this);
            
            _selectedElements.SetModifiable(true);
            _selectedShapes.SetModifiable(true);
            _selectedLines.SetModifiable(true);

            foreach (Shape shape in Shapes.Values)
            {
                if (shape.Selected)
                {
                    _selectedElements.Add(shape.Key, shape);
                    _selectedShapes.Add(shape.Key, shape);
                }
            }

            foreach (Link line in Lines.Values)
            {
                if (line.Selected)
                {
                    _selectedElements.Add(line.Key, line);
                    _selectedLines.Add(line.Key, line);
                }
            }

            _selectedElements.SetModifiable(false);
            _selectedShapes.SetModifiable(false);
            _selectedLines.SetModifiable(false);
        }

        private Elements GetSelectedElements(System.Type type)
        {
            Elements elements = new Elements();

            foreach (Shape shape in Shapes.Values)
            {
                if (shape.Selected && (type.IsInstanceOfType(shape) || shape.GetType().IsSubclassOf(type))) elements.Add(shape.Key, shape);
            }

            foreach (Link line in Lines.Values)
            {
                if (line.Selected && (type.IsInstanceOfType(line) || line.GetType().IsSubclassOf(type))) elements.Add(line.Key, line);
            }

            elements.SetModifiable(false);
            return elements;
        }

        private void SetTheme(Theme theme)
        {
            //Set the level for all the shapes in the group
            foreach (Element element in Elements)
            {
                element.ApplyTheme(theme);
            }
        }
    }
}
