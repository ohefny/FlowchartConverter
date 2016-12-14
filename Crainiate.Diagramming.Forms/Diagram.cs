// (c) Copyright Crainiate Software 2010

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.Serialization;

using Crainiate.Diagramming.Serialization;
using Crainiate.Diagramming.Forms.Rendering;

namespace Crainiate.Diagramming.Forms
{
    [ToolboxBitmap(typeof(Crainiate.Diagramming.Forms.Diagram), "Resource.diagram.bmp")]
    public class Diagram: View
    {
        //Properties
        private bool _feedback;
        private DiagramUnit _feedbackUnit;
        private InteractiveMode _interactiveMode;

        private bool _alignGrid;
        private bool _dragScroll = true;
        private bool _dragSelect = true;
        
        private Element _dragElement;

        //Working variables
        private bool _handled;
        private Timer _timer;
        private bool _syncTimer;

        private Label _label;
        private ILabelEdit _labelEdit;
        private ToolTip _tooltip;

        private const int WM_KEYDOWN = 0x100;

        private TransformCommand _command;
        private MouseDownCommand _mouseDownCommand;
        private MouseMoveCommand _mouseMoveCommand;
        private MouseUpCommand _mouseUpCommand;

        #region Interface

        //Events
        [Category("Action"), Description("Occurs after user changes have been successfully applied to a diagram.")]
        public event UserActionEventHandler UpdateActions;

        [Category("Action"), Description("Occurs when user changes are cancelled without being applied to the diagram.")]
        public event UserActionEventHandler CancelActions;

        [Category("Action"), Description("Occurs when elements contained in the diagram are selected or deselected.")]
        public event EventHandler SelectedChanged;

        [Category("Action"), Description("Occurs when the user begins a drag selection with the mouse.")]
        public event MouseEventHandler BeginDragSelect;

        [Category("Action"), Description("Occurs when the user ends a drag selection with the mouse.")]
        public event MouseEventHandler EndDragSelect;

        [Category("Action"), Description("Occurs when the user ends a drag selection with the mouse.")]
        public event EventHandler CancelDragSelect;

        //Constructors
        public Diagram(): base()
        {
            Suspend();

            AllowDrop = true;

            Render = new ControlRender(); //Replace the render instance with a control render instance

            SetMouseElements(new MouseElements());

            ControlRender.DrawSelections = true;
            ControlRender.DrawGrid = true;

            Feedback = false;
            FeedbackUnit = DiagramUnit.Pixel;
            InteractiveMode = InteractiveMode.Normal;

            Resume();
        }

        //Properties
        [Category("Behavior"), DefaultValue(false), Description("Determines whether position and size information is provided to the user during an action.")]
        public virtual bool Feedback
        {
            get
            {
                return _feedback;
            }
            set
            {
                _feedback = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual DiagramUnit FeedbackUnit
        {
            get
            {
                return _feedbackUnit;
            }
            set
            {
                _feedbackUnit = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual InteractiveMode InteractiveMode
        {
            get
            {
                return _interactiveMode;
            }
            set
            {
                _interactiveMode = value;
            }
        }
        
        [Category("Behavior"), DefaultValue(false), Description("A boolean value determining whether elements are aligned to the grid.")]
        public virtual bool AlignGrid
        {
            get
            {
                return _alignGrid;
            }
            set
            {
                _alignGrid = value;
            }
        }

        [Category("Behavior"), DefaultValue(true), Description("Determines whether the diagram is drawn with a grid.")]
        public virtual bool DrawGrid
        {
            get
            {
                return ControlRender.DrawGrid;
            }
            set
            {
                if (DrawGrid != value)
                {
                    ControlRender.DrawGrid = value;
                    Render.Layers = Model.Layers;
                    Render.Elements = Model.Elements;
                    Render.RenderDiagram(PageRectangle, Paging);
                    DrawDiagram(ControlRectangle);
                }
            }
        }


        [Category("Behavior"), DefaultValue(true), Description("Determines whether selection handles are shown for a diagram.")]
        public virtual bool DrawSelections
        {
            get
            {
                return ControlRender.DrawSelections;
            }
            set
            {
                if (DrawSelections != value)
                {
                    ControlRender.DrawSelections = value;
                    Render.Layers = Model.Layers;
                    Render.Elements = Model.Elements;
                    Render.RenderDiagram(PageRectangle, Paging);
                    DrawDiagram(ControlRectangle);
                }
            }
        }

        [Category("Appearance"), Description("Sets or retrieves the color used to render the grid.")]
        public virtual Color GridColor
        {
            get
            {
                return ControlRender.GridColor;
            }
            set
            {
                ControlRender.GridColor = value;
                Render.Layers = Model.Layers;
                Render.Elements = Model.Elements;
                Render.RenderDiagram(PageRectangle, Paging);
                DrawDiagram(ControlRectangle);
            }
        }

        [Category("Appearance"), Description("Sets or retrieves the size object used to determine the grid spacing.")]
        public virtual Size GridSize
        {
            get
            {
                return ControlRender.GridSize;
            }
            set
            {
                if (!ControlRender.GridSize.Equals(value))
                {
                    if (value.Width < 1) value.Width = 1;
                    if (value.Height < 1) value.Height = 1;

                    ControlRender.GridSize = value;
                    Render.Layers = Model.Layers;
                    Render.Elements = Model.Elements;
                    Render.RenderDiagram(PageRectangle, Paging);
                    DrawDiagram(ControlRectangle);
                }
            }
        }

        [DefaultValue(GridStyle.Complex), Category("Appearance"), Description("Sets or retrieves the size object used to determine the grid spacing.")]
        public virtual GridStyle GridStyle
        {
            get
            {
                return ControlRender.GridStyle;
            }
            set
            {
                if (ControlRender.GridStyle != value)
                {
                    ControlRender.GridStyle = value;
                    Render.Layers = Model.Layers;
                    Render.Elements = Model.Elements;
                    Render.RenderDiagram(PageRectangle, Paging);
                    DrawDiagram(ControlRectangle);
                }
            }
        }

        [Category("Behavior"), DefaultValue(true), Description("Determines whether the diagram scrolls when an element is dragged outside the viewable area.")]
        public virtual bool DragScroll
        {
            get
            {
                return _dragScroll;
            }
            set
            {
                _dragScroll = value;
            }
        }

        [Category("Behavior"), DefaultValue(true), Description("Determines whether elements can be selected by dragging a selection rectangle around them.")]
        public virtual bool DragSelect
        {
            get
            {
                return _dragSelect;
            }
            set
            {
                _dragSelect = value;
            }
        }

        [Browsable(false)]
        public virtual Element DragElement
        {
            get
            {
                return _dragElement;
            }
            set
            {
                _dragElement = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual ControlRender ControlRender
        {
            get
            {
                return (ControlRender) Render;
            }
        }

        //Methods
        [Description("Starts editing for a label contained in the diagram.")]
        public virtual ILabelEdit StartEdit(Label label)
        {
            if (label == null) throw new ArgumentNullException("Label may not be null.");
            if (label.Parent == null) throw new DiagramException("Label cannot have null parent.");
            if (label.Parent.Model == null) throw new DiagramException("Label Parent container cannot be null.");
            if (!(label.Parent is ILabelContainer)) throw new DiagramException("Label Parent container must be an ILabelContainer object.");
            if (_labelEdit != null) return _labelEdit;

            //Set up control
            _labelEdit = CreateLabelEdit();
            if (!(_labelEdit is System.Windows.Forms.Control)) throw new DiagramException("Label editor does not inherit from System.Windows.Forms.Control.");

            System.Windows.Forms.Control labelEdit = (System.Windows.Forms.Control)_labelEdit;

            this.Controls.Add(labelEdit);
            _label = label;

            //Set up location and size 
            Element parent = label.Parent;
            ILabelContainer container = (ILabelContainer)parent;
            PointF location = container.GetLabelLocation();
            SizeF size = container.GetLabelSize();

            //Offset the location for the container
            location = new PointF(location.X + parent.Bounds.X, location.Y + parent.Bounds.Y);

            //Adjust the location and size to screen co-ordinates and pixels according to scale and units
            location = new PointF(location.X * Render.ScaleFactor, location.Y * Render.ScaleFactor);
            location = new PointF(location.X + DisplayRectangle.X, location.Y + DisplayRectangle.Y);
            size = new SizeF(size.Width * Render.ScaleFactor, size.Height * Render.ScaleFactor);

            //Adjust the location if the diagram is paged
            if (Paging.Enabled) location = new PointF(location.X + Paging.WorkspaceOffset.X, location.Y + Paging.WorkspaceOffset.Y);

            labelEdit.Location = Point.Round(location);
            labelEdit.Size = Size.Round(size);
            labelEdit.Font = label.Font;
            labelEdit.Text = label.Text;
            _labelEdit.StringFormat = label.GetStringFormat();
            labelEdit.ForeColor = label.Color;
            labelEdit.BackColor = Color.White;
            _labelEdit.Zoom = Zoom;
            _labelEdit.SendEnd();

            //Set event handlers
            _labelEdit.Complete += new EventHandler(mLabelEdit_Complete);
            _labelEdit.Cancel += new EventHandler(mLabelEdit_Complete);

            labelEdit.Visible = true;
            labelEdit.Focus();

            return _labelEdit;
        }

        [Description("Stops annotation editing for the current edited annotation.")]
        public virtual void StopEdit()
        {
            if (_labelEdit != null)
            {
                if (!_labelEdit.Cancelled) _label.Text = _labelEdit.Text;
                CancelEdit();
            }
        }

        [Description("Cancels annotation editing for the current edited annotation without updating the annotation.")]
        public virtual void CancelEdit()
        {
            if (_labelEdit != null)
            {
                //Remove event handlers
                _labelEdit.Complete -= new EventHandler(mLabelEdit_Complete);
                _labelEdit.Cancel -= new EventHandler(mLabelEdit_Complete);

                _labelEdit.Visible = false;
                this.Controls.Remove(_labelEdit as System.Windows.Forms.Control);
                _labelEdit = null;
            }
        }

        public virtual ILabelEdit CreateLabelEdit()
        {
            return new LabelEdit();
        }

        public virtual void SetFeedback(HandleType mouseHandleType)
        {
            ControlRender render = ControlRender;

            if (render.Actions == null) return;

            foreach (Element element in render.Actions)
            {
                if (element.ActionElement == CurrentMouseElements.MouseStartElement)
                {
                    System.Text.StringBuilder builder = new System.Text.StringBuilder();
                    string abbrev = Geometry.Abbreviate(FeedbackUnit);

                    if (element is Shape)
                    {
                        int decimals = DecimalsFromUnit(FeedbackUnit);

                        if (mouseHandleType == HandleType.Move)
                        {
                            PointF location = element.Bounds.Location;

                            //Check if must translate unit
                            if (FeedbackUnit != DiagramUnit.Pixel)
                            {
                                location = Geometry.ConvertPoint(location, _feedbackUnit);
                            }

                            //Round units
                            location = new PointF(Convert.ToSingle(Math.Round(location.X, decimals)), Convert.ToSingle(Math.Round(location.Y, decimals)));

                            builder.Append("x: ");
                            builder.Append(location.X.ToString());
                            builder.Append(" ");
                            builder.Append(abbrev);
                            builder.Append(" ");
                            builder.Append(" y: ");
                            builder.Append(location.Y.ToString());
                            builder.Append(" ");
                            builder.Append(abbrev);

                            render.Feedback = builder.ToString();
                        }
                        else if (mouseHandleType == HandleType.Rotate)
                        {
                            ITransformable transform = (ITransformable)element;
                            builder.Append(Convert.ToSingle(Math.Round(transform.Rotation, decimals)).ToString());
                            builder.Append(" degrees");

                            render.Feedback = builder.ToString();
                        }
                        else
                        {
                            SizeF size = element.Bounds.Size;

                            //Check if must translate unit
                            if (FeedbackUnit != DiagramUnit.Pixel)
                            {
                                Graphics g = null;
                                if (render.Bitmap != null) g = Graphics.FromImage(render.Bitmap);
                                size = Geometry.ConvertSize(size, FeedbackUnit, g);
                                if (g != null) g.Dispose();
                            }

                            //Round units
                            size = new SizeF(Convert.ToSingle(Math.Round(size.Width, decimals)), Convert.ToSingle(Math.Round(size.Height, decimals)));

                            builder.Append("w: ");
                            builder.Append(size.Width.ToString());
                            builder.Append(" ");
                            builder.Append(abbrev);
                            builder.Append(" ");
                            builder.Append(" h: ");
                            builder.Append(size.Height.ToString());
                            builder.Append(" ");
                            builder.Append(abbrev);

                            render.Feedback = builder.ToString();
                        }
                    }
                }
            }
        }

        [Description("Raises the UpdateActions event.")]
        protected virtual void OnUpdateActions(ElementList actions)
        {
            if (UpdateActions != null) UpdateActions(this, new UserActionEventArgs(actions));
        }

        [Description("Raises the CancelActions event.")]
        protected virtual void OnCancelActions(ElementList actions)
        {
            if (CancelActions != null) CancelActions(this, new UserActionEventArgs(actions));
        }

        [Description("Raises the selected changed event.")]
        protected virtual void OnSelectedChanged()
        {
            if (SelectedChanged != null) SelectedChanged(this, EventArgs.Empty);
        }

        [Description("Raises the BeginDragSelect event.")]
        protected virtual void OnBeginDragSelect(MouseEventArgs e)
        {
            if (BeginDragSelect != null) BeginDragSelect(this, e);
        }

        [Description("Raises the EndDragSelect event.")]
        protected virtual void OnEndDragSelect(MouseEventArgs e)
        {
            if (EndDragSelect != null) EndDragSelect(this, e);
        }

        [Description("Raises the CancelDragSelect event.")]
        protected virtual void OnCancelDragSelect()
        {
            if (CancelDragSelect != null) CancelDragSelect(this, EventArgs.Empty);
        }

        #endregion

        #region Overrides

        protected override void OnMouseDown(MouseEventArgs e)
        {
            //Cancel any text editing
            StopEdit();

            //Selects the mouse elements
            base.OnMouseDown(e);

            //If the status has changed then the mouse up event has been triggered
            //eg by opening a context menu
            if (Status != Status.Updating) return;

            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Suspend();
                CurrentMouseElements.StartPoint = new Point(e.X, e.Y);

                //Change the start location to a shape point if scaling on a grid align
                if (AlignGrid && CurrentMouseElements.MouseHandle != null)
                {
                    if (CurrentMouseElements.MouseHandle.Type != HandleType.Arrow && CurrentMouseElements.MouseHandle.Type != HandleType.Move && CurrentMouseElements.MouseHandle.Type != HandleType.Origin && CurrentMouseElements.MouseHandle.Type != HandleType.LeftRight && CurrentMouseElements.MouseHandle.Type != HandleType.UpDown && CurrentMouseElements.MouseHandle.Type != HandleType.Expand)
                    {
                        SetScaleStartingPoint();
                    }
                }

                CurrentMouseElements.LastPoint = CurrentMouseElements.StartPoint;
                _handled = false;

                //Deselect all elements
                if (ModifierKeys != Keys.Control && (CurrentMouseElements.MouseStartElement == null || !CurrentMouseElements.MouseStartSelectable.Selected || CurrentMouseElements.MouseStartElement is Port)) Controller.SelectElements(false);

                //Select the new element
                if (CurrentMouseElements.MouseStartElement != null)
                {
                    //Select the shape and bring to front if in same layer
                    if (CurrentMouseElements.MouseStartElement.Layer == Model.Layers.CurrentLayer)
                    {
                        //Check if expander
                        if (CurrentMouseElements.MouseStartElement is IExpandable)
                        {
                            Element element = CurrentMouseElements.MouseStartElement;
                            IExpandable expand = (IExpandable) element;
                            PointF location = element.PointToElement(PointToDiagram(e));

                            //Modify expanded property
                            if (expand.DrawExpand && expand.Expander != null && expand.Expander.IsVisible(location))
                            {
                                _handled = true;

                                //Suspend to avoid multiple repaints
                                Suspend();
                                expand.Expanded = !expand.Expanded;
                                Resume();

                                Invalidate();
                            }
                        }

                        //Check if handles mouse
                        if (CurrentMouseElements.MouseStartElement is IMouseEvents)
                        {
                            IMouseEvents element = (IMouseEvents) CurrentMouseElements.MouseStartElement;
                            _mouseDownCommand = Controller.CommandFactory.CreateMouseDownCommand(element);
                            _mouseDownCommand.MouseButtons = (MouseCommandButtons) Enum.Parse(typeof(MouseCommandButtons), e.Button.ToString());
                            _mouseDownCommand.Location = PointToDiagram(e);

                            _mouseDownCommand.Execute();

                            _handled = _mouseDownCommand.Handled;
                        }

                        //Process if mouse has not been handled
                        if (!_handled)
                        {
                            //Select or invert
                            if (ModifierKeys == Keys.Control)
                            {
                                CurrentMouseElements.MouseStartSelectable.Selected = !CurrentMouseElements.MouseStartSelectable.Selected;
                            }
                            else
                            {
                                CurrentMouseElements.MouseStartSelectable.Selected = true;
                            }

                            //Check if must come to front
                            if (IsOrderable(CurrentMouseElements.MouseStartElement))
                            {
                                if (CurrentMouseElements.MouseStartElement is Shape) Model.Elements.BringToFront(CurrentMouseElements.MouseStartElement);
                                if (CurrentMouseElements.MouseStartElement is Link) Model.Elements.BringToFront(CurrentMouseElements.MouseStartElement);
                                Model.Elements.Sort();

                                //Redraw all connectors with jumps
                                if (CurrentMouseElements.MouseStartElement is Connector) RedrawConnectors();
                            }

                            //Start a drag scroll timer if required
                            if (DragScroll)
                            {
                                _timer = new Timer();
                                _timer.Interval = Singleton.Instance.DragScrollInterval;
                                _timer.Tick += new EventHandler(mTimer_Tick);
                                _timer.Start();
                            }
                        }
                    }
                }

                Resume();
                Invalidate();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            //Check if in dragscroll and if so exit
            if (_syncTimer) return;

            Point mousePoint = new Point(e.X, e.Y);

            if (IsAlignable(CurrentMouseElements.MouseStartElement)) mousePoint = AlignMouseCoordinates(e);

            //Check if handles mouse
            if (!_handled && CurrentMouseElements.MouseMoveElement is IMouseEvents)
            {
                IMouseEvents element = (IMouseEvents) CurrentMouseElements.MouseMoveElement;
                if (_mouseMoveCommand == null) _mouseMoveCommand = Controller.CommandFactory.CreateMouseMoveCommand(element);
                _mouseMoveCommand.MouseButtons = (MouseCommandButtons) Enum.Parse(typeof(MouseCommandButtons), e.Button.ToString());
                _mouseMoveCommand.Location = PointToDiagram(e);

                _mouseMoveCommand.Execute();

                _handled = _mouseMoveCommand.Handled;
            }

            if (!_handled)
            {
                //Set the cursor
                if (e.Button == MouseButtons.None)
                {
                    if (CurrentMouseElements.MouseMoveElement == null)
                    {
                        Cursor = Singleton.Instance.GetCursor(HandleType.None);
                        CancelTooltip();
                    }
                    else
                    {
                        if (CurrentMouseElements.MouseMoveElement.Layer == Model.Layers.CurrentLayer)
                        {
                            CurrentMouseElements.MouseHandle = CurrentMouseElements.MouseMoveElement.Handle(PointToDiagram(e));

                            if (CurrentMouseElements.MouseMoveElement.Cursor != null && CurrentMouseElements.MouseHandle.Type == HandleType.Move)
                            {
                                Cursor = CurrentMouseElements.MouseMoveElement.Cursor;
                            }
                            else
                            {
                                Cursor = Singleton.Instance.GetCursor(CurrentMouseElements.MouseHandle.Type);
                            }

                            //Set the tooltip if it is null or has changed
                            if (_tooltip == null || (_tooltip.GetToolTip(this) != CurrentMouseElements.MouseMoveElement.Tooltip)) SetTooltip(CurrentMouseElements.MouseMoveElement);
                        }
                    }
                }

                //Check left button only
                bool leftButton = (e.Button & MouseButtons.Left) == MouseButtons.Left;

                if (leftButton)
                {
                    Suspend();

                    ControlRender.Highlights = new ElementList(true);

                    float dx = (mousePoint.X - CurrentMouseElements.LastPoint.X) * Render.ZoomFactor;
                    float dy = (mousePoint.Y - CurrentMouseElements.LastPoint.Y) * Render.ZoomFactor;

                    if (InteractiveMode == InteractiveMode.Normal && leftButton)
                    {
                        if (CurrentMouseElements.MouseStartElement == null)
                        {
                            //Set up drag rectangle
                            if (DragSelect && !CurrentMouseElements.StartPoint.Equals(new Point(e.X, e.Y)))
                            {
                                //Set the decoration selection rectangle
                                Rectangle select = System.Drawing.Rectangle.Round(Geometry.CreateRectangle(CurrentMouseElements.StartPoint, new Point(e.X, e.Y)));

                                //Offset the rectangle by the scroll
                                //Select the elements in the rectangle
                                select.Offset(-DisplayRectangle.X, -DisplayRectangle.Y);
                                Controller.SelectElements(RectangleToDiagram(select));

                                //Raise the event and set in the render
                                if (ControlRender.SelectionRectangle.IsEmpty) OnBeginDragSelect(e);
                                ControlRender.SelectionRectangle = select;
                            }
                        }
                        else
                        {
                            //If this is the first mouse move then create the action renderlist and lock the renderer
                            if (!Render.Locked)
                            {
                                ElementList actions = CreateActionRenderList();

                                //If align to grid then set up positions of shapes
                                //Reset dx,dy because last mouse point wasnt aligned
                                if (AlignGrid && CurrentMouseElements.MouseHandle.Type == HandleType.Move)
                                {
                                    AlignElementLocations(actions);
                                    dx = 0;
                                    dy = 0;
                                }

                                //Hide the actioned elements and render before lock
                                if (Singleton.Instance.HideActions)
                                {
                                    HideActionElements(actions);
                                    Render.Layers = Model.Layers;
                                    Render.Elements = Model.Elements;
                                    Render.RenderDiagram(Render.RenderRectangle, Paging);
                                }

                                Render.Lock();
                                ControlRender.Actions = actions;

                                //Make sure command has been cleared
                                _command = null;
                            }

                            //Loop through and offset each element in the action renderlist
                            if (CurrentMouseElements.MouseHandle != null)
                            {
                                ControlRender.Vectors = null;

                                if (CurrentMouseElements.MouseHandle.Type == HandleType.Move)
                                {
                                    if (_command == null)
                                    {
                                        _command = Controller.CommandFactory.CreateTranslateCommand();
                                        _command.MouseElements = CurrentMouseElements;
                                        _command.Elements = ControlRender.Actions;
                                    }
                                    
                                    TranslateCommand command = _command as TranslateCommand;
                                    command.Dx = (mousePoint.X - CurrentMouseElements.StartPoint.X) * Render.ZoomFactor; //distance cursor has moved
                                    command.Dy = (mousePoint.Y - CurrentMouseElements.StartPoint.Y) * Render.ZoomFactor;

                                    command.Translate();

                                    //Update any recalculated vectors
                                    ControlRender.Vectors = command.Vectors;
                                }
                                else if (CurrentMouseElements.MouseHandle.Type == HandleType.Rotate)
                                {
                                    if (_command == null)
                                    {
                                        _command = Controller.CommandFactory.CreateRotateCommand();
                                        _command.MouseElements = CurrentMouseElements;
                                        _command.Elements = ControlRender.Actions;
                                    }

                                    RotateCommand command = _command as RotateCommand;

                                    command.Degrees = Convert.ToInt32(Math.Atan2(dy, dx) * (180 / Math.PI)) + 90;
                                    command.MousePosition = PointToClient(Control.MousePosition);
                                    command.Rotate();

                                    //Update any recalculated vectors
                                    ControlRender.Vectors = command.Vectors;
                                }
                                else
                                {
                                    if (_command == null)
                                    {
                                        _command = Controller.CommandFactory.CreateScaleCommand();
                                        _command.MouseElements = CurrentMouseElements;
                                        _command.Elements = ControlRender.Actions;
                                        _command.InteractiveMode = InteractiveMode;
                                    }

                                    ScaleCommand command = _command as ScaleCommand;

                                    command.Dx = (CurrentMouseElements.LastPoint.X - CurrentMouseElements.StartPoint.X) * Render.ZoomFactor; //distance cursor has moved
                                    command.Dy = (CurrentMouseElements.LastPoint.Y - CurrentMouseElements.StartPoint.Y) * Render.ZoomFactor; 
                                    command.KeepAspect = ModifierKeys == Keys.Shift;

                                    command.Scale();

                                    //Update any recalculated vectors
                                    ControlRender.Vectors = command.Vectors;
                                }
                            }
                        }

                        //Set any highlights
                        if (CurrentMouseElements.MouseStartOrigin != null && CurrentMouseElements.MouseStartOrigin.AllowMove && CurrentMouseElements.MouseMoveElement != null)
                        {
                            //Check origin is dockable, containers are compatible etc
                            if (CurrentMouseElements.IsDockable())
                            {
                                if (Controller.CanDock(this.InteractiveMode, CurrentMouseElements))
                                {
                                    ControlRender.Highlights.SetModifiable(true);
                                    ControlRender.Highlights.Add(CurrentMouseElements.MouseMoveElement);
                                    ControlRender.Highlights.SetModifiable(false);
                                }
                            }
                        }

                        //Set any feedback 
                        if (Feedback && CurrentMouseElements.MouseHandle != null)
                        {
                            SetFeedback(CurrentMouseElements.MouseHandle.Type);
                            ControlRender.FeedbackLocation = new Point(e.X + 16 - DisplayRectangle.X, e.Y + 16 - DisplayRectangle.Y);
                        }
                    }

                    //Add interactive items
                    if (InteractiveMode != InteractiveMode.Normal)
                    {
                        //Create interactive items
                        if (!Render.Locked)
                        {
                            ControlRender.Actions = CreateInteractiveRenderList(mousePoint);
                            Render.Lock(); //Not locked as interactive creates a new item every time
                        }
                        //Loop through and offset each element in the action renderlist
                        else if (CurrentMouseElements.MouseStartElement != null)
                        {
                            if (CurrentMouseElements.MouseHandle.Type == HandleType.Move)
                            {
                                if (_command == null)
                                {
                                    _command = Controller.CommandFactory.CreateTranslateCommand();

                                    _command.MouseElements = CurrentMouseElements;
                                    _command.Elements = ControlRender.Actions;
                                }

                                TranslateCommand command = _command as TranslateCommand;

                                command.Dx = (CurrentMouseElements.LastPoint.X - CurrentMouseElements.StartPoint.X) * Render.ZoomFactor; //distance cursor has moved
                                command.Dy = (CurrentMouseElements.LastPoint.Y - CurrentMouseElements.StartPoint.Y) * Render.ZoomFactor;
                                
                                command.Translate();

                                //Update any recalculated vectors
                                ControlRender.Vectors = command.Vectors;
                            }
                            else
                            {
                                if (_command == null)
                                {
                                    _command = Controller.CommandFactory.CreateScaleCommand();

                                    _command.MouseElements = CurrentMouseElements;
                                    _command.Elements = ControlRender.Actions;
                                    _command.InteractiveMode = InteractiveMode;
                                }

                                ScaleCommand command = _command as ScaleCommand;

                                command.Dx = (CurrentMouseElements.LastPoint.X - CurrentMouseElements.StartPoint.X) * Render.ZoomFactor; //distance cursor has moved
                                command.Dy = (CurrentMouseElements.LastPoint.Y - CurrentMouseElements.StartPoint.Y) * Render.ZoomFactor;
                                command.KeepAspect = ModifierKeys == Keys.Shift;

                                command.Scale();

                                //Update any recalculated vectors
                                ControlRender.Vectors = command.Vectors;
                            }
                        }

                        //Set any highlights
                        if (InteractiveMode != InteractiveMode.AddShape && InteractiveMode != InteractiveMode.AddComplexShape && CurrentMouseElements.MouseMoveElement != null)
                        {
                            //Check origin is dockable, containers are compatible etc
                            if (CurrentMouseElements.IsDockable())
                            {
                                if (Controller.CanDock(this.InteractiveMode, CurrentMouseElements))
                                {
                                    ControlRender.Highlights.SetModifiable(true);
                                    ControlRender.Highlights.Add(CurrentMouseElements.MouseMoveElement);
                                    ControlRender.Highlights.SetModifiable(false);
                                }
                            }
                        }

                        //Set any feedback 
                        if (Feedback)
                        {
                            SetFeedback(CurrentMouseElements.MouseHandle.Type);
                            ControlRender.FeedbackLocation = new Point(e.X + 16 - DisplayRectangle.X, e.Y + 16 - DisplayRectangle.Y);
                        }
                    }

                    Resume();
                    Invalidate();
                }
            }

            CurrentMouseElements.LastPoint = mousePoint;
            
            //Raise events etc
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            //Dispose of Dragscroll timer
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }

            //Check if handles mouse
            if (!_handled && CurrentMouseElements.MouseStartElement is IMouseEvents)
            {
                IMouseEvents element = (IMouseEvents) CurrentMouseElements.MouseStartElement;
                _mouseUpCommand = Controller.CommandFactory.CreateMouseUpCommand(element);
                _mouseUpCommand.MouseButtons = (MouseCommandButtons) Enum.Parse(typeof(MouseCommandButtons), e.Button.ToString());
                _mouseUpCommand.Location = PointToDiagram(e);

                _mouseUpCommand.Execute();

                _handled = _mouseUpCommand.Handled;
            }

            //Process left mouse button
            if (!_handled)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (InteractiveMode == InteractiveMode.Normal && (e.Button & MouseButtons.Left) == MouseButtons.Left)
                    {
                        //Reset highlights
                        ControlRender.Highlights = new ElementList(true);

                        if (CurrentMouseElements.MouseStartElement == null)
                        {
                            //Reset the selection rectangle decoration
                            if (!ControlRender.SelectionRectangle.IsEmpty)
                            {
                                ControlRender.SelectionRectangle = new Rectangle();
                                OnEndDragSelect(e);
                            }
                        }
                        else
                        {
                            //Moves the elements according to the renderlist
                            if (ControlRender.Actions != null)
                            {
                                //Unhides the action elements
                                if (Singleton.Instance.HideActions) ShowActionElements(ControlRender.Actions);

                                Suspend();

                                //Execute the command
                                Controller.ExecuteCommand(_command);

                                //Dont remove all references to cloned objects if required for undo/redo
                                if (!Controller.AllowUndo)
                                {
                                    foreach (Element element in ControlRender.Actions)
                                    {
                                        element.ActionElement = null;
                                    }
                                }

                                RedrawConnectors();
                                Resume();
                                Invalidate();

                                OnUpdateActions(ControlRender.Actions);
                                ControlRender.Actions = null;
                                ControlRender.Feedback = null;
                                ControlRender.FeedbackLocation = new Point();
                                ControlRender.Vectors = new RectangleF[] { };
                                Render.Unlock();

                                _command = null;
                            }
                        }
                    }

                    if (InteractiveMode != InteractiveMode.Normal)
                    {
                        //Reset highlights
                        ControlRender.Highlights = new ElementList(true);

                        //Cant suspend as events are required
                        UpdateInteractiveElements(this);

                        Controller.ExecuteCommand(_command);

                        //Remove all references to cloned objects
                        foreach (Element element in ControlRender.Actions)
                        {
                            element.ActionElement = null;
                        }

                        OnUpdateActions(ControlRender.Actions);
                        ControlRender.Actions = null;
                        ControlRender.Feedback = null;
                        ControlRender.FeedbackLocation = new Point();
                        Render.Unlock();

                        _command = null;
                    }
                    Invalidate();
                }
            }

            _handled = false;

            base.OnMouseUp(e);
        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            SaveStatus();
            SetStatus(Status.DragOver);

            if (DragElement != null)
            {

                //Translate the drag point from screen co-ordinates to control co-ordinates
                Point controlPoint = PointToClient(new Point(drgevent.X, drgevent.Y));

                //Get the offset so that the shape is centered round the cursor
                PointF dropPoint = OffsetDrop(controlPoint, DragElement.Bounds);

                //Reset the decoration path
                ControlRender.DecorationPath = DragElement.GetPath().Clone() as GraphicsPath;

                //Move the decoration path to the new position
                Matrix translate = new Matrix();
                translate.Translate(dropPoint.X, dropPoint.Y);
                ControlRender.DecorationPath.Transform(translate);

                Invalidate();
            }

            base.OnDragOver(drgevent);
            RestoreStatus();
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            SaveStatus();
            SetStatus(Status.DragEnter);

            //Get the type 
            Type type;

            foreach (String str in drgevent.Data.GetFormats())
            {
                //Try resolve the type, possibly across assemblies
                type = null;
                type = global::Crainiate.Diagramming.Serialization.Serialize.ResolveType(str);

                if (type != null && type.IsSubclassOf(typeof(Element)))
                {
                    drgevent.Effect = DragDropEffects.Copy;

                    //Set up the drag element
                    DragElement = (Element)drgevent.Data.GetData(type); //gets the data
                    
                    //Apply some scaling if a shape
                    if (DragElement is Shape)
                    {
                        //if (this.GetType() == typeof(Diagram)) drgevent.Effect = DragDropEffects.Copy;
                        var shape = DragElement as Shape;
                        var defaultSize = GetDefaultSize();
                        var existing = shape.Size;
                        var keepAspect = shape.KeepAspect;

                        shape.MinimumSize = GetMinimumSize();
                        shape.MaximumSize = GetMaximumSize();
                        shape.BorderWidth = Singleton.Instance.DefaultBorderWidth;

                        shape.Scale(defaultSize.Width / existing.Width, defaultSize.Height / existing.Height, 0, 0, true);
                    }

                    //Translate the drag point from screen co-ordinates to control co-ordinates
                    PointF dragPoint = PointToClient(new Point(drgevent.X, drgevent.Y));
                    dragPoint = OffsetDrop(dragPoint, DragElement.Bounds);

                    //Reset the decoration path
                    ControlRender.DecorationPath = DragElement.GetPath().Clone() as GraphicsPath;

                    //Move the decoration path to the new position
                    Matrix translate = new Matrix();
                    translate.Translate(dragPoint.X, dragPoint.Y);
                    ControlRender.DecorationPath.Transform(translate);

                    Invalidate();
                    break;
                }
            }

            base.OnDragEnter(drgevent);
            RestoreStatus();
        }

        protected virtual SizeF GetMinimumSize()
        {
            return Singleton.Instance.DefaultMinimumSize;
        }

        protected virtual SizeF  GetMaximumSize()
        {
            return Singleton.Instance.DefaultMaximumSize;
        }

        protected virtual SizeF GetDefaultSize()
        {
            return Singleton.Instance.DefaultSize;
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            SaveStatus();
            SetStatus(Status.DragDrop);

            if (DragElement != null)
            {
                Suspend();

                //Set the container
                IContainer container = this.Model as IContainer;

                //Translate the drag point from screen co-ordinates to control co-ordinates
                Point controlPoint = PointToClient(new Point(drgevent.X, drgevent.Y));

                //Get any container elements under the cursor
                Element element = ElementFromPoint(controlPoint.X, controlPoint.Y);
                if (element is IContainer)
                {
                    container = element as IContainer;
                }

                PointF dropPoint = OffsetDrop(controlPoint, DragElement.Bounds);;

                //Move any ports
                if (DragElement is IPortContainer)
                {
                    IPortContainer portContainer = (IPortContainer)DragElement;

                    //Store values before updating underlying rectangle
                    float dx = dropPoint.X - DragElement.Bounds.X;
                    float dy = dropPoint.Y - DragElement.Bounds.Y;

                    //Move ports by change in x and y
                    if (portContainer.Ports != null)
                    {
                        foreach (Port port in portContainer.Ports.Values)
                        {
                            port.SuspendValidation();
                            port.Move(dx, dy);
                            port.ResumeValidation();
                        }
                    }
                }

                //Change settings				
                DragElement.SetRectangle(dropPoint);
                if (DragElement is Solid)
                {
                    Solid solid = (Solid)DragElement;
                    solid.SetTransformRectangle(dropPoint);
                }

                DragElement.SetLayer(null);

                if (DragElement is Shape)
                {
                    Shape shape = (Shape)DragElement;

                    shape.AllowMove = true;
                    shape.AllowScale = true;
                    //shape.MinimumSize = new SizeF(32, 32);
                    //shape.MaximumSize = new SizeF(320, 320);

                    //Align to grid using common grid alignment
                    if (AlignGrid)
                    {
                        ElementList list = new ElementList(true);
                        list.Add(shape);
                        AlignElementLocations(list);
                    }

                    //Add the element to lines or shapes
                    Model.Shapes.Add(Model.Shapes.CreateKey(), DragElement as Shape);
                }

                //Clear the render path
                ControlRender.DecorationPath = null;
                ControlRender.Highlights = null;

                //Repaint
                Resume();
                Refresh();
            }

            base.OnDragDrop(drgevent);
            DragElement = null; //Set to null only after the event

            RestoreStatus();
        }

        protected override void OnDragLeave(EventArgs e)
        {
            base.OnDragLeave(e);

            DragElement = null;
            ControlRender.DecorationPath = null;
            ControlRender.Highlights = null;
            Invalidate();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //Dont process messages if the text editor is active
            if (_labelEdit != null) return false;

            if (msg.Msg == WM_KEYDOWN)
            {
                switch (keyData)
                {
                    case Keys.Control | Keys.Z:

                        if (Controller.AllowUndo)
                        {
                            //DoCommand("undo");
                            return true;
                        }
                        break;

                    case Keys.Control | Keys.Y:

                        if (Controller.AllowRedo)
                        {
                            //DoCommand("redo");
                            return true;
                        }
                        break;

                    case Keys.Control | Keys.X:

                        if (Controller.AllowCut)
                        {
                            //DoCommand("cut");
                            return true;
                        }
                        break;

                    case Keys.Control | Keys.C:

                        if (Controller.AllowCopy)
                        {
                            //DoCommand("copy");
                            return true;
                        }
                        break;

                    case Keys.Control | Keys.V:

                        if (Controller.AllowPaste)
                        {
                            //DoCommand("paste");
                            return true;
                        }
                        break;

                    case Keys.Control | Keys.A:

                        Suspend();
                        Controller.SelectElements(true);
                        Resume();
                        Refresh();

                        return true;

                    case Keys.Control | Keys.PageDown:

                        ZoomModel(true);

                        return true;

                    case Keys.Control | Keys.PageUp:

                        ZoomModel(false);

                        return true;

                    case Keys.Delete:

                        if (Controller.AllowDelete)
                        {
                            //DoCommand("delete");
                            return true;
                        }
                        break;

                    case Keys.Escape:

                        CancelAction();
                        return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnElementInserted(Element element)
        {
            //Attach handler to detect if selected
            if (element is ISelectable)
            {
                ISelectable selectable = (ISelectable)element;
                selectable.SelectedChanged += new EventHandler(selectable_SelectedChanged);
            }

            base.OnElementInserted(element);
        }

        protected override void OnElementRemoved(Element element)
        {
            //Remove handler to detect if selected
            if (element is ISelectable)
            {
                ISelectable selectable = (ISelectable)element;
                selectable.SelectedChanged -= new EventHandler(selectable_SelectedChanged);
            }

            base.OnElementRemoved(element);
        }

        #endregion

        #region Events

        //Occurs when the timer ticks
        private void mTimer_Tick(object sender, EventArgs e)
        {
            _syncTimer = true;
            CheckDragScroll();
            _syncTimer = false;
        }

        private void mLabelEdit_Complete(object sender, EventArgs e)
        {
            StopEdit();
        }

        //Add or remove items from the selected list
        private void selectable_SelectedChanged(object sender, EventArgs e)
        {
            Model.ResetSelectedElements();
            OnSelectedChanged();
        }

        #endregion

        #region Implementation

        //Builds up a renderlist containing copies of the elements to be rendered in an action
        protected internal virtual ElementList CreateActionRenderList()
        {
            ElementList renderList = Model.Elements;
            ElementList actions = new ElementList(true);
            Element newElement;
            Port newPort;

            actions.SetModifiable(true);

            foreach (Element element in renderList)
            {
                if (element is ISelectable && element.Layer == Model.Layers.CurrentLayer)
                {
                    //Add shapes and lines
                    ISelectable selectable = (ISelectable)element;

                    if (selectable.Selected)
                    {
                        newElement = Controller.CloneElement(element);
                        actions.Add(newElement);
                    }
                }
            }

            //Connect any shapes and lines, adding any non-selected items as hidden actions
            ConnectInteractiveElements(actions);

            //Loop through and connect the lines
            foreach (Element element in actions)
            {
                if (element is Link)
                {
                    Link line = (Link)element;
                    if (line.Start.Shape != null) line.Start.Shape = (Shape)actions[line.Start.Shape.Key];
                    if (line.End.Shape != null) line.End.Shape = (Shape)actions[line.End.Shape.Key];
                }
            }

            //Add any ports
            if (CurrentMouseElements.MouseStartElement is Port)
            {
                newPort = CurrentMouseElements.MouseStartElement.Clone() as Port;
                newPort.Validate = true;
                newPort.ActionElement = CurrentMouseElements.MouseStartElement;
                actions.Add(newPort);
            }

            return actions;
        }

        //Builds up a renderlist containing copies of the elements to be rendered in an action
        protected internal virtual ElementList CreateInteractiveRenderList(Point mousePoint)
        {
            ElementList actions = new ElementList(true);
            PointF start = PointToDiagram(CurrentMouseElements.StartPoint.X, CurrentMouseElements.StartPoint.Y);
            PointF end = PointToDiagram(mousePoint.X, mousePoint.Y);

            //Prepare a new mouse elements structure
            MouseElements mouseElements = new MouseElements(CurrentMouseElements);

            //Add a line interactively
            if (InteractiveMode == InteractiveMode.AddLine)
            {
                //Create new line
                Link line = Controller.Factory.CreateLine(start, end);

                //Set temporary container and layer
                line.SetLayer(Model.Layers.CurrentLayer);

                //Set line container
                line.SetModel(Model);

                line.DrawPath();

                //Create action line
                Link newLine = (Link)line.Clone();
                newLine.ActionElement = line;
                actions.Add(newLine);

                mouseElements.MouseHandle = new Handle(HandleType.Origin);
                mouseElements.MouseHandle.CanDock = true;

                //Set up mouse elements
                mouseElements.MouseStartElement = line;
                mouseElements.MouseStartOrigin = line.End;
            }

            //Add an connector interactively
            if (InteractiveMode == InteractiveMode.AddConnector)
            {
                //Create new line
                Connector line = Controller.Factory.CreateConnector(start, end);

                //Set temporary container and layer
                line.SetLayer(Model.Layers.CurrentLayer);

                //Set line container
                line.SetModel(Model);

                line.Avoid = true;
                line.CalculateRoute();
                line.DrawPath();

                //Create action line
                Connector newLine = line.Clone() as Connector;
                newLine.ActionElement = line;
                newLine.Avoid = true;
                actions.Add(newLine);

                mouseElements.MouseHandle = new Handle(HandleType.Origin);
                mouseElements.MouseHandle.CanDock = true;

                //Set up mouse elements
                mouseElements.MouseStartElement = line;
                mouseElements.MouseStartOrigin = line.End;
            }

            if (InteractiveMode == InteractiveMode.AddComplexLine)
            {
                //Create new line
                ComplexLine line = Controller.Factory.CreateComplexLine(start, end);

                //Set temporary container and layer
                line.SetLayer(Model.Layers.CurrentLayer);

                //Set line container
                line.SetModel(Model);

                line.DrawPath();

                //Create action line
                Link newLine = line.Clone() as ComplexLine;
                newLine.ActionElement = line;
                actions.Add(newLine);

                mouseElements.MouseHandle = new Handle(HandleType.Origin);
                mouseElements.MouseHandle.CanDock = true;

                //Set up mouse elements
                mouseElements.MouseStartElement = line;
                mouseElements.MouseStartOrigin = line.End;
            }

            if (InteractiveMode == InteractiveMode.AddCurve)
            {
                //Create new line
                Curve line = Controller.Factory.CreateCurve(start, end);

                //Set temporary container and layer
                line.SetLayer(Model.Layers.CurrentLayer);

                //Set line container
                line.SetModel(Model);

                line.DrawPath();

                //Create action line
                Link newLine = (Curve)line.Clone();
                newLine.ActionElement = line;
                actions.Add(newLine);

                mouseElements.MouseHandle = new Handle(HandleType.Origin);
                mouseElements.MouseHandle.CanDock = true;

                //Set up mouse elements
                mouseElements.MouseStartElement = line;
                mouseElements.MouseStartOrigin = line.End;
            }

            if (InteractiveMode == InteractiveMode.AddShape)
            {
                SizeF size = new SizeF(end.X - start.X, end.Y - start.Y);
                Shape shape = Controller.Factory.CreateShape(start, size);

                //Set temporary container and layer
                shape.SetLayer(Model.Layers.CurrentLayer);

                //Set line container
                shape.SetModel(Model);

                //Create action shape
                Shape newShape = shape.Clone() as Shape;
                newShape.ActionElement = shape;
                actions.Add(newShape);

                mouseElements.MouseHandle = new Handle(HandleType.BottomRight);
            }

            if (InteractiveMode == InteractiveMode.AddComplexShape)
            {
                SizeF size = new SizeF(end.X - start.X, end.Y - start.Y);
                ComplexShape shape = Controller.Factory.CreateComplexShape(start, size);

                //Set temporary container and layer
                shape.SetLayer(Model.Layers.CurrentLayer);

                //Set line container
                shape.SetModel(Model);

                //Create action shape
                ComplexShape newShape = shape.Clone() as ComplexShape;
                newShape.ActionElement = shape;
                actions.Add(newShape);

                //Set the action shapes for the complex shape children
                foreach (Solid solid in newShape.Children.Values)
                {
                    solid.ActionElement = shape.Children[solid.Key];
                }

                mouseElements.MouseHandle = new Handle(HandleType.BottomRight);
            }

            //Check for interactive docking
            if (InteractiveMode == InteractiveMode.AddLine || InteractiveMode == InteractiveMode.AddConnector || InteractiveMode == InteractiveMode.AddComplexLine || InteractiveMode == InteractiveMode.AddCurve)
            {
                foreach (Element element in actions)
                {
                    if (element is Link)
                    {
                        Link line = (Link)element;
                        Link action = (Link)line.ActionElement;

                        //Set up the elements
                        mouseElements.InteractiveElement = line;
                        mouseElements.InteractiveOrigin = line.Start;
                        mouseElements.MouseMoveElement = CurrentMouseElements.MouseMoveElement;

                        //Check if start is shape
                        if (CurrentMouseElements.MouseMoveElement is Shape && Controller.CanDock(InteractiveMode, mouseElements))
                        {
                            //line.Start.Shape = (Shape) CurrentMouseElements.MouseStartElement;
                            action.Start.Shape = CurrentMouseElements.MouseStartElement as Shape;
                        }

                        //Check if start is port
                        if (CurrentMouseElements.MouseMoveElement is Port && Controller.CanDock(InteractiveMode, mouseElements))
                        {
                            //line.Start.Port = (Port) CurrentMouseElements.MouseStartElement;
                            action.Start.Port = CurrentMouseElements.MouseStartElement as Port;
                        }
                    }
                }
            }

            ConnectInteractiveElements(actions);

            //Set mouse elements
            SetMouseElements(mouseElements);

            return actions;
        }

        //Connect any shapes and lines, adding any non-selected items as hidden actions
        private void ConnectInteractiveElements(ElementList actions)
        {
            //Set up origins for any lines
            ElementList hidden = new ElementList(true);
            hidden.SetModifiable(true);

            foreach (Element element in actions)
            {
                if (element is Link)
                {
                    Link line = (Link)element;
                    Link actionLine = (Link)element.ActionElement;
                    Shape newShape;
                    Element newElement;
                    IPortContainer ports;

                    //Add any shapes and shapes with ports that are connected to lines as invisible items
                    if (actionLine.Start.Shape != null)
                    {
                        if (!actions.ContainsKey(actionLine.Start.Shape.Key))
                        {
                            newShape = (Shape)actionLine.Start.Shape.Clone();
                            newShape.ActionElement = actionLine.Start.Shape;
                            newShape.SetKey(actionLine.Start.Shape.Key);
                            newShape.Visible = false;
                            hidden.Add(newShape);

                            line.Start.Shape = newShape;
                        }
                        else
                        {
                            line.Start.Shape = (Shape)actions[actionLine.Start.Shape.Key];
                        }
                    }
                    else if (actionLine.Start.Port != null)
                    {
                        Element parent = (Element)actionLine.Start.Port.Parent;

                        if (!actions.ContainsKey(parent.Key))
                        {
                            newElement = (Element)parent.Clone();
                            newElement.ActionElement = parent;
                            newElement.SetKey(parent.Key);
                            newElement.Visible = false;
                            hidden.Add(newElement);

                            ports = (IPortContainer)newElement;
                            line.Start.Port = (Port)ports.Ports[actionLine.Start.Port.Key];
                        }
                        else
                        {
                            ports = (IPortContainer)actions[parent.Key];
                            line.Start.Port = (Port)ports.Ports[actionLine.Start.Port.Key];
                        }
                    }

                    if (actionLine.End.Shape != null)
                    {
                        if (!actions.ContainsKey(actionLine.End.Shape.Key))
                        {
                            newShape = (Shape)actionLine.End.Shape.Clone();
                            newShape.ActionElement = actionLine.End.Shape;
                            newShape.SetKey(actionLine.End.Shape.Key);
                            newShape.Visible = false;
                            hidden.Add(newShape);
                            line.End.Shape = newShape;
                        }
                        else
                        {
                            line.End.Shape = (Shape)actions[actionLine.End.Shape.Key];
                        }
                    }
                    else if (actionLine.End.Port != null)
                    {
                        Element parent = (Element)actionLine.End.Port.Parent;

                        if (!actions.ContainsKey(parent.Key))
                        {
                            newElement = (Element)parent.Clone();
                            newElement.ActionElement = parent;
                            newElement.SetKey(parent.Key);
                            newElement.Visible = false;
                            hidden.Add(newElement);

                            ports = (IPortContainer)newElement;
                            line.End.Port = (Port)ports.Ports[actionLine.End.Port.Key];
                        }
                        else
                        {
                            ports = (IPortContainer)actions[parent.Key];
                            line.End.Port = (Port)ports.Ports[actionLine.End.Port.Key];
                        }
                    }
                }
            }

            //Add any hidden shapes to the actions renderlist
            if (hidden.Count > 0) actions.AddRange(hidden);
        }

        protected internal virtual void UpdateInteractiveElements(Diagram diagram)
        {
            if (diagram.ControlRender.Actions == null) return;

            //Add the elements to the shapes or lines collections and update as usual
            foreach (Element element in diagram.ControlRender.Actions)
            {
                if (element.Visible)
                {
                    //Reset temporary layer so that the element can be added normally
                    element.ActionElement.SetLayer(null);

                    //Determine the correct container from the starting object
                    if (element is Link)
                    {
                        Link line = (Link)element;;

                        if (Controller.CanAdd(element)) Model.Lines.Add(Model.Lines.CreateKey(), (Link)element.ActionElement);
                    }

                    if (element is Shape && Controller.CanAdd(element)) Model.Shapes.Add(Model.Shapes.CreateKey(), (Shape)element.ActionElement);

                }
            }
        }

        private void HideActionElements(ElementList actions)
        {
            foreach (Element element in actions)
            {
                if (element.ActionElement.Visible && element.Visible)
                {
                    element.ActionElement.Visible = false;
                }
            }
        }

        private void ShowActionElements(ElementList actions)
        {
            foreach (Element element in actions)
            {
                if (!element.ActionElement.Visible && element.Visible)
                {
                    element.ActionElement.Visible = true;
                }
            }
        }

        protected virtual void CancelAction()
        {
            //Reset highlights
            ControlRender.Highlights = new ElementList(true);

            if (CurrentMouseElements.MouseStartElement == null)
            {
                //Reset the selection rectangle decoration
                if (!ControlRender.SelectionRectangle.IsEmpty)
                {
                    ControlRender.SelectionRectangle = new Rectangle();
                    OnCancelDragSelect();
                }
            }
            else
            {
                //Moves the elements according to the renderlist
                if (ControlRender.Actions != null)
                {
                    //Unhides the action elements
                    if (Singleton.Instance.HideActions) ShowActionElements(ControlRender.Actions);

                    Invalidate();

                    OnCancelActions(ControlRender.Actions);
                    ControlRender.Actions = null;
                    ControlRender.Feedback = null;
                    ControlRender.FeedbackLocation = new Point();
                    ControlRender.Vectors = new RectangleF[] {};
                    Render.Unlock();
                    
                    _command = null;
                }
            }

            Cursor = Singleton.Instance.GetCursor(HandleType.None);
            CancelTooltip();
            Invalidate();
            _handled = true;
            
        }

        //Return an aligned screen point
        protected virtual Point AlignMouseCoordinates(MouseEventArgs e)
        {
            Point point = new Point();
            SizeF gridSize = GridSize;
            Size zoomSize = new Size(Convert.ToInt32(gridSize.Width * Render.ScaleFactor), Convert.ToInt32(gridSize.Height * Render.ScaleFactor));

            if (zoomSize.Width < 1) zoomSize.Width = 1;
            if (zoomSize.Height < 1) zoomSize.Height = 1;

            point = Point.Round(new PointF(e.X / zoomSize.Width, e.Y / zoomSize.Height));
            point = new Point(point.X * zoomSize.Width, point.Y * zoomSize.Height);

            return point;
        }

        //Determines if an element is alignable
        private bool IsAlignable(Element element)
        {
            //Always true of align grid is on
            if (AlignGrid) return true;

            //If doesnt implement the IUserInteractive interface then false
            if (!(element is IUserInteractive)) return false;

            //Check the multiple flag enumeration to see if align to grid is set
            IUserInteractive interact = (IUserInteractive)element;
            return ((interact.Interaction & UserInteraction.AlignToGrid) == UserInteraction.AlignToGrid);
        }

        private bool IsOrderable(Element element)
        {
            //If doesnt implement the IUserInteractive interface then false
            if (!(element is IUserInteractive)) return false;

            //Check the multiple flag enumeration to see if alibring to front is set
            IUserInteractive interact = (IUserInteractive)element;
            return ((interact.Interaction & UserInteraction.BringToFront) == UserInteraction.BringToFront);
        }

        private void CheckDragScroll()
        {
            //Only drag scroll if moving elements
            if (CurrentMouseElements.MouseHandle == null) return;
            if (Render == null) return;
            if (CurrentMouseElements.MouseHandle.Type != HandleType.Move) return;

            SaveStatus();
            SetStatus(Status.DragScroll);

            Point autoScrollPoint = new Point(AutoScrollPosition.X * -1, AutoScrollPosition.Y * -1);
            int dx = 0;
            int dy = 0;

            if (CurrentMouseElements.LastPoint.X > Width) dx = Singleton.Instance.DragScrollAmount;
            if (CurrentMouseElements.LastPoint.X < 0 && autoScrollPoint.X > 0) dx = -Singleton.Instance.DragScrollAmount;
            if (CurrentMouseElements.LastPoint.Y > Height) dy = Singleton.Instance.DragScrollAmount;
            if (CurrentMouseElements.LastPoint.Y < 0 && autoScrollPoint.Y > 0) dy = -Singleton.Instance.DragScrollAmount;

            if (dx != 0 || dy != 0)
            {
                //Prevent control from repainting
                Suspend();

                //Get the renderer
                ControlRender render = (ControlRender) Render;

                //Unlock the renderer so that the elements can be re-rendered
                render.Unlock();

                //Offset all the action elements by the amount of the drag scroll
                //~~debug (should include dxzoom, dyroom)
                TranslateCommand command = Controller.CommandFactory.CreateTranslateCommand();

                command.MouseElements = CurrentMouseElements;
                command.Dx = dx * Render.ZoomFactor; //distance cursor has moved
                command.Dy = dy * Render.ZoomFactor;
                command.Elements = ControlRender.Actions;

                Controller.ExecuteCommand(command);

                //Update any recalculated vectors
                ControlRender.Vectors = command.Vectors;

                //Offset the scrollable control base autoscrollposition by the drag scroll
                autoScrollPoint.X += dx;
                autoScrollPoint.Y += dy;
                base.AutoScrollPosition = autoScrollPoint;

                //Update the internal render rectangles and re-render
                //Must not render decoration path
                //Initial scroll rectangle must equal invalidate rectangle so that not destroyed by renderer
                render.DrawDecorations = false;
                base.SetScrollRectangles();
                Resume();
                Invalidate();
                render.DrawDecorations = true;
                render.Lock();

                //Allow the control to paint itself and repaint
                Invalidate();
            }

            RestoreStatus();
        }

        private void SetTooltip(Element element)
        {
            CancelTooltip();

            if (element.Tooltip != null && element.Tooltip != string.Empty)
            {
                _tooltip = new ToolTip();
                _tooltip.SetToolTip(this, element.Tooltip);
                _tooltip.Active = true;
            }
        }

        //Can be called via reflection to remove a tooltip
        private void CancelTooltip()
        {
            if (_tooltip != null)
            {
                _tooltip.Active = false;
                _tooltip.Dispose();
                _tooltip = null;
            }
        }

        private PointF OffsetDrop(PointF point, RectangleF rectangle)
        {
            PointF offset = PointToDiagram(Convert.ToInt32(point.X), Convert.ToInt32(point.Y));
            return new PointF(offset.X - (rectangle.Width / 2), offset.Y - (rectangle.Height / 2));
        }

        private void AlignElementLocations(ElementList actions)
        {
            foreach (Element element in actions)
            {
                if (element is Shape)
                {
                    Shape shape = (Shape)element;
                    PointF location = shape.Location;
                    SizeF gridSize = GridSize;

                    Point newLocation = Point.Round(new PointF(location.X / gridSize.Width, location.Y / gridSize.Height));
                    shape.Location = new PointF(newLocation.X * gridSize.Width, newLocation.Y * gridSize.Height);
                }
            }
        }

        private void SetScaleStartingPoint()
        {
            if (CurrentMouseElements.MouseStartElement == null || !(CurrentMouseElements.MouseStartElement is Shape)) return;

            Element element = CurrentMouseElements.MouseStartElement;

            //Convert the rectangle to a screen rectangle
            Point topleft = DiagramToPoint(element.Bounds.Location);
            Point bottomright = DiagramToPoint(element.Bounds.Right, element.Bounds.Bottom);

            //X Coordinates
            if (CurrentMouseElements.MouseHandle.Type == HandleType.TopLeft || CurrentMouseElements.MouseHandle.Type == HandleType.Left || CurrentMouseElements.MouseHandle.Type == HandleType.BottomLeft)
            {
                CurrentMouseElements.StartPoint = new Point(topleft.X, CurrentMouseElements.StartPoint.Y);
            }
            else if (CurrentMouseElements.MouseHandle.Type == HandleType.TopRight || CurrentMouseElements.MouseHandle.Type == HandleType.Right || CurrentMouseElements.MouseHandle.Type == HandleType.BottomRight)
            {
                CurrentMouseElements.StartPoint = new Point(bottomright.X, CurrentMouseElements.StartPoint.Y);
            }

            //Y coordinates
            if (CurrentMouseElements.MouseHandle.Type == HandleType.TopLeft || CurrentMouseElements.MouseHandle.Type == HandleType.Top || CurrentMouseElements.MouseHandle.Type == HandleType.TopRight)
            {
                CurrentMouseElements.StartPoint = new Point(CurrentMouseElements.StartPoint.X, topleft.Y);
            }
            else if (CurrentMouseElements.MouseHandle.Type == HandleType.BottomLeft || CurrentMouseElements.MouseHandle.Type == HandleType.Bottom || CurrentMouseElements.MouseHandle.Type == HandleType.BottomRight)
            {
                CurrentMouseElements.StartPoint = new Point(CurrentMouseElements.StartPoint.X, bottomright.Y);
            }

            //Align to grid
            Point newLocation = Point.Round(new PointF(CurrentMouseElements.StartPoint.X / GridSize.Width, CurrentMouseElements.StartPoint.Y / GridSize.Height));
            CurrentMouseElements.StartPoint = Point.Round(new PointF(newLocation.X * GridSize.Width, newLocation.Y * GridSize.Height));
        }

        private void RedrawConnectors()
        {
            foreach (Line line in Model.Lines.Values)
            {
                if (line is Connector)
                {
                    Connector connector = (Connector)line;
                    if (connector.Jump) connector.DrawPath();
                }
            }
        }

        private void ZoomModel(bool zoomIn)
        {
            if (zoomIn)
            {
                if (Zoom < 200) Zoom = Convert.ToInt32(((Zoom + 25) / 25)) * 25;
            }
            else
            {
                if (Zoom > 25) Zoom = Convert.ToInt32(((Zoom - 25) / 25)) * 25;
            }
        }

        private int DecimalsFromUnit(DiagramUnit unit)
        {
            if (unit == DiagramUnit.Pixel || unit == DiagramUnit.Display || unit == DiagramUnit.Point) return 1;
            if (unit == DiagramUnit.Millimeter) return 1;
            if (unit == DiagramUnit.Document) return 1;
            if (unit == DiagramUnit.Inch) return 2;

            return 1;
        }

        #endregion
    }
}
