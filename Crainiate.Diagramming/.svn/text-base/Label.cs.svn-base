// (c) Copyright Crainiate Software 2010

using System;
using System.Drawing;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	public class Label: ICloneable, IRenderable
	{
		//Property variables
		private Element _parent;

		private Font _font;
		
		private string _text;
		private Color _color;
		private byte _opacity;
		
		private PointF _offset;
        private bool _visible;
        private bool _wrap;

        private SizeF _size;
        private StringFormat _stringFormat;

		//Events
		public event EventHandler LabelInvalid;

		//Constructors
		public Label()
		{
			Color = Color.Black;
			Opacity = Singleton.Instance.DefaultOpacity;
            Visible = true;
            Wrap = true;
		}

		public Label(string text): this()
		{
			Text = text;
		}

		public Label(Label prototype)
		{
			_text = prototype.Text;
			_color = prototype.Color;
			_opacity = prototype.Opacity;
			_visible = prototype.Visible;
			_offset = prototype.Offset;

			//Check if font, alignment and line alignment are defaults, else clone
			_font = (prototype.Font == Singleton.Instance.DefaultFont) ? null : (Font) prototype.Font.Clone();

            _size = prototype.Size;
            _stringFormat = prototype.StringFormat;
            _visible = prototype.Visible;
            _wrap = prototype.Wrap;
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
				_text = value;
				OnLabelInvalid();
			}
		}

		//The color used to draw the text.
		public virtual Color Color
		{
			get
			{
				return _color;
			}
			set
			{
				_color = value;
				OnLabelInvalid();
			}
		}

		public virtual PointF Offset
		{
			get
			{
				return _offset;
			}
			set
			{
				if (!_offset.Equals(value))
				{
					_offset = value;
					OnLabelInvalid();
				}
			}
		}

		//Defines the percentage opacity for this annotation.
		public virtual byte Opacity
		{
			get
			{
				return _opacity;
			}
			set
			{
				if (_opacity != value)
				{
					_opacity = value;
					OnLabelInvalid();
				}
			}
		}
		
		//Indicates whether the shape is currently visible and rendered during drawing operations.
		public virtual bool Visible
		{
			get
			{
				return _visible;
			}
			set
			{
				if (_visible != value)
				{
					_visible = value;
					OnLabelInvalid();
				}
			}
		}

        //Indicates whether the shape is currently visible and rendered during drawing operations.
        public virtual bool Wrap
        {
            get
            {
                return _wrap;
            }
            set
            {
                if (_wrap != value)
                {
                    _wrap = value;
                    OnLabelInvalid();
                }
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
				OnLabelInvalid();
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
				OnLabelInvalid();
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
				OnLabelInvalid();
			}
		}

		public virtual bool Bold
		{
			get
			{
				return Font.Bold;
			}
			set
			{
				FontStyle = Singleton.Instance.GetFontStyle(value,Italic,Underline,Strikeout);
			}
		}

		public virtual bool Italic
		{
			get
			{
				return Font.Italic;
			}
			set
			{
				FontStyle = Singleton.Instance.GetFontStyle(Bold,value,Underline,Strikeout);
			}
		}

		public virtual bool Underline
		{
			get
			{
				return Font.Underline;
			}
			set
			{
				FontStyle = Singleton.Instance.GetFontStyle(Bold,Italic,value,Strikeout);
			}
		}

		public virtual bool Strikeout
		{
			get
			{
				return Font.Strikeout;
			}
			set
			{
				FontStyle = Singleton.Instance.GetFontStyle(Bold,Italic,Underline,value);
			}
		}

		public virtual Element Parent
		{
			get
			{
				return _parent;
			}
		}

        public virtual SizeF Size
        {
            get
            {
                return _size;
            }
            set
            {
                if (!_size.Equals(value))
                {
                    _size = value;
                    OnLabelInvalid();
                }
            }
        }

        public virtual StringAlignment Alignment
        {
            get
            {
                if (_stringFormat == null) return Singleton.Instance.DefaultStringFormat.Alignment;
                return _stringFormat.Alignment;
            }
            set
            {
                CheckStringFormatDefault();
                _stringFormat.Alignment = value;
                OnLabelInvalid();
            }
        }

        public virtual StringAlignment LineAlignment
        {
            get
            {
                if (_stringFormat == null) return Singleton.Instance.DefaultStringFormat.LineAlignment;
                return _stringFormat.LineAlignment;
            }
            set
            {
                CheckStringFormatDefault();
                _stringFormat.LineAlignment = value;
                OnLabelInvalid();
            }
        }

        public virtual StringTrimming Trimming
        {
            get
            {
                if (_stringFormat == null) return Singleton.Instance.DefaultStringFormat.Trimming;
                return _stringFormat.Trimming;
            }
            set
            {
                CheckStringFormatDefault();
                _stringFormat.Trimming = value;
                OnLabelInvalid();
            }
        }

        protected virtual StringFormat StringFormat
        {
            get
            {
                if (_stringFormat == null) return Singleton.Instance.DefaultStringFormat;
                return _stringFormat;
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

        //Contains a reference the current renderer being used to render the element
        public IRenderer Renderer { get; set; }

        //Methods
        public virtual void Reset()
        {
            _size = new SizeF();
            Offset = new PointF();
            _stringFormat = null;
            OnLabelInvalid();
        }

        public virtual StringFormat GetStringFormat()
        {
            return (StringFormat)StringFormat.Clone();
        }

        //Sets the internal string format directly
        protected internal virtual void SetFormat(StringFormat format)
        {
            _stringFormat = format;
        }
	
		//Methods
		public void SetParent(Element parent)
		{
			_parent = parent;
		}

		//Raises the element invalid event.
		protected virtual void OnLabelInvalid()
		{
			if (LabelInvalid != null) LabelInvalid(this,EventArgs.Empty);
		}

		//Sets the internal font directly
		protected internal virtual void SetFont(Font font)
		{
			_font = font;
		}

		//Returns a clone of this object
		public virtual object Clone()
		{
			return new Label(this);
		}

        //Clones the default string format
        private void CheckStringFormatDefault()
        {
            if (_stringFormat == null) _stringFormat = (StringFormat)Singleton.Instance.DefaultStringFormat.Clone();
        }
	}
}
