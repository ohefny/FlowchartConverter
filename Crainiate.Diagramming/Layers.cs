// (c) Copyright Crainiate Software 2010




using System;
using System.Runtime.Serialization;

using Crainiate.Diagramming.Collections;

namespace Crainiate.Diagramming
{
	public class Layers: List<Layer>
	{
		//Property variables
		private Layer _currentLayer = null;
        private Model _model;

		#region Interface

		public Layers(Model model): base()
		{
            //Add default layer
            Layer layer = new Layer(true);
            layer.Name = "Default";
            Add(layer);
            CurrentLayer = layer;

            SetModel(model);
		}

        //Parameterless constructor to be called during deserialization
        protected internal Layers(): base()
        {

        }

        public virtual Model Model
        {
            get
            {
                return _model;
            }
        }

		//Sets or gets the current Layer
		public virtual Layer CurrentLayer
		{
			get 
			{
				return _currentLayer;
			}
			set
			{
				if (value == null) throw new ArgumentNullException("CurrentLayer","Layer cannot be null reference.");
				if (! this.Contains(value)) throw new ArgumentException("Layer not found in collection.","CurrentLayer");
                SetCurrentLayer(value);
			}
		}
		
		//Methods
		//Moves all shape references to the target layer and removes the source layer
		public virtual void Collapse(Layer source, Layer target)
		{
            if (source.Default) throw new LayerException("Default layer cannot be collapsed.");
            MoveElementLayer(source, target);
            Remove(source);
		}

		public virtual void MoveElement(Element element, Layer source, Layer target)
		{
            element.SetLayer(target);
		}

        public void SetCurrentLayer(Layer layer)
        {
            _currentLayer = layer;
        }

        public void SetModel(Model model)
        {
            _model = model;
        }

        private void MoveElementLayer(Layer source, Layer target)
        {
            foreach (Element element in Model.Elements)
            {
                if (element.Layer == source) element.SetLayer(target);
            }
        }

		#endregion
	}
}