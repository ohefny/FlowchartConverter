// (c) Copyright Crainiate Software 2010

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using Crainiate.Diagramming.Collections;

using Crainiate.Diagramming.Forms.Rendering;

namespace Crainiate.Diagramming.Forms
{
    //View is part of the Model-View-Controller design pattern
    [ToolboxBitmap(typeof(View), "Resource.view.bmp")]
    public partial class View: UserControl, IView
    {
        //Property variables
        private Render _render;
        private Model _model;
        private Controller _controller;
        private Paging _paging;

        private bool _showToolTips = true;

        //Working variables
        private Rectangle _pageRect; //The currently visible area of the diagram
        private Rectangle _controlRect; //The control area in screen co-ordinates
        private bool _suspended;

        private MouseElements _mouseElements;

        private Status _saveStatus;
        private Status _status;

        private const int WM_HSCROLL = 0x114;
        private const int WM_VSCROLL = 0x115;
        private const int SB_ENDSCROLL = 8;

        #region Interface

        //Events
        //Element Mouse Events
        [Category("Mouse"), Description("Occurs when a mouse button is pressed over an element.")]
        public event MouseEventHandler ElementMouseDown;
        [Category("Mouse"), Description("Occurs when a mouse button is released over an element.")]
        public event MouseEventHandler ElementMouseUp;
        [Category("Action"), Description("Occurs when an element is clicked.")]
        public event EventHandler ElementClick;
        [Category("Action"), Description("Occurs when an element is double-clicked.")]
        public event EventHandler ElementDoubleClick;
        [Category("Mouse"), Description("Occurs when the mouse enters an element.")]
        public event EventHandler ElementEnter;
        [Category("Mouse"), Description("Occurs when the mouse leaves an element.")]
        public event EventHandler ElementLeave;

        //Diagram Mouse Events
        [Category("Mouse"), Description("Occurs when a mouse button is pressed over the diagram.")]
        public event MouseEventHandler DiagramMouseDown;
        [Category("Mouse"), Description("Occurs when a mouse button is released over the diagram.")]
        public event MouseEventHandler DiagramMouseUp;
        [Category("Action"), Description("Occurs when the diagram is clicked.")]
        public event EventHandler DiagramClick;
        [Category("Action"), Description("Occurs when the diagram is double-clicked.")]
        public event EventHandler DiagramDoubleClick;

        //Elements events
        [Category("Behavior"), Description("Occurs when an element is inserted into the diagram.")]
        public event ElementsEventHandler ElementInserted;
        [Category("Behavior"), Description("Occurs when an element is removed form the diagram.")]
        public event ElementsEventHandler ElementRemoved;

        //Diagram events
        [Category("Layout"), Description("Occurs when the value of the zoom property has changed.")]
        public event EventHandler ZoomChanged;
      
        //Constructor
        public View()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            SetStyle(System.Windows.Forms.ControlStyles.UserPaint, true);
            SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
            SetStatus(Status.Default);

            SetModel(new Model());
            Controller = new Controller(Model);
            Controller.Views.Add(this);

            Paging = new Paging();
            Render = new Render();
            
            Model.Clear();

