// (c) Copyright Crainiate Software 2010

using System;
using System.Drawing;

using Crainiate.Diagramming;

namespace Crainiate.Diagramming.Forms
{
	public class Tab: Layer 
	{
		//Property variables
		private RectangleF _rectangle;
		private bool _pressed;
		private bool _buttonPressed;
		private bool _buttonEnabled;
		private ButtonStyle _buttonStyle;
		private RectangleF _buttonRectangle;
		private float _scroll;

		#region Interface

		//Events
		public event EventHandler TabInvalid;

		//Constructor
		public Tab():base()
		{
		}

        public Tab(bool defaultTab): base(defaultTab)
        {
        }

		//Gets or sets the offset from the top of the palette
		public virtual RectangleF Rectangle
		{
			get
			{
				return _rectangle;
			}
		}

		public virtual RectangleF ButtonRectangle
		{
			get
			{
				return _buttonRectangle;
			}
		}

		internal bool Pressed
		{
			get
			{
				return _pressed;
			}
			set
			{
				_pressed = value;
			}
		}

		internal bool ButtonPressed
		{
			get
			{
				return _buttonPressed;
			}
			set
			{
				_buttonPressed = value;
			}
		}

		internal bool ButtonEnabled
		{
			get
			{
				return _buttonEnabled;
			}
			set
			{
				_buttonEnabled = value;
			}
		}

		internal ButtonStyle ButtonStyle
		{
			get
			{
				return _buttonStyle;
			}
			set
			{
				_buttonStyle = value;
			}
		}

		internal float Scroll
		{
			get
			{
				return _scroll;
			}
			set
			{
				_scroll = value;
			}
		}

		//Methods
		protected internal virtual void SetRectangle(RectangleF value)
		{
			_rectangle = value;
		}

		protected internal virtual void SetButtonRectangle(RectangleF value)
		{
			_buttonRectangle = value;
		}

		protected virtual void OnTabInvalid()
		{
			if (TabInvalid !=null) TabInvalid(this,EventArgs.Empty);
		}

		#endregion
	}
}
