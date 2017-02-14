using Crainiate.Diagramming.Flowcharting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crainiate.Diagramming;
using System.Windows.Forms;
using FlowchartConverter.Dialogs;

namespace FlowchartConverter.Nodes
{
    class DoNode : DecisionNode
    {
        
        HolderNode startNode;
        public override void onShapeClicked()
        {
            if (Shape.Selected && Controller.DeleteChoosed)
            {

                removeFromModel();
                Controller.DeleteChoosed = false;
                Shape.Selected = false;

            }
            if (Shape.Selected)
            {
                //AssignmentDialog db = new AssignmentDialog();
                DoDialog doWhileBox = new DoDialog();

                //doWhileBox.setExpression(extractExpression(Statement));
                DialogResult dr = doWhileBox.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    Statement = doWhileBox.LoopExpression;

                    setText(Statement);
                    
                   // Statement = surrondExpression(Statement);
                    //setText(Statement);       
                    //Shape.Label = new Crainiate.Diagramming.Label(Statement);
                }
                //MessageBox.Show();
                
            }
            Shape.Selected = false;



        }

        private string surrondExpression(string str)
        {
            return "while ( " + str + " );";
        }

        private string extractExpression(string str)
        {
            if (String.IsNullOrEmpty(str))
                return str;
            
            string res = str.Remove(0, 8);
            res = res.Remove(res.Count() - 2);
            return res;
        }

        public DoNode() {
            //  startNode = new HolderNode(this);
            //Shape.Label = new Crainiate.Diagramming.Label("Do");
            Name = "DoWhile";
            setText("Do While");
           // Statement = "false";
          
        }
    
        protected override void makeConnections()
        {
            startNode = new HolderNode(this);
            TrueConnector = new ConnectorNode(this);
            TrueConnector.Connector.Opacity = 50;
            TrueNode = new HolderNode(this);
            TrueConnector.EndNode = startNode;
            startNode.OutConnector.EndNode = TrueNode;
            TrueConnector.Connector.Label = new Crainiate.Diagramming.Label("True");
            OutConnector.Connector.Label = new Crainiate.Diagramming.Label("False");
            BackNode = new HolderNode(this);
            BackNode.OutConnector.Connector.End.Shape = Shape;
            BackNode.Shape.Label = new Crainiate.Diagramming.Label("B");
            TrueNode.OutConnector.EndNode = BackNode;
            BackNode.OutConnector.setEndNode(this);
            BackNode.OutConnector.Selectable = false;
            TrueConnector.Selectable = false;
            startNode.OutConnector.Selectable = false;
            BackNode.OutConnector.Connector.Opacity = 50;
            startNode.OutConnector.Connector.Opacity = 50;
            TrueConnector.Connector.Opacity = 50;   
        }
        protected override void moveConnections()
        {   
            Shape.Location = new PointF(nodeLocation.X, nodeLocation.Y + shiftY);
            PointF point = new PointF(Shape.Width + Shape.Location.X + horizontalSpace, startNode.Shape.Center.Y - TrueNode.Shape.Size.Height / 2);
            PointF oldPlace = TrueNode.NodeLocation;
            TrueNode.NodeLocation = point;
            // backNode.NodeLocation = new PointF(point.X, point.Y + 100);
            if (TrueConnector.EndNode == null)
            {
                TrueConnector.EndNode = TrueNode;
                //this.OutConnector.EndNode.shiftDown();
                TrueNode.attachNode(BackNode);
                return;
                //      holderNode.attachNode(this, backConnector);
            }
            BackNode.NodeLocation = new PointF(point.X, BackNode.NodeLocation.Y);
            if (moveDirection == MOVE_DOWN)
                TrueNode.OutConnector.EndNode.shiftDown(moreShift);
            else if (moveDirection == MOVE_UP)
            {
                TrueNode.OutConnector.EndNode.shiftUp(oldPlace.Y - point.Y);
                //   OutConnector.EndNode.shiftUp(); //shift main track
            }
           /* if (TrueNode.OutConnector.EndNode is HolderNode)
            {
                BackNode.NodeLocation = new PointF(point.X, point.Y + 60);
                shiftMainTrack();
            }

            else
            {
                TrueNode.OutConnector.EndNode.shiftDown(moreShift);
                
            }*/
        }
        public override Shape connectedShape()
        {
            return startNode.Shape;
        }
        override public void addToModel()
        {
            Controller.Model.Shapes.Add(Shape);

            Controller.Model.Shapes.Add(startNode.Shape);
            Controller.Model.Lines.Add(BackNode.OutConnector.Connector);
            Controller.Model.Lines.Add(startNode.OutConnector.Connector);
            base.addToModel();
            
            
        }
        public override void shiftMainTrack(int moreShift=0)
        {
            Shape.Location = new PointF(Shape.Location.X ,BackNode.Shape.Center.Y- Shape.Size.Height / 2);
            if (OutConnector.EndNode != null)
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
