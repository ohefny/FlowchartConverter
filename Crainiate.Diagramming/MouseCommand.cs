// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

namespace Crainiate.Diagramming
{
    public abstract class MouseCommand: AbstractCommand
    {
        private MouseCommandButtons _mouseButtons;
        private IMouseEvents _element;
        private PointF _location;
        private bool _handled;

        public MouseCommand(IMouseEvents element)
        {
            Element = element;
        }

        public virtual MouseCommandButtons MouseButtons
        {
            get
            {
                return _mouseButtons;
            }
            set
            {
                _mouseButtons = value;
            }
        }

        public virtual IMouseEvents Element
        {
            get
            {
                return _element;
            }
            set
            {
                if (value == null) throw new ArgumentNullException();
                _element = value;
            }
        }

        public virtual PointF Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }

        public virtual bool Handled
        {
            get
            {
                return _handled;
            }
            set
            {
                _handled = value;
            }
        }

        public override void Execute()
        {
            Element.ExecuteMouseCommand(this);
        }
    }

    public class MouseDownCommand : MouseCommand
    {
        public MouseDownCommand(IMouseEvents element):base(element)
        {
        }
    }

    public class MouseMoveCommand : MouseCommand
    {
        public MouseMoveCommand(IMouseEvents element): base(element)
        {
        }
    }

    public class MouseUpCommand : MouseCommand
    {
        public MouseUpCommand(IMouseEvents element): base(element)
        {
        }
    }
}
