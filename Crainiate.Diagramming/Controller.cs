// (c) Copyright Crainiate Software 2010

using System;
using System.Drawing;

namespace Crainiate.Diagramming
{
	[Serializable]
	public class Controller
	{
		//Property variables
        private Views _views;
        private Model _model;
        private ClipboardCommand _command;
        private Clipboard _clipboard;
        private Factory _factory;
        private CommandFactory _commandFactory;

        private CommandStack _undoStack;
        private CommandStack _redoStack;

		private bool _cut;
		private bool _copy;
		private bool _paste;
		private bool _delete;
		private bool _undo;
		private bool _redo;

		private bool _roundPixels;
        private bool _checkBounds;

		//Constructors
		public Controller(Model model)
		{
            Model = model;
            Factory = new Factory();
            CommandFactory = new CommandFactory(this);
            
            _views = new Views();
            _clipboard = new Clipboard();
            _checkBounds = true;
            _undoStack = new CommandStack();
            _redoStack = new CommandStack();
            
			AllowCut = true;
			AllowCopy = true;
			AllowPaste = true;
			AllowDelete = true;
			AllowUndo = true;
			AllowRedo = true;
			RoundPixels = true;
		}

        public virtual Views Views
        {
            get
            {
                return _views;
            }
        }

        public virtual Model Model
        {
            get
            {
                return _model;
            }
            set
            {
                if (value == null) throw new ArgumentNullException();
                _model = value;

                //Loop through each view and set the model reference
                if (Views != null)
                {
                    foreach (IView view in Views)
                    {
                        view.SetModel(value);
                    }
                }
            }
        }

        public virtual CommandStack UndoStack
        {
            get
            {
                return _undoStack;
            }
        }

        public virtual CommandStack RedoStack
        {
            get
            {
                return _redoStack;
            }
        }   

        public Clipboard Clipboard
        {
            get
            {
                return _clipboard;
            }
        }

		public virtual bool AllowCut
		{
			get
			{
				return _cut;
			}
			set
			{
				_cut = value;
			}
		}

		public virtual bool AllowCopy
		{
			get
			{
				return _copy;
			}
			set
			{
				_copy = value;
			}
		}

		public virtual bool AllowPaste
		{
			get
			{
				return _paste;
			}
			set
			{
				_paste = value;
			}
		}

		public virtual bool AllowDelete
		{
			get
			{
				return _delete;
			}
			set
			{
				_delete = value;
			}
		}

		public virtual bool AllowUndo
		{
			get
			{
				return _undo;
			}
			set
			{
				_undo = value;
			}
		}

		public virtual bool AllowRedo
		{
			get
			{
				return _redo;
			}
			set
			{
				_redo = value;
			}
		}

		public virtual bool RoundPixels
		{
			get
			{
				return _roundPixels;
			}
			set
			{
				_roundPixels = value;
			}
		}

        public virtual bool CheckBounds
        {
            get
            {
                return _checkBounds;
            }
            set
            {
                _checkBounds = value;
            }
        }

        //Used to create elements at runtime. Follows the Factory design pattern
        public virtual Factory Factory
        {
            get
            {
                return _factory;
            }
            set
            {
                if (value == null) throw new ArgumentNullException();
                _factory = value;
            }
        }

        //Creates commands at runtime. Follows the Factory design pattern
        public virtual CommandFactory CommandFactory
        {
            get
            {
                return _commandFactory;
            }
            set
            {
                if (value == null) throw new ArgumentNullException();
                _commandFactory = value;
            }
        }

		//Methods
        //Execute the comand and push it onto the top of the undo stack
        public virtual void ExecuteCommand(Command command)
        {
            if (command == null) return;
            command.Execute();
            if (AllowUndo) UndoStack.Push(command);
        }

        //Pop the top command off the undo stack, execute it and push it onto the redo stack.
        public virtual void UndoCommand()
        {
            if (!AllowUndo || UndoStack.Count == 0) return;
            Command command = UndoStack.Pop();
            command.Undo();

            RedoStack.Push(command);
        }

        //Pop the command off the redo stack and process it back through ExecuteCommand
        public virtual void RedoCommand()
        {
            if (!AllowRedo || RedoStack.Count == 0) return;
            Command command = RedoStack.Pop();
            command.Redo();

            if (AllowUndo) UndoStack.Push(command);
        }

