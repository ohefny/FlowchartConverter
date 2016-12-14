// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	//Contains appearance information that can be applied to all elements in a diagram
	public class Theme
	{
		//Property variables
		private Pen _customPen;
		private byte _opacity;
		private Color _borderColor;
		private DashStyle _borderStyle;
		private float _borderWidth;
		private SmoothingMode _smoothingMode;

        private Color _backColor;
        private Color _gradientColor;
        private LinearGradientMode _gradientMode;
        private Brush _customBrush;

        public Theme()
        {
            _opacity = 100;
            _smoothingMode = SmoothingMode.HighQuality;
            _borderWidth = 2;
            _backColor = SystemColors.Window;
            _gradientMode = LinearGradientMode.ForwardDiagonal;
        }

		//The color used to draw the shape's borders.
		public virtual Color BorderColor
		{
			get
			{
				return _borderColor;
			}
			set
			{
				_borderColor = value;
			}
		}

		//Sets or retrieves the dash style used to draw the shape's border.
		public virtual DashStyle BorderStyle
		{
			get
			{
				return _borderStyle;
			}
			set
			{
				_borderStyle = value;
			}
		}

		//Sets or retrieves the width used to draw the shape's border.
		public virtual float BorderWidth
		{
			get
			{
				return _borderWidth;
			}
			set
			{
				_borderWidth = value;
			}
		}

		//gets or sets the value determining how anti aliasing is performed
		public virtual SmoothingMode SmoothingMode
		{
			get
			{
				return _smoothingMode;
			}
			set
			{
				_smoothingMode = value;
			}
		}

		//Defines the percentage opacity for the background of this shape.
		public virtual byte Opacity
		{
			get
			{
				return _opacity;
			}
			set
			{

				_opacity = value;
			}
		}

		public virtual Pen CustomPen
		{
			get
			{
				return _customPen;
			}
			set
			{
				_customPen = value;
			}
		}

        //The color used to draw the shape's background.
        public virtual Color BackColor
        {
            get
            {
                return _backColor;
            }
            set
            {
                _backColor = value;
            }
        }

        //The color used to combine the shape's background color when drawing a gradient effect.
        public virtual Color GradientColor
        {
            get
            {
                return _gradientColor;
            }
            set
            {

                _gradientColor = value;
            }
        }

        //Determines how gradients are drawn for this shape.
        public virtual LinearGradientMode GradientMode
        {
            get
            {
                return _gradientMode;
            }
            set
            {
                _gradientMode = value;
            }
        }

        //Gets or sets a custom brush used for drawing this shape.
        public virtual Brush CustomBrush
        {
            get
            {
                return _customBrush;
            }
            set
            {
                _customBrush = value;
            }
        }
	}
}
