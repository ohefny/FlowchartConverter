// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming
{
    public class EventArgs<T> : EventArgs
    {
        private T _item;

        public EventArgs(T item)
        {
            _item = item;
        }

        public T Value
        {
            get
            {
                return _item;
            }
        }
    }

	public class ElementEventArgs: EventArgs<Element>
	{
        public ElementEventArgs(Element element): base(element)
        { }
	}

	public class ElementsEventArgs: EventArgs<Element>
	{
        public ElementsEventArgs(Element element): base(element)
        { }
	}

	public class TableItemsEventArgs: EventArgs<TableItem>
	{
        public TableItemsEventArgs(TableItem item): base(item)
        { }
	}

	public class SegmentsEventArgs: EventArgs<Segment>
	{
        public SegmentsEventArgs(Segment segment): base(segment)
        { }
	}

    public class LayoutChangedEventArgs : EventArgs
    {
        private RectangleF _rectangle;

        //Contructor
        public LayoutChangedEventArgs(RectangleF rect)
        {
            _rectangle = rect;
        }

        //Properties
        public RectangleF Rectangle
        {
            set
            {
                _rectangle = value;
            }
            get
            {
                return _rectangle;
            }
        }
    }

    //Not virtual to improve rendering performance 
    public class RenderEventArgs : EventArgs
    {
        private Graphics _graphics;
        private bool _cancel;
        private Rectangle _renderRectangle;

        //Constructor
        public RenderEventArgs(Graphics graphics, Rectangle renderRectangle)
        {
            _graphics = graphics;
        }

        public Graphics Graphics
        {
            get
            {
                return _graphics;
            }
        }

        public bool Cancel
        {
            get
            {
                return _cancel;
            }
        }

        public Rectangle RenderRectangle
        {
            get
            {
                return _renderRectangle;
            }
        }
    }

    public class ElementRenderEventArgs : EventArgs
    {
        private Graphics _graphics;

        //Constructor
        public ElementRenderEventArgs(Graphics graphics)
        {
            _graphics = graphics;
        }

        public Graphics Graphics
        {
            get
            {
                return _graphics;
            }
        }
    }

    public class LayerRenderEventArgs : RenderEventArgs
    {
        private Layer _layer;

        //Constructor
        public LayerRenderEventArgs(Graphics graphics, Rectangle renderRectangle, Layer layer): base(graphics, renderRectangle)
        {
            _layer = layer;
        }

        public Layer Layer
        {
            get
            {
                return _layer;
            }
        }
    }

	public class SerializationEventArgs: EventArgs
	{
		private IFormatter _formatter;
		private SurrogateSelector _selector;

		public SerializationEventArgs(IFormatter formatter, SurrogateSelector selector)
		{
			_formatter = formatter;
			_selector = selector;
		}

		public IFormatter Formatter
		{
			get
			{
				return _formatter;
			}
		}

		public SurrogateSelector Selector
		{
			get
			{
				return _selector;
			}
		}
	}

	public class SerializationCompleteEventArgs: SerializationEventArgs
	{
		private object _graph;
		
		public SerializationCompleteEventArgs(object graph, IFormatter formatter, SurrogateSelector selector): base(formatter,selector)
		{
			_graph = graph;
		}

		public object Graph
		{
			get
			{
				return _graph;
			}
		}
	}

	public class DrawShapeEventArgs: EventArgs
	{
		private GraphicsPath _graphicsPath;
		private float mWidth;
		private float mHeight;

		//Constructor
		public DrawShapeEventArgs(GraphicsPath path, float width, float height)
		{
			_graphicsPath = path;
			mWidth = width;
			mHeight = height;
		}

		public GraphicsPath Path
		{
			get
			{
				return _graphicsPath;
			}
		}

		public float Width
		{
			get
			{
				return mWidth;
			}
		}

		public float Height
		{
			get
			{
				return mHeight;
			}
		}
	}

	public class UserActionEventArgs: EventArgs
	{
		private ElementList _actions;

		public UserActionEventArgs(ElementList actions)
		{
			_actions = actions;
		}
		public ElementList Actions
		{
			get
			{
				return _actions;
			}
		}
	}

}


