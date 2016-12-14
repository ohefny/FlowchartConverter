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
    abstract class LoopNode :BaseNode
    {
        protected HolderNode trueNode;
        protected HolderNode backNode;
        protected ConnectorNode trueConnector;
        // ConnectorNode backConnector;
        readonly private string holderTag;
        readonly private string backConnectorTag;
        readonly private string trueConnectorTag;
        public bool shifted = false;
        public override PointF NodeLocation
        {
            get
            {
                return nodeLocation;
            }

            set
            {
                if (value.X != NodeLocation.X || value.Y != NodeLocation.Y)
                {
                    base.NodeLocation = value;
                    moveConnections();
                }
            }
        }



        public override void onShapeClicked()
        {
            WhileBox od = new WhileBox();
            DialogResult dr = od.ShowDialog();
        }
        public LoopNode()
        {

            Shape.StencilItem = Stencil[FlowchartStencilType.Preparation];
            Shape.BackColor = System.Drawing.ColorTranslator.FromHtml("#e06000");
            Shape.GradientColor = Color.Black;
           

            holderTag = shapeTag + " holder";
            backConnectorTag = shapeTag + " backConnector";
            trueConnectorTag = shapeTag + " trueConnector";
          
                       
            makeConnections();

        }

        abstract protected void makeConnections();
       
        abstract protected void moveConnections();
        
        override public void addToModel()
        {
            base.addToModel();
            Model.Lines.Add(trueConnectorTag, trueConnector.Connector);
            trueNode.addToModel();
            backNode.addToModel();
            //  Model.Shapes.Add(holderTag,holderNode.Shape);
            //  Model.Lines.Add(backConnectorTag, backConnector.Connector);
        }
        protected void attachToTrue(BaseNode newNode, bool addToEnd)
        {
            if (addToEnd) //this means that the clicked link is between holder and loop
            {
                //add this node to last node in true link
                BaseNode lastNode = trueNode;
                while (!(lastNode.OutConnector.EndNode is HolderNode))
                {
                    lastNode = lastNode.OutConnector.EndNode;

                }
                lastNode.attachNode(newNode);
            }
            else
                trueNode.attachNode(newNode);


        }
        /*    public override void shiftDown()
            {
                if (!shifted)
                {
                    shifted = true;
                    base.shiftDown();
                    trueNode.shiftDown();

                }
            }
        */

        public override void attachNode(BaseNode newNode, ConnectorNode clickedConnector)
        {
           
            clickedConnector.StartNode.attachNode(newNode);
            if (OutConnector.EndNode.NodeLocation.Y < backNode.NodeLocation.Y)
                shiftMainTrack(); //this causes a problem when 
                              //backNode shifts dirctely after being attached to another while node

        }

        public abstract void shiftMainTrack();
       
    }

}
