using Crainiate.Diagramming;
using Crainiate.Diagramming.Flowcharting;
using DrawShapes.Dialogs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestingLast.Nodes
{
    class WhileNode : LoopNode
    {
        
        public WhileNode()
        {

            
            Shape.Label = new Crainiate.Diagramming.Label("While");
            //setText("true tur true tur true tur true tur");
           

        }

        public override void shiftMainTrack()
        {
            OutConnector.EndNode.shiftDown(moreShift);
        }

        protected override void makeConnections()
        {
            trueNode = new HolderNode(this);
            trueConnector = new ConnectorNode(this);
            trueConnector.Connector.Opacity = 50;
            // trueConnector.Connector.Forward
            trueConnector.Connector.Label = new Crainiate.Diagramming.Label("True");
            OutConnector.Connector.Label = new Crainiate.Diagramming.Label("False");
            backNode = new HolderNode(this);
            backNode.OutConnector.EndNode = this;
            backNode.OutConnector.Connector.Opacity = 50;
            trueNode.OutConnector.EndNode = backNode;
            trueConnector.Selectable = false;
            backNode.OutConnector.Selectable = false;

        }

        protected override void moveConnections()
        {
            PointF point = new PointF(Shape.Width + Shape.Location.X + 100, Shape.Center.Y - trueNode.Shape.Size.Height / 2);
            trueNode.NodeLocation = point;
            // backNode.NodeLocation = new PointF(point.X, point.Y + 100);
            if (trueConnector.EndNode == null)
            {
                trueConnector.EndNode = trueNode;
                //this.OutConnector.EndNode.shiftDown();
                trueNode.attachNode(backNode);
                return;
                //      holderNode.attachNode(this, backConnector);
            }
            if (trueNode.OutConnector.EndNode is HolderNode)
            {
                backNode.NodeLocation = new PointF(point.X, point.Y + 100);
            }
            else
                trueNode.OutConnector.EndNode.shiftDown(moreShift);
            // trueNode.NodeLocation = point;
        }
    }
}


