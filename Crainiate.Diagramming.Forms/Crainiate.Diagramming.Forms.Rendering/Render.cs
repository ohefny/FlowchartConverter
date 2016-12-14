// (c) Copyright Crainiate Software 2010

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections.Generic;

namespace Crainiate.Diagramming.Forms.Rendering
{
    //Base class for rendering elements in a winforms diagram
	public class Render
	{
		//Property variables
		private ElementList _elementRenderList;

		private Layers _layers;
		private System.Drawing.Image _backgroundImage = null;
		
		private bool _alphaCorrection = true;
		private bool _drawGrid = false;
		private bool _drawSelections = false;
		
		private int _suspendCount;
		private bool _locked;
		private float _zoomPerc = 100;

		private Rectangle _renderRectangle;
		private SizeF _diagramSize;
		
		private SizeF _pageLineSize;
		private bool _drawPageLines = false;

		private Color _backcolor = SystemColors.Window;

		private CompositingMode _compositingMode = CompositingMode.SourceOver;
		private CompositingQuality _compositingQuality = CompositingQuality.AssumeLinear;
		private PixelOffsetMode _pixelOffsetMode = PixelOffsetMode.Default;
		private ActionStyle _actionStyle;
		
		private Color _gridColor = Color.FromArgb(128,Color.Silver);
		private Size _gridSize = new Size(20, 20);
		private GridStyle _gridStyle = GridStyle.Complex;

		private Matrix _transform;

        private bool _summary;

		//Working Variables
		private float _zoomFactor = 1;
		private float _scaleFactor = 1;

		private byte _worldOpacity = 100;

		private Bitmap _graphicsStateBitmap; //Stores a copy of the current back buffer
		private Bitmap _gridBitmap; //Stores a render of the current grid
		private Bitmap _bitmap; //Stores the current render back buffer

		private Layer _currentLayer = null;

        private Dictionary<Type, IFormsRenderer> _renderers;

		#region Interface

		//Events
		public event RenderEventHandler PreRender;
		public event RenderEventHandler PostRender;
        public event LayerRenderEventHandler LayerPreRender;
        public event LayerRenderEventHandler LayerPostRender;

		//Constructor
		public Render()
		{
			ActionStyle = Singleton.Instance.DefaultActionStyle;

            _renderers = new Dictionary<Type, IFormsRenderer>();
            AddDefaultRenderers();
		}

		//Properties
		//Sets or retrieves the value of the alpha correction property.
		public virtual bool AlphaCorrection
		{
			get
			{
				return _alphaCorrection;
			}
			set
			{
				_alphaCorrection = value;
				DisposeGraphicsStateBitmap();
				DisposeGridBitmap();
			}
		}

		public virtual ActionStyle ActionStyle
		{
			get
			{
				return _actionStyle;
			}
			set
			{
				_actionStyle = value;
			}
		}
			
		//Sets or retrieves the color used to render the background
		public virtual Color BackColor
		{
			get
			{
				return _backcolor;
			}
			set
			{
				_backcolor = value;
				DisposeGraphicsStateBitmap();
				DisposeGridBitmap();
			}
		}

		//Sets or retrieves the background image for the diagram.
		public virtual System.Drawing.Image BackgroundImage
		{
			get
			{
				return _backgroundImage;
			}
			set
			{
				_backgroundImage = value;
				DisposeGraphicsStateBitmap();
				DisposeGridBitmap();
			}
		}

		//Retrieves the backbuffer bitmap
		public virtual Bitmap Bitmap
		{
			get
			{
				return _bitmap;
			}
		}

        public virtual Bitmap GraphicsStateBitmap
        {
            get
            {
                return _graphicsStateBitmap;
            }
        }

		//Specifies the whether objects are drawn with alpha blending
		public virtual CompositingMode CompositingMode
		{
			get
			{
				return _compositingMode;
			}
			set
			{
				_compositingMode = value;
				DisposeGraphicsStateBitmap();
				DisposeGridBitmap();
			}
		}

		//Specifies how objects are compositied when drawn together
		public virtual CompositingQuality CompositingQuality
		{
			get
			{
				return _compositingQuality;
			}
			set
			{
				_compositingQuality = value;
				DisposeGraphicsStateBitmap();
				DisposeGridBitmap();
			}
		}

		//Sets or retrieves the color used to render the grid.
		public virtual Color GridColor
		{
			get
			{
				return _gridColor;
			}
			set
			{
				_gridColor = value;
				DisposeGraphicsStateBitmap();
				DisposeGridBitmap();
			}
		}

		//Sets or retrieves the size object used to determine the grid spacing.
		public virtual Size GridSize
		{
			get
			{
				return _gridSize;
			}
			set
			{
				_gridSize = value;
				DisposeGraphicsStateBitmap();
				DisposeGridBitmap();
			}
		}

		//Sets or retrieves the size object used to determine the grid spacing.
		public virtual GridStyle GridStyle
		{
			get
			{
				return _gridStyle;
			}
			set
			{
				_gridStyle = value;
				DisposeGraphicsStateBitmap();
				DisposeGridBitmap();
			}
		}

		//Determines whether the grid is displayed
		public virtual bool DrawGrid
		{
			get
			{
				return _drawGrid;
			}
			set
			{
				_drawGrid = value;
				DisposeGraphicsStateBitmap();
				DisposeGridBitmap();
			}
		}

		//Determines whether the grid is displayed
		public virtual bool DrawSelections
		{
			get
			{
				return _drawSelections;
			}
			set
			{
				_drawSelections = value;
			}
		}

		//Returns the current transform matrix
		public virtual Matrix Transform
		{
			get
			{
				return _transform;
			}
		}

		//Returns the layer that is currently being rendered, or null if not rendering
		public virtual Layer CurrentLayer
		{
			get
			{
				return _currentLayer;
			}
		}

		//Gets or set a value specifying how pixels are offset during rendering.")]
		public virtual PixelOffsetMode PixelOffsetMode
		{
			get
			{
				return _pixelOffsetMode;
			}
			set
			{
				_pixelOffsetMode = value;
			}
		}

		//Sets or gets the list of elements to be rendered
		public virtual ElementList Elements
		{
			get
			{
				return _elementRenderList;
			}
			set
			{
				_elementRenderList = value;
                ClearRenderers();
			}
		}

		//Sets or gets the list of elements to be rendered
		public virtual Layers Layers
		{
			get
			{
				return _layers;
			}
			set
			{
				_layers = value;
			}
		}

		//Returns whether the render engine is suspended")]
		public virtual bool Suspended
		{
			get
			{
				return _suspendCount > 0;
			}
		}

		//Returns whether the renderer is currently locked.")]
		public virtual bool Locked
		{
			get
			{
				return _locked;
			}
		}

		//Sets or retrieves the rectangle defining the area currently in view.
		public virtual Rectangle RenderRectangle
		{
			get
			{
				return _renderRectangle;
			}
			set
			{
				_renderRectangle = value;
				DisposeGraphicsStateBitmap();
			}
		}

		//Sets or gets the total size of the diagram that is being rendered
		public virtual SizeF DiagramSize
		{
			get
			{
				return _diagramSize;
			}
			set
			{
				_diagramSize = value;
			}
		}

		//Sets or gets the total size of the diagram that is being rendered
		public virtual SizeF PageLineSize
		{
			get
			{
				return _pageLineSize;
			}
			set
			{
				if (! _pageLineSize.Equals(value))
				{
					_pageLineSize = value;
					DisposeGraphicsStateBitmap();
				}
			}
		}

		//Determines whether page lines are displayed
		public virtual bool DrawPageLines
		{
			get
			{
				return _drawPageLines;
			}
			set
			{
				_drawPageLines = value;
				DisposeGraphicsStateBitmap();
			}
		}

