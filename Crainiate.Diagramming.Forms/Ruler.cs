// (c) Copyright Crainiate Software 2010




using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Data;
using System.Windows.Forms;

namespace Crainiate.Diagramming.Forms
{
	[ToolboxBitmap(typeof(Ruler), "Resource.ruler.bmp")]
	public class Ruler : System.Windows.Forms.Control, IMessageFilter	
	{
		//Property variables
		private Bitmap _renderBitmap = null;
		private float _major;
		private float _minor;
		private float _mid;
		private float _start;
		private float _paddingOffset;
		private RulerUnit _unit;
		private Font _font;
		private bool _mouseTracking;
		private float _zoom;
		private RulerOrientation _orientation;
		private float _marginOffset;
		private RulerBorderStyle _borderStyle;
		private Color _gradientColor;
		
		private bool _drawGuides;
		private RectangleF[] _guides;

		private View _diagram;

		//Working variables
		private System.ComponentModel.Container components = null;
		private float _scaleFactor;
		private bool _suspended;
		private PointF _unitScaleFactor;

		private const int WM_MOUSEMOVE = 0x0200;

		#region Interface

		//Constructors
		public Ruler()
		{
			InitializeComponent();
			
			_font = new Font("Microsoft Sans Serif",7.0F);
			_unit = RulerUnit.Pixel;
			_major = 100;
			_minor = 10;
			_mid = 50;
			_start = 0;
			_zoom = 100;
			_scaleFactor = 1;
			_unitScaleFactor = new PointF(1,1);
			_orientation = RulerOrientation.Top;
			_borderStyle = RulerBorderStyle.Edge;
			_drawGuides = true;
			_gradientColor = BackColor;
		}

		//Properties
		//Major divisions
		[Category("Behaviour"), Description("Sets or retrieves the major increment in units for the ruler.")]
		public virtual float Major
		{
			get
			{
				return _major;
			}
			set
			{
				if (value != _major)
				{
					_major = value;
					Refresh();
				}
			}
		}

		//Minor divisions
		[Category("Behaviour"), Description("Sets or retrieves the minor increment in units for the ruler.")]
		public virtual float Minor
		{
			get
			{
				return _minor;
			}
			set
			{
				if (value != _minor)
				{
					_minor = value;
					Refresh();
				}
			}
		}

		//Middle divisions
		[Category("Behaviour"), Description("Sets or retrieves the middle increment in units for the ruler.")]
		public virtual float Mid
		{
			get
			{
				return _mid;
			}
			set
			{
				if (value != _mid)
				{
					_mid = value;
					Refresh();
				}
			}
		}

		//Start number
		[Category("Behaviour"), DefaultValue(0), Description("Sets or gets the starting value in units for the ruler.")]
		public virtual float Start
		{
			get
			{
				return _start;
			}
			set
			{
				if (value != _start)
				{
					_start = value;
					Refresh();
				}
			}
		}

		//Padding offset before start of measured area
		[Category("Appearance"), DefaultValue(0F), Description("Sets the distance a ruler beings from the start of the control.")]
		public virtual float PaddingOffset
		{
			get
			{
				return _paddingOffset;
			}
			set
			{
				if (value != _paddingOffset)
				{
					_paddingOffset = value;
					Refresh();
				}
			}
		}

		[Category("Appearance"), DefaultValue(0F), Description("Sets the distance from the padding to the origin of the ruler.")]
		public virtual float MarginOffset
		{
			get
			{
				return _marginOffset;
			}
			set
			{
				if (value != _marginOffset)
				{
					_marginOffset = value;
					Refresh();
				}
			}
		}

		[Category("Behaviour"),DefaultValue(RulerUnit.Pixel), Description("Sets or retrieves units used for measurement for the ruler.")]
		public virtual RulerUnit Units
		{
			get
			{
				return _unit;
			}
			set
			{
				_unit = value;
				SetDefaultUnitValues();
				SetUnitScaleFactors();
				Refresh();
			}
		}

		[Category("Appearance"),DefaultValue(RulerBorderStyle.None), Description("Sets or retrieves a value determining how the ruler border is drawn.")]
		public virtual RulerBorderStyle BorderStyle
		{
			get
			{
				return _borderStyle;
			}
			set
			{
				_borderStyle = value;
				Refresh();
			}
		}

		[Category("Appearance"), Description("Sets or retrieves the color used to render the background gradient.")]
		public virtual Color GradientColor
		{
			get
			{
				return _gradientColor;
			}
			set
			{
				_gradientColor = value;
				Refresh();
			}
		}

		[Category("Behaviour"),DefaultValue(false), Description("Determines whether the mouse position is tracked on the ruler.")]
		public virtual bool MouseTracking
		{
			get
			{
				return _mouseTracking;
			}
			set
			{
				if (_mouseTracking != value)
				{
					_mouseTracking = value;
					if (_mouseTracking)
					{
						Application.AddMessageFilter(this);
					}
					else
					{
						Application.RemoveMessageFilter(this);
					}
				}
			}
		}

		[Category("Behaviour"),DefaultValue(true), Description("When enabled draws shape tracking highlights on the ruler.")]
		public virtual bool DrawGuides
		{
			get
			{
				return _drawGuides;
			}
			set
			{
				if (_drawGuides != value)
				{
					_drawGuides = value;
					Refresh();
				}
			}
		}
		
		[Category("Behavior"), DefaultValue(100F), Description("Sets or retrieves the current zoom level as a percentage.")]
		public virtual float Zoom
		{
			get
			{
				return _zoom;
			}
			set
			{
				if (value != _zoom)
				{
					_zoom = value;
					_scaleFactor = Convert.ToSingle(value / 100);
					Refresh();
				}
			}
		}

		[Category("Behavior"), DefaultValue(RulerOrientation.Top), Description("Determines the orientation of the ruler.")]
		public virtual RulerOrientation Orientation
		{
			get
			{
				return _orientation;
			}
			set
			{
				if (_orientation != value)
				{
					_orientation = value;
					Refresh();
				}
			}
		}

		[Browsable(false), Category("Data"), Description("Retrieves a boolean value determining whether render and draw operations are suspended.")]
		public virtual bool Suspended
		{
			get
			{
				return _suspended;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Data"), Description("Sets or gets the diagram this ruler is measuring.")]
		public virtual View Diagram
		{
			get
			{
				return _diagram;
			}
			set
			{				
				_diagram = value;
				
				if (_diagram != null)
				{
					_diagram.ZoomChanged +=new EventHandler(Diagram_ZoomChanged);
					_diagram.Scroll += new ScrollEventHandler(Diagram_Scroll);
		
					if (_diagram is Diagram)
					{
						Diagram diagram = (Diagram) _diagram;
			
						diagram.UpdateActions += new UserActionEventHandler(Diagram_UpdateActions);
					}
				}
			}
		}

		[Browsable(false), Category("Data"), Description("Returns an array of rectangles describing the areas on the ruler to be highlighted.")]
		protected virtual RectangleF[] Guides
		{
			get
			{
				return _guides;
			}
		}

		//Methods
		[Description("Suspends draw operations for the ruler.")]
		public virtual void Suspend()
		{
			_suspended = true;
		}

		[Description("Resumes draw operations for the ruler.")]
		public virtual void Resume()
		{
			_suspended = false;
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		protected virtual void SetGuides(RectangleF[] rectangles)
		{
			_guides = rectangles;
		}

		#endregion

		#region Component Designer generated code
		
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		
		#endregion

		#region Events

		private void Diagram_ZoomChanged(object sender, EventArgs e)
		{
			Zoom = Diagram.Zoom;
		}

		private void Diagram_PagedChanged(object sender, EventArgs e)
		{
			if (Orientation == RulerOrientation.Top)
			{
				MarginOffset = Diagram.Paging.WorkspaceOffset.X;
			}
			else
			{
				MarginOffset = Diagram.Paging.WorkspaceOffset.Y;					
			}
		}
		
		private void Diagram_Scroll(object sender, ScrollEventArgs e)
		{
			Refresh();
		}

		private void Diagram_UserAction(object sender, UserActionEventArgs e)
		{
			if (DrawGuides)
			{
				CalculateGuides(e.Actions);
				Refresh();
			}
		}

		private void Diagram_UpdateActions(object sender, UserActionEventArgs e)
		{
			SetGuides(null);
			Refresh();
		}


		#endregion

		#region Overrides

		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
				Refresh();
			}
		}

		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
				Refresh();
			}
		}
		
		protected override void OnPaint(PaintEventArgs pe)
		{
			if (_renderBitmap == null) return;

			pe.Graphics.DrawImageUnscaled(_renderBitmap,new Point(0,0));	

			//Draw the mouse movement indicator
			if (MouseTracking)
			{
				//get mouse position in screen co-ordinates and convert to control
				Point mousePoint = PointToClient(Control.MousePosition);
				
				if (Orientation == RulerOrientation.Top)
				{
					if (mousePoint.X >= PaddingOffset && mousePoint.X < Width) 
					{
						pe.Graphics.DrawLine(Singleton.Instance.HighlightPen,mousePoint.X,0,mousePoint.X,Height);
					}
				}
				else
				{
					if (mousePoint.Y >= PaddingOffset && mousePoint.Y < Height) 
					{
						pe.Graphics.DrawLine(Singleton.Instance.HighlightPen,0,mousePoint.Y,Width,mousePoint.Y);
					}
				}
			}

			base.OnPaint(pe);
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			if (_renderBitmap == null) return;

			pevent.Graphics.DrawImageUnscaled(_renderBitmap,new Point(0,0));
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
			base.OnLayout (levent);
			
			if (DisplayRectangle.Width <=0 || DisplayRectangle.Height <= 0) return;

			_renderBitmap = new Bitmap(DisplayRectangle.Width,DisplayRectangle.Height,PixelFormat.Format32bppPArgb);
			Refresh();
		}

		public override void Refresh()
		{
			UpdateBuffer();
			base.Refresh();
		}
		
		#endregion

		#region Implementation

		public bool PreFilterMessage(ref Message m)
		{
			if (m.Msg == WM_MOUSEMOVE)
			{
				base.Refresh();
			}
			return false;
		}

		private void UpdateFromDiagram()
		{
			Suspend();
			Zoom = _diagram.Zoom;
			
			if (Orientation == RulerOrientation.Top)
			{
				MarginOffset = _diagram.Paging.WorkspaceOffset.X;
			}
			else
			{
				MarginOffset = _diagram.Paging.WorkspaceOffset.Y;					
			}

			Resume();
			Refresh();
		}

		private void UpdateBuffer()
		{
			if (_renderBitmap == null) return;
			if (Suspended) return;

			Graphics graphics = Graphics.FromImage(_renderBitmap);
			graphics.Clear(BackColor);

			if (Orientation == RulerOrientation.Top)
			{
				RenderRuler(graphics);
			}
			else
			{
				RenderRulerVertical(graphics);
			}
			graphics.Dispose();
		}

		private void RenderRuler(Graphics graphics)
		{
			float position;
			float endPosition;
			
			float increment = Minor * _scaleFactor * _unitScaleFactor.X; //Get the scaled ruler increments

			float third = Convert.ToSingle(Height * 0.7); //The size of the marks on the ruler
			float twoThirds = Convert.ToSingle(Height * 0.3);
			float full = Convert.ToSingle(Height * 0.1);
			float height = Height;
			float onePixel = 1;

			Pen pen = new Pen(ForeColor,-1);
			SolidBrush brush = new SolidBrush(ForeColor);
			
			//Set up a gradient brush size that does not contain zeros
			Size size = Size;
			if (size.Width == 0) size.Width = 1;
			if (size.Height == 0) size.Height = 1;

			//Draw background
			LinearGradientBrush gradientBrush = new LinearGradientBrush(new RectangleF(new PointF(0,0),size),BackColor,GradientColor,System.Drawing.Drawing2D.LinearGradientMode.Vertical);
			graphics.FillRectangle(gradientBrush, new RectangleF(new PointF(0,0),size));

			//Calculate end width
			endPosition = Width / _scaleFactor / _unitScaleFactor.X;

			//Translate for scroll, add onto the width if scrolled 
			if (_diagram != null)
			{
				graphics.TranslateTransform(_diagram.AutoScrollPosition.X,0);
				endPosition -= _diagram.AutoScrollPosition.X; //Scroll position is a negative value
			}
			
			//Translate for padding. Padding is always in pixels
			graphics.TranslateTransform(PaddingOffset,0);
			
			//Translate for margin. Margin is always in pixels
			graphics.TranslateTransform(MarginOffset,0);

			//Translate back in perfect multiples of the minor increment.
			graphics.TranslateTransform(-Convert.ToInt32(MarginOffset / increment) * increment,0);

			//Set the initial position value
			int temp = Convert.ToInt32((-MarginOffset / _scaleFactor / _unitScaleFactor.X) / Minor);
			position = temp * Minor;
			
			//Draw a minor, major or mid mark, incrementing by minor
			while (position < endPosition)
			{
				if (position % Major == 0)
				{	
					//Draw full line
					graphics.DrawLine(pen,0,height,0,full);

					//Draw string
					string number = position.ToString();
					if (position == 0) number += "  " + Abbreviate(Units);

					graphics.DrawString(number,_font,brush,0,- onePixel);
				}
				else if (position % Mid == 0)
				{
					graphics.DrawLine(pen,0,height,0,twoThirds);
				}
				else
				{
					graphics.DrawLine(pen,0,height,0,third);
				}

				//Translate by the minor increment and increment position
				graphics.TranslateTransform(increment,0);
				position += Minor;
			}

			graphics.ResetTransform();
			if (_diagram != null) graphics.TranslateTransform(_diagram.AutoScrollPosition.X,0);
			
			//Draw highlights
			if (DrawGuides && _guides != null)
			{
				brush = new SolidBrush(Color.FromArgb(16,Singleton.Instance.HighlightBrush.Color));
				Pen pen2 = new Pen(Color.FromArgb(255,Singleton.Instance.HighlightPen.Color), 1);
							
				foreach (RectangleF rect in Guides)
				{
					RectangleF fill = new RectangleF((rect.X * _scaleFactor) + MarginOffset + PaddingOffset, 0, (rect.Width * _scaleFactor), Height);

					graphics.FillRectangle(brush, fill);
					graphics.DrawLine(pen2, fill.X, 0, fill.X, height);
					graphics.DrawLine(pen2, fill.Right, 0, fill.Right, height);
				}
			}

			graphics.ResetTransform();

			//Clear padding area + 1 (to overwrite 0 line)
			RectangleF paddingRect = new RectangleF(0, 0, PaddingOffset + 1, Height);
			graphics.FillRectangle(gradientBrush, paddingRect);

			//Draw padding seperator
			graphics.DrawLine(pen, PaddingOffset - 1, 0,PaddingOffset - 1, Height);

			//Draw border
			if (_borderStyle != RulerBorderStyle.None)
			{
				//Draw bottom edge
				graphics.DrawLine(pen, 0, Height-1, Width - 1, Height - 1);

				//Draw other sides
				if (_borderStyle == RulerBorderStyle.Full)
				{
					graphics.DrawLine(pen,0,0,0,Height-1);
					graphics.DrawLine(pen,Width-1,0,Width-1,Height-1);
					graphics.DrawLine(pen,0,0,Width-1,0);
				}
			}
		}

