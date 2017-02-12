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
    class IfNode : WhileNode
    {  
        private HolderNode middleNode;
        
        public HolderNode MiddleNode
        {
            get
            {
                return middleNode;
            }

            set
            {
                middleNode = value;
            }
        }

      

        public override void onShapeClicked()
        {
            base.onShapeClicked();
            if (Shape.Selected)
            {
                //AssignmentDialog db = new AssignmentDialog();
                IfBox ifBox = new IfBox();
                if (!String.IsNullOrEmpty(Statement)) {
                  // ifBox.setExpression(extractExpression(Statement)); 
                    
                }
                DialogResult dr = ifBox.ShowDialog();
                if (dr == DialogResult.OK) {
                    Statement = ifBox.getExpression();
                   // Statement = surrondExpression(Statement);
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
        private String extractExpression(String str) {
            if (String.IsNullOrEmpty(str))
                return str;
            String res=str.Remove(0,5);
            res = res.Remove(res.Count() - 1);
            return res;
        }
       /* public bool isEmptyElse() {
            return FalseNode.OutConnector.EndNode == BackfalseNode;
        }*/
        protected override void makeConnections()
        {
            MiddleNode = new HolderNode(this);
            
            MiddleNode.Shape.Label = new Crainiate.Diagramming.Label("Done");
            ///////////////////truepart
            TrueNode = new HolderNode(this);
            TrueNode.Shape.Label = new Crainiate.Diagramming.Label("Start IF");
            TrueConnector = new ConnectorNode(this);
            TrueConnector.Connector.Opacity = 50;
            TrueConnector.Connector.Label = new Crainiate.Diagramming.Label("True");
            BackNode = new HolderNode(this);
            BackNode.Shape.Label = new Crainiate.Diagramming.Label("End IF");
            BackNode.OutConnector.EndNode = this;
            BackNode.OutConnector.Connector.End.Shape = MiddleNode.Shape;
            BackNode.OutConnector.Connector.Opacity = 50;        
            TrueNode.OutConnector.EndNode = BackNode;
            TrueConnector.Selectable = false;
            BackNode.OutConnector.Selectable = false;
            BackNode.OutConnector.Connector.Label = new Crainiate.Diagramming.Label("Done");
           
        }

        protected override void moveConnections()
        {
            //move middle Node
            MiddleNode.NodeLocation = new PointF(Shape.Center.X - MiddleNode.Shape.Width / 2, Shape.Center.Y - MiddleNode.Shape.Size.Height / 2);
            base.moveConnections();
            MiddleNode.NodeLocation = new PointF(MiddleNode.NodeLocation.X,BackNode.NodeLocation.Y);
            if (OutConnector.EndNode != null && OutConnector.EndNode.NodeLocation.Y < MiddleNode.NodeLocation.Y + shiftY)
                shiftMainTrack();
        }

       
        public override void attachNode(BaseNode newNode, ConnectorNode clickedConnector)
        {
            clickedConnector.StartNode.attachNode(newNode);
            MiddleNode.NodeLocation = new PointF(MiddleNode.NodeLocation.X,BackNode.NodeLocation.Y);
            if (OutConnector.EndNode == null)
                return;
            if (OutConnector.EndNode.NodeLocation.Y < BackNode.NodeLocation.Y)
            {
                
                shiftMainTrack();
                
            }

            //balanceMiddleNode();
        }

       

        public IfNode()
        {
            Name = "If";
            Shape.StencilItem = Stencil[FlowchartStencilType.Decision];
            Shape.BackColor = System.Drawing.ColorTranslator.FromHtml("#c04040");
            Shape.GradientColor = Color.Black;
            //Statement = "false";
            setText("IF");
            
           
        }
        public override void addToModel()
        {
            base.addToModel();
            MiddleNode.addToModel();
            
           
        }

    }
}
