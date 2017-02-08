using Crainiate.Diagramming;
using DrawShapes.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestingLast.Nodes
{
    public class ConnectorNode : Crainiate.Diagramming.OnShapeClickListener
    {
        BaseNode startNode;
        BaseNode endNode;
        Connector connector = new Connector();
        bool selectable = true;
        public Connector Connector
        {
            get
            {
                return connector;
            }
            set
            {
                connector = value;
            }
        }

        public BaseNode StartNode
        {
            get
            {
                return startNode;
            }
            set {
                connector.Start.Shape = value.connectedShape();
                startNode = value;
            }
           
        }

        public BaseNode EndNode
        {
            get
            {
                return endNode;
            }

            set
            {
                endNode = value;
                connector.End.Shape = value.connectedShape();
                
            }
        }

        public bool Selectable
        {
            get
            {
                return selectable;
            }

            set
            {
                selectable = value;
            }
        }

        public void setEndNode(BaseNode endNode) {
            this.endNode = endNode;
        }
        public ConnectorNode(BaseNode startNode) {
            this.startNode = startNode;
            connector.Start.Shape = startNode.Shape;
            connector.OnShapeSelectedListener = this;
            connector.End.Marker = new Arrow();
            // connector.Start.Marker = new Marker(MarkerStyle.Ellipse);
            connector.AllowMove = true; 
            connector.DrawSelected=true;
           // connector.Rounded = true;
        }
        

        public void onShapeClicked()
        {
            if (!connector.Selected || !selectable || Form1.openDialogs==false)
                return;
            addNewNode(getPickedNode());
            connector.Selected = false;
            Form1.openDialogs = false;
        }

        public void addNewNode(BaseNode toAttachNode)
        {
             
            if (toAttachNode != null)
            {
                if (checkIfHolderExist() != null)
                {
                    toAttachNode.ParentNode = (checkIfHolderExist()).ParentNode;
                    toAttachNode.ParentNode.attachNode(toAttachNode, this);
                }

                else
                {
                    toAttachNode.ParentNode = startNode.ParentNode;
                    startNode.attachNode(toAttachNode);

                }
                toAttachNode.addToModel();
            }
        }

        private HolderNode checkIfHolderExist()
        {
            if (startNode is HolderNode)
                return (HolderNode)startNode;
            BaseNode testNode = endNode;
            while (testNode.OutConnector.EndNode != null) {
                if (testNode is HolderNode)
                    return (HolderNode)testNode;
                testNode = testNode.OutConnector.EndNode;
            }
            return null;
        }

        public static BaseNode getPickedNode()
        {
            BaseNode toAttachNode = null;
            PickDialog pd = new PickDialog();
            DialogResult res = pd.ShowDialog();

            int selectedShape = pd.getSelectedShape();
            if (selectedShape == 0)
            {
                toAttachNode = new DeclareNode();
            }
            else if (selectedShape == 1)
            {
                toAttachNode = new AssignNode();
            }

            else if (selectedShape == 2)//"whileImg"
                toAttachNode = new WhileNode();
            else if (selectedShape == 3)//"forImg"
                toAttachNode = new ForNode();

            else if (selectedShape == 4)//"doImg"
                toAttachNode = new DoNode();
            else if (selectedShape == 5)//"ifImg"
                toAttachNode = new IfNode();
            else if (selectedShape == 6)//"inputImg"
                toAttachNode = new InputNode();
            else if (selectedShape == 7)// "outputImg"
                toAttachNode = new OutputNode();
            else if (selectedShape == 8)
                toAttachNode = new IfElseNode();
            return toAttachNode;
        }
    }
}