		//Sets or retrieves the current zoom Layer as a percentage.")]
		public virtual float Zoom
		{
			get
			{
				return _zoomPerc;
			}
			set
			{
				if (value > 0 && value != _zoomPerc)
				{
					_zoomPerc = value;
					_zoomFactor = Convert.ToSingle(100 / value);
					_scaleFactor = Convert.ToSingle(value / 100);
					DisposeGraphicsStateBitmap();
					DisposeGridBitmap();
				}
			}
		}

		//Retrieves the scaling factor for this render
		public virtual float ScaleFactor
		{
			get
			{
				return _scaleFactor;
			}
		}

		//Retrieves the zooming factor for this render
		public virtual float ZoomFactor
		{
			get
			{
				return _zoomFactor;
			}
		}

        //Determines whether the grid is displayed
        public virtual bool Summary
        {
            get
            {
                return _summary;
            }
            set
            {
                _summary = value;
                DisposeGraphicsStateBitmap();
                DisposeGridBitmap();
            }
        }

		//Methods

		//Locks the render class.
		public virtual void Lock()
		{
			_graphicsStateBitmap = (Bitmap) _bitmap.Clone();
			_locked = true;
		}

		//Unlocks the render class.")]
		public virtual void Unlock()
		{
			DisposeGraphicsStateBitmap();
			_locked = false;
		}

        public virtual void AddRenderer(Type type, IFormsRenderer render)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (render == null) throw new ArgumentNullException("render");
            
            _renderers.Add(type, render);
        }

        public virtual void RemoveRenderer(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            
            _renderers.Remove(type);
        }

        public virtual IFormsRenderer GetRenderer(IRenderable element)
        {
            if (element.Renderer == null)
            {
                //Lookup renderer for type
                IFormsRenderer render = null;
                if (!_renderers.TryGetValue(element.GetType(), out render)) throw new RenderException(string.Format("A renderer was not found for type {0}.", element.GetType().FullName));
                element.Renderer = render; 
            }

            return element.Renderer as IFormsRenderer;
        }

        //Clesrs the current renderer for all elements
        public virtual void ClearRenderers()
        {
            foreach (Element element in Elements)
            {
                element.Renderer = null;
            }
        }

		//Draws the diagram area contained in the specified rectangle
		public virtual void RenderDiagram(Rectangle renderRectangle, Paging paging)
		{			
            if (! Suspended)
			{
				if (! renderRectangle.Equals(_renderRectangle) || paging.HasChanges) DisposeGraphicsStateBitmap();
                paging.HasChanges = false;

                using (Graphics graphics = GetGraphics(renderRectangle, paging))
                {
                    if (graphics == null) return;

                    RenderEventArgs e = new RenderEventArgs(graphics, renderRectangle);
                    OnPreRender(e);

                    RenderLayers(graphics, renderRectangle, paging);

                    OnPostRender(e);
                }
            }
			
			_renderRectangle = renderRectangle;
		}

		//Suspends all render operations.
		public virtual void Suspend()
		{
			_suspendCount += 1;
		}

		//Resumes all render operations.
		public virtual void Resume()
		{
			_suspendCount -= 1;
		}

		//Resumes all render operations.
		public virtual void Resume(bool Force)
		{
			if (Force)
			{
				_suspendCount = 0;
			}
			else
			{
				_suspendCount -= 1;
			}
		}

		//Adjusts color Layers using width and opacity
		public virtual Color AdjustColor(Color color, float width,float opacity)
		{
			return AdjustColorImplementation(color,width,opacity);
		}

        protected internal void SetBitmap(Bitmap value)
        {
            _bitmap = value;
        }

        protected internal void SetTransform(Matrix value)
        {
            _transform = value;
        }