            //Create an initial render and draw onto control surface
            Render.Layers = Model.Layers;
            Render.Elements = Model.Elements;
            Render.RenderDiagram(new Rectangle(0, 0, this.Width, this.Height), Paging);
            DrawDiagram(new Rectangle(0, 0, this.Width, this.Height));
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Model Model
        {
            get
            {
                return _model;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Controller Controller
        {
            get
            {
                return _controller;
            }
            set
            {
                if (value == null) throw new ArgumentNullException();

                _controller = value;
                _controller.Model = this.Model;
            }
        }
		
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Data"), Description("Returns an instance of the render class used to draw the diagram.")]
		public virtual Render Render
		{
			get
			{
				return _render;
			}
            set
            {
                if (value == null) throw new ArgumentNullException();
                _render = value;
                _render.Elements = Model.Elements;
                _render.Layers = Model.Layers;

                //Render and draw
                Invalidate();
            }
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Data"), Description("Provides access to the current elements under the mouse pointer.")]
		public virtual MouseElements CurrentMouseElements
		{
			get
			{
				return _mouseElements;
			}
		}

		[Category("Behavior"), DefaultValue(100), RefreshProperties(RefreshProperties.All), Description("Sets or retrieves the current zoom level as a percentage.")]
		public virtual float Zoom
		{
			get
			{
				return _render.Zoom;
			}
			set
			{
				if (value != _render.Zoom)
				{
					_render.Zoom = value;
					CheckDiagramSize();
					SetScrollRectangles();
					SetPagedSettings();
					Invalidate();

					OnZoomChanged();
				}
			}
		}

		[Browsable(false), Description("Sets or retrieves the current page rectangle.")]
		protected virtual Rectangle PageRectangle
		{
			get
			{
				return _pageRect;
			}
		}

		[Browsable(false), Description("Sets or retrieves the current control rectangle.")]
		protected virtual Rectangle ControlRectangle
		{
			get
			{
				return _controlRect;
			}
		}

        public virtual Paging Paging
        {
            get
            {
                return _paging;
            }
            set
            {
                if (value == null) throw new ArgumentNullException();

                _paging = value;
            }
        }

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Data"), Description("Retrieves a boolean value determining whether render and draw operations are suspended.")]
		public virtual bool Suspended
		{
			get
			{
                return _suspended;
			}
		}

		[Browsable(false), Category("Behavior"),Description("Returns the current action status of the diagram.")]
		public virtual Status Status
		{
			get
			{
				return _status;
			}
		}

		[Category("Behavior"), DefaultValue(true), Description("Determines if the diagram shows tooltips for elements in the diagram.")]
		public virtual bool ShowTooltips
		{
			get
			{
				return _showToolTips;
			}
			set
			{
				_showToolTips = value;
			}
		}

		//Methods
		[Browsable(false), Description("Resets all collections and properties of the diagram to their defaults.")]
		public virtual void Clear()
		{
            Model.Clear();
			Refresh();
		}

		[Browsable(false), Description("Determines whether the diagram contains this point.")]
		public bool Contains(PointF location)
		{
			return new RectangleF(new PointF(0,0),Model.Size).Contains(location);
		}

		[Description("Causes the control to be re-rendered and redrawn.")]
		public new void Invalidate()
		{
			InvalidateImplementation(new Rectangle());
		}

		[Description("Causes the control to be re-rendered and redrawn.")]
		public new void Invalidate(Rectangle rectangle)
		{
			InvalidateImplementation(rectangle);
		}

		[Description("Suspends draw operations for a diagram object.")]
		public virtual void Suspend()
		{
			_suspended = true;
		}

		[Description("Resumes draw operations for a diagram object.")]
		public virtual void Resume()
		{
			_suspended = false;
		}

        [Description("Adds a shape to the diagram")]
        public virtual Shape AddShape(PointF location)
        {
            return AddShapeImplementation("", location, null, new SizeF());
        }

        [Description("Adds a shape to the diagram")]
        public virtual Shape AddShape(PointF location, SizeF size)
        {
            return AddShapeImplementation("", location, null, size);
        }

        [Description("Adds a shape to the diagram")]
        public virtual Shape AddShape(string key, PointF location)
        {
            return AddShapeImplementation(key, location, null, new SizeF());
        }

        [Description("Adds a shape to the diagram")]
        public virtual Shape AddShape(string key, PointF location, SizeF size)
        {
            return AddShapeImplementation(key, location, null, size);
        }

        [Description("Adds a shape with the specified key and stencil to the diagram")]
        public virtual Shape AddShape(string key, PointF location, StencilItem stencil)
        {
            return AddShapeImplementation(key, location, stencil, new SizeF());
        }

        [Description("Adds a shape with the specified key and stencil to the diagram")]
        public virtual Shape AddShape(string key, PointF location, SizeF size, StencilItem stencil)
        {
            return AddShapeImplementation(key, location, stencil, size);
        }

        [Description("Adds a shape with the specified stencil to the diagram")]
        public virtual Shape AddShape(PointF location, StencilItem stencil)
        {
            return AddShapeImplementation(Model.Shapes.CreateKey(), location, stencil, new SizeF());
        }

        [Description("Adds a shape with the specified stencil to the diagram")]
        public virtual Shape AddShape(PointF location, SizeF size, StencilItem stencil)
        {
            return AddShapeImplementation(Model.Shapes.CreateKey(), location, stencil, size);
        }

        [Description("Adds a line to the diagram")]
        public virtual Link AddLine(PointF start, PointF end)
        {
            Link line = Controller.Factory.CreateLine(start, end);
            Model.Lines.Add(Model.Lines.CreateKey(), line);
            return line;
        }

        [Description("Adds a line with the specified key to the diagram")]
        public virtual Link AddLine(string key, PointF start, PointF end)
        {
            Link line = Controller.Factory.CreateLine(start, end);
            Model.Lines.Add(key, line);
            return line;
        }

        [Description("Adds a line with the specified key to the diagram")]
        public virtual Link AddLine(Shape start, Shape end)
        {
            Link line = Controller.Factory.CreateLine();
            line.Start.Shape = start;
            line.End.Shape = end;
            Model.Lines.Add(Model.Lines.CreateKey(), line);
            return line;
        }

        [Description("Adds a line with the specified key to the diagram")]
        public virtual Link AddLine(string key, Shape start, Shape end)
        {
            Link line = Controller.Factory.CreateLine();
            line.Start.Shape = start;
            line.End.Shape = end;
            Model.Lines.Add(key, line);
            return line;
        }

        [Description("Adds a line with the specified key to the diagram")]
        public virtual Link AddLine(Port start, Port end)
        {
            Link line = Controller.Factory.CreateLine();
            line.Start.Port = start;
            line.End.Port = end;
            Model.Lines.Add(Model.Lines.CreateKey(), line);
            return line;
        }

        [Description("Adds a line with the specified key to the diagram")]
        public virtual Link AddLine(string key, Port start, Port end)
        {
            Link line = Controller.Factory.CreateLine();
            line.Start.Port = start;
            line.End.Port = end;
            Model.Lines.Add(key, line);
            return line;
        }

        [Description("Adds an connector to the diagram")]
        public virtual Connector AddConnector(PointF start, PointF end)
        {
            Connector line = Controller.Factory.CreateConnector(start, end);
            Model.Lines.Add(Model.Lines.CreateKey(), line);
            return line;
        }

        [Description("Adds an connector with the specified key to the diagram")]
        public virtual Connector AddConnector(string key, PointF start, PointF end)
        {
            Connector line = Controller.Factory.CreateConnector(start, end);
            Model.Lines.Add(key, line);
            return line;
        }

        [Description("Adds an connector with the specified key to the diagram")]
        public virtual Connector AddConnector(Shape start, Shape end)
        {
            Connector line = Controller.Factory.CreateConnector();
            line.Start.Shape = start;
            line.End.Shape = end;
            Model.Lines.Add(Model.Lines.CreateKey(), line);
            return line;
        }

        [Description("Adds an connector with the specified key to the diagram")]
        public virtual Connector AddConnector(string key, Shape start, Shape end)
        {
            Connector line = Controller.Factory.CreateConnector();
            line.Start.Shape = start;
            line.End.Shape = end;
            Model.Lines.Add(key, line);
            return line;
        }

        [Description("Adds an connector with the specified key to the diagram")]
        public virtual Connector AddConnector(Port start, Port end)
        {
            Connector line = Controller.Factory.CreateConnector();
            line.Start.Port = start;
            line.End.Port = end;
            Model.Lines.Add(Model.Lines.CreateKey(), line);
            return line;
        }

        [Description("Adds an connector with the specified key to the diagram")]
        public virtual Connector AddConnector(string key, Port start, Port end)
        {
            Connector line = Controller.Factory.CreateConnector();
            line.Start.Port = start;
            line.End.Port = end;
            Model.Lines.Add(key, line);
            return line;
        }

        [Description("Adds an connector with the specified key to the diagram")]
        public virtual Connector AddConnector(Port start, Shape end)
        {
            Connector line = Controller.Factory.CreateConnector();
            line.Start.Port = start;
            line.End.Shape = end;
            Model.Lines.Add(Model.Lines.CreateKey(), line);
            return line;
        }

        [Description("Adds an connector with the specified key to the diagram")]
        public virtual Connector AddConnector(string key, Port start, Shape end)
        {
            Connector line = Controller.Factory.CreateConnector();
            line.Start.Port = start;
            line.End.Shape = end;
            Model.Lines.Add(key, line);
            return line;
        }

        [Description("Adds a recursive line to the diagram")]
        public virtual Connector AddRecursiveLine(Port start, Port end)
        {
            if (start.Parent != end.Parent) throw new ArgumentException("The start and end ports must belong to the same shape.");
            Connector line = Controller.Factory.CreateConnector();
            line.Start.Port = start;
            line.End.Port = end;
            Model.Lines.Add(Model.Lines.CreateKey(), line);
            return line;
        }

        [Description("Adds a recursive line with the specified key to the diagram")]
        public virtual Connector AddRecursiveLine(string key, Port start, Port end)
        {
            if (start.Parent != end.Parent) throw new ArgumentException("The start and end ports must belong to the same shape.");

            Connector line = Controller.Factory.CreateConnector();
            line.Start.Port = start;
            line.End.Port = end;
            Model.Lines.Add(key, line);
            return line;
        }

		[Description("Returns a diagram point from a mouse point.")]
		public virtual PointF PointToDiagram(int x, int y)
		{
			return TranslatePoint(x, y);
		}

		[Description("Returns a diagram point from a mouse point.")]
		public virtual PointF PointToDiagram(MouseEventArgs e)
		{
			return TranslatePoint(e.X, e.Y);
		}

		[Description("Returns a mouse point from a diagram point.")]
		public virtual Point DiagramToPoint(float x, float y)
		{
			return TranslateDiagram(x, y);
		}

		[Description("Returns a mouse point from a diagram point.")]
		public virtual Point DiagramToPoint(PointF point)
		{
			return TranslateDiagram(point.X, point.Y);
		}

		[Description("Returns a diagram rectangle from a control rectangle.")]
		public virtual RectangleF RectangleToDiagram(RectangleF rect)
		{
			return TranslateRectangle(rect);
		}

		[Description("Returns the top most element from this control point.")]
		public virtual Element ElementFromPoint(int x, int y)
		{
			return GetElement(TranslatePoint(x, y),Model.Elements);
		}

		[Description("Returns the top most element from this control point.")]
		public virtual Element ElementFromPoint(MouseEventArgs e)
		{
			return GetElement(TranslatePoint(e.X, e.Y),Model.Elements);
		}

		[Description("Returns the top most element from this diagram point.")]
		public virtual Element ElementFromDiagram(float x, float y)
		{
			return GetElement(new PointF(x, y),Model.Elements);
		}

		[Description("Returns the top most element from this diagram point.")]
		public virtual Element ElementFromDiagram(PointF location)
		{
			return GetElement(location,Model.Elements);
		}

		[Description("Returns an origin from a screen location for the specified line.")]
		public virtual Origin OriginFromPoint(Link line,int x, int y)
		{
			return line.OriginFromLocation(TranslatePoint(x,y));
		}

		[Description("Returns an origin from a screen location for the specified line.")]
		public virtual Origin OriginFromPoint(Link line, MouseEventArgs e)
		{
			return line.OriginFromLocation(TranslatePoint(e.X, e.Y));
		}

		[Description("Returns an origin from a diagram location for the specified line.")]
		public virtual Origin OriginFromDiagram(Link line, float x, float y)
		{
			return line.OriginFromLocation(new PointF(x, y));
		}

		public virtual void ScrollToElement(Element element)
		{
			PointF diagra_point = new PointF(element.Bounds.X + (element.Bounds.Width /2),element.Bounds.Y + (element.Bounds.Height /2));
			ScrollToPoint(diagra_point);
		}

		public virtual void ScrollToPoint(PointF point)
		{
			int ix = Convert.ToInt32((point.X * Render.ScaleFactor) - (Width / 2));
			int iy = Convert.ToInt32((point.Y * Render.ScaleFactor) - (Height / 2));

			//Offset for paging
			if (Paging.Enabled) 
			{
				ix += Paging.WorkspaceOffset.X;
                iy += Paging.WorkspaceOffset.Y;
			}

			if (ix < 0) ix = 0;
			if (iy < 0) iy = 0;

			AutoScrollPosition = new Point(ix, iy);
		}

		//Scrolls the control to the diagram point specified
		public virtual void ScrollToPoint(float x, float y)
		{
			ScrollToPoint(new PointF(x, y));
		}

        //Calculate zoom best fit
        public virtual void ZoomBestFit()
        {
            //Get width of control, less PagedMargin divided by diagram size as a percent
            float sx = (Width - Paging.Padding.Width) / Model.Size.Width;
            float sy = (Height - Paging.Padding.Height) / Model.Size.Height;

            //Convert to whole percentage
            sx = Convert.ToSingle(Math.Round(sx * 100, 0));
            sy = Convert.ToSingle(Math.Round(sy * 100, 0));

            //Select zoom
            float zoom = (sx < sy) ? sx : sy;

            if (zoom > 100) zoom = 100;

            Zoom = zoom;
        }

        public virtual void SetModel(Model model)
        {
            if (model == null) throw new ArgumentNullException();

            _model = model;

            _model.ElementInserted += new ElementsEventHandler(Element_Insert);
            _model.ElementRemoved += new ElementsEventHandler(Element_Remove);

            Invalidate();
        }

		[Description("Sets the current diagram status")]
		protected internal virtual void SetStatus(Status status)
		{
			_status = status;
		}

		[Description("Sets the current mouse elements")]
		protected internal virtual void SetMouseElements(MouseElements elements)
		{
			_mouseElements = elements;
		}
        
		//Event methods		
		[Description("Raises the ElementMouseDown event.")]
		protected virtual void OnElementMouseDown(Element element,MouseEventArgs e)
		{
			if (ElementMouseDown != null) ElementMouseDown(element,e);
		}

		[Description("Raises the ElementMouseUp event.")]
		protected virtual void OnElementMouseUp(Element element,MouseEventArgs e)
		{
			if (ElementMouseUp != null) ElementMouseUp(element,e);
		}
		
		[Description("Raises the ElementClick event.")]
		protected virtual void OnElementClick(Element element)
		{
			if (ElementClick != null) ElementClick(element,EventArgs.Empty);
		}

		[Description("Raises the ElementDoubleClick event.")]
		protected virtual void OnElementDoubleClick(Element element)
		{
			if (ElementDoubleClick != null) ElementDoubleClick(element,EventArgs.Empty);
		}

		[Description("Raises the ElementEnter event.")]
		protected virtual void OnElementEnter(Element element)
		{
			if (ElementEnter != null) ElementEnter(element,EventArgs.Empty);
		}

		[Description("Raises the ElementLeave event.")]
		protected virtual void OnElementLeave(Element element)
		{
			if (ElementLeave != null) ElementLeave(element,EventArgs.Empty);
		}

		[Description("Raises the DiagramMouseDown event.")]
		protected virtual void OnDiagramMouseDown(MouseEventArgs e)
		{
			if (DiagramMouseDown != null) DiagramMouseDown(this,e);
		}

		[Description("Raises the DiagramMouseUp event.")]
		protected virtual void OnDiagramMouseUp(MouseEventArgs e)
		{
			if (DiagramMouseUp != null) DiagramMouseUp(this,e);
		}
		
		[Description("Raises the DiagramClick event.")]
		protected virtual void OnDiagramClick()
		{
			if (DiagramClick != null) DiagramClick(this,EventArgs.Empty);
		}

		[Description("Raises the DiagramDoubleClick event.")]
		protected virtual void OnDiagramDoubleClick()
		{
			if (DiagramDoubleClick != null) DiagramDoubleClick(this,EventArgs.Empty);
		}

		[Description("Raises the ElementInserted event.")]
		protected virtual void OnElementInserted(Element element)
		{
			if (ElementInserted != null) ElementInserted(this,new ElementsEventArgs(element));
		}

		[Description("Raises the ElementRemoved event.")]
		protected virtual void OnElementRemoved(Element element)
		{
			if (ElementRemoved != null) ElementRemoved(this,new ElementsEventArgs(element));
		}
		
		[Description("Raises the ZoomChanged event.")]
		protected virtual void OnZoomChanged()
		{
			if (ZoomChanged != null) ZoomChanged(this,EventArgs.Empty);
		}
		
		#endregion

		#region Overrides

		public override Color BackColor
		{
			get
			{
				return _render.BackColor;
			}
			set
			{
				base.BackColor = value;
				_render.BackColor = value;
				Invalidate();
			}
		}

		[Description("Forces the control to render and paint itself and it's child controls.")]
		public override void Refresh()
		{
			if (Suspended) return;

            Render.Layers = Model.Layers;
            Render.Elements = Model.Elements;
            Render.DisposeGraphicsStateBitmap();
			Render.RenderDiagram(Render.RenderRectangle, Paging);

			base.Refresh();
		}

		[Description("Gets or sets the minimum size of the auto-scroll."), RefreshProperties(RefreshProperties.All)]
		public new Size AutoScrollMinSize
		{
			get
			{
				return base.AutoScrollMinSize;
			}
			set
			{
				if (! value.Equals(base.AutoScrollMinSize)) SetPagedSettings();
			}
		}

		[Description("Gets or sets a value indicating whether the container will allow the user to scroll to any controls placed outside of its visible boundaries.")]
		public override bool AutoScroll
		{
			get
			{
				return base.AutoScroll;
			}
			set
			{
				if (value != base.AutoScroll)
				{
					base.AutoScroll = value;
				}
			}
		}
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new System.Drawing.Image BackgroundImage
		{
			get
			{
				return _render.BackgroundImage;
			}
			set
			{
				Render.BackgroundImage = value;

                Render.Layers = Model.Layers;
                Render.Elements = Model.Elements;
				Render.RenderDiagram(_pageRect, Paging);
				DrawDiagram(_controlRect);
			}
		}

        public virtual PointF GetOffset()
        {
            return PointF.Empty;
        }

		protected override void OnMouseDown(MouseEventArgs e)
		{
			SetStatus(Status.Updating);
			_mouseElements.MouseStartElement = ElementFromPoint(e);

			//Check for origin 
			if (_mouseElements.MouseStartElement is Link)
			{
				Link line = (Link) _mouseElements.MouseStartElement;
				_mouseElements.MouseStartOrigin = OriginFromPoint(line,e);
			}

			if (_mouseElements.MouseStartElement != null) 
			{
				OnElementMouseDown(_mouseElements.MouseStartElement,e);
			}
			else
			{
				OnDiagramMouseDown(e);
			}
			base.OnMouseDown (e);
		}

        protected override void OnMouseMove(MouseEventArgs e)
        {
            Element element = ElementFromPoint(e);
            if (element != null)
            {
                if (element != _mouseElements.MouseMoveElement)
                {
                    if (element != null && _mouseElements.MouseMoveElement != null) OnElementLeave(_mouseElements.MouseMoveElement);
                    _mouseElements.MouseMoveElement = element;
                    OnElementEnter(element);
                }
            }
            else
            {
                if (element == null && _mouseElements.MouseMoveElement != null)
                {
                    _mouseElements.MouseMoveElement = null;
                    OnElementLeave(_mouseElements.MouseMoveElement);
                }
            }

            base.OnMouseMove(e);
        }

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (_mouseElements.MouseStartElement != null)
			{
				OnElementMouseUp(_mouseElements.MouseStartElement,e);
			}
			else
			{
				OnDiagramMouseUp(e);
			}

			//Reset all mouse element references
			_mouseElements.Clear();
			SetStatus(Status.Default);

			base.OnMouseUp (e);
		}

		protected override void OnClick(EventArgs e)
		{
			if (_mouseElements.MouseStartElement != null && _mouseElements.MouseStartElement == _mouseElements.MouseMoveElement) 
			{
				OnElementClick(_mouseElements.MouseStartElement);
			}
			else
			{
				OnDiagramClick();
			}

			base.OnClick (e);
		}

		protected override void OnDoubleClick(EventArgs e)
		{
			//Commented out second clause due to non firing of event with high volume of elements
			if (_mouseElements.MouseStartElement != null) // && _mouseElements.MouseStartElement == _mouseElements.MouseMoveElement) 
			{
				OnElementDoubleClick(_mouseElements.MouseStartElement);
			}
			else
			{
				OnDiagramDoubleClick();
			}

			base.OnDoubleClick (e);
		}

		protected override void OnLayout(System.Windows.Forms.LayoutEventArgs levent)
		{
			base.OnLayout(levent);
			
			CheckDiagramSize();
			SetResizeRectangles();
			
			DrawDiagram(_controlRect);
		}

		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			SetScrollRectangles();
			DrawDiagram(e.ClipRectangle);
			base.OnPaint(e);
		}

		//Do not call base implementation
		protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs pevent)
		{
			DrawDiagram(pevent.ClipRectangle);
		}