		private void RenderRulerVertical(Graphics graphics)
		{
			float position;
			float endPosition;

			float increment = Minor * _scaleFactor * _unitScaleFactor.Y; //Get the scaled ruler increments

			float third = Convert.ToSingle(Width-(Width * 0.2));
			float twoThirds = Convert.ToSingle(Width-(Width * 0.6));
			float full = Convert.ToSingle(Width-(Width * 0.9));
			float width = Width;
			float onePixel = 1;
			
			Pen pen = new Pen(ForeColor,-1);
			SolidBrush brush = new SolidBrush(ForeColor);

			//Set up a gradient brush size that does not contain zeros
			Size size = Size;
			if (size.Width == 0) size.Width = 1;
			if (size.Height == 0) size.Height = 1;

			//Draw background
			LinearGradientBrush gradientBrush = new LinearGradientBrush(new RectangleF(new PointF(0,0),size),BackColor,GradientColor,System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
			graphics.FillRectangle(gradientBrush, new RectangleF(new PointF(0,0),size));

			//Calculate end height
			endPosition = Height / _scaleFactor / _unitScaleFactor.Y;

			//Translate for scroll, add onto the height if scrolled
			if (_diagram != null)
			{
				graphics.TranslateTransform(0,_diagram.AutoScrollPosition.Y);
				endPosition -= _diagram.AutoScrollPosition.Y; //Scroll position is a negative value
			}

			//Translate for padding. Padding is always in pixels
			graphics.TranslateTransform(0, PaddingOffset);
			
			//Translate for margin. Margin is always in pixels
			graphics.TranslateTransform(0, MarginOffset);

			//Translate back in perfect multiples of the minor increment.
			graphics.TranslateTransform(0, -Convert.ToInt32(MarginOffset / increment) * increment);

			//Set the initial position value
			position = (-Convert.ToInt32((MarginOffset / _scaleFactor / _unitScaleFactor.Y) / Minor) * Minor);

			//Draw a minor, major or mid mark, incrementing by minor
			while (position < endPosition)
			{
				//Determine if major, middle or normal (minor mark)
				if (position % Major == 0)
				{	
					//Draw full line
					graphics.DrawLine(pen,full,0,Width,0);

					//Store the current translate
					Matrix transform = graphics.Transform;
					
					//Rotate graphics at position
					Matrix matrix = new Matrix();
					matrix.Translate(10, 0);
					matrix.Translate(transform.OffsetX,transform.OffsetY);
					matrix.RotateAt(90,new PointF(0,0));
					graphics.Transform = matrix;

					string number = position.ToString();
					if (position == 0) number += "  " + Abbreviate(Units);
					
					graphics.DrawString(number,_font,brush,full,-onePixel);

					//Restore the transform
					graphics.Transform = transform;
				}
				else if (position % Mid == 0)
				{
					graphics.DrawLine(pen,twoThirds,0,Width,0);
				}
				else
				{
					graphics.DrawLine(pen,third,0,Width,0);
				}

				//Translate by the minor increment and increment position
				graphics.TranslateTransform(0, increment);
				position += Minor;
			}

			graphics.ResetTransform();
			if (_diagram != null) graphics.TranslateTransform(0, _diagram.AutoScrollPosition.Y);

			//Draw highlights
			if (DrawGuides && _guides != null)
			{
				brush = new SolidBrush(Color.FromArgb(16,Singleton.Instance.HighlightBrush.Color));
				Pen pen2 = new Pen(Color.FromArgb(255,Singleton.Instance.HighlightPen.Color), 1);
							
				foreach (RectangleF rect in Guides)
				{
					RectangleF fill = new RectangleF(0, (rect.Y * _scaleFactor) + MarginOffset + PaddingOffset, Width, (rect.Height * _scaleFactor));

					graphics.FillRectangle(brush, fill);
					graphics.DrawLine(pen2, 0, fill.Y, Width, fill.Y);
					graphics.DrawLine(pen2, 0, fill.Bottom, Width, fill.Bottom);
				}
			}

			graphics.ResetTransform();

			//Clear padding area + 1 (to overwrite 0 line)
			RectangleF paddingRect = new RectangleF(0, 0, Width, PaddingOffset + 1);
			graphics.FillRectangle(gradientBrush, paddingRect);

			//Draw padding seperator
			graphics.DrawLine(pen, 0, PaddingOffset - 1, Width, PaddingOffset - 1);

			//Draw border
			if (_borderStyle != RulerBorderStyle.None)
			{
				//Draw right edge
				graphics.DrawLine(pen,width-1,0,Width-1,Height-1);

				//Draw other sides
				if (_borderStyle == RulerBorderStyle.Full)
				{
					graphics.DrawLine(pen,0,0,Width-1,0);
					graphics.DrawLine(pen,0,0,0,Height-1);
					graphics.DrawLine(pen,0,Height-1,Width-1,Height-1);
				}
			}
		}

		protected virtual void SetDefaultUnitValues()
		{
			switch (_unit)
			{
				case RulerUnit.Pixel:
					_major = 100;
					_minor = 10;
					_mid = 50;
					break;
				case RulerUnit.Point:
					_major = 72;
					_minor = 6;
					_mid = 36;
					break;
				case RulerUnit.Inch:
					_major = 1;
					_minor = 1F;
					_mid = 1F;
					break;
				case RulerUnit.Millimeter:
					_major = 20;
					_minor = 2;
					_mid = 5;
					break;
				case RulerUnit.Document:
					_major = 200;
					_minor = 20;
					_mid = 100;
					break;
				default:
					_major = 100;
					_minor = 10;
					_mid = 50;
					break;
			}
		}

		private void SetUnitScaleFactors()
		{
			Graphics graphics = Singleton.Instance.CreateGraphics();
			graphics.PageUnit = ConvertUnit(Units);
			_unitScaleFactor = Geometry.CalculateUnitFactors(graphics);
			graphics.Dispose();
		}

		private GraphicsUnit ConvertUnit(RulerUnit rulerUnit)
		{
			return (GraphicsUnit) Enum.Parse(typeof(GraphicsUnit),rulerUnit.ToString());
		}

		public static string Abbreviate(RulerUnit unit)
		{
			switch (unit)
			{
				case RulerUnit.Pixel:
					return "px";
				case RulerUnit.Display:
					return "ds";
				case RulerUnit.Document:
					return "dc";
				case RulerUnit.Inch:
					return "in";
				case RulerUnit.Millimeter:
					return "mm";
				case RulerUnit.Point:
					return "pt";
				default:
					return "";
			}
		}
	
		private void CalculateGuides(ElementList actions)
		{
			ArrayList list = new ArrayList();
			
			//Loop through actions and add rectangles
			foreach (Element element in actions)
			{   
				if (element is Shape && element.Visible)
				{
					ArrayList newList = new ArrayList();

					//Get the rectangle from element
					RectangleF newRect = element.Bounds;

					//If a solid element, get transform rectangle
					if (element is Solid)
					{
						Solid solid = (Solid) element;
						newRect = solid.TransformRectangle;
					}

					bool combine = false;
				
					//Loop through the existing rectangles and see if they intersect
					foreach (RectangleF rect in list)
					{
						//Determine if rectangles can be combined
						if (Orientation == RulerOrientation.Top)
						{
							combine = rect.Contains(new PointF(newRect.X, rect.Y )) || rect.Contains(new PointF(newRect.Right, rect.Y ));
						}
						else
						{
							combine = rect.Contains(new PointF(rect.X, newRect.Y)) || rect.Contains(new PointF(rect.X, newRect.Bottom));
						}

						//Add combined rectangle rectangle together, or add to new list if no intersection
						if (combine) 
						{
							newRect = CombineRectangle(newRect, rect); 
						}
						else
						{
							newList.Add(rect);
						}
					}
				
					//Add to new list if not combined
					newList.Add(newRect);
					list = newList;
				}
			}
			
			SetGuides((RectangleF[]) list.ToArray(typeof(RectangleF)));
		}

		private RectangleF CombineRectangle(RectangleF a,RectangleF b)
		{
			RectangleF c = new RectangleF();
			c.X = (a.Left < b.Left) ? a.Left : b.Left;
			c.Y = (a.Top < b.Top) ? a.Top : b.Top;
			c.Width = (a.Right > b.Right) ? a.Right - c.X : b.Right - c.X;
			c.Height = (a.Bottom > b.Bottom) ? a.Bottom - c.Y : b.Bottom - c.Y;
			return c;
		}

		#endregion

	}
}
