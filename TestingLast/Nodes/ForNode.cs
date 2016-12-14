using Crainiate.Diagramming.Flowcharting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingLast.Nodes
{
    class ForNode : WhileNode
    {
        public override void onShapeClicked()
        {
            throw new NotImplementedException();
        }
        public ForNode()
        {
            //base();
            Shape.Label = new Crainiate.Diagramming.Label("For");
            trueConnector.Connector.Label = new Crainiate.Diagramming.Label("Next");
            OutConnector.Connector.Label = new Crainiate.Diagramming.Label("Done");
        }
    }
}
