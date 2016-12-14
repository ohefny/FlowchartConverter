// (c) Copyright Crainiate Software 2010

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

namespace Crainiate.Diagramming
{
    public class ModelCommand: Command 
    {
        //Property variables
        private ModelMomento _modelMomento;

        //Constructors
        public ModelCommand(Model model, Controller controller): base(controller)
        {
            _modelMomento = new ModelMomento(model);
        }

        //Properties
        public virtual ModelMomento ModelMomento
        {
            get
            {
                return _modelMomento;
            }
        }

        //Methods
        public override void Execute()
        {
            _modelMomento.CreateItem();
        }

        public override void Undo()
        {
            UndoRedo();
        }

        public override void Redo()
        {
            UndoRedo();
        }

        private void UndoRedo()
        {
            _modelMomento.WriteItem(Controller.Model);
        }
    }
}