        protected virtual void AddDefaultRenderers()
        {
            AddRenderer(typeof(Element), new ElementRender());
            AddRenderer(typeof(Solid), new SolidRender());
            AddRenderer(typeof(Shape), new ShapeRender());
            AddRenderer(typeof(Group), new GroupRender());
            AddRenderer(typeof(ComplexShape), new ComplexShapeRender());
            AddRenderer(typeof(Table), new TableRender());
            AddRenderer(typeof(TableGroup), new TableGroupRender());
            AddRenderer(typeof(TableRow), new TableRowRender());
            AddRenderer(typeof(Marker), new MarkerRender());
            AddRenderer(typeof(Arrow), new MarkerRender());
            AddRenderer(typeof(Port), new PortRender());
            AddRenderer(typeof(TablePort), new PortRender());

            AddRenderer(typeof(Link), new LinkRender());
            AddRenderer(typeof(Connector), new ConnectorRender());
            AddRenderer(typeof(ComplexLine), new ComplexLineRender());
            AddRenderer(typeof(BlockLine), new BlockLineRender());
            AddRenderer(typeof(TreeLine), new TreeLineRender());

            AddRenderer(typeof(Image), new ImageRender());
            AddRenderer(typeof(Label), new LabelRender());
        }

		//Raises the PreRender event
		protected virtual void OnPreRender(RenderEventArgs e)
		{
			if (PreRender != null) PreRender(this, e);
		}

		//Raises the PostRender event
        protected virtual void OnPostRender(RenderEventArgs e)
		{
			if (PostRender != null) PostRender(this, e);
		}

        //Raises the PreRender event
        protected virtual void OnLayerPreRender(LayerRenderEventArgs e)
        {
            if (LayerPreRender != null) LayerPreRender(this, e);
        }

        //Raises the PostRender event
        protected virtual void OnLayerPostRender(LayerRenderEventArgs e)
        {
            if (LayerPostRender != null) LayerPostRender(this, e);
        }

		#endregion

		#region Implementation

		//Clears memory and resets graphicsstate bitmap
		protected void DisposeBufferBitmap()
		{
			try
			{
				if (! (_bitmap == null))
				{
					_bitmap.Dispose();
					_bitmap = null;
				}
			}
			catch {}
		}

		//Clears memory and resets graphicsstate bitmap
		protected internal void DisposeGraphicsStateBitmap()
		{
			try
			{
				if (! (_graphicsStateBitmap == null))
				{
					_graphicsStateBitmap.Dispose();
					_graphicsStateBitmap = null;
				}
			}
			catch {}
		}

		//Clears memory and resets grid bitmap
		protected void DisposeGridBitmap()
		{
			try
			{
				if (! (_gridBitmap == null))
				{
					_gridBitmap.Dispose();
					_gridBitmap = null;
				}
			}
			catch {}
		}