		//Detect end of acroll
		protected override void WndProc(ref Message m)
		{
			//Check for scroll message, with end scroll w param
			if (m.Msg == WM_VSCROLL || m.Msg == WM_HSCROLL)
			{
				if (m.WParam.ToInt32() == SB_ENDSCROLL)
				{
					//Get a sorted list and rerender and draw
					Refresh();
				}
			}
			base.WndProc (ref m);
		}

		
		#endregion

		#region Events

		//Occurs when an element is added to the elements collection
		private void Element_Insert(object sender, ElementsEventArgs e)
		{
			//Raise the ElementInserted event
			OnElementInserted(e.Value);
		}

		//Occurs when an element is removed from the elements collection
		private void Element_Remove(object sender, ElementsEventArgs e)
		{
            //Raise the ElementRemoved event
            OnElementRemoved(e.Value);
		}

		#endregion

		#region Implementation
		
		private void InvalidateImplementation(Rectangle rectangle)
		{
			//Exit if suspended
			if (Suspended) return;

			//Render and draw
			if (Render != null)
			{
				if (rectangle.IsEmpty) rectangle = Render.RenderRectangle;

                Render.Layers = Model.Layers;
                Render.Elements = Model.Elements;
				Render.RenderDiagram(rectangle, Paging);
				DrawDiagram(_controlRect);
			}
		}