        public virtual Element CloneElement(Element element)
        {
            Element newElement = (Element) element.Clone();
            newElement.ActionElement = element;
            newElement.SetKey(element.Key);
            newElement.SetModel(element.Model);

            //Set the action shapes for the complex shape children
            if (element is ComplexShape)
            {
                ComplexShape complex = (ComplexShape)element;
                ComplexShape newComplex = (ComplexShape)newElement;

                foreach (Solid solid in newComplex.Children.Values)
                {
                    solid.ActionElement = complex.Children[solid.Key];
                }
            }

            //Keep size of table
            if (element is Table)
            {
                Table table = (Table)element;
                Table newTable = (Table)newElement;

                newTable.Size = table.Size;
            }

            return newElement;
        }

        public virtual void Suspend()
        {
            foreach (IView view in Views)
            {
                view.Suspend();
            }
        }

        public virtual void Resume()
        {
            foreach (IView view in Views)
            {
                view.Resume();
            }
        }

        public virtual void Invalidate()
        {
            foreach (IView view in Views)
            {
                view.Invalidate();
            }
        }

        public virtual void Refresh()
        {
            foreach (IView view in Views)
            {
                view.Refresh();
            }
        }

        //Sets all elements selected to true/false
        public void SelectElements(bool selection)
        {
            Suspend();
            
            foreach (Element element in Model.Elements)
            {
                if (element is ISelectable && element.Layer == Model.Layers.CurrentLayer)
                {
                    ISelectable selectable = (ISelectable)element;
                    selectable.Selected = selection;
                }
            }

            Resume();
            Invalidate();
        }

        //Selects elements from a rectangle
        public virtual void SelectElements(RectangleF diagramRectangle)
        {
            foreach (Element element in Model.Elements)
            {
                if (element is ISelectable)
                {
                    ISelectable selectable = (ISelectable)element;
                    selectable.Selected = (element.Intersects(diagramRectangle) && element.Layer == Model.Layers.CurrentLayer);
                }
            }
        }

		//Determine if an element can dock
		public virtual bool CanDock(InteractiveMode interactiveMode, MouseElements mouseElements)
		{
			if (interactiveMode == InteractiveMode.Normal)
			{
                //Check is shape or port
				if (mouseElements.MouseMoveElement is Shape || mouseElements.MouseMoveElement is Port)
				{
					//return false if permission not available to dock
					if (mouseElements.MouseStartOrigin != null)
					{
						Origin origin = mouseElements.MouseStartOrigin;
						Link line = (Link) mouseElements.MouseStartElement;
				
						if (mouseElements.MouseMoveElement is Shape)
						{
							Shape shape = (Shape) mouseElements.MouseMoveElement;
							if (shape.Direction == Direction.None) return false;
							if (origin == line.Start && shape.Direction== Direction.In) return false;
							if (origin == line.End && shape.Direction == Direction.Out) return false;
						}
				
						if (mouseElements.MouseMoveElement is Port)
						{
							Port port = (Port) mouseElements.MouseMoveElement;
							if (port.Direction == Direction.None) return false;
							if (origin == line.Start && port.Direction == Direction.In) return false;
							if (origin == line.End && port.Direction == Direction.Out) return false;
						}
					}
			

					return true;
				}
			}
			//Can dock for interactive elements
			else
			{
				if (mouseElements.InteractiveElement is Link)
				{
					Link line = (Link) mouseElements.InteractiveElement;
				
					//Determine if applies to start or end origin
					if (mouseElements.InteractiveOrigin == line.Start)
					{
						if (mouseElements.MouseMoveElement is Shape)
						{
							Shape shape = (Shape) mouseElements.MouseMoveElement;
							if (shape.Direction == Direction.None) return false;
							if (shape.Direction== Direction.In) return false;
						}

						if (mouseElements.MouseMoveElement is Port)
						{
							Port port = (Port) mouseElements.MouseMoveElement;
							if (port.Direction == Direction.None) return false;
							if (port.Direction == Direction.In) return false;
						}
					}
					else
					{
						if (mouseElements.MouseMoveElement is Shape)
						{
							Shape shape = (Shape) mouseElements.MouseMoveElement;
							if (shape.Direction == Direction.None) return false;
							if (shape.Direction == Direction.Out) return false;
						}

						if (mouseElements.MouseMoveElement is Port)
						{
							Port port = (Port) mouseElements.MouseMoveElement;
							if (port.Direction == Direction.None) return false;
							if (port.Direction == Direction.Out) return false;
						}
					}

					return true;
				}
			}
			
			return false;
		}

		//Determines if an element can be added interactively
		public virtual bool CanAdd(Element element)
		{
			return true;
		}

		public virtual bool CanDelete(Element element)
		{
			return true;
		}

        public virtual PointF SnapToLocation(PointF location, SizeF size, TransformCommand action)
        {
            Nullable<float> bestX = null;
            Nullable<float> bestY = null;
            Nullable<RectangleF> verticalVector = null;
            Nullable<RectangleF> horizontalVector = null;
            PointF bestLocation = new PointF();

            foreach (Shape shape in Model.Shapes.Values)
            {
                if (shape.UpdateElement == null)
                {
                    //Left
                    float diff = Math.Abs(shape.X - location.X);
                    if (diff <= 9)
                    {
                        if (!bestX.HasValue || diff < bestX.Value)
                        {
                            bestX = diff;
                            bestLocation.X = shape.X;
                            verticalVector = new RectangleF(shape.X, 0, 0, Model.Size.Height);
                        }
                    }

                    //Right
                    diff = Math.Abs(shape.Bounds.Right - location.X - size.Width);
                    if (diff <= 9)
                    {
                        if (!bestX.HasValue || diff < bestX.Value)
                        {
                            bestX = diff;
                            bestLocation.X = shape.Bounds.Right - size.Width;
                            verticalVector = new RectangleF(shape.Bounds.Right, 0, 0, Model.Size.Height);
                        }
                    }

                    //Top
                    diff = Math.Abs(shape.Y - location.Y);
                    if (diff <= 9)
                    {
                        if (!bestY.HasValue || diff < bestY.Value)
                        {
                            bestY = diff;
                            bestLocation.Y = shape.Y;
                            horizontalVector = new RectangleF(0, shape.Y, Model.Size.Width, 0);
                        }
                    }

                    //Bottom
                    diff = Math.Abs(shape.Bounds.Bottom - location.Y - size.Height);
                    if (diff <= 9)
                    {
                        if (!bestY.HasValue || diff < bestY.Value)
                        {
                            bestY = diff;
                            bestLocation.Y = shape.Bounds.Bottom - size.Height;
                            horizontalVector = new RectangleF(0, shape.Bounds.Bottom, Model.Size.Width, 0);
                        }
                    }
                }
            }

            if (bestX.HasValue) location = new PointF(bestLocation.X, location.Y);
            if (bestY.HasValue) location = new PointF(location.X, bestLocation.Y);
            
            //Set the vectors
            if (verticalVector.HasValue)
            {
                if (horizontalVector.HasValue)
                {
                    action.SetVectors(new RectangleF[] { verticalVector.Value, horizontalVector.Value });
                }
                else
                {
                    action.SetVectors(new RectangleF[] { verticalVector.Value });
                }
            }
            else
            {
                if (horizontalVector.HasValue) action.SetVectors(new RectangleF[] {horizontalVector.Value });
            }
            return location;
        }

        public virtual SizeF BoundsCheck(ElementList actions, SizeF delta)
        {
            //Return pass if the diagram does not have boundary checking
            if (!CheckBounds) return delta;

            foreach (Element element in actions)
            {
                delta = BoundsCheck(element, delta);
                if (delta.IsEmpty) return delta;
            }
            return delta;
        }

        //Bounds checking by element
        public virtual SizeF BoundsCheck(Element element, SizeF delta)
        {
            //Return pass if the controller does not have boundary checking or the delta is empty
            if (!CheckBounds) return delta;
            if (delta.IsEmpty) return delta;

            Element action = element.ActionElement;
            bool contains = false;

            //If the original (action) is contained in the container
            contains = (Model.Contains(new PointF(action.Bounds.X  + delta.Width, action.Bounds.Y + delta.Height))); // && container.Contains(new PointF(action.Rectangle.Right + container.Offset.X + delta.Width, action.Rectangle.Bottom + container.Offset.Y + delta.Height), container.Margin));
            contains = contains && (Model.Contains(new PointF(action.Bounds.Right + delta.Width, action.Bounds.Bottom + delta.Height)));

            if (contains) return delta;

            return SizeF.Empty;
        }

        //Bounds checking by point
        public virtual bool BoundsCheck(PointF action, float dx, float dy)
        {
            //Return pass if the diagram does not have boundary checking
            if (!CheckBounds) return true;

            //If the original (action) is contained in the container
            if (Model.Contains(new PointF(action.X, action.Y)))
            {
                //Check top left
                if (!Model.Contains(new PointF(action.X + dx, action.Y + dy))) return false;
            }

            return true;
        }
	}
}