		//sets up the internal graphics object from a bitmap back buffer
		public virtual Graphics GetGraphics(Rectangle renderRectangle, Paging paging)
		{
			if (renderRectangle.Width == 0 || renderRectangle.Height == 0) return null;
            if (paging == null) throw new ArgumentNullException("paging");

			Graphics graphics = null;

			try
			{
				if (_graphicsStateBitmap == null)
				{
					//Set up a new bitmap, can throw errors when in use eg whilst scrolling
					DisposeBufferBitmap();
					_bitmap = new Bitmap(Convert.ToInt32(renderRectangle.Width) , Convert.ToInt32(renderRectangle.Height), PixelFormat.Format32bppPArgb);

					//Get a graphics handle from the new back buffer
					graphics = Graphics.FromImage(_bitmap);
					
					//Set up the page transforms and draw the workspace
					if (paging.Enabled) 
					{
						graphics.TranslateTransform(-renderRectangle.X,-renderRectangle.Y);
						RenderPage(graphics,paging);
                      
						//Translate the offset
						if (! paging.WorkspaceOffset.IsEmpty)
						{
							//Add a region so that only the page gets drawn
							graphics.Clip = new Region(new RectangleF(paging.WorkspaceOffset,paging.PageSize));
						}
						graphics.TranslateTransform(renderRectangle.X,renderRectangle.Y);
					}

                    

					//Draw grid over background image
					if (DrawGrid)
					{
						float gridWidth = _gridSize.Width * _scaleFactor;
						float gridHeight = _gridSize.Height * _scaleFactor;

						float gridOffsetX = gridWidth-(paging.WorkspaceOffset.X % gridWidth);
						float gridOffsetY = gridHeight-(paging.WorkspaceOffset.Y % gridHeight);

						//If there is no grid map, then create it
						if (_gridBitmap == null)
						{
							Graphics gridGraphics = null;

							//Background image will have been pre-sized
							//If there is one then gridmap becomes size of background image
							//else gridmap becomes 10 times gridsize
							if (_backgroundImage == null)
							{
								_gridBitmap = new Bitmap(Convert.ToInt32(gridWidth * 10), Convert.ToInt32(gridHeight * 10), PixelFormat.Format32bppPArgb);
							}
							else
							{
								_gridBitmap = new Bitmap(_backgroundImage.Width, _backgroundImage.Height, PixelFormat.Format32bppPArgb);
							}

							//Get the graphics object from the gridmap, clear it or paint it with the background
							gridGraphics = Graphics.FromImage(_gridBitmap);
							if (_backgroundImage == null)
							{
								gridGraphics.Clear(_backcolor);
							}
							else
							{
								gridGraphics.DrawImageUnscaled(_backgroundImage, new Point(0, 0));
							}

							RenderGrid(gridGraphics, 0, 0, gridWidth, gridHeight, _gridBitmap.Width, _gridBitmap.Height);
						}
						
						//Set up grid texture rectangle
						RectangleF textureRectangle = renderRectangle;

						//The grid is shifted to compensate for the page offset and therefore the renderrectangle needs to be increased slightly to make up for the shift
                        if (paging.Enabled) textureRectangle = new RectangleF(renderRectangle.X, renderRectangle.Y, renderRectangle.Width + gridWidth, renderRectangle.Height + gridHeight);
						
						//Adjust grid texture position for paged offset and render rectangle
						graphics.TranslateTransform(-renderRectangle.X, -renderRectangle.Y);
                        if (paging.Enabled) graphics.TranslateTransform(-gridOffsetX, -gridOffsetY);

						TextureBrush brush = new TextureBrush(_gridBitmap);
						graphics.FillRectangle(brush, textureRectangle);
						
						//Reset transform
                        if (paging.Enabled) graphics.TranslateTransform(gridOffsetX, gridOffsetY);
						graphics.TranslateTransform(renderRectangle.X, renderRectangle.Y);
					}
					else
					{
						//No background image then just clear, else just background image
						if (_backgroundImage == null)
						{
							graphics.Clear(_backcolor);
						}
						else
						{
							graphics.TranslateTransform(renderRectangle.X, renderRectangle.Y);
							TextureBrush objBrush = new TextureBrush(_backgroundImage);
							graphics.FillRectangle(objBrush, renderRectangle);
							graphics.TranslateTransform(renderRectangle.X, renderRectangle.Y);
						}
					}
					_graphicsStateBitmap = (Bitmap) _bitmap.Clone();
				}
				else
				{
					DisposeBufferBitmap();
					_bitmap = (Bitmap) _graphicsStateBitmap.Clone();
					graphics = Graphics.FromImage(_bitmap);
				}

				//Set up the transform matrix
				_transform = new Matrix();

				//Set up the page transform
				//Translate the workspace and page offset**
                if (paging.Enabled)
				{
					if (!paging.WorkspaceOffset.IsEmpty) _transform.Translate(paging.WorkspaceOffset.X,paging.WorkspaceOffset.Y);
                    if (!paging.PageOffset.IsEmpty) _transform.Translate(-paging.PageOffset.X, -paging.PageOffset.Y);

					//Disable page clipping set earlier
					graphics.ResetClip();
				}

				//Set the scale and world transformation
				if (_zoomPerc != 100) _transform.Scale(_scaleFactor, _scaleFactor);
				_transform.Translate(-renderRectangle.X * _zoomFactor, -renderRectangle.Y * _zoomFactor);
				
				//Apply transform matrix
				graphics.Transform = _transform;

				//Draw page outlines
				if (DrawPageLines && !PageLineSize.IsEmpty && !Locked) RenderPageLines(graphics,PageLineSize, DiagramSize);
				
				//Set the drawing options
				graphics.CompositingMode = _compositingMode;
				graphics.CompositingQuality = _compositingQuality;
				graphics.PixelOffsetMode = _pixelOffsetMode;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Error getting render graphics" + ex.ToString());

			}
			return graphics;
		}

