using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

using Crainiate.Diagramming;
using Crainiate.Diagramming.Forms;

namespace Crainiate.Diagramming.Forms.Rendering
{
	public class PaletteRender: ControlRender
	{
		private bool _upPressed;
		private bool _downPressed;

		private bool _drawScroll;
        private Palette _palette;

		//Working Variables
		private Tab _scrollTab;

        //Constructors
        public PaletteRender(Palette palette): base()
        {
            Palette = palette;

            _scrollTab = new Tab();
            _scrollTab.Visible = false;
            _scrollTab.ButtonStyle = ButtonStyle.Down;

            DecorationPen = new Pen(Color.FromArgb(66, 65, 66));
            DecorationBrush = new SolidBrush(Color.FromArgb(192,Color.White));
        }

        public virtual Palette Palette
        {
            get
            {
                return _palette;
            }
            set
            {
                if (value == null) throw new ArgumentNullException();
                _palette = value;
            }
        }

		public virtual bool DrawScroll
		{
			get
			{
				return _drawScroll;
			}
			set
			{
				_drawScroll = value;
			}
		}

		public virtual bool UpPressed
		{
			get
			{
				return _upPressed;
			}
			set
			{
				_upPressed = value;
			}
		}		

		public virtual bool DownPressed
		{
			get
			{
				return _downPressed;
			}
			set
			{
				_downPressed = value;
			}
		}

        public virtual Pen DecorationPen { get; set; }

        public virtual Brush DecorationBrush { get; set; }

		internal Tab ScrollTab
		{
			get
			{
				return _scrollTab;
			}
		}

		//sets up the internal graphics object from a bitmap back buffer
		public override Graphics GetGraphics(Rectangle renderRectangle, Paging paging)
		{
			if (renderRectangle.Width == 0 || renderRectangle.Height == 0) return null;

			Graphics graphics = null;

			try
			{
				if (GraphicsStateBitmap == null)
				{
					//Set up a new bitmap, can throw errors when in use eg whilst scrolling
					//Unlike other renderers, palette render creates the entire diagram buffer
					//This is to simplify the gradient rendering process
					DisposeBufferBitmap();
					SetBitmap(new Bitmap(Convert.ToInt32(DiagramSize.Width),Convert.ToInt32(DiagramSize.Height),PixelFormat.Format32bppPArgb));

					//Get a graphics handle from the new back buffer
					graphics = Graphics.FromImage(Bitmap);
					graphics.Clear(BackColor);

					//Draw the background gradient
					LinearGradientBrush gradient = new LinearGradientBrush(new RectangleF(new PointF(0,0),DiagramSize), BackColor, Palette.GradientColor, LinearGradientMode.Vertical);
					graphics.FillRectangle(gradient,new RectangleF(new PointF(0,0),renderRectangle.Size));
				}
				else
				{
					DisposeBufferBitmap();
					SetBitmap((Bitmap) GraphicsStateBitmap.Clone());
					graphics = Graphics.FromImage(Bitmap);
				}

				//Palette has no scale transform

				//Set the drawing options
				graphics.CompositingMode = CompositingMode;
				graphics.CompositingQuality = CompositingQuality;
				graphics.PixelOffsetMode = PixelOffsetMode;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Error getting render graphics" + ex.ToString());

			}
			return graphics;
		}

        public override void RenderLayers(Graphics graphics, Rectangle renderRectangle, Paging paging)
        {
            bool isup = false;
            bool isdown = false;
            RectangleF region = new RectangleF(0, 0, DiagramSize.Width, DiagramSize.Height);

            foreach (Tab tab in Layers)
            {
                if (tab.Visible)
                {
                    //Determine if an up or down arrow should be drawn
                    if (isup)
                    {
                        isdown = true; //set down
                        isup = false; //reset up
                        region.Height = tab.Rectangle.Top - region.Top;
                    }
                    //Reset down
                    if (isdown) isdown = false;

                    //Set up
                    if (Palette.Tabs.CurrentTab == tab)
                    {
                        isup = true;
                        region.Y = tab.Rectangle.Bottom + 1;
                    }

                    RenderTab(graphics, tab);
                }

                if (tab.ButtonStyle != ButtonStyle.None && DrawScroll)
                {
                    RenderButton(graphics, tab.ButtonStyle, tab.ButtonRectangle, tab.ButtonPressed, tab.ButtonEnabled);
                }
            }

            ///Draw final button
            if (ScrollTab.Visible && DrawScroll)
            {
                RenderButton(graphics, ButtonStyle.Down, ScrollTab.ButtonRectangle, ScrollTab.ButtonPressed, ScrollTab.ButtonEnabled);
            }

            //Set up the region to confine the elements drawn
            graphics.Clip = new Region(region);
            
            base.RenderLayers(graphics, renderRectangle, paging);
        }

        protected override void RenderDecorations(Graphics graphics, Rectangle renderRectangle, Paging paging)
        {
            GraphicsState state = graphics.Save();
            graphics.SmoothingMode = SmoothingMode.HighQuality;

            //Undo any clipping
            if (paging.Enabled) graphics.ResetClip();

            //Draw any decorations
            if (DecorationPath != null)
            {
                graphics.FillPath(DecorationBrush, DecorationPath);
                graphics.DrawPath(DecorationPen, DecorationPath);
            }

            graphics.Restore(state);
        }

        private void RenderPaletteAction(IRenderable renderable, Graphics graphics)
        {
            Solid solid = renderable as Solid;
            GraphicsPath path = solid.GetPath();

            if (solid.DrawBackground)
            {
                if (solid.CustomBrush == null)
                {
                    //Use a linear gradient brush if gradient requested
                    if (solid.DrawGradient)
                    {
                        LinearGradientBrush brush;
                        brush = new LinearGradientBrush(new RectangleF(0, 0, solid.Bounds.Width, solid.Bounds.Height), AdjustColor(solid.BackColor, 0, solid.Opacity), AdjustColor(solid.GradientColor, 0, solid.Opacity), solid.GradientMode);
                        brush.GammaCorrection = true;
                        graphics.FillPath(brush, path);
                    }
                    //Draw normal solid brush
                    else
                    {
                        SolidBrush brush;
                        brush = new SolidBrush(AdjustColor(solid.BackColor, 0, solid.Opacity));
                        graphics.FillPath(brush, path);
                    }
                }
                else
                {
                    graphics.FillPath(solid.CustomBrush, path);
                }

                //Render annotation and image
                if (solid.Label != null)
                {
                    var label = solid.Label;
                    if (solid.Label.Visible)
                    {
                        graphics.TranslateTransform(label.Offset.X, label.Offset.Y);

                        SolidBrush brush = new SolidBrush(AdjustColor(label.Color, 0, label.Opacity));
                        graphics.DrawString(label.Text, label.Font, brush, 0, 0);

                        graphics.TranslateTransform(-label.Offset.X, -label.Offset.Y);
                    }
                }
            }
            if (solid.DrawBorder)
            {
                graphics.DrawPath(Singleton.Instance.ActionPen, path);
            }
        }

		private void RenderTab(Graphics graphics, Tab tab)
		{
			Pen pen;

			//Setup background and text rectangle
			RectangleF rect = tab.Rectangle;
			RectangleF innerRect = tab.Rectangle;
			innerRect.Inflate(-1,-1);

			if (rect.IsEmpty) return;

			//Draw the gradient background
			LinearGradientBrush gradient = new LinearGradientBrush(rect, Palette.Tabs.BackColor, Palette.Tabs.GradientColor, LinearGradientMode.Vertical);
			graphics.FillRectangle(gradient,rect);

			//Draw left + top
			pen = (tab.Pressed) ? SystemPens.ControlDark : SystemPens.ControlLightLight;
			graphics.DrawLine(pen,new PointF(rect.Left,rect.Bottom),new PointF(rect.Left,rect.Top));
			graphics.DrawLine(pen,new PointF(rect.Left,rect.Top),new PointF(rect.Right,rect.Top));

			//Draw right bottom
			pen = (tab.Pressed) ? SystemPens.ControlLightLight : SystemPens.ControlDark;
			graphics.DrawLine(pen,new PointF(rect.Right,rect.Top),new PointF(rect.Right,rect.Bottom));
			graphics.DrawLine(pen,new PointF(rect.Right,rect.Bottom),new PointF(rect.Left,rect.Bottom));

			StringFormat format = new StringFormat();
			format.LineAlignment = StringAlignment.Center;

			//Offset by one if pressed
			if (tab.Pressed) innerRect.Offset(1,1);

			SolidBrush brush = new SolidBrush(Palette.Tabs.ForeColor);
			graphics.DrawString(tab.Name, Palette.Font, brush, innerRect, format);
		}

		private void RenderButton(Graphics graphics, ButtonStyle style, RectangleF rect, bool pressed, bool enabled)
		{
			Pen pen;

			if (rect.IsEmpty) return;

			//Draw a pale white underlay
            LinearGradientBrush gradient = new LinearGradientBrush(rect, Palette.Tabs.BackColor, Palette.Tabs.GradientColor, LinearGradientMode.Vertical);
			graphics.FillRectangle(gradient,rect);

			pen = (pressed) ? SystemPens.ControlDark : SystemPens.ControlLightLight;
			graphics.DrawLine(pen,new PointF(rect.Left,rect.Bottom),new PointF(rect.Left,rect.Top));
			graphics.DrawLine(pen,new PointF(rect.Left,rect.Top),new PointF(rect.Right,rect.Top));

			//Draw right bottom
			pen = (pressed) ? SystemPens.ControlLightLight : SystemPens.ControlDark;
			graphics.DrawLine(pen,new PointF(rect.Right,rect.Top),new PointF(rect.Right,rect.Bottom));
			graphics.DrawLine(pen,new PointF(rect.Right,rect.Bottom),new PointF(rect.Left,rect.Bottom));

			//Draw up or down arrow
			GraphicsPath path = new GraphicsPath();
			Matrix matrix = new Matrix();
			
			if (style == ButtonStyle.Up)
			{
				path.AddLine(4,0,0,4);
				path.AddLine(0,4,8,4);
				matrix.Translate(rect.X+4,rect.Y+6);
			}
			else
			{
				path.AddLine(0,0,3,3);
				path.AddLine(3,3,6,0);
				matrix.Translate(rect.X+6,rect.Y+8);
			}
			path.CloseFigure();			

			//Translate to correct position on button
			if (pressed) matrix.Translate(1,1);
			path.Transform(matrix);

			Brush systemBrush = (enabled) ? new SolidBrush(Color.FromArgb(66,65,66)) : new SolidBrush(Color.FromArgb(192,SystemColors.ControlDark));

			graphics.SmoothingMode = SmoothingMode.Default;
			graphics.FillPath(systemBrush,path);
		}

	}
}