		protected void DrawDiagram(Rectangle clipRectangle)
		{
			//if (Suspended) return;
			if (_render == null) return;
			if (_render.Bitmap == null) return;
			if (clipRectangle.IsEmpty) return;

			RectangleF sourceRectF = new RectangleF();
	
			//Set up the source rectangle from the rendered backbuffer
			sourceRectF.X = _pageRect.X + clipRectangle.X - _render.RenderRectangle.X;
			sourceRectF.Y = _pageRect.Y + clipRectangle.Y - _render.RenderRectangle.Y;
			sourceRectF.Width = clipRectangle.Width;
			sourceRectF.Height = clipRectangle.Height;

			Graphics graphics = base.CreateGraphics();

			try
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
	
				//Draw image unscaled a lot faster then drawimage, so try use
				if (sourceRectF.X == 0 && sourceRectF.Y == 0 && sourceRectF.Width == _controlRect.Width && sourceRectF.Height == _controlRect.Height)
				{
					graphics.DrawImageUnscaled(_render.Bitmap, new Point(0,0));
				}
				else
				{
					graphics.DrawImage(_render.Bitmap, clipRectangle.X, clipRectangle.Y, sourceRectF, GraphicsUnit.Pixel); //##in use during hectic movement
				}

				graphics.Dispose();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Error drawing diagram " + ex.ToString());
			}
			finally
			{
				if (graphics != null) graphics.Dispose();
			}
		}
		
		private void CheckDiagramSize()
		{
			if (_render == null) return;
			
			//Call base sub to prevent recursive loop
			if (base.AutoScroll) base.AutoScrollMinSize = Size.Round(GetDiagramSize());		
		}

        private SizeF GetDiagramSize()
        {
            float width;
            float height;

            //If page mode, make sure autoscroll size is diagram size
            if (Paging.Enabled)
            {
                width = Paging.PageSize.Width;
                height = Paging.PageSize.Height;

                //Scale the width and height depending on the zoom
                width = width * Render.ScaleFactor;
                height = height * Render.ScaleFactor;

                //Add 20 pixels on each side of the diagram to give paged look
                width = width + Paging.Padding.Width;
                height = height + Paging.Padding.Height;
            }
            //Make diagram size at least control size
            else
            {
                width = Model.Size.Width;
                height = Model.Size.Height;

                if (width < this.Width) width = this.Width;
                if (height < this.Height) height = this.Height;

                //**DiagramSize = Size.Round(new SizeF(width, height));

                //Scale down by zoom for autoscroll
                width = width * Zoom / 100;
                height = height * Zoom / 100;
            }

            return new SizeF(width, height);
        }

		private void SetResizeRectangles()
		{
			if (_render == null) return;

			//Sets up the diagram size in the renderer
			Render.DiagramSize = GetDiagramSize();
			
			//Set up workspace size
			SetPagedSettings();

			Rectangle renderRect;

			int x = 0;
			int y = 0;
			int width = 0;
			int height = 0;

			//Get the scroll offsets as positive values
			//Catch errors in vs 2005 beta 2
			try
			{
				x = Math.Abs(this.DisplayRectangle.X);
				y = Math.Abs(this.DisplayRectangle.Y);
			}
			catch
			{
				
			}
			
			width = this.Width;
			height = this.Height;

			//check for resize
            if (Paging.Enabled || (width > _render.RenderRectangle.Width || height > _render.RenderRectangle.Height))
			{
				//Calculate the zoom width and height
				int scaleWidth = Convert.ToInt32(Model.Size.Width * Render.ScaleFactor);
				int scaleHeight = Convert.ToInt32(Model.Size.Height * Render.ScaleFactor);

				//Add page offsets
                if (Paging.Enabled)
				{
					scaleWidth += Convert.ToInt32(Paging.Padding.Width); 
					scaleHeight += Convert.ToInt32(Paging.Padding.Height);
				}

				if (x + width > scaleWidth) width -= x + width - scaleWidth;
				if (y + height > scaleHeight) height -= y + height - scaleHeight;

				//Adjust for zoomed off areas
				if (width * Render.ScaleFactor < this.Width) width = Convert.ToInt32(this.Width * Render.ZoomFactor);
				if (height * Render.ScaleFactor < this.Height) height = Convert.ToInt32(this.Height * Render.ZoomFactor);

				//Set the new draw and render rectangles
				renderRect = new Rectangle(x, y, width, height);

				//Select the new render lists
				if (!Suspended) 
				{
                    Render.Layers = Model.Layers;
                    Render.Elements = Model.Elements;
					Render.RenderDiagram(renderRect, Paging);
				}
			}

			_controlRect = new Rectangle(0, 0, this.Width, this.Height);
		}

		internal bool SetScrollRectangles()
		{
			//During deserialization the DisplayRectangle property may throw an error
			//under .net 2.0. Check the parent instead of raising an exception.
			if (Parent == null) return false;
			
			Rectangle renderRect;

			int x = 0;
			int y = 0;
			int width = 0;
			int height = 0;

			bool returnFlag = false;

			x = Math.Abs(this.DisplayRectangle.X);
			y = Math.Abs(this.DisplayRectangle.Y);
			width = this.Width;
			height = this.Height;

			//Adjust for scrollbars
			//if (this.VScroll) width -= vScrollWidth;
			//if (this.HScroll) height -= hScrollWidth;
			
			Rectangle displayRect = new Rectangle(x, y, width, height);

			//check for scroll by comparing last control display rectangle to the current one
			if (x != _pageRect.X || y != _pageRect.Y)
			{
				returnFlag = true;

				//Set up a new render rectangle if the current render rectangle doesnt contain the display rect
				//Use the last rendered rectangle to check against
				if (! (_render.RenderRectangle.Contains(displayRect)))
				{
					//if we are scrolling positive on the x or y axis then double that axis display width render
					if (x > _pageRect.X) width *= 3;
					if (y > _pageRect.Y) height *= 3;

					//If we scroll negative on an axis, pull the render back by x by the display width and double
					if (x < _pageRect.X)
					{
						x -= width;
						width *= 3;
						if (x < 0)
						{
							width += x;
							x = 0;
						}
					}
					if (y < _pageRect.Y)
					{
						y -= height;
						height *= 3;
						if (y < 0)
						{
							height += y;
							y = 0;
						}
					}

					//Calculate the zoom width and height
					int scaleWidth = Convert.ToInt32(Model.Size.Width * Render.ScaleFactor);
					int scaleHeight = Convert.ToInt32(Model.Size.Height * Render.ScaleFactor);

					//Add page offsets
                    if (Paging.Enabled)
					{
						scaleWidth += Convert.ToInt32(Paging.Padding.Width); 
						scaleHeight += Convert.ToInt32(Paging.Padding.Height);
					}

					if (x + width > scaleWidth) width -= x + width - scaleWidth;
					if (y + height > scaleHeight) height -= y + height - scaleHeight;

					//Adjust for zoomed off areas
					if (width * Render.ScaleFactor < this.Width) width = Convert.ToInt32(this.Width * Render.ZoomFactor);
					if (height * Render.ScaleFactor < this.Height) height = Convert.ToInt32(this.Height * Render.ZoomFactor);

					//Set the new draw and render rectangles
					renderRect = new Rectangle(x, y, width, height);

                    Render.Layers = Model.Layers;
                    Render.Elements = Model.Elements;
					Render.RenderDiagram(renderRect, Paging);
				}
			}

			//Set the page rectangle to the display area for refreshes
			_pageRect = displayRect;
			_controlRect = new Rectangle(0, 0, width, height);

			return returnFlag;
		}

		private PointF TranslatePoint(Point point)
		{
			return TranslatePoint(point.X,point.Y);
		}

		//Returns a diagram point from the screen
		private PointF TranslatePoint(int x, int y)
		{
			float zoom = _render.ZoomFactor;
			PointF workspaceOffset = new PointF(0,0);

            if (Paging.Enabled) workspaceOffset = Paging.WorkspaceOffset;

			return new PointF(((x - DisplayRectangle.X - workspaceOffset.X ) * zoom), ((y - DisplayRectangle.Y - workspaceOffset.Y) * zoom));
		}

		//Returns a control point from a diagram point
		private Point TranslateDiagram(float x, float y)
		{
			float zoom = _render.ZoomFactor;
			PointF workspaceOffset = new PointF(0,0);
            if (Paging.Enabled) workspaceOffset = Paging.WorkspaceOffset;

			return Point.Round(new PointF((x / zoom) + DisplayRectangle.X + workspaceOffset.X, (y / zoom) + DisplayRectangle.Y + workspaceOffset.Y));
		}

		//Zooms a standard rectangle
		private RectangleF TranslateRectangle(RectangleF rect)
		{
			float zoom = _render.ZoomFactor;
			PointF workspaceOffset = new PointF(0,0);

            if (Paging.Enabled) workspaceOffset = Paging.WorkspaceOffset;
		
			return new RectangleF((rect.X - workspaceOffset.X) * zoom, (rect.Y - workspaceOffset.Y) * zoom, rect.Width * zoom, rect.Height * zoom);
		}

