// (c) Copyright Crainiate Software 2010

using System;
using System.Collections.Generic;
using System.Text;

namespace Crainiate.Diagramming
{
    //Command is part of the Command pattern
    public abstract class Command: AbstractCommand
    {
        private Controller _controller;

        //Constructors
        public Command(Controller controller): base()
        {
            if (controller == null) throw new ArgumentNullException("controller");
            _controller = controller;
        }

        //Properties
        public Controller Controller
        {
            get
            {
                return _controller;
            }
        }

        //Methods
        public abstract void Undo();
        public abstract void Redo();
    }
}
