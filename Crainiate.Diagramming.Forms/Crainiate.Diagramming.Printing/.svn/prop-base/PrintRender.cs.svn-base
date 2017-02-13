using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

using Crainiate.Diagramming.Forms;
using Crainiate.Diagramming.Forms.Rendering;

namespace Crainiate.Diagramming.Printing
{
	public class PrintRender: Render
	{
		//Property variables
		private Color _backcolor = SystemColors.Window;
		private bool _selectedOnly;

		private bool _drawShadows = false;
		private bool _drawBackground = true;
		private bool _greyscale = false;

		//Working Variables
        private Graphics _graphics;
	
		#region Interface

		//Constructors
		public PrintRender()
		{
		}

		public virtual bool SelectedOnly
		{
			get
			{
				return _selectedOnly;
			}
			set
			{
				_selectedOnly = value;
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
				_drawBackground = value;
			}
		}

		public virtual bool DrawShadows
		{
			get
			{
				return _drawShadows;
			}
			set
			{
				_drawShadows = value;
			}
		}

		public virtual bool Greyscale
		{
			get
			{
				return _greyscale;
			}
			set
			{
				_greyscale = value;
			}
		}

        public virtual void SetGraphics(Graphics graphics)
        {
            _graphics = graphics;
        }

		#endregion

        public override Graphics GetGraphics(Rectangle renderRectangle, Paging paging)
        {
            return _graphics;
        }

        public override Color AdjustColor(Color color, float width, float opacity)
        {
            Color result = base.AdjustColor(color, width, opacity);
            
            if (Greyscale)
			{
				int average = (Convert.ToInt32(result.R) + Convert.ToInt32(result.G) + Convert.ToInt32(result.B)) / 3;
				return Color.FromArgb(result.A,average,average,average);
			}

            return color;
        }
	}
}
