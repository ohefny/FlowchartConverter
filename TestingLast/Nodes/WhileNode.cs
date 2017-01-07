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
    class WhileNode : DecisionNode
    {
        
        public WhileNode()
        {

            
            //Shape.Label = new Crainiate.Diagramming.Label("While");
            setText("While");
            Statement = "while(false)";

        }

        public override void shiftMainTrack()
        {
            OutConnector.EndNode.shiftDown(moreShift);
        }

        protected override void makeConnections()
        {
            TrueNode = new HolderNode(this);
            trueConnector = new ConnectorNode(this);
            trueConnector.Connector.Opacity = 50;
            // trueConnector.Connector.Forward
            trueConnector.Connector.Label = new Crainiate.Diagramming.Label("True");
            OutConnector.Connector.Label = new Crainiate.Diagramming.Label("False");
            BackNode = new HolderNode(this);
            BackNode.OutConnector.EndNode = this;
            BackNode.OutConnector.Connector.Opacity = 50;
            TrueNode.OutConnector.EndNode = BackNode;
            trueConnector.Selectable = false;
            BackNode.OutConnector.Selectable = false;
            BackNode.Shape.Label = new Crainiate.Diagramming.Label("B");
            
        }

        protected override void moveConnections()
        {
            PointF point = new PointF(Shape.Width + Shape.Location.X + 100, Shape.Center.Y - TrueNode.Shape.Size.Height / 2);
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
            }
            else
                TrueNode.OutConnector.EndNode.shiftDown(moreShift);
            // trueNode.NodeLocation = point;
        }
        public override void onShapeClicked()
        {
           
           // DialogResult dr = whileBox.ShowDialog();
            if (Shape.Selected)
            {
                //AssignmentDialog db = new AssignmentDialog();
                WhileBox whileBox = new WhileBox();

                whileBox.setExpression(extractExpression(Statement));

                
                DialogResult dr = whileBox.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    Statement = whileBox.getExpression();
                    Statement = surrondExpression(Statement);
                    //setText(Statement);       
                    //Shape.Label = new Crainiate.Diagramming.Label(Statement);
                }
                //MessageBox.Show();
                Shape.Selected = false;
            }
        }

        private string surrondExpression(string str)
        {
            return "while ( " + str + " )";
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


