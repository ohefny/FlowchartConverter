// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	public abstract class TableItem: IRenderable
	{
		//Property variables
		private string _text;
		private Font _font;
		private Color _forecolor;
		private Color _backcolor;
		private float _indent;
		private object _tag;
		private Table _table;
		private TableItem _parent;

		//Working variables
		private RectangleF _rectangle;
		
		//Events
		public event EventHandler TableItemInvalid;
		
		//Constructors
		public TableItem()
		{
			Forecolor = Color.Black;
			Backcolor = Color.FromArgb(235, 235, 235);
		}

        public TableItem(string text): this()
        {
            Text = text;
        }

		public TableItem(TableItem prototype)
		{
			_text = prototype.Text;
			_font = prototype.Font;
			_forecolor = prototype.Forecolor;
			_backcolor = prototype.Backcolor;
			_indent = prototype.Indent;
			_tag = prototype.Tag;
			_parent = prototype.Parent;
			_table = prototype.Table;
		}

		//Properties
		public virtual string Text
		{
			get
			{
				return _text;
			}
			set
			{
				if (_text != value)
				{
					_text = value;
					OnTableItemInvalid();
				}
			}
		}

		public virtual Color Forecolor
		{
			get
			{
				return _forecolor;
			}
			set
			{
				_forecolor = value;
				OnTableItemInvalid();
			}
		}

		public virtual Color Backcolor
		{
			get
			{
				return _backcolor;
			}
			set
			{
				_backcolor = value;
				OnTableItemInvalid();
			}
		}

		public Font Font
		{
			get 
			{
				if (_font == null) return Singleton.Instance.DefaultFont;
				return _font;
			}
		}

		public virtual string FontName
		{
			get
			{
				if (_font == null) return Singleton.Instance.DefaultFont.FontFamily.Name;
				return _font.FontFamily.Name;
			}
			set
			{
				_font = Singleton.Instance.GetFont(value,FontSize,FontStyle);
				OnTableItemInvalid();
			}
		}

		public virtual float FontSize
		{
			get
			{
				if (_font == null) return Singleton.Instance.DefaultFont.Size;
				return _font.Size;
			}
			set
			{
				_font = Singleton.Instance.GetFont(FontName,value,FontStyle);
				OnTableItemInvalid();
			}
		}

		public virtual FontStyle FontStyle
		{
			get
			{
				if (_font == null) return Singleton.Instance.DefaultFont.Style;
				return _font.Style;
			}
			set
			{
				_font = Singleton.Instance.GetFont(FontName,FontSize,value);
				OnTableItemInvalid();
			}
		}

		public virtual RectangleF Rectangle
		{
			get
			{
				return _rectangle;
			}
		}

		public virtual float Indent
		{
			get
			{
				return _indent;
			}
		}

		//Sets or gets the tag for this object.
		public virtual object Tag
		{
			get
			{
				return _tag;
			}
			set
			{
				_tag = value;
			}
		}

		public virtual Table Table
		{
			get
			{
				return _table;
			}
		}

		public virtual TableItem Parent
		{
			get
			{
				return _parent;
			}
		}

        //Contains a reference the current renderer being used to render the element
        public IRenderer Renderer { get; set; }

		//Methods
		public void SetItemRectangle(RectangleF rectangle)
		{
			_rectangle = rectangle;
		}

		//Sets the internal font directly
		protected internal virtual void SetFont(Font font)
		{
			_font = font;
		}

		protected internal void SetIndent(float indent)
		{
			_indent = indent;
		}

		//Sets the internal table reference
		protected internal virtual void SetTable(Table table)
		{
			_table = table;
		}

		//Sets the internal table reference
		protected internal virtual void SetParent(TableItem parent)
		{
			_parent = parent;
		}

		//Raises the table group invalid event.
		protected virtual void OnTableItemInvalid()
		{
			if (TableItemInvalid != null) TableItemInvalid(this,EventArgs.Empty);
		}
	}
}