		public virtual void RenderLayers(Graphics graphics, Rectangle renderRectangle, Paging paging)
		{
			//Render the elements if the renderer isnt locked
            if (!_locked)
            {
                foreach (Layer layer in Layers)
		        {
			        if (layer.Visible) 
			        {
				        _worldOpacity = layer.Opacity;
				        _currentLayer = layer;

                        LayerRenderEventArgs e2 = new LayerRenderEventArgs(graphics, renderRectangle, layer);
                        OnLayerPreRender(e2);

                        if (!e2.Cancel)
                        {
                            RenderLayer(graphics, layer, _elementRenderList, renderRectangle);
                            OnLayerPostRender(e2);
                        }
                    }
                }

		        //Reset current layer
		        _currentLayer = null;
            }
		}

		//Loop through layers and elements and render
        public virtual void RenderLayer(Graphics graphics, Layer layer, ElementList elementRenderList, RectangleF renderRectangle)
		{
            if (elementRenderList == null) throw new ArgumentNullException("elementRenderList");
            IFormsRenderer renderer = null;

			//Render shadows
			if (layer.DrawShadows)
			{
				foreach (Element element in elementRenderList)
				{
                    if (element.Layer == layer && element.DrawShadow && element.Visible && element.Bounds.IntersectsWith(renderRectangle) && (element.Group == null || element.Group.Expanded))
					{
                        GraphicsState graphicsState = graphics.Save();
                        Matrix matrix = graphics.Transform;
                        matrix.Translate(element.Bounds.X + layer.ShadowOffset.X, element.Bounds.Y + layer.ShadowOffset.Y);

                        //Shadow is not rotated
                        graphics.Transform = matrix;
                        graphics.SmoothingMode = element.SmoothingMode;

                        //Get the renderer for this type and render
                        renderer = GetRenderer(element);
                        renderer.RenderShadow(element, graphics, this);

                        graphics.Restore(graphicsState);
					}
				}
			}

			//Draw each element by checking if it is renderable and calling the render method
			foreach (Element element in elementRenderList)
			{
                if (element.Layer == layer && element.Visible && element.Bounds.IntersectsWith(renderRectangle) && (element.Group == null || element.Group.Expanded))
				{
                    //Draw shapes
                    GraphicsState graphicsState = graphics.Save();
                    Matrix matrix = graphics.Transform;

                    matrix.Translate(element.Bounds.X, element.Bounds.Y);

                    //Set up rotation and other transforms
                    if (element is ITransformable)
                    {
                        ITransformable rotatable = (ITransformable)element;
                        PointF center = new PointF(element.Bounds.Width / 2, element.Bounds.Height / 2);
                        matrix.RotateAt(rotatable.Rotation, center);
                    }

                    //Apply transform, mode, and render element
                    graphics.Transform = matrix;
                    graphics.SmoothingMode = element.SmoothingMode;

                    //Get the renderer for this type and render
                    renderer = GetRenderer(element);
                    renderer.RenderElement(element, graphics, this);

                    graphics.Restore(graphicsState);
				}
			}
		}

        protected virtual void RenderGrid(Graphics graphics, float startX, float startY, float gridWidth, float gridHeight, float totalWidth, float totalHeight)
        {
            Color gridColor;

            float x = 0;
            float y = 0;

            graphics.CompositingQuality = CompositingQuality.HighSpeed;
            graphics.SmoothingMode = SmoothingMode.None;

            //Set the grid color intensity
            if (_alphaCorrection)
            {
                graphics.CompositingMode = CompositingMode.SourceOver;
                int intensity = Convert.ToInt32(64 * _scaleFactor);
                if (intensity > 255) intensity = 255;
                if (intensity < 20) intensity = 20;
                gridColor = Color.FromArgb(intensity, _gridColor.R, _gridColor.G, _gridColor.B);
            }
            else
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                gridColor = _gridColor;
            }

