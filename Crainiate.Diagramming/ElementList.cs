// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;

using Crainiate.Diagramming.Collections;

namespace Crainiate.Diagramming
{
	public sealed class ElementList: Crainiate.Diagramming.Collections.List<Element>	
	{
        //Property variables
        private IComparer<Element> _comparer;

        //Events
        public event EventHandler ChangeOrder;

        //Constructors
        public ElementList(bool modifiable): base()
        {
            SetModifiable(modifiable);
        }

        protected internal ElementList(): base()
        {
            SetModifiable(true);
        }

        //Sets the comparer used to sort elements contained in the collection
        public IComparer<Element> Comparer
        {
            get
            {
                if (_comparer == null)  _comparer = new RenderListComparer();
                return _comparer;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("Comparer", "Comparer may not be null.");
                _comparer = value;
            }
        }

        public Element this[string key]
        {
            get
            {
                foreach (Element element in this)
                {
                    if (element.Key == key) return element;
                }

                return null;
            }
        }

        //Create an iterator that returns all elements in this layer
        public IEnumerable<Element> Containing(Layer layer)
        {
            foreach (Element element in this)
            {
                  if (element.Layer == layer) yield return element;  
            }
        }

        //Create an iterator that returns all elements in this layer
        public bool ContainsKey(string key)
        {
            foreach (Element element in this)
            {
                if (element.Key == key) return true;
            }
            return false;
        }

        //Brings a Element to the front of the zorder.
        public void BringToFront(Element element)
        {
            foreach (Element loop in this)
            {
                if (loop.ZOrder < element.ZOrder) loop.SetOrder(loop.ZOrder + 1);
            }

            element.SetOrder(0);

            //Sort using comparer implemented in the element class
            Sort();
            OnChangeOrder();
        }
        
        //Loop through and set seected to true or false
        public void Select(bool value)
        {
            foreach (Element element in this)
            {
                if (element is ISelectable)
                {
                    ISelectable selectable = element as ISelectable;
                    selectable.Selected = value;
                }
            }
        }

        protected internal override void OnInserted(Element item)
        {
            base.OnInserted(item);

            foreach (Element loop in this)
            {
                loop.SetOrder(loop.ZOrder + 1);
            }

            item.SetOrder(0);
        }

        //Ensure that a comparer is provided as elements are not comparable
        public override void Sort()
        {
            base.Sort(Comparer);
        }

        //Returns the rectangle bounding the elements in the renderlist
        public RectangleF GetBounds()
        {
            float minx = 0;
            float miny = 0;
            float maxx = 0;
            float maxy = 0;

            bool flag = true;

            foreach (Element element in this)
            {
                if (element.Visible)
                {
                    RectangleF bounds = element.Bounds;

                    if (flag)
                    {
                        flag = false;
                        minx = bounds.Left;
                        miny = bounds.Top;
                        maxx = bounds.Right;
                        maxy = bounds.Bottom;
                    }
                    else
                    {
                        if (bounds.Left < minx) minx = bounds.Left;
                        if (bounds.Top < miny) miny = bounds.Top;
                        if (bounds.Right > maxx) maxx = bounds.Right;
                        if (bounds.Bottom > maxy) maxy = bounds.Bottom;
                    }
                }
            }

            return new RectangleF(minx, miny, maxx - minx, maxy - miny);
        }

        //Raises the ChangeOrder event.
        protected void OnChangeOrder()
        {
            if (ChangeOrder != null) ChangeOrder(this, EventArgs.Empty);
        }

        protected void SetOrder(Element element, int order)
        {
            //Get existing zorder of element
            int existingOrder = element.ZOrder;

            //Check that zorder is within bounds of collection
            if (order >= this.Count) throw new ArgumentException("Order value must be less than the number of elements.", "order");
            if (order < 0) throw new ArgumentException("Order value cannot be less than zero.", "value");
            if (existingOrder == order) return;

            //Loop through internal collection
            foreach (Element loop in this)
            {
                if (loop == element)
                {
                    loop.SetOrder(order);
                }
                else
                {
                    if (existingOrder < order)
                    {
                        if (loop.ZOrder <= order && loop.ZOrder > existingOrder) loop.SetOrder(loop.ZOrder - 1);
                    }
                    else
                    {
                        if (loop.ZOrder >= order && loop.ZOrder < existingOrder) loop.SetOrder(loop.ZOrder + 1);
                    }
                }
            }
        }
	}
}