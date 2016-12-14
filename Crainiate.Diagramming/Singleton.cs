// (c) Copyright Crainiate Software 2010

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;

using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
    //Singelton follows the singleton design pattern
	sealed public class Singleton	
	{
		//Component instance
		public static readonly Singleton Instance = new Singleton();
		
		//Property variables
		private StringFormat _stringFormat;
		private StringFormatFlags _stringFormatFlags;
		private Font _font;
		private Pen _selectionPen;
		private Pen _selectionStartPen;
		private Pen _selectionEndPen;
		private Pen _selectionRotatePen;
		private Pen _expandPen;
		private Pen _defaultPen;
		private SolidBrush _selectionBrush;
        private SolidBrush _groupBrush;
		private SolidBrush _selectionStartBrush;
		private SolidBrush _selectionEndBrush;
		private SolidBrush _selectionFillBrush;
		private SolidBrush _selectionRotateBrush;
		private SolidBrush _expandBrush;
		private Pen _selectionHatchPen;
		private Pen _actionPen;
		private SolidBrush _actionBrush;
		private Pen _highlightPen;
		private SolidBrush _highlightBrush;
		private Pen _vectorPen;
		private Pen _pageOutlinePen;
		private byte _defaultOpacity = 100;
		private float _defaultBorderWidth = 2;
		private int _dragScrollInterval = 500;
		private int _dragScrollAmount = 100;
		private PointF _defaultShadowOffset = new PointF(5, 5);
		private Color _defaultShadowColor = Color.FromArgb(48, 0, 0, 0);
		private GraphicsPath _defaultHandlePath;
		private GraphicsPath _defaultLargeHandlePath;
		private StencilItem _defaultStencilItem;
		private bool _hideActions = false;
		private ActionStyle _defaultActionStyle = ActionStyle.Default;
		private float _roundingRadius = 6;
		private KeyCreateMode _keyCreateMode = KeyCreateMode.Normal;
		private bool _clipExport = true;

		private Hashtable _fonts; 
		private Hashtable _bitmaps;
		private Hashtable _resources;
		private Hashtable _stencils;
		private Hashtable _cursors;
		private Hashtable _resourceCursors;
        private Hashtable _themes;

        private XmlFormatter _formatter;

		private Handle _defaultHandle;

		#region Interface

		//Create a private constructor
		private Singleton() 
        {
            DefaultSize = new SizeF(100, 60);
            DefaultMinimumSize = new SizeF(32,32);
            DefaultMaximumSize = new SizeF(320, 320);
        }

		public bool HideActions
		{
			get
			{
				return _hideActions;
			}
			set
			{
				_hideActions = value;
			}
		}

		public ActionStyle DefaultActionStyle
		{
			get
			{
				return _defaultActionStyle;
			}
			set
			{
				_defaultActionStyle = value;
			}
		}

		//Gets or sets the default string format
		//Can be set to eg automatically provide right to left
		public StringFormat DefaultStringFormat
		{
			get
			{
				if (_stringFormat == null) CreateDefaultStringFormat();
				return _stringFormat;
			}
			set
			{
				_stringFormat = value;
			}
		}

		public StringFormatFlags DefaultStringFormatFlags
		{
			get
			{
				return _stringFormatFlags;
			}
			set
			{
				_stringFormatFlags = value;
			}
		}

		public Font DefaultFont
		{
			get
			{
				if (_font == null) CreateDefaultFont();
				return _font;
			}
			set
			{
				_font = value;
			}
		}

		public Handle DefaultHandle
		{
			get
			{
				if (_defaultHandle == null) CreateDefaultHandle();
				return _defaultHandle;
			}
		}

		public StencilItem DefaultStencilItem
		{
			get
			{
				if (_defaultStencilItem  == null) CreateDefaultStencilItem();
				return _defaultStencilItem;
			}
			set
			{
				_defaultStencilItem = value;
			}
		}

		public byte DefaultOpacity
		{
			get
			{
				return _defaultOpacity;
			}
			set
			{
				_defaultOpacity = value;
			}
		}

		public float DefaultBorderWidth
		{
			get
			{
				return _defaultBorderWidth;
			}
			set
			{
				_defaultBorderWidth = value;
			}
		}

		public int DragScrollInterval
		{
			get
			{
				return _dragScrollInterval;
			}
			set
			{
				_dragScrollInterval = value;
			}
		}

		public int DragScrollAmount
		{
			get
			{
				return _dragScrollAmount;
			}
			set
			{
				_dragScrollAmount = value;
			}
		}

		public float RoundingRadius
		{
			get
			{
				return _roundingRadius;
			}
			set
			{
				_roundingRadius = value;
			}
		}

		public Pen DefaultPen
		{
			get
			{
				if (_defaultPen == null) CreateDefaultPen();
				return _defaultPen;
			}
			set
			{
				_defaultPen = value;
			}
		}

		public Pen SelectionPen
		{
			get
			{
				if (_selectionPen == null) CreateSelectionPen();
				return _selectionPen;
			}
			set
			{
				_selectionPen = value;
			}
		}

		public Pen SelectionStartPen
		{
			get
			{
				if (_selectionStartPen == null) CreateSelectionStartPen();
				return _selectionStartPen;
			}
			set
			{
				_selectionStartPen = value;
			}
		}

		public Pen SelectionEndPen
		{
			get
			{
				if (_selectionEndPen == null) CreateSelectionEndPen();
				return _selectionEndPen;
			}
			set
			{
				_selectionEndPen = value;
			}
		}

		public Pen SelectionRotatePen
		{
			get
			{
				if (_selectionRotatePen == null) CreateSelectionRotatePen();
				return _selectionRotatePen;
			}
			set
			{
				_selectionRotatePen = value;
			}
		}

		public Pen ExpandPen
		{
			get
			{
				if (_expandPen == null) CreateExpandPen();
				return _expandPen;
			}
			set
			{
				_expandPen = value;
			}
		}

		public Color DefaultShadowColor
		{
			get
			{
				return _defaultShadowColor;
			}
			set
			{
				_defaultShadowColor = value;
			}
		}

		public PointF DefaultShadowOffset
		{
			get
			{
				return _defaultShadowOffset;
			}
			set
			{
				_defaultShadowOffset = value;
			}
		}

        public SizeF DefaultMinimumSize {get; set;}
        public SizeF DefaultMaximumSize { get; set; }
		public SizeF DefaultSize {get; set;}

		public GraphicsPath DefaultHandlePath
		{
			get
			{
				if (_defaultHandlePath == null) CreateDefaultHandlePath();
				return _defaultHandlePath;
			}
			set
			{
				_defaultHandlePath = value;
			}
		}

		public GraphicsPath DefaultLargeHandlePath
		{
			get
			{
				if (_defaultLargeHandlePath == null) CreateDefaultLargeHandlePath();
				return _defaultLargeHandlePath;
			}
			set
			{
				_defaultLargeHandlePath = value;
			}
		}

		public SolidBrush SelectionBrush
		{
			get
			{
				if (_selectionBrush == null) CreateSelectionBrush();
				return _selectionBrush;
			}
			set
			{
				_selectionBrush = value;
			}
		}

        public SolidBrush GroupBrush
        {
            get
            {
                if (_groupBrush == null) CreateGroupBrush();
                return _groupBrush;
            }
            set
            {
                _groupBrush = value;
            }
        }
			
		public SolidBrush SelectionStartBrush
		{
			get
			{
				if (_selectionStartBrush == null) CreateSelectionStartBrush();
				return _selectionStartBrush;
			}
			set
			{
				_selectionStartBrush = value;
			}
		}

		public SolidBrush SelectionEndBrush
		{
			get
			{
				if (_selectionEndBrush == null) CreateSelectionEndBrush();
				return _selectionEndBrush;
			}
			set
			{
				_selectionEndBrush = value;
			}
		}

		public SolidBrush SelectionRotateBrush
		{
			get
			{
				if (_selectionRotateBrush == null) CreateSelectionRotateBrush();
				return _selectionRotateBrush;
			}
			set
			{
				_selectionRotateBrush = value;
			}
		}

		public SolidBrush ExpandBrush
		{
			get
			{
				if (_expandBrush == null) CreateExpandBrush();
				return _expandBrush;
			}
			set
			{
				_expandBrush = value;
			}
		}

		public Pen ActionPen
		{
			get
			{
				if (_actionPen == null) CreateActionPen();
				return _actionPen;
			}
			set
			{
				_actionPen = value;
			}
		}

		public SolidBrush ActionBrush
		{
			get
			{
				if (_actionBrush == null) CreateActionBrush();
				return _actionBrush;
			}
			set
			{
				_actionBrush = value;
			}
		}

		public Pen HighlightPen
		{
			get
			{
				if (_highlightPen == null) CreateHighlightPen();
				return _highlightPen;
			}
			set
			{
				_highlightPen = value;
			}
		}

		public SolidBrush HighlightBrush
		{
			get
			{
				if (_highlightBrush == null) CreateHighlightBrush();
				return _highlightBrush;
			}
			set
			{
				_highlightBrush = value;
			}
		}

		public Pen VectorPen
		{
			get
			{
				if (_vectorPen == null) CreateVectorPen();
				return _vectorPen;
			}
			set
			{
				_vectorPen = value;
			}
		}

		public Pen PageOutlinePen
		{
			get
			{
				if (_pageOutlinePen == null) CreatePageOutlinePen();
				return _pageOutlinePen;
			}
			set
			{
				_pageOutlinePen = value;
			}
		}

		public SolidBrush SelectionFillBrush
		{
			get
			{
				if (_selectionFillBrush == null) CreateSelectionFillBrush();
				return _selectionFillBrush;
			}
			set
			{
				_selectionFillBrush = value;
			}
		}

		public Pen SelectionHatchPen
		{
			get
			{
				if (_selectionHatchPen == null) CreateSelectionHatchPen();
				return _selectionHatchPen;
			}
			set
			{
				_selectionHatchPen = value;
			}
		}

		public Hashtable Fonts
		{
			get
			{
				return _fonts;
			}
		}

        public Hashtable Themes
        {
            get
            {
                return _themes;
            }
        }

		public Hashtable Bitmaps
		{
			get
			{
				return _bitmaps;
			}
		}

		public Hashtable Resources
		{
			get
			{
				return _resources;
			}
		}

		public Hashtable Stencils
		{
			get
			{
				return _stencils;
			}
		}

		public Hashtable Cursors
		{
			get
			{
				return _cursors;
			}
			set
			{
				_cursors = value;
			}
		}

        public XmlFormatter XmlFormatter
        {
            get
            {
                if (_formatter == null) _formatter = new XmlFormatter();
                return _formatter;
            }
            set
            {
                _formatter = value;
            }
        }

		public KeyCreateMode KeyCreateMode
		{
			get
			{
				return _keyCreateMode;
			}
			set
			{
				_keyCreateMode = value;
			}
		}

		public bool ClipExport
		{
			get
			{
				return _clipExport;
			}
			set
			{
				_clipExport = value;
			}
		}

		//Methods
        public static void Preload(string assemblyName)
        {
            Assembly.LoadWithPartialName(assemblyName);
        }

		public Cursor GetCursor(HandleType type)
		{
			if (_cursors == null) CreateCursors();
			return _cursors[type] as Cursor;
		}

		public Cursor LoadCursor(string resource)
		{
			if (_resourceCursors == null) _resourceCursors = new Hashtable();
			
			if (_resourceCursors.ContainsKey(resource))
			{
				return (Cursor) _resourceCursors[resource];
			}
			else
			{
				Cursor cursor = new Cursor(typeof(Crainiate.Diagramming.Singleton), resource);
				_resourceCursors.Add(resource, cursor);
				
				return cursor;
			}
		}

		public Graphics CreateGraphics()
		{
			Bitmap temp = new Bitmap(1,1,System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
			return Graphics.FromImage(temp);
		}

		public Stencil GetStencil(Type stencilType)
		{
			if (_stencils == null) _stencils = new Hashtable();

			if (_stencils.ContainsKey(stencilType))
			{
				return (Stencil) _stencils[stencilType];
			}
			else
			{
				object stencil = Activator.CreateInstance(stencilType);
				_stencils.Add(stencilType,stencil);
				return (Stencil) stencil;
			}
		}
		
		public Font GetFont(string name,float size,FontStyle fontStyle)
		{
			Font font;
			string key = name + size.ToString() + Convert.ToInt32(fontStyle).ToString();
			
			if (_fonts == null) _fonts = new Hashtable();
			
			if (_fonts.ContainsKey(key))
			{
				return (Font) _fonts[key];
			}
			else
			{
				font = new Font(name,size,fontStyle);
				_fonts.Add(key,font);
				return font;
			}
		}

        public Theme GetTheme(Themes theme)
        {
            if (_themes == null) _themes = new Hashtable();

            if (_themes.ContainsKey(theme))
            {
                return (Theme) _themes[theme];
            }
            else
            {
                Theme add = CreateTheme(theme);
                _themes.Add(theme, add);

                return add;
            }
        }

		public FontStyle GetFontStyle(bool bold, bool italic, bool underline, bool strikeout)
		{
			FontStyle style = FontStyle.Regular;

			if (bold) style  = style | FontStyle.Bold;
			if (italic) style = style | FontStyle.Italic;
			if (underline) style = style | FontStyle.Underline;
			if (strikeout) style = style| FontStyle.Strikeout;
			
			return style;
		}

		//Returns a bitmap from the internal resource file
		public Bitmap GetResource(string name,string assembly)
		{
			Bitmap bitmap;
			string key = name+assembly;

			if (_resources == null) _resources = new Hashtable();				

			if (_resources.ContainsKey(key))
			{
				return (Bitmap) _resources[key];
			}
			else
			{
				//Load from assembly
				if (assembly == "")
				{
					bitmap = new Bitmap(typeof(Singleton),name);
				}
				else
				{
                    Type type = GetAnyAssemblyType(assembly);

                    if (type == null) throw new ApplicationException("Assembly type " + assembly + " does not contain resource " + name + ". Make sure the resource is an embedded resource and the reference is included.");

					bitmap = new Bitmap(type, name);
				}
				_resources.Add(key,bitmap);
				return bitmap;
			}
		}

		//Returns a resource from disk
		public Bitmap GetBitmap(string path)
		{
			Bitmap bitmap;

			if (_bitmaps == null) _bitmaps = new Hashtable();

			if (_bitmaps.ContainsKey(path)) 
			{
				return (Bitmap) _bitmaps[path];
			}
			else
			{
				//Check if must load image from uri 
				if (path.Substring(0,7) == "http://" || path.Substring(0,8) == "https://")
				{
					try
					{
						System.IO.Stream imageStream = new System.Net.WebClient().OpenRead(path);
						bitmap = (Bitmap) System.Drawing.Image.FromStream(imageStream);
					}
					catch
					{
						throw new ComponentException("An image could not be loaded from the URI provided: " + path);
					}
				}
				else
				{
					try
					{
						bitmap = new Bitmap(path);
					}
					catch
					{
						throw new ComponentException("An image could not be loaded from the path specified: " + path);
					}
				}
				_bitmaps.Add(path,bitmap);
				return bitmap;
			}
		}	

		#endregion

		#region Implementation

		private void CreateDefaultStencilItem()
		{
			_defaultStencilItem = new StencilItem(); 
			_defaultStencilItem.Redraw = true;
		}

		private void CreateDefaultStringFormat()
		{
			_stringFormat = new StringFormat();
			_stringFormat.Alignment = StringAlignment.Center;
			_stringFormat.LineAlignment = StringAlignment.Center;
		}

		private void CreateDefaultHandlePath()
		{
			_defaultHandlePath = new GraphicsPath();
            _defaultHandlePath.AddLine(1, 0, 5, 0);
            _defaultHandlePath.AddLine(6, 1, 6, 5);
            _defaultHandlePath.AddLine(5, 6, 1, 6);
            _defaultHandlePath.AddLine(0, 5, 0, 1);

		}

		private void CreateDefaultLargeHandlePath()
		{
			_defaultLargeHandlePath = new GraphicsPath();
			
			_defaultLargeHandlePath.AddArc(0, 3, 3, 6, 90, 180);
			_defaultLargeHandlePath.AddArc(10, 3, 3, 6, 270, 180);
			_defaultLargeHandlePath.CloseFigure();

			Geometry.MovePathToOrigin(_defaultLargeHandlePath);
		}

		private void CreateDefaultFont()
		{
			_font = new Font("Microsoft Tahoma",8.25F);
		}

		private void CreateDefaultHandle()
		{
			_defaultHandle = new Handle();
			_defaultHandle.Type = HandleType.Move;
		}

		private void CreateSelectionPen()
		{
            _selectionPen = new Pen(Color.FromArgb(227, SystemColors.Highlight), -1);
		}

		private void CreateDefaultPen()
		{
			_defaultPen = new Pen(Color.Black);
			_defaultPen.Width = 2;
		}

		private void CreateSelectionBrush()
		{
            _selectionBrush = new SolidBrush(Color.White);
		}

        private void CreateGroupBrush()
		{
            _groupBrush = new SolidBrush(Color.FromArgb(16, SelectionPen.Color));
		}

		private void CreateSelectionStartPen()
		{
			_selectionStartPen = new Pen(Color.FromArgb(192,Color.Green),-1);
		}

		private void CreateSelectionStartBrush()
		{
			_selectionStartBrush = new SolidBrush(Color.FromArgb(32,Color.Green));
		}

		private void CreateSelectionEndPen()
		{
			_selectionEndPen = new Pen(Color.FromArgb(192,Color.Red),-1);
		}

		private void CreateSelectionRotatePen()
		{
			_selectionRotatePen = new Pen(Color.FromArgb(227,SystemColors.Highlight),-1);
		}

		private void CreateExpandPen()
		{
			_expandPen = new Pen(Color.FromArgb(160,Color.Black),-1);
		}

		private void CreateSelectionEndBrush()
		{
			_selectionEndBrush = new SolidBrush(Color.FromArgb(32,Color.Red));
		}

		private void CreateSelectionFillBrush()
		{
			_selectionFillBrush = new SolidBrush(Color.FromArgb(4,SystemColors.Highlight));
		}

		private void CreateSelectionRotateBrush()
		{
			_selectionRotateBrush = new SolidBrush(Color.FromArgb(24,SystemColors.Highlight));
		}

		private void CreateExpandBrush()
		{
			_expandBrush = new SolidBrush(Color.White);
		}

		private void CreateSelectionHatchPen()
		{
            _selectionHatchPen = new Pen(Color.FromArgb(144, SystemColors.Highlight), 1);
            _selectionHatchPen.DashStyle = DashStyle.Dot;
		}

		private void CreateActionPen()
		{
			_actionPen = new Pen(Color.FromArgb(192, SystemColors.Highlight),1);
		}

		private void CreateActionBrush()
		{
            _actionBrush = new SolidBrush(Color.FromArgb(32, SystemColors.Highlight));
		}

		private void CreateHighlightPen()
		{
            _highlightPen = new Pen(Color.FromArgb(64, SystemColors.Highlight));
			_highlightPen.DashStyle = DashStyle.Solid;
            _highlightPen.Width = 7;
			_highlightPen.Alignment = PenAlignment.Center;
		}

		private void CreateVectorPen()
		{
            _vectorPen = new Pen(Color.FromArgb(192, SystemColors.Highlight), 1);
            _vectorPen.DashStyle = DashStyle.Dot;
		}

		private void CreateHighlightBrush()
		{
            _highlightBrush = new SolidBrush(Color.FromArgb(32, SystemColors.Highlight));
		}

		private void CreatePageOutlinePen()
		{
			_pageOutlinePen = new Pen(Color.FromArgb(128,Color.Teal),1);
			_pageOutlinePen.DashStyle = DashStyle.Dot;
		}

		private void CreateCursors()
		{
			_cursors = new Hashtable();

			_cursors.Add(HandleType.Move, System.Windows.Forms.Cursors.SizeAll);
			_cursors.Add(HandleType.Bottom, System.Windows.Forms.Cursors.SizeNS);
			_cursors.Add(HandleType.BottomLeft, System.Windows.Forms.Cursors.SizeNESW);
			_cursors.Add(HandleType.BottomRight, System.Windows.Forms.Cursors.SizeNWSE);
			_cursors.Add(HandleType.Left, System.Windows.Forms.Cursors.SizeWE);
			_cursors.Add(HandleType.Right, System.Windows.Forms.Cursors.SizeWE);
			_cursors.Add(HandleType.Top, System.Windows.Forms.Cursors.SizeNS);
			_cursors.Add(HandleType.TopLeft, System.Windows.Forms.Cursors.SizeNWSE);
			_cursors.Add(HandleType.TopRight, System.Windows.Forms.Cursors.SizeNESW);
			_cursors.Add(HandleType.Origin, System.Windows.Forms.Cursors.PanNorth);
			_cursors.Add(HandleType.LeftRight, System.Windows.Forms.Cursors.SizeWE);
			_cursors.Add(HandleType.UpDown, System.Windows.Forms.Cursors.SizeNS);
			_cursors.Add(HandleType.Rotate, LoadCursor("Resource.rotate.cur"));
			_cursors.Add(HandleType.Expand, System.Windows.Forms.Cursors.PanEast);
			_cursors.Add(HandleType.None, System.Windows.Forms.Cursors.Arrow);
		}

        private Type GetAnyAssemblyType(string typeName)
        {
            Assembly[] assemblyArray = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblyArray)
            {
                if (!assembly.FullName.StartsWith("mscorlib") && !assembly.FullName.StartsWith("System"))
                {
                    Type type = assembly.GetType(typeName);
                    if (type != null) return type;
                }
            }

            return null;
        }

        private Theme CreateTheme(Themes themeType)
        {
            Theme theme = new Theme();

            if (themeType == Crainiate.Diagramming.Themes.LightBlue)
            {
                theme.BorderWidth = 2;
                theme.GradientColor = Color.FromArgb(192, 145, 186, 222);
                theme.BorderColor = Color.FromArgb(145, 186, 222);
            }
            else if (themeType == Crainiate.Diagramming.Themes.Orange)
            {
                theme.BorderWidth = 2;
                theme.GradientColor = Color.FromArgb(192, 255, 178, 68);
                theme.BorderColor = Color.FromArgb(255, 232, 104, 50);
            } 
            else if (themeType == Crainiate.Diagramming.Themes.Green)
            {
                theme.BorderWidth = 2;
                theme.GradientColor = Color.FromArgb(192, 159, 255, 105);
                theme.BorderColor = Color.FromArgb(255, 100, 178, 56);
            }

            return theme;
        }

		#endregion
	}
}

