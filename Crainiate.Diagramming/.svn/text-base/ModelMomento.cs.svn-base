// (c) Copyright Crainiate Software 2010

using System;
using System.Collections.Generic;
using System.Text;

namespace Crainiate.Diagramming
{
    public class ModelMomento: IMomento<Model>
    {
        private Model _model;
        private Model _newModel;

        public ModelMomento(Model model)
        {
            _model = model;
        }

        public Model CreateItem()
        {
            _newModel = new Model();
            _newModel.SetSize(_model.Size);

            foreach (var shape in _model.Shapes.Values)
            {
                _newModel.Shapes.Add(shape.Key, shape);
            }

            foreach (var line in _model.Lines.Values)
            {
                _newModel.Lines.Add(line.Key, line);
            }

            return _newModel;
        }

        public void WriteItem(Model item)
        {
            item.Clear();
            item.SetSize(_newModel.Size);

            foreach (var shape in _newModel.Shapes.Values)
            {
                item.Shapes.Add(shape.Key, shape);
            }

            foreach (var line in _newModel.Lines.Values)
            {
                item.Lines.Add(line.Key, line);
            }
        }
    }
}
