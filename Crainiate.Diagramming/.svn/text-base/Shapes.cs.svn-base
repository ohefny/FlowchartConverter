// (c) Copyright Crainiate Software 2010

using System;
using Crainiate.Diagramming.Collections;

namespace Crainiate.Diagramming
{
    public class Shapes: Elements<Shape>
    {
        public Shapes(Model model): base(model)
        {
        }

        //For deserialization only
        internal Shapes(): base()
        {

        }
    
        protected internal override void ElementInsert(Shape shape)
        {
            base.ElementInsert(shape);

            //Set container and layers for children of complex shape
            if (shape is ComplexShape)
            {
                ComplexShape complex = shape as ComplexShape;

                foreach (Element child in complex.Children.Values)
                {
                    child.SetModel(Model);
                    child.SetLayer(Model.Layers.CurrentLayer);
                }
            }

            //Set the height of the table
            //Proposed Removal due to issue 4374. 
            //Final revision 
            if (shape is Table)
            {
                Table table = shape as Table;
                table.SetHeight();
            }
        }
    }
}
