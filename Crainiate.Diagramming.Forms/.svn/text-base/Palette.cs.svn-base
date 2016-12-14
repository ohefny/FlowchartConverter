// (c) Copyright Crainiate Software 2010

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;

using Crainiate.Diagramming.Forms;
using Crainiate.Diagramming.Forms.Rendering;

namespace Crainiate.Diagramming.Forms
{
	[ToolboxBitmap(typeof(Crainiate.Diagramming.Forms.Palette), "Resource.palette.bmp")]
	public class Palette : Diagram
	{
		//Properties
		private Size _itemSize;
		private SizeF _spacing;
		private Tabs _tabs;
		private Margin _margin;
		private PaletteStyle _paletteStyle;
        private Color _gradientColor;
        private Color _borderColor;
        private Color _fillColor;
		
		//Working variables
		private int _lastCols;
		private Timer _timer;
		private ButtonStyle _currentStyle;

		#region Interface
		
		//Events

		public event EventHandler ArrangeElements;
		
		//Constructors
		public Palette() : base()
		{
			Suspend();
			
			Render = new PaletteRender(this);
			Margin = new Margin(10,10,10,10);
            Tabs = new Tabs(Model);
            Model.Layers = Tabs;  //Override the layers collection
			Spacing = new Size(20, 22);
			BackColor  = SystemColors.Control;
			GradientColor = SystemColors.Control;
			BorderColor = Color.Black;
			ForeColor = Color.Black;
			FillColor = Color.White;
			ItemSize = new Size(18, 18);
			Style = PaletteStyle.Multiple;
			DrawScroll = true;
            Feedback = false;
            DragSelect = false;

			Resume();
			
			//Set up scroll timer
			_timer = new Timer();
			_timer.Interval = 20;
			_timer.Tick += new EventHandler(Timer_Tick);
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Data"), Description("Returns the Tabs collection for the palette.")]
		public virtual Tabs Tabs
		{
			get
			{
				return _tabs;
			}
			set
			{
                if (value == null) throw new ArgumentNullException("");
				
				_tabs = value;

				_tabs.InsertTab += new TabEventHandler(Tabs_TabsInvalid);
				_tabs.RemoveTab += new TabEventHandler(Tabs_TabsInvalid);
				_tabs.TabsInvalid += new EventHandler(Tabs_TabsInvalid);

				Invalidate();
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Sets or retrieves the current margin values.")]
		public virtual Margin Margin
		{
			get
			{
				return _margin;
			}
			set
			{
				_margin = value;
			}
		}

		[Category("Appearance"), Description("Sets or retrieves the color used to render background gradient.")]
		public virtual Color GradientColor
		{
			get
			{
				return _gradientColor;
			}
			set
			{
				_gradientColor = value;
				Render.RenderDiagram(PageRectangle, Paging);
				DrawDiagram(ControlRectangle);
			}
		}

		[Category("Appearance"), Description("Sets or retrieves the color used to draw the borders of the items in the palette.")]
		public virtual Color BorderColor
		{
			get
			{
				return _borderColor;
			}
			set
			{
				_borderColor = value;
				Render.RenderDiagram(PageRectangle, Paging);
				DrawDiagram(ControlRectangle);
			}
		}

		[Category("Appearance"), Description("Sets or retrieves the color used to fill the background of the items contained in the palette.")]
		public virtual Color FillColor
		{
			get
			{
				return _fillColor;
			}
			set
			{
				_fillColor = value;
				Render.RenderDiagram(PageRectangle, Paging);
				DrawDiagram(ControlRectangle);
			}
		}

		[Description("Determines the size of the element when added to the palette.")]
		public virtual Size ItemSize
		{
			get
			{
				return _itemSize;
			}
			set
			{
				_itemSize = value;
			}
		}

		[Category("Appearance"), Description("Sets or the amount of spacing between items in the palette.")]
		public virtual Size Spacing
		{
			get
			{
				return Size.Round(_spacing);
			}
			set
			{
				_spacing = value;
				Arrange();
			}
		}

		[Category("Appearance"), DefaultValue(true), Description("Determines if the scroll buttons are drawn for the palette.")]
		public virtual bool DrawScroll
		{
			get
			{
				return Render.DrawScroll;
			}
			set
			{
				Render.DrawScroll = value;
			}
		}

		[Category("Behaviour"), DefaultValue(PaletteStyle.Multiple), Description("Determines whether the palette shows more than one pane at a time.")]
		public virtual PaletteStyle Style
		{
			get
			{
				return _paletteStyle;
			}
			set
			{
				if (value != _paletteStyle)
				{
					_paletteStyle = value;
					Render.RenderDiagram(PageRectangle, Paging);
					DrawDiagram(ControlRectangle);
				}
			}
		}

		//Methods
		[Description("Arranges the palette items using the internal margin and spacing values.")]
		public virtual void Arrange()
		{
			Suspend();
			ArrangeImplementation(_spacing.Width, _spacing.Height);
			Resume();
			Invalidate();
		}

		[Description("Creates a palette of shapes from the stencil provided.")]
		public virtual void AddStencil(Stencil stencil)
		{
			Suspend();
			AddStencilImplementation(stencil);
			ArrangeImplementation(50,50);
			Resume();
			Invalidate();
		}

        protected override SizeF GetMinimumSize()
        {
            return ItemSize;
        }

        protected override SizeF GetMaximumSize()
        {
            return ItemSize;
        }

        protected override SizeF GetDefaultSize()
        {
            return ItemSize;
        }

		protected virtual void OnArrangeElements()
		{
			if (ArrangeElements != null) ArrangeElements(this,EventArgs.Empty);
		}

        protected override void OnElementMouseDown(Element element, MouseEventArgs e)
        {
            base.OnElementMouseDown(element, e);

            if (element is Shape)
            {
                //Start the drag operation
                Shape prototype = (Shape) element;

                Shape shape = new Shape(prototype);
                shape.MinimumSize = new Size(32, 32);
                //shape.MaximumSize = new Size(320, 320);
                //shape.Scale(3, 3, 0, 0, false);

                shape.Label = null;
                shape.Image = null;

                DoDragDrop(shape, DragDropEffects.All);
            }
        }

		#endregion

		#region Overrides

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged (e);
			Render.RenderDiagram(PageRectangle, Paging);
			DrawDiagram(ControlRectangle);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown (e);

			//Prepare the actionpath
			if (e.Button == MouseButtons.Left)
			{
				//See if we pressed a tab or a button
				Tab tab = GetMouseTab(e);

				if (tab != null)
				{
					//Check if tab or button
					if (tab.Rectangle.Contains(e.X,e.Y) && tab.Visible)
					{
						tab.Pressed = true;
					}
					else
					{
						if (DrawScroll && tab.ButtonEnabled)
						{
							tab.ButtonPressed = true;
							_currentStyle = tab.ButtonStyle;
							_timer.Start();
						}
					}
					Invalidate();
				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				//Set to unpressed
				foreach (Tab tab in Tabs)
				{
					if (tab.Pressed)
					{
						Tabs.CurrentTab = tab;
						tab.Pressed = false;
						SetTabs();
						Arrange();
						CheckScrollButtons();
					}
					if (DrawScroll && tab.ButtonPressed)
					{
						_timer.Stop();
						tab.ButtonPressed = false;
					}
				}
				if (Render.ScrollTab.ButtonPressed)
				{
					_timer.Stop();
					Render.ScrollTab.ButtonPressed = false;
				}
				Invalidate();
			}

			base.OnMouseUp (e);
		}

		protected override void OnElementInserted(Element element)
		{
			//Make sure the element added is the right size
			if (element is Shape)
			{
				Shape shape = (Shape) element;
				
				//Determines if shapes keep their aspect in the palette
				if (shape.KeepAspect == true)
				{
					shape.MinimumSize = new SizeF(1,1);
				}
				else
				{
					shape.MinimumSize = ItemSize;
				}
				
				shape.MaximumSize = ItemSize;
				
				//Set size taking current width and height ratio into consideration
				//The max/min size will determine if the ratio is taken into account
				if (shape.Width > shape.Height)
				{
					float ratio = ItemSize.Width / shape.Width;
					shape.Size = new SizeF(ItemSize.Width,shape.Height * ratio);
				}
				else
				{
					float ratio = ItemSize.Height / shape.Height;
					shape.Size = new SizeF(shape.Width * ratio, ItemSize.Height);
				}
				
				shape.AllowMove = false;
				shape.AllowScale = false;
				shape.Clip = false;
                shape.AllowSnap = false;

                shape.BorderWidth = 1;
				shape.BorderColor = Color.FromArgb(66,65,66); //SystemColors.ControlDarkDark;
				shape.BackColor = Color.White;
				shape.SmoothingMode = SmoothingMode.HighQuality;

				if (shape.Label != null) shape.Label.Color = Color.FromArgb(66,65,66); //SystemColors.ControlDarkDark;
			}	

			SetTabs();
			base.OnElementInserted(element);
		}

		public override bool AutoScroll
		{
			get
			{
				return base.AutoScroll;
			}
			set
			{
				base.AutoScroll = false;
			}
		}

		//Redraw because of gradient
		protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);
			SetTabs();
			Render.RenderDiagram(ControlRectangle, Paging);
			DrawDiagram(ControlRectangle);
		}

		//Initial gradient draw
		protected override void OnLayout(LayoutEventArgs levent)
		{
			base.OnLayout (levent);
			SetTabs();
			Arrange();

            if (Render != null)
            {
                Render.RenderDiagram(ControlRectangle, Paging);
                DrawDiagram(ControlRectangle);
            }
		}

        //We dont want anythign to happen
        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            DragElement = null;
            ControlRender.DecorationPath = null;
            ControlRender.Highlights = null;
            Invalidate();
        }

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new PaletteRender Render
		{
			get
			{
				return (PaletteRender) base.Render;
			}
			set
			{
				if (value == null) throw new ArgumentException();
				base.Render = value;	
			}
		}
		
		[Description("Zoom is always 100 percent for a palette control.")]
		public override float Zoom
		{
			get
			{
				return base.Zoom;
			}
			set
			{
                return;
			}
		}

        //Events
		private void Tabs_TabsInvalid(object sender, EventArgs e)
		{
			SetTabs();
			Invalidate();
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			CheckScrollButtons();
			ScrollTab(_currentStyle);
		}

        //Implementation
		private void SetTabs()
		{
            if (Tabs == null) return;

			float offset = 1;
			float width = this.Width-2;
			int count = 0;
			Tab previousTab = null;
			Tab lastTab = null;

			foreach (Tab tab in Tabs)
			{
				count+=1;
				tab.ButtonStyle = ButtonStyle.None;
				tab.SetButtonRectangle(new RectangleF());

				//Check to see if must reduce width for scroll
				if (DrawScroll && (Tabs.CurrentTab == previousTab || Tabs.CurrentTab == tab))
				{
					tab.SetRectangle(new RectangleF(1,offset,width-18,Tabs.TabHeight));
					if (Tabs.CurrentTab == tab) tab.ButtonStyle = ButtonStyle.Up;
					if (Tabs.CurrentTab == previousTab) tab.ButtonStyle = ButtonStyle.Down;
					tab.SetButtonRectangle(new RectangleF(tab.Rectangle.Right+1,tab.Rectangle.Top,17,tab.Rectangle.Height));
				}
				else
				{
					tab.SetRectangle(new RectangleF(1,offset,width,Tabs.TabHeight));
				}
				previousTab = tab;

				//Check if tabs must move to bottom
				if (Tabs.CurrentTab == tab)
				{
					offset = (Height - (Tabs.Count - count) * (Tabs.TabHeight + 1));
				}
				else
				{
					offset += Tabs.TabHeight + 1;	
				}

				//Store last tab
				lastTab = tab;
			}
			
			//work out scroll button tab at the bottom if required
			if (DrawScroll && lastTab == Tabs.CurrentTab)
			{
				Render.ScrollTab.Visible = true;
				Render.ScrollTab.SetRectangle(new RectangleF(1,Height - Tabs.TabHeight -1,width-19,Tabs.TabHeight));
				Render.ScrollTab.SetButtonRectangle(new RectangleF(width-16,Height - Tabs.TabHeight -1,17,Tabs.TabHeight));
			}
			else
			{
				Render.ScrollTab.Visible = false;
			}
		}

