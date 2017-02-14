using Crainiate.Diagramming;
using Crainiate.Diagramming.Flowcharting;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlowchartConverter.Dialogs;

namespace FlowchartConverter.Nodes
{
    class WhileNode : DecisionNode
    {
        
        public WhileNode()
        {

            Name = "While";
            setText("While");

        }

        public override void shiftMainTrack(int moreShift=0)
        {
            if(OutConnector.EndNode!=null)
                OutConnector.EndNode.shiftDown(moreShift);
        }

        protected override void makeConnections()
        {
            TrueNode = new HolderNode(this);
            TrueConnector = new ConnectorNode(this);
            TrueConnector.Connector.Opacity = 50;
            // trueConnector.Connector.Forward
            TrueConnector.Connector.Label = new Crainiate.Diagramming.Label("True");
            OutConnector.Connector.Label = new Crainiate.Diagramming.Label("False");
            BackNode = new HolderNode(this);
            BackNode.OutConnector.EndNode = this;
            BackNode.OutConnector.Connector.Opacity = 50;
            TrueNode.OutConnector.EndNode = BackNode;
            TrueConnector.Selectable = false;
            BackNode.OutConnector.Selectable = false;
            BackNode.Shape.Label = new Crainiate.Diagramming.Label("B");
            
        }

        protected override void moveConnections()
        {
            PointF point = new PointF(Shape.Width + Shape.Location.X + horizontalSpace, Shape.Center.Y - TrueNode.Shape.Size.Height / 2);
            PointF oldPlace = TrueNode.NodeLocation;
            TrueNode.NodeLocation = point;
            // backNode.NodeLocation = new PointF(point.X, point.Y + 100);
            if (TrueConnector.EndNode == null)
            {
                TrueConnector.EndNode = TrueNode;
                TrueNode.OutConnector.EndNode = BackNode;
              //  TrueNode.attachNode(BackNode);
               // return;
               
            }
            if (TrueNode.OutConnector.EndNode is HolderNode)
            {
                
                if (moveDirection == MOVE_UP)
                {
                    TrueNode.OutConnector.EndNode.shiftUp(oldPlace.Y - point.Y);
                    //   OutConnector.EndNode.shiftUp(); //shift main track
                }
                else
                    BackNode.NodeLocation = new PointF(point.X, point.Y + 60);
            }
            else
            {
                BackNode.NodeLocation = new PointF(point.X, BackNode.NodeLocation.Y);
                if (moveDirection==MOVE_DOWN)
                    TrueNode.OutConnector.EndNode.shiftDown(moreShift);
                else if(moveDirection==MOVE_UP)
                {
                    TrueNode.OutConnector.EndNode.shiftUp(oldPlace.Y-point.Y);
                 //   OutConnector.EndNode.shiftUp(); //shift main track
                }
            }
            
            // trueNode.NodeLocation = point;
        }
        public override void onShapeClicked()
        {
            base.onShapeClicked();
            if (Shape.Selected)
            {
                //AssignmentDialog db = new AssignmentDialog();
                WhileDialog whileBox = new WhileDialog();

              //  whileBox.setExpression(extractExpression(Statement));

                
                DialogResult dr = whileBox.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    Statement = whileBox.LoopExpression;
                    setText(Statement);
                    //Statement = surrondExpression(Statement);
                    //setText(Statement);       
                    //Shape.Label = new Crainiate.Diagramming.Label(Statement);
                }
                //MessageBox.Show();
                
            }
            Shape.Selected = false;
        }

        
        private string extractExpression(string str) {
            if (String.IsNullOrEmpty(str))
                return str;
            string res = str.Remove(0,8);
            res = res.Remove(res.Count() - 1);
            return res;
        }
    }
}


