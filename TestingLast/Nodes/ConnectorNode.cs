using Crainiate.Diagramming;

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
        private static Controller controller;
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

        public static Controller Controller
        {
            get
            {
                return controller;
            }

            set
            {
                controller = value;
            }
        }

        public void setEndNode(BaseNode endNode) {
            this.endNode = endNode;
        }
        public ConnectorNode(BaseNode startNode) {
            if (Controller == null)
                throw new Exception("Controller must be set to use Connectors");
            this.startNode = startNode;
            connector.Start.Shape = startNode.Shape;
            connector.OnShapeSelectedListener = this;
            connector.End.Marker = new Arrow();
            // connector.Start.Marker = new Marker(MarkerStyle.Ellipse);
            connector.AllowMove = true;
            connector.Avoid = connector.Jump = false;
            connector.SetOrder(1);
            connector.DrawSelected=true;
           // connector.Rounded = true;
        }
        

        public void onShapeClicked()
        {
            if (!connector.Selected || !selectable || Controller.AllowMove==true)
                return;
            BaseNode node = getPickedNode();
            if (node == null)
                return;
            addNewNode(node);
            connector.Selected = false;
            
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
            PickerDialog pd = new PickerDialog();
            DialogResult res = pd.ShowDialog();
            if (res != DialogResult.OK)
                return null;
            toAttachNode = pd.SelectedShape;
            
            return toAttachNode;
        }
    }
}
