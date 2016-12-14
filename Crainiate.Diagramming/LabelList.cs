// (c) Copyright Crainiate Software 2010




using System;
using System.Text;

using Crainiate.Diagramming.Collections;

namespace Crainiate.Diagramming
{
    public class LabelList : List<Label>
    {
        //Creates a new empty label list
        public LabelList(): base()
        {
        }

        //Creates a label list from a shapes collection
        public LabelList(Shapes shapes): base()
        {
            foreach (Shape shape in shapes.Values)
            {
                if (shape.Label != null) Add(shape.Label);
            }
        }
    }
}