		//Returns the top element from a diagram point
		private Element GetElement(PointF location, ElementList renderlist)
		{
			Element bestElement = null;
			Port bestPort = null;
			Layer currentLayer = Model.Layers.CurrentLayer;

			if (!currentLayer.Visible) return null;

			foreach (Element element in renderlist)
			{
				if (element.Layer == currentLayer)
				{
					//Selection Handles always beat anything else
					if (element is ISelectable)
					{
						ISelectable selectable = (ISelectable) element;

						if (selectable.Selected && element.Handles != null) 
						{
							PointF local = new PointF(location.X - element.Bounds.X, location.Y - element.Bounds.Y);

							foreach (Handle handle in element.Handles)
							{
								if (handle.Path.IsVisible(local)) return element;
							}
						}
					}
				
					//Check for ports
					if (element is IPortContainer)
					{		
						IPortContainer container = (IPortContainer) element;
						foreach (Port port in container.Ports.Values)
						{
							if (port.Visible && port.Contains(location))
							{
								if (bestElement == null || element.ZOrder < bestElement.ZOrder) 
								{
									bestElement = element;	
									bestPort = port;
								}
							}
						}
					}

					//Check element by calling Contains method which is overriden in most classes
					if (element.Visible && element.Contains(location))
					{
						if (bestElement == null) bestElement = element;
						if (element.ZOrder < bestElement.ZOrder) 
						{
							bestElement = element;
							bestPort = null;
						}
					}
				}
			}

			if (bestPort == null)
			{
				return bestElement;
			}
			else
			{
				return bestPort;
			}
		}

		private void ResetLines(Shape shape)
		{
			Suspend();

			//Loop through each line and create a remove list
			foreach (Link line in Model.Lines.Values)
			{
				if (line.Start.DockedElement != null && line.Start.DockedElement == shape) line.Start.Location = line.FirstPoint;
				if (line.End.DockedElement != null && line.End.DockedElement == shape) line.End.Location = line.LastPoint;
			}

			Resume();
		}

		private void SetPagedSettings()
		{
            if (Paging.Enabled) 
			{
				if (AutoScroll)
				{
					int width;
					int height;

					width = (AutoScrollMinSize.Width < Width) ? Width : AutoScrollMinSize.Width;
					height = (AutoScrollMinSize.Height < Height) ? Height : AutoScrollMinSize.Height;

					Paging.SetWorkspaceSize(new Size(width,height));
				}
				else
				{
					Paging.SetWorkspaceSize(Size);
				}

				//Set up page size**
				//Paging.PageSize = new SizeF(Model.Size.Width * Render.ScaleFactor, Model.Size.Height * Render.ScaleFactor);
                
                //Set to portrait A4
                Graphics g = null;
                if (Render.Bitmap != null) g = Graphics.FromImage(Render.Bitmap);
                SizeF size = Geometry.GetPaperSize(DiagramUnit.Pixel, Crainiate.Diagramming.Printing.PaperSizes.Iso.A4, g);
                SizeF scaledSize = new SizeF(size.Width * Render.ScaleFactor, size.Height * Render.ScaleFactor);
                if (g != null) g.Dispose();

                Paging.PageSize = scaledSize;
				
				//Set up workspace offset
				int offsetX = Convert.ToInt32((Paging.WorkspaceSize.Width - Paging.PageSize.Width) / 2);
				int offsetY = Convert.ToInt32((Paging.WorkspaceSize.Height - Paging.PageSize.Height) / 2);

				if (offsetX < 0) offsetX = 0;
				if (offsetY < 0) offsetY = 0;

				Paging.SetWorkspaceOffset(new Point(offsetX,offsetY));

                //Setup paging offset by using the page number as a column and row of the model size
                if (Paging.Page > 1 && !Model.Size.IsEmpty)
                {
                    //Get number of columns
                    float columns = (Model.Size.Width / Paging.PageSize.Width);
                    int columnsInteger = Convert.ToInt32(columns);
                    
                    //Add a column if we have a fractional column at the end
                    if (columns != columnsInteger) columnsInteger++;

                    //Work out the column and row for the page
                    int column = Paging.Page % columnsInteger;
                    int row = Paging.Page / columnsInteger;

                    //Add to the row if not the fnal column
                    if (column < columns) row += 1;

                    //Create an offset from the column and row position of the page
                    Paging.SetPageOffset(Point.Round(new PointF((column - 1) * Paging.PageSize.Width, (row - 1) * Paging.PageSize.Height)));
                }
			}
		}

		protected internal void SaveStatus()
		{
			_saveStatus = _status;
		}

		protected internal void RestoreStatus()
		{
			SetStatus(_saveStatus);
		}

        private Shape AddShapeImplementation(string key, PointF location, StencilItem stencil, SizeF size)
        {
            Shape shape = Controller.Factory.CreateShape(location, size);
            shape.Location = location;

            //Set values if not provided
            if (key == null || key == "") key = Model.Shapes.CreateKey();
            if (stencil == null) stencil = Singleton.Instance.DefaultStencilItem;

            //Set size
            if (!size.IsEmpty) shape.Size = size;

            //Set shape's stencil
            shape.StencilItem = stencil;
            stencil.CopyTo(shape);

            //Add and return the new shape
            Controller.Model.Shapes.Add(key, shape);
            return shape;
        }

		#endregion
	}
}

