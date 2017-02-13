using Crainiate.Diagramming.Flowcharting;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestingLast.Dialogs;

namespace TestingLast.Nodes
{
    class IfElseNode : IfNode
    {
        private HolderNode falseNode;
        private HolderNode backfalseNode;
       // private HolderNode middleNode;
        private ConnectorNode falseConnector;
        private bool moveFalsePart = true;

        public HolderNode FalseNode
        {
            get
            {
                return falseNode;
            }

            set
            {
                falseNode = value;
            }
        }

        public HolderNode BackfalseNode
        {
            get
            {
                return backfalseNode;
            }

            set
            {
                backfalseNode = value;
            }
        }

      

        public ConnectorNode FalseConnector
        {
            get
            {
                return falseConnector;
            }

            set
            {
                falseConnector = value;
            }
        }

        internal bool MoveFalsePart
        {
            get
            {
                return moveFalsePart;
            }

            set
            {
                moveFalsePart = value;
            }
        }

        
        public override void addRemoveFlag(bool v)
        {
            base.addRemoveFlag(v);
            BackfalseNode.ToBeRemoved = true;
            FalseNode.ToBeRemoved = true;
            BaseNode nextNode = FalseNode;
            while (nextNode.OutConnector.EndNode != BackfalseNode)
            {
                nextNode.OutConnector.EndNode.addRemoveFlag(true);
                nextNode = nextNode.OutConnector.EndNode;
            }
            nextNode = null;
            ToBeRemoved = true;

        }
        public override void onShapeClicked()
        {
            base.onShapeClicked();
            if (Shape.Selected)
            {
                
                IfDialog ifBox = new IfDialog();
               /* if (!String.IsNullOrEmpty(Statement))
                {
                    ifBox.setExpression(extractExpression(Statement));

                }*/
                DialogResult dr = ifBox.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    Statement = ifBox.DecisionExpression;
                    //Statement = surrondExpression(Statement);
                    setText(Statement);       
                    //Shape.Label = new Crainiate.Diagramming.Label(Statement);
                }
                //MessageBox.Show();
                
            }
            Shape.Selected = false;
        }

        private String surrondExpression(String str)
        {
            return "if ( " + str + " )";


        }
        private String extractExpression(String str)
        {
            if (String.IsNullOrEmpty(str))
                return str;
            String res = str.Remove(0, 5);
            res = res.Remove(res.Count() - 1);
            return res;
        }
        public bool isEmptyElse()
        {
            return FalseNode.OutConnector.EndNode == BackfalseNode;
        }
        protected override void makeConnections()
        {
            
            base.makeConnections();
            this.OutConnector = MiddleNode.OutConnector;
            
            /////////////////////////////false part
            FalseNode = new HolderNode(this);
            FalseNode.Shape.Label = new Crainiate.Diagramming.Label("Start Else");
            FalseConnector = new ConnectorNode(this);
            FalseConnector.Connector.Opacity = 50;
            FalseConnector.Connector.Label = new Crainiate.Diagramming.Label("False");
            BackfalseNode = new HolderNode(this);
            BackfalseNode.Shape.Label = new Crainiate.Diagramming.Label("End Else");
            BackfalseNode.OutConnector.EndNode = this;
            BackfalseNode.OutConnector.Connector.End.Shape = MiddleNode.Shape;
            BackfalseNode.OutConnector.Connector.Opacity = 50;
            FalseNode.OutConnector.EndNode = BackfalseNode;
            FalseConnector.Selectable = false;
            BackfalseNode.OutConnector.Selectable = false;
            BackfalseNode.OutConnector.Connector.Label = new Crainiate.Diagramming.Label("Done");
        }

        protected override void moveConnections()
        {
           
            /////////////// move true part
            
            base.moveConnections();
            if (!MoveFalsePart)
                return;
            ///////////////////////////////False Part
            PointF point2 = new PointF(Shape.Location.X - horizontalSpace, TrueNode.NodeLocation.Y);
            PointF oldPlace = FalseNode.NodeLocation;
            FalseNode.NodeLocation = point2;
            // backNode.NodeLocation = new PointF(point.X, point.Y + 100);
            if (FalseConnector.EndNode == null)
            {
                FalseConnector.EndNode = FalseNode;
                //this.OutConnector.EndNode.shiftDown();
                //FalseNode.attachNode(BackfalseNode);
                FalseNode.OutConnector.EndNode = BackfalseNode;
                //      holderNode.attachNode(this, backConnector);
            }
            if (FalseNode.OutConnector.EndNode is HolderNode)
            {
                if (moveDirection == MOVE_UP)
                {
                    FalseNode.OutConnector.EndNode.shiftUp(oldPlace.Y - point2.Y);
                    //   OutConnector.EndNode.shiftUp(); //shift main track
                }
                else
                    BackfalseNode.NodeLocation = new PointF(point2.X, point2.Y + 60);
            }
            else
            {
                BackfalseNode.NodeLocation = new PointF(point2.X, BackfalseNode.NodeLocation.Y);
                if (moveDirection == MOVE_DOWN)
                    FalseNode.OutConnector.EndNode.shiftDown(moreShift);
                else if (moveDirection == MOVE_UP)
                    FalseNode.OutConnector.EndNode.shiftUp(oldPlace.Y - point2.Y);
            }
            balanceHolderNodes();
        }

        public override void attachNode(BaseNode newNode, ConnectorNode clickedConnector)
        {
            ConnectorNode temp = clickedConnector;
            while (temp.EndNode is HolderNode) {
                temp = temp.EndNode.OutConnector;
            }
            clickedConnector.StartNode.attachNode(newNode);
            if (OutConnector.EndNode == null)
                return;
            if (OutConnector.EndNode.NodeLocation.Y < BackNode.NodeLocation.Y ||
                OutConnector.EndNode.NodeLocation.Y < BackfalseNode.NodeLocation.Y)
            {

                shiftMainTrack();

            }
            
            balanceHolderNodes();
        }

        public void balanceHolderNodes()
        {  
           //choose the nodelocation with larger y
            float y = MiddleNode.NodeLocation.Y;
            MiddleNode.NodeLocation = new PointF(MiddleNode.NodeLocation.X,
                (BackfalseNode.NodeLocation.Y > BackNode.NodeLocation.Y) ?
                BackfalseNode.NodeLocation.Y : BackNode.NodeLocation.Y);
            if (OutConnector.EndNode != null && OutConnector.EndNode.NodeLocation.Y < MiddleNode.NodeLocation.Y + shiftY)
                shiftMainTrack();
            
        }

        public IfElseNode()
        {
            Name = "IfElse";
            Shape.StencilItem = Stencil[FlowchartStencilType.Decision];
            Shape.BackColor = System.Drawing.ColorTranslator.FromHtml("#c04040");
            Shape.GradientColor = Color.Black;
            
           // Statement = "false";
            setText("IF");

        }
        public override void addToModel()
        {
            base.addToModel();
            FalseNode.addToModel();
            BackfalseNode.addToModel();
            //MiddleNode.addToModel();
            //Controller.Model.Lines.Add(FalseConnector.Connector);
        }
    }
}