            if (_gridStyle == GridStyle.Pixel)
            {
                for (x = startX; x <= totalWidth - 1; x = x + gridWidth)
                {
                    for (y = startX; y <= totalHeight - 1; y = y + gridHeight)
                    {
                        _gridBitmap.SetPixel(Convert.ToInt32(x), Convert.ToInt32(y), gridColor);
                    }
                }
            }
            else
            {
                Pen pen = new Pen(gridColor);
                bool swap = false;

                if (_gridStyle == GridStyle.Dot)
                {
                    pen.DashStyle = DashStyle.Dot;
                }
                else if (_gridStyle == GridStyle.DashDot)
                {
                    pen.DashStyle = DashStyle.DashDot;
                }
                else if (_gridStyle == GridStyle.DashDotDot)
                {
                    pen.DashStyle = DashStyle.DashDotDot;
                }
                else if (_gridStyle == GridStyle.Dash)
                {
                    pen.DashStyle = DashStyle.Dash;
                }

                //Draw vertical grid lines
                swap = true;
                for (x = startX; x <= totalWidth - 1; x = x + gridWidth)
                {
                    //Swap styles if complex
                    if (_gridStyle == GridStyle.Complex)
                    {
                        pen.DashStyle = (x % 100 == 0) ? DashStyle.Dash : DashStyle.Dot;
                        swap = !swap;
                    }
                    graphics.DrawLine(pen, x, 0, x, totalHeight);

                }

                //Draw horizontal grid lines
                swap = true;
                for (y = startX; y <= totalHeight - 1; y = y + gridHeight)
                {
                    //Swap styles if complex
                    if (_gridStyle == GridStyle.Complex)
                    {
                        pen.DashStyle = (y % 100 == 0) ? DashStyle.Dash : DashStyle.Dot;
                        swap = !swap;
                    }
                    graphics.DrawLine(pen, 0, y, totalWidth, y);
                }
                pen.Dispose();
            }
        }

		//Renders workspace and page outline
		protected virtual void RenderPage(Graphics graphics, Paging paging)
		{
			graphics.Clear(paging.WorkspaceColor);

			//Draw border
			Pen pen = new Pen(Color.FromArgb(66,65,66));
			RectangleF border = new RectangleF(paging.WorkspaceOffset, paging.PageSize);
			border.Inflate(1,1);
			graphics.DrawRectangle(pen, border.X, border.Y, border.Width, border.Height);
		}

		//Render lines to show page outlines
		protected virtual void RenderPageLines(Graphics graphics,SizeF pageSize,SizeF DiagramSize)
		{
			Pen pen = Singleton.Instance.PageOutlinePen;

			//Draw vertical lines
			for (float x = pageSize.Width; x < DiagramSize.Width; x += pageSize.Width)
			{
				graphics.DrawLine(pen,x,0,x,DiagramSize.Height);
			}

			//Draw horizontal lines
			for (float y = pageSize.Height; y < DiagramSize.Height; y += pageSize.Height )
			{
				graphics.DrawLine(pen,0,y,DiagramSize.Width,y);
			}
		}

		//Adjusts for opacity and alpha scale blending
		//Will reduce the opacity of a line with width * scale < 1
		private Color AdjustColorImplementation(Color color, float width,float opacity)
		{
			if (CompositingMode == CompositingMode.SourceCopy) return Color.FromArgb(255,color);

			float widthScale = width * _scaleFactor;
			
			if (widthScale == 0 || widthScale > 1) widthScale =1;
									
			//original alpha x width scaled x local opacity x Layer opacity
			int intensity = Convert.ToInt32(color.A * widthScale * opacity * _worldOpacity / 10000);

			//Half the intensity if locked ie is an action
			if (Locked) intensity = Convert.ToInt32(intensity * 0.75);
			
			if (intensity > 255) intensity = 255;
			if (intensity < 20) intensity = 20;

			return Color.FromArgb(intensity, color.R, color.G, color.B);
		}

    #endregion

	}
}
