// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming
{
	public abstract class MarkerBase: Element, ICloneable
	{
		//Properties
		private Color _backColor;
		private float _width;
		private float _height;
		private bool _drawBackground;
		private bool _centered;

		private Image _image;
		private Label _label;

		#region Interface

		//Constructor
		public MarkerBase()
		{
			DrawBackground = true;
			BackColor = System.Drawing.Color.Black;
			Width = 8;
			Height = 8;
		}

		public MarkerBase(MarkerBase prototype): base(prototype)
		{
			_width = prototype.Width;
			_height = prototype.Height;
			_backColor = prototype.BackColor;
			_drawBackground = prototype.DrawBackground;
			_centered = prototype.Centered;
		}

		//Properties
		//The color used to draw the shape's background.
		public virtual System.Drawing.Color BackColor
		{
			get
			{
				return _backColor;
			}
			set
			{
				if (! _backColor.Equals(value))
				{
					_backColor = value;
					OnElementInvalid();
				}
			}
		}

		public virtual bool DrawBackground
		{
			get
			{
				return _drawBackground;
			}
			set
			{
				if (_drawBackground != value)
				{
					_drawBackground = value;
					OnElementInvalid();
				}
			}
		}

		public virtual bool Centered
		{
			get
			{
				return _centered;
			}
			set
			{
				if (_centered != value)
				{
					_centered = value;
					OnElementInvalid();
				}
			}
		}

		//Sets or gets the width of the marker
		public virtual float Width
		{
			get
			{
				return _width;
			}
			set
			{
				_width = value;
				OnElementInvalid();
			}
		}

		//Sets or gets the height of the marker
		public virtual float Height
		{
			get
			{
				return _height;
			}
			set
			{
				_height = value;
				OnElementInvalid();
			}
		}

		//Returns the Image object which which displays an image for this marker.
		public Image Image
		{
			get
			{
				return _image;
			}
			set
			{
				//Remove any existing handlers
				if (_image != null) 
				{
					_image.ImageInvalid -= new EventHandler(Image_ImageInvalid);
				}

				_image = value;
				if (_image != null) 
				{
					_image.ImageInvalid += new EventHandler(Image_ImageInvalid);
				}

				OnElementInvalid();
			}
		}

		//Returns the annotation for this segment
		public virtual Label Label
		{
			get
			{
				return _label;
			}
			set
			{
				//Remove any existing handlers
				if (_label != null)
				{
					_label.LabelInvalid -= new EventHandler(Label_LabelInvalid);
				}

				_label = value;
				if (_label != null)
				{
					_label.LabelInvalid += new EventHandler(Label_LabelInvalid);
				}
				OnElementInvalid();
			}
		}

		#endregion

		#region Events
		
		//Handles annotation invalid events
		private void Label_LabelInvalid(object sender, EventArgs e)
		{
			OnElementInvalid();
		}

		//Handles image invalid events
		private void Image_ImageInvalid(object sender, EventArgs e)
		{
			OnElementInvalid();
		}

		#endregion

		#region Implementation

		public override object Clone()
		{
			return null;
		}

		#endregion
	}
}
