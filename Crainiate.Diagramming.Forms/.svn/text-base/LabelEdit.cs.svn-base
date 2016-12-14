// (c) Copyright Crainiate Software 2010




using System;
using System.ComponentModel;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Crainiate.Diagramming.Forms
{
	[ToolboxItem(false)]
	public class LabelEdit: System.Windows.Forms.Control, ILabelEdit
	{
		//Property variables
		private StringFormat _stringFormat = StringFormat.GenericDefault;
		private bool _control;
		private int _blinkRate = 500;
		private float _zoom = 100;
		private bool _cancelled;
		private AutoSizeMode _autoSize;
		private bool _allowEnter;

		//Working variables
		private System.ComponentModel.Container components = null;
		private RectangleF[] _characterRectangles;
		private Bitmap _renderBitmap = null;
		private Timer _timer;
		private bool _caretOn;
		private int _caret;
		private int _select;

		private Stack _undo = new Stack();
		private Stack _redo = new Stack();

		private bool _completed;

		//Constants
		private const int WM_KEYDOWN = 0x100;
		private const int WM_SYSKEYDOWN = 0x104;

		#region Interface

		[Category("Action"),Description("Occurs when the user wants to complete editing.")]
		public event EventHandler Complete;
		[Category("Action"),Description("Occurs when the user wants to cancel editing.")]
		public event EventHandler Cancel;

		//Constructors
		public LabelEdit():base()
		{
			InitializeComponent();
			_control = true;
			_stringFormat = new StringFormat();
			//SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			//this.BackColor = Color.Transparent;
			
			this.Cursor = Cursors.IBeam;
		}

		public LabelEdit(bool RenderOnly)
		{
			InitializeComponent();
			_control = !RenderOnly;
			_stringFormat = new StringFormat();
		}

		//Properties
		[Category("Data"),Description("Sets or gets the text displayed by this control.")]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				_undo.Push(_caret); //Put the caret position on the undo stack
				_undo.Push(base.Text); //Put the text on the undo stack
				base.Text = value;

				if (AutoSizeMode != AutoSizeMode.None) CheckSize();

				Refresh();
			}
		}
		
		[Category("Behaviour"),Description("Sets or gets the stringformat object used to draw the text.")]
		public virtual StringFormat StringFormat
		{
			get
			{
				return _stringFormat;
			}
			set
			{
				_stringFormat = value;
				Refresh();
			}
		}

		[Category("Behaviour"),Description("Determines how the control grows with the size of the text.")]
		public virtual AutoSizeMode AutoSizeMode
		{
			get
			{
				return _autoSize;
			}
			set
			{
				if (value != _autoSize)
				{
					_autoSize = value;
					if (_autoSize != AutoSizeMode.None) CheckSize();
				}
			}
		}

		[Category("Behaviour"),Description("Determines if the label editor inserts carriage returns into the text.")]
		public virtual bool AllowEnter
		{
			get
			{
				return _allowEnter;
			}
			set
			{
				_allowEnter = value;
			}
		}

		[Category("Data"),Description("Returns the rendered bitmap.")]
		public virtual Bitmap Bitmap
		{
			get
			{
				return _renderBitmap;
			}
		}

		[Category("Behaviour"),Description("Sets or gets the zoom percentage for the control.")]
		public virtual float Zoom
		{
			get
			{
				return _zoom;
			}
			set
			{
				if (_zoom != value)
				{
					_zoom = value;
					Refresh();
				}
			}
		}

		[Category("Data"),Description("Sets or gets the start of the text selection.")]
		public virtual int SelectionStart
		{
			get
			{
				return _select;
			}
			set
			{
				if (value >= 0 && value <= Text.Length)
				{
					_select = value;
					_caret = value;

					Refresh();
				}
			}
		}

		[Category("Data"),Description("Sets or gets the position of the end of the text selection.")]
		public virtual int SelectionEnd
		{
			get
			{
				return _caret;
			}
			set
			{
				if (value >= 0 && value <= Text.Length)
				{
					_caret = value;
					Refresh();
				}
			}
		}

		[Category("Data"),Description("Returns a value indicating whether editing has been cancelled.")]
		public virtual bool Cancelled
		{
			get
			{
				return _cancelled;
			}
		}

		//Methods
		public virtual void Undo()
		{
			if (_undo.Count > 0)
			{
				_redo.Push(_caret); //Add the caret postion to the redo
				_redo.Push(Text); //Add the text to the redo
				base.Text = (string) _undo.Pop();
				_caret = (int) _undo.Pop();
				_select = _caret;
				Refresh();
			}
		}

		public virtual void Redo()
		{
			if (_redo.Count > 0)
			{
				Text = (string) _redo.Pop();
				_caret = (int) _redo.Pop();
				_select = _caret;
			}
		}

		[Description("Renders the control onto the supplied graphics handle")]
		public virtual void Render(Graphics graphics)
		{
			RenderLabel(graphics);
		}

		[Description("Returns an array of rectangles containing the measurements of the control's text")]
		public virtual RectangleF[] MeasureCharacters()
		{
			MeasureCharactersImplementation(Graphics.FromImage(_renderBitmap));
			return _characterRectangles;
		}

		[Description("Returns an array of rectangles containing the measurements of the control's text")]
		public virtual RectangleF[] MeasureCharacters(Graphics graphics)
		{
			MeasureCharactersImplementation(graphics);
			return _characterRectangles;
		}

		[Description("Returns a rectangle containing the measurement of the control's text")]
		public virtual SizeF MeasureText()
		{
			return MeasureTextImplementation(Graphics.FromImage(_renderBitmap));
		}

		[Description("Returns a rectangle containing the measurement of the control's text")]
		public virtual SizeF MeasureText(Graphics graphics)
		{
			return MeasureTextImplementation(graphics);
		}

		[Description("Sends a character to the current caret position in the annotation text.")]
		public virtual void SendCharacter(char Value)
		{
			InsertCharacter(Value);
		}

		[Description("Sends a delete character to the current caret position in the annotation text.")]
		public virtual void SendDelete()
		{
			DeleteCharacter();
		}

		[Description("Sends a backspace character to the current caret position in the annotation text.")]
		public virtual void SendBackSpace()
		{
			BackspaceCharacter();
		}

		[Description("Sends an arrow character to the annotation.")]
		public virtual void SendArrow(Keys Key)
		{
			ArrowCharacter(Key);
		}

		[Description("Sends an end keystroke to the annotation.")]
		public virtual void SendEnd()
		{
			EndCharacter();
		}

		[Description("Sends a home keystroke to the annotation.")]
		public virtual void SendHome()
		{
			HomeCharacter();
		}

		//Raises the Complete event
		protected virtual void OnComplete()
		{
			if (!_completed)
			{
				_completed = true;
				if (Complete != null) Complete(this,EventArgs.Empty);
			}
		}

		//Raises the Cancel event
		protected virtual void OnCancel()
		{
			if (Cancel != null) Cancel(this,EventArgs.Empty);
		}

		#endregion

		#region Component Designer generated code

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion

		#region Overrides

		public override void Refresh()
		{
			if (_control) 
			{
				UpdateBuffer();
				base.Refresh();
			}
		}

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

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (Focused) 
			{
				if (_timer != null) _timer.Stop();
				_caretOn = true;
				BlinkCaret();

				_select = CaretFromLocation(e.X,e.Y);
				_caret = _select;
				Refresh();
			}
			base.OnMouseDown (e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (Focused && e.Button == MouseButtons.Left) 
			{
				_caret = CaretFromLocation(e.X,e.Y);
				Refresh();
			}
			base.OnMouseMove (e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (Focused) 
			{
				BlinkCaret();
				if (_timer != null) _timer.Start();
			}
			base.OnMouseUp(e);
		}

		//Turn on caret
		protected override void OnGotFocus(EventArgs e)
		{
			_caretOn = false;

			_timer = new Timer();
			_timer.Interval = _blinkRate;
			_timer.Tick +=new EventHandler(Timer_Tick);

			SendEnd();
			_select = _caret;
			UpdateCaret();

			BlinkCaret();
			_timer.Start();
			
			base.OnGotFocus(e);
		}

		//Turn off caret
		protected override void OnLostFocus(EventArgs e)
		{
			if (_timer != null) _timer.Stop();
			_caretOn = true;
			BlinkCaret();
			
			OnComplete();

			_timer = null;

			base.OnLostFocus(e);
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			pe.Graphics.DrawImageUnscaled(_renderBitmap,new Point(0,0));	
			base.OnPaint(pe);
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			//base.OnPaintBackground (pevent);
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
			base.OnLayout (levent);

			if (DisplayRectangle.Width == 0 || DisplayRectangle.Height == 0) return;
			_renderBitmap = new Bitmap(DisplayRectangle.Width,DisplayRectangle.Height,PixelFormat.Format32bppPArgb);
			RenderLabel(Graphics.FromImage(_renderBitmap));
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			//Stop displaying caret
			if (_timer != null) _timer.Stop();
			_caretOn = false;
			BlinkCaret();
			
			SendCharacter(e.KeyChar);
			UpdateCaret(); //Will restart timer

			e.Handled = true;

			base.OnKeyPress(e);
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
			{
				//Shift left
				if (keyData == (Keys.Shift | Keys.Left))
				{
					SendArrow(Keys.Left);
					Refresh();
					UpdateCaret();
					return true;
				}
				
				//Shift right
				if (keyData == (Keys.Shift | Keys.Right))
				{
					SendArrow(Keys.Right);
					Refresh();
					UpdateCaret();
					return true;
				}

				//Shift Home
				if (keyData == (Keys.Shift | Keys.Home))
				{
					SendHome();
					Refresh();
					UpdateCaret();
					return true;
				}

				//Shift End
				if (keyData == (Keys.Shift | Keys.End))
				{
					SendEnd();
					Refresh();
					UpdateCaret();
					return true;
				}

				//Undo
				if (keyData == (Keys.Control | Keys.Z))
				{
					Undo();
					return true;
				}

				//Redo
				if (keyData == (Keys.Control | Keys.Y))
				{
					Redo();
					return true;
				}
				
				//Enter
				if (keyData == Keys.Enter)
				{
					//Stop displaying caret
					if (_timer != null) _timer.Stop();
					_caretOn = false;
					BlinkCaret();

					if (AllowEnter)
					{
						SendCharacter(Convert.ToChar("\n"));
						_select = _caret;
						Refresh();
						UpdateCaret();
					}
					else
					{
						OnComplete();
					}

					return true;
				}

				//Left
				if (keyData == Keys.Left)
				{
					SendArrow(Keys.Left);
					_select = _caret;
					Refresh();
					UpdateCaret();
					return true;
				}

				//Right
				if (keyData == Keys.Right)
				{
					SendArrow(Keys.Right);
					_select = _caret;
					Refresh();
					UpdateCaret();
					return true;
				}

				//Top
				if (keyData == Keys.Up)
				{
					SendArrow(Keys.Up);
					_select = _caret;
					Refresh();
					UpdateCaret();
					return true;
				}

				//Bottom
				if (keyData == Keys.Down)
				{
					SendArrow(Keys.Down);
					_select = _caret;
					Refresh();
					UpdateCaret();
					return true;
				}

				//Backspace
				if (keyData == Keys.Back)
				{
					SendBackSpace();
					UpdateCaret();
					return true;
				}

				//Delete
				if (keyData == Keys.Delete)
				{
					SendDelete();
					UpdateCaret();
					return true;
				}

				//Home
				if (keyData == Keys.Home)
				{
					SendHome();
					_select = _caret;
					Refresh();
					UpdateCaret();
					return true;
				}

				//End
				if (keyData == Keys.End)
				{
					SendEnd();
					_select = _caret;
					Refresh();
					UpdateCaret();
					return true;
				}

				//Escape
				if (keyData == Keys.Escape)
				{
					_cancelled = true;
					OnCancel();
					return true;
				}
			}
			return base.ProcessCmdKey (ref msg, keyData);
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

		#endregion

		#region Events

		private void Timer_Tick(object sender, EventArgs e)
		{
			BlinkCaret();
		}

		#endregion

		#region Implementation
		
		private void UpdateBuffer()
		{
			if (_renderBitmap == null) return;
			Graphics graphics = Graphics.FromImage(_renderBitmap);
			graphics.Clear(BackColor);
			RenderLabel(graphics);
			graphics.Dispose();
		}
		
		private void RenderLabel(Graphics graphics)
		{
			float scale = _zoom / 100;
			RectangleF scaleRect = DisplayRectangle;

			//Apply local transformation
			if (_zoom != 100)
			{
				scaleRect = new RectangleF(0,0,DisplayRectangle.Width / scale,DisplayRectangle.Height / scale);
				graphics.ScaleTransform(scale,scale);
			}

			string text = Text;
			graphics.DrawString(text,Font,new SolidBrush(ForeColor),scaleRect,_stringFormat);
						
			MeasureCharactersImplementation(graphics);

			//Draw selection if applicable
			//Reset the scale because thats how the characters have been measured
			graphics.ScaleTransform(1/scale,1/scale);
			DrawSelection(graphics);
		}

		//make sure caret is re-rendered
		private void UpdateCaret()
		{
			if (this.Focused)
			{
				if (_timer != null) _timer.Stop();
				_caretOn = true;
				BlinkCaret();
				BlinkCaret();
				if (_timer != null) _timer.Start();
			}
		}
		
		private void BlinkCaret()
		{
			Graphics graphics = CreateGraphics();
			
			try
			{
				//Invert whatever the last action was
				if (_caretOn)
				{
					graphics.DrawImageUnscaled(_renderBitmap, new Point(0,0));
				}
				else
				{
					float scale = _zoom / 100;
					float caretWidth = 1 / scale;
					graphics.ScaleTransform(scale,scale);
			
					//Draw caret as black rectangle
					SolidBrush brush = new SolidBrush(Color.Black);
					RectangleF rect = _characterRectangles[_caret];
					graphics.FillRectangle(brush,rect.X+rect.Width,rect.Y,caretWidth,rect.Height);
				}
				_caretOn = ! _caretOn;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Error blinking caret " + ex.ToString());	
			}
			graphics.Dispose();
		}

		private void DrawSelection(Graphics graphics)
		{
			if (_select != _caret)
			{
				SolidBrush brush = new SolidBrush(Color.FromArgb(64,SystemColors.Highlight));

				int start;
				int end;

				//Calculate start and end
				if (_select < _caret)
				{
					start = _select+1;
					end = _caret;
				}
				else
				{
					start = _caret+1;
					end = _select;
				}

				//Render each rectangle
				for (int i = start; i<= end;i++)
				{
					graphics.FillRectangle(brush,_characterRectangles[i]);
				}
			}
		}
		
		private int CaretFromLocation(int x, int y)
		{
			if (Text == null || Text == "") return 0;

			//Scale the points
			float scale = _zoom / 100;
			float cx = x / scale;
			float cy = y / scale;
			int caret;
						
			try
			{
				//Reset the caret
				caret = -1;

				//Check if the point is anywhere in one of the character rectangles
				if (_characterRectangles != null)
				{
					for (int i = 0; i <= _characterRectangles.GetUpperBound(0); i++)
					{
						if (_characterRectangles[i].Contains(cx, cy)) caret = i;
					}
				}

				//If we havent hit a rectangle, then caret must be at the beginning or the end
				if (caret == -1)
				{
					if (cy <= _characterRectangles[0].Y)
					{
						caret = 0;
					}
					else
					{
						caret = _characterRectangles.GetUpperBound(0);
					}
				}

			}
			catch
			{
				caret = 0;
			}
			return caret;
		}

		private void InsertCharacter(char Value)
		{
			string restore = Text;

			try
			{
				//Clear any selections
				if (_select != _caret) DeleteSelection();
				
				string text = "";

				if (_caret > 0 && Text.Length >= _caret) text = Text.Substring(0, _caret);
				text += Value.ToString();
				if (_caret < Text.Length) text += Text.Substring(_caret);
				Text = text;

				//Move the caret one along
				_caret += 1;
				_select += 1;
			}
			catch
			{
				Text = restore;
			}
		}

		private void DeleteCharacter()
		{
			//Perform selection delete instead)
			if (_select != _caret)
			{
				DeleteSelection();
				return;
			}

			string restore = Text;

			try
			{
				string text = "";

				if (_caret > 0)	text = Text.Substring(0, _caret);
				if (_caret < Text.Length) text += Text.Substring(_caret + 1);

				Text = text;
			}
			catch
			{
				Text = restore;
			}
		}

		private void BackspaceCharacter()
		{
			//Perform selection delete instead)
			if (_select != _caret)
			{
				DeleteSelection();
				return;
			}
			//If at start of line then cant backspace
			if (_caret == 0) return;
			
			string restore = Text;

			try
			{
				//Reform text
				string text = "";

				text = Text.Substring(0, _caret - 1);
			
				if (_caret < Text.Length) text += Text.Substring(_caret);
				Text = text;

				//Move the caret back one
				_caret -= 1;
				_select = _caret;
			}
			catch
			{
				Text = restore;
			}
		}

		private void DeleteSelection()
		{
			string text = "";
			
			int start;
			int end;

			//Calculate start and end
			if (_select < _caret)
			{
				start = _select;
				end = _caret-1;
				_caret -= end-start+1;
			}
			else
			{
				start = _caret;
				end = _select-1;
			}

			string restore = Text;

			try
			{
				if (start > 0) text = Text.Substring(0, start);
				if (end < Text.Length) text += Text.Substring(end + 1);

				_select = _caret;
				Text = text;
			}
			catch
			{
				Text = restore;
			}
		}

		//Process an arrow charater
		private void ArrowCharacter(Keys key)
		{
			if (key == Keys.Left)
			{
				if (_caret > 0)	_caret -= 1;
			}
			else if (key == Keys.Right)
			{
				if (_caret < Text.Length) _caret += 1;
			}
			else if (key == Keys.Down)
			{
				for (int i = _caret + 1; i <= Text.Length - 1; i++)
				{
					if (_characterRectangles[i].Contains(_characterRectangles[_caret].X, _characterRectangles[i].Y))
					{
						_caret = i;
						break;
					}
				}

			}
			else if (key == Keys.Up)
			{
				for (int i = _caret - 1; i >= 0; i--)
				{
					if (_characterRectangles[i].Contains(_characterRectangles[_caret].X, _characterRectangles[i].Y))
					{
						_caret = i;
						break;
					}
				}
			}
		}

		private void EndCharacter()
		{
			_caret = Text.Length;
		}

		private void HomeCharacter()
		{
			_caret = 0;
		}

		//Measures the characters provided into the control character ranges 
		private void MeasureCharactersImplementation(Graphics graphics)
		{
			//Set the character rectangles defined by the character ranges 
			string text = Text;
			Region[] regions = null;
			CharacterRange[] ranges = new CharacterRange[1];
			StringFormat format = (StringFormat) StringFormat.Clone();
			RectangleF scaleRect = DisplayRectangle;
			float scale = _zoom / 100;

			if (_zoom != 100)
			{
				scaleRect = new RectangleF(0,0,DisplayRectangle.Width / scale,DisplayRectangle.Height / scale);
				graphics.ScaleTransform(scale,scale);
			}
			
			//Initialise
			if (text == "") text = ".";
			_characterRectangles = new RectangleF[text.Length+1];
			RectangleF rect;

			//Measure into rectangles starting at position 1 in the array
			try
			{
				for (int i = 0; i <= text.Length - 1; i++)
				{
					ranges[0].First = i;
					ranges[0].Length = 1;
					format.SetMeasurableCharacterRanges(ranges);
					regions = graphics.MeasureCharacterRanges(text, Font, scaleRect, format);
					rect = regions[0].GetBounds(graphics);
					_characterRectangles[i + 1] = rect;
				}

				_characterRectangles[0] = _characterRectangles[1];
				_characterRectangles[0].Width = 1;
				_characterRectangles[0].X -= 1;
			}
			catch (Exception objEx)
			{
				System.Diagnostics.Debug.WriteLine("Error measuring characters" + objEx.ToString());
			}
		}

		private SizeF MeasureTextImplementation(Graphics graphics)
		{
			return graphics.MeasureString(Text, Font, Width, StringFormat);
		}

		//Check to see if width of control smaller than characters
		private void CheckSize()
		{
			Graphics graphics = Graphics.FromImage(_renderBitmap);

			string text = Text;

			Size size = Size.Round(graphics.MeasureString(text, Font, Width, StringFormat));
			size.Width += 2;
			size.Height += 2;
			
			if (size.Width > Width && (AutoSizeMode == AutoSizeMode.Horizontal || AutoSizeMode == AutoSizeMode.Both)) Width = size.Width;
			if (size.Height > Height && (AutoSizeMode == AutoSizeMode.Vertical || AutoSizeMode == AutoSizeMode.Both)) Height = size.Height;

			Refresh();
		}

		#endregion
	}
}
