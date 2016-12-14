using Crainiate.Diagramming.Flowcharting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crainiate.Diagramming;

namespace TestingLast.Nodes
{
    class DoNode : LoopNode
    {
        HolderNode startNode;
        public override void onShapeClicked()
        {
            throw new NotImplementedException();
        }
        public DoNode() {
          //  startNode = new HolderNode(this);
            Shape.Label = new Crainiate.Diagramming.Label("Do");
            
            
        }
        protected override void makeConnections()
        {
            startNode = new HolderNode(this);
            trueConnector = new ConnectorNode(this);
            trueConnector.Connector.Opacity = 50;
            trueNode = new HolderNode(this);
            trueConnector.EndNode = startNode;
            startNode.OutConnector.EndNode = trueNode;
            trueConnector.Connector.Label = new Crainiate.Diagramming.Label("True");
            OutConnector.Connector.Label = new Crainiate.Diagramming.Label("False");
            backNode = new HolderNode(this);
            backNode.OutConnector.Connector.End.Shape = Shape;
            trueNode.attachNode(backNode);
            backNode.OutConnector.setEndNode(this);
            backNode.OutConnector.Selectable = false;
            trueConnector.Selectable = false;
            startNode.OutConnector.Selectable = false;
            backNode.OutConnector.Connector.Opacity = 50;
            startNode.OutConnector.Connector.Opacity = 50;
            trueConnector.Connector.Opacity = 50;   
        }
        protected override void moveConnections()
        {
           
            
            Shape.Location = new PointF(nodeLocation.X, nodeLocation.Y + shiftY);
            PointF point = new PointF(Shape.Width + Shape.Location.X + 100, startNode.Shape.Center.Y - trueNode.Shape.Size.Height / 2);
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
                shiftMainTrack();
            }

            else
            {
                trueNode.OutConnector.EndNode.shiftDown(moreShift);
                
            }
        }
        public override Shape connectedShape()
        {
            return startNode.Shape;
        }
        override public void addToModel()
        {
            Model.Shapes.Add(Shape);
           
            Model.Shapes.Add(startNode.Shape);
           Model.Lines.Add(backNode.OutConnector.Connector);
            Model.Lines.Add(startNode.OutConnector.Connector);
            base.addToModel();
            
            
        }
        public override void shiftMainTrack()
        {
            Shape.Location = new PointF(Shape.Location.X ,backNode.Shape.Center.Y- Shape.Size.Height / 2);
            OutConnector.EndNode.shiftDown(moreShift);
        }
        public override void attachNode(BaseNode newNode, ConnectorNode clickedConnector) {
            
            if (clickedConnector == OutConnector)
            {
                base.attachNode(newNode, clickedConnector);
            }
            else if (clickedConnector.StartNode == backNode)
            {
                attachToTrue(newNode, true);
            }
            else if (clickedConnector.StartNode == startNode )
                attachToTrue(newNode, false);
            else
                clickedConnector.StartNode.attachNode(newNode);
            shiftMainTrack();
        }
    }
}
