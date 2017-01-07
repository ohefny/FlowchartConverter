using Crainiate.Diagramming.Flowcharting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crainiate.Diagramming;
using DrawShapes.Dialogs;

namespace TestingLast.Nodes
{
    class DoNode : DecisionNode
    {
        HolderNode startNode;
        public override void onShapeClicked()
        {
            DoWhileBox doWhileBox = new DoWhileBox();
            doWhileBox.ShowDialog();
        }
        public DoNode() {
            //  startNode = new HolderNode(this);
            //Shape.Label = new Crainiate.Diagramming.Label("Do");
            setText("Do While");
            Statement = "do{ \n }while(x>5)";
        }        protected override void makeConnections()
        {
            startNode = new HolderNode(this);
            trueConnector = new ConnectorNode(this);
            trueConnector.Connector.Opacity = 50;
            TrueNode = new HolderNode(this);
            trueConnector.EndNode = startNode;
            startNode.OutConnector.EndNode = TrueNode;
            trueConnector.Connector.Label = new Crainiate.Diagramming.Label("True");
            OutConnector.Connector.Label = new Crainiate.Diagramming.Label("False");
            BackNode = new HolderNode(this);
            BackNode.OutConnector.Connector.End.Shape = Shape;
            BackNode.Shape.Label = new Crainiate.Diagramming.Label("B");
            TrueNode.attachNode(BackNode);
            BackNode.OutConnector.setEndNode(this);
            BackNode.OutConnector.Selectable = false;
            trueConnector.Selectable = false;
            startNode.OutConnector.Selectable = false;
            BackNode.OutConnector.Connector.Opacity = 50;
            startNode.OutConnector.Connector.Opacity = 50;
            trueConnector.Connector.Opacity = 50;   
        }
        protected override void moveConnections()
        {
           
            
            Shape.Location = new PointF(nodeLocation.X, nodeLocation.Y + shiftY);
            PointF point = new PointF(Shape.Width + Shape.Location.X + 100, startNode.Shape.Center.Y - TrueNode.Shape.Size.Height / 2);
            TrueNode.NodeLocation = point;
            // backNode.NodeLocation = new PointF(point.X, point.Y + 100);
            if (trueConnector.EndNode == null)
            {
                trueConnector.EndNode = TrueNode;
                //this.OutConnector.EndNode.shiftDown();
                TrueNode.attachNode(BackNode);
                return;
                //      holderNode.attachNode(this, backConnector);
            }
            if (TrueNode.OutConnector.EndNode is HolderNode)
            {
                BackNode.NodeLocation = new PointF(point.X, point.Y + 100);
                shiftMainTrack();
            }

            else
            {
                TrueNode.OutConnector.EndNode.shiftDown(moreShift);
                
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
           Model.Lines.Add(BackNode.OutConnector.Connector);
            Model.Lines.Add(startNode.OutConnector.Connector);
            base.addToModel();
            
            
        }
        public override void shiftMainTrack()
        {
            Shape.Location = new PointF(Shape.Location.X ,BackNode.Shape.Center.Y- Shape.Size.Height / 2);
            OutConnector.EndNode.shiftDown(moreShift);
        }
        public override void attachNode(BaseNode newNode, ConnectorNode clickedConnector) {
            
            if (clickedConnector == OutConnector)
            {
                base.attachNode(newNode, clickedConnector);
            }
            else if (clickedConnector.StartNode == BackNode)
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