		private void ArrangeImplementation(float spacingWidth, float spacingHeight)
		{

            if (Margin == null) return;
            if (Tabs == null) return;
            if (Tabs.CurrentTab == null) return;

            float totalwidth = DisplayRectangle.Width - Margin.Left - Margin.Right;
			float width = Margin.Left;
			float height = Margin.Top + Tabs.CurrentTab.Scroll;
			Element lastElement = null;
			PointF lastLocation = new PointF();

			//Check if must include tab in height
			if (Tabs.CurrentTab.Visible) height += Tabs.CurrentTab.Rectangle.Bottom;

			Suspend();

			//Arrange each node according to the order of the renderlist
			foreach (Element element in Model.Elements)
			{
				//Set the location
				if (lastElement != null) 
				{
					if (lastElement is Solid)
					{
						Solid solid = (Solid) lastElement;
						solid.Location = lastLocation;
					}
				}

				lastElement = element;
				lastLocation = new PointF(width,height);

				height += spacingHeight;

				SetLabel(lastElement);
			}
			 
			//Set the final item
			if (lastElement != null)
			{	
				if (lastElement is Solid)
				{
					Solid solid = (Solid) lastElement;
					solid.Location = lastLocation;
				}
				SetLabel(lastElement);
			}

			//Save the number of columns
			_lastCols = (int) System.Math.Floor((decimal) (DisplayRectangle.Width - Margin.Left - Margin.Right) / Spacing.Width);
			
			//Enable disable scroll buttons
			CheckScrollButtons();

			Resume();
			Invalidate();
		}

		private void AddStencilImplementation(Stencil stencil)
		{
			Suspend();
			foreach(StencilItem item in stencil.Values)
			{
				Shape shape = new Shape(item);
				shape.Label = new Label(item.Key);
				
				Model.Shapes.Add(shape);
			}
			Resume();
		}

		private void SetLabel(Element element)
		{
			if (element is Shape)
			{
				Shape shape = (Shape) element;

				if (shape.Label != null)
				{
					Label label = shape.Label;
					
					label.Offset = new PointF(ItemSize.Width + 18, 0F);
					label.Size = new SizeF(Math.Abs(Width - ItemSize.Width - shape.Width) , ItemSize.Height);
					label.LineAlignment = StringAlignment.Center;
					label.Alignment = StringAlignment.Near;
					label.FontSize = 8;
                    label.Wrap = false;

                    shape.Clip = false;
				}
			}
		}

		private Tab GetMouseTab(MouseEventArgs e)
		{
			foreach (Tab tab in Tabs)
			{
				if (tab.Rectangle.Contains(e.X,e.Y)) return tab;
				if (tab.ButtonStyle != ButtonStyle.None && tab.ButtonRectangle.Contains(e.X,e.Y)) return tab;
			}
			if (Render.ScrollTab.Visible && Render.ScrollTab.ButtonRectangle.Contains(e.X,e.Y))
			{
				return Render.ScrollTab;
			}
			return null;
		}

		private void OffsetRenderlist(float offset)
		{
			Suspend();
			foreach (Element element in Model.Elements)
			{
				if (element is Shape)
				{
					Shape shape = (Shape) element;
					shape.Location = new PointF(shape.X,shape.Y+offset);
				}
			}
			Resume();
		}

		private void CheckScrollButtons()
		{
			Tab next = GetNextTab();
			Tab tab = Tabs.CurrentTab;

			//check for current tab button enabled
			tab.ButtonEnabled = (tab.Scroll < 0);

			//Loop through and check if any element greater in height than next tab
			next.ButtonEnabled = false;
			foreach (Element element in Model.Elements)
			{
                if (element.Layer == Tabs.CurrentLayer)
                {
                    if (element.Bounds.Bottom > next.Rectangle.Top)
                    {
                        next.ButtonEnabled = true;
                        break;
                    }
                }
			}

			//Check if button must be disabled and the timer stopped
			if (tab.ButtonPressed && !tab.ButtonEnabled)
			{
				_timer.Stop();
				tab.ButtonPressed = false;
			}
			if (next.ButtonPressed && !next.ButtonEnabled) 
			{
				_timer.Stop();
				next.ButtonPressed = false;
			}
		}

		private void ScrollTab(ButtonStyle style)
		{
			Tab tab = Tabs.CurrentTab;
			Tab next = GetNextTab();
			
			//Scroll
			if (style == ButtonStyle.Up && tab.ButtonEnabled)
			{
				tab.Scroll += Tabs.TabHeight;
				OffsetRenderlist(Tabs.TabHeight);
			}
			else if (style == ButtonStyle.Down && next.ButtonEnabled)
			{
				tab.Scroll -= Tabs.TabHeight;
				OffsetRenderlist(-Tabs.TabHeight);
			}
			CheckScrollButtons();
			Invalidate();
		}

		private Tab GetNextTab()
		{
			Tab previous = null;

			foreach (Tab tab in Tabs)
			{
				if (Tabs.CurrentTab == previous) return tab ;
				previous = tab;
			}
			return Render.ScrollTab;
		}

		private PointF OffsetDrop(PointF point,RectangleF rectangle)
		{
			PointF offset = new PointF(point.X - (rectangle.Width / 2), point.Y -  (rectangle.Height / 2));
			
			//Adjust for scale
			return PointToDiagram(Convert.ToInt32(offset.X),Convert.ToInt32(offset.Y));
		}

		#endregion
	}
}
