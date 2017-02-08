using Crainiate.Diagramming;
using Crainiate.Diagramming.Flowcharting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace TestingLast.Nodes
{

    public abstract class BaseNode : Crainiate.Diagramming.OnShapeClickListener
    {
        static Form1 form;
        public static List<BaseNode> nodes = new List<BaseNode>();      
        static Model model;
        private ConnectorNode outConnector;
        Form dialog;
        String statement;
        Shape shape;
        BaseNode parentNode;
        FlowchartStencil stencil;
        protected String shapeTag;
        String connectorTag;
        static int counter;
        protected int shiftY = 85;
        protected PointF nodeLocation;//= new PointF(0, 0);
        protected float moreShift = 0;
        public Form Dialog
        {
            get
            {
                return dialog;
            }

            set
            {
                dialog = value;
            }
        }

        public string Statement
        {
            get
            {
                return statement;
            }

            set
            {
                statement = value;
            }
        }

        public virtual Shape connectedShape()
        {
            return Shape;
        }

        public Shape Shape
        {
            get
            {
                return shape;
            }

            /*  set
              {
                  shape = value;
              } */
        }

        public FlowchartStencil Stencil
        {
            get
            {
                return stencil;
            }

            set
            {
                stencil = value;
            }
        }

        public static Model Model
        {
            get
            {
                return model;
            }

            set
            {
                model = value;
            }
        }

        virtual public PointF NodeLocation
        {
            get
            {
                return nodeLocation;
            }

            set
            {
                nodeLocation = value;
                connectedShape().Location = value;
            }
        }

        public ConnectorNode OutConnector
        {
            get
            {
                return outConnector;
            }

            set
            {
                outConnector = value;
            }
        }

        public static Form1 Form
        {
            get
            {
                return form;
            }

            set
            {
                form = value;
            }
        }

        public string Name { get; internal set; }

        public BaseNode ParentNode
        {
            get
            {
                return parentNode;
            }

            set
            {
                parentNode = value;
            }
        }

        public BaseNode()
        {
            shape = new Shape();
            Shape.Label = new Crainiate.Diagramming.Label();
            Shape.Label.Color = Color.White;
            Shape.KeepAspect = false;
            shape.ShapeListener = this;
            Shape.AllowMove = Shape.AllowScale = Shape.AllowRotate = Shape.AllowSnap = false;
            Shape.Size = new SizeF(80, 50);
            Shape.KeepAspect = false;
            Stencil = (FlowchartStencil)Singleton.Instance.GetStencil(typeof(FlowchartStencil));
            Shape.Label.Color = Color.White;
            OutConnector = new ConnectorNode(this);
            counter++;
            shapeTag = "Shape_" + counter.ToString();
            connectorTag = "Connector_" + counter.ToString();
           
            
        }
        virtual public void setText(String label) {
            Shape.Label = new Crainiate.Diagramming.Label(label);
            SizeF size;
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                size = g.MeasureString(label, Shape.Label.Font);
            }
            // SizeF size =TextRenderer.MeasureText("Input SAFSAF ASFSAFS ASSAFFS ", Singleton.Instance.DefaultFont);
            Shape.Size = new SizeF(size.Width + 70, Shape.Size.Height);
            Shape.Label.Color = Color.White;
        }
        virtual public void addToModel()
        {
            if (model == null)
            {
                throw new Exception("Model should be set before calling addToModel");
            }
            if (OutConnector.EndNode != null)
                model.Lines.Add(connectorTag, OutConnector.Connector);
            if (Shape != null)
                model.Shapes.Add(shapeTag, shape);

            if (!nodes.Contains(this))
            {
                nodes.Add(this);
                if (this is IfElseNode && NodeLocation.X < 100)
                    shiftRight(100,null);
            }
        }
        virtual public void removeFromModel()
        {
            if (model == null)
            {
                throw new Exception("Model should be set before calling addToModel");
            }
            nodes.Remove(this);
            foreach (BaseNode node in nodes)
            {
               if (node.outConnector.EndNode == this && node.OutConnector.EndNode!=node.parentNode) //problem for backnode
                {
                    node.outConnector.EndNode = outConnector.EndNode;
                    if (this is DecisionNode)
                        node.outConnector.EndNode.shiftUp(this.NodeLocation.Y);
                    else
                        node.outConnector.EndNode.shiftUp();
                }
            }
            redrawNodes();

        }

        protected void redrawNodes()
        {
            
            model.Clear();

            foreach (BaseNode node in nodes)
            {
                if (node is HolderNode) continue;
                node.addToModel();

            }
        }


        public abstract void onShapeClicked();
        public void attachNode(BaseNode newNode)
        {

              if (this is TerminalNode && newNode is TerminalNode ||
                  this is HolderNode && newNode is HolderNode)
              {
                  if (newNode.connectedShape() == null)
                  {
                      //do nothing
                  }
                  if (this.connectedShape() == null)
                  {
                      //donothing
                  }
                  OutConnector.EndNode = newNode;
                  newNode.NodeLocation = this.NodeLocation;
                  newNode.shiftDown(0);
                  return;

              }
           
            BaseNode oldOutNode = OutConnector.EndNode;
            OutConnector.EndNode = newNode;
            newNode.OutConnector.EndNode = oldOutNode;
          //  if (this.NodeLocation.X == oldOutNode.NodeLocation.X)
            {
               
                newNode.NodeLocation = oldOutNode.NodeLocation;
                oldOutNode.shiftDown(0);
                if (newNode is DecisionNode)
                {
                    balanceParentTrack(newNode);
                }
                if (newNode is IfElseNode) {
                    balanceParentTrack(newNode as IfElseNode);
                }
            }
            
        }

        private void balanceParentTrack(BaseNode newNode)
        {
            BaseNode trackNode = null;
            do {
                if (trackNode == null)
                    trackNode = newNode.ParentNode;
                else
                    trackNode = trackNode.ParentNode;

                if (trackNode.NodeLocation.X > newNode.NodeLocation.X
                    && (newNode as DecisionNode).TrueNode.NodeLocation.X > trackNode.NodeLocation.X)
                    shiftRight(200,trackNode.ParentNode);
            }
            while (!(trackNode is TerminalNode));
        }
        private void balanceParentTrack(IfElseNode newNode) {
            BaseNode trackNode = null;
            do
            {
                if (trackNode == null)
                    trackNode = newNode.ParentNode;
                else
                    trackNode = trackNode.ParentNode;

                if (trackNode.NodeLocation.X < newNode.NodeLocation.X
                    && (newNode).FalseNode.NodeLocation.X < trackNode.NodeLocation.X)
                {
                    shiftRight(150, newNode.ParentNode.ParentNode);
                    nodes.Add(newNode);
                    shiftRight(150, newNode.ParentNode);
                }
            }
            while (!(trackNode is TerminalNode));
        }

        private void shiftRight(int distance,BaseNode trackNode)
        {
            foreach (BaseNode node in nodes)
            {
                
                if((!(node is HolderNode)) && (trackNode==null||node.ParentNode==trackNode))
                    node.NodeLocation = new PointF(node.NodeLocation.X + distance, node.NodeLocation.Y);
            }
        }

        public virtual void attachNode(BaseNode newNode, ConnectorNode connectorNode)
        {
            attachNode(newNode);

        }
       
        virtual public void shiftDown(float moreShift)
        {
            this.moreShift = moreShift;
            //moreShift = 0;
            if (connectedShape() != null)
                NodeLocation = new PointF(connectedShape().Location.X, connectedShape().Location.Y + shiftY+moreShift);
            
            if (!(this is HolderNode) && OutConnector.EndNode != null)
                OutConnector.EndNode.shiftDown(moreShift);
        }
        public void shiftUp()
        {
            if(connectedShape()!=null)
                NodeLocation = new PointF(connectedShape().Location.X, connectedShape().Location.Y - shiftY + moreShift);
            /*  if (!(this is HolderNode) && OutConnector.EndNode != null)
              {
                  OutConnector.EndNode.shiftUp();
                  if(this.ParentNode is DecisionNode){
                      (this.ParentNode as DecisionNode).BackNode.shiftUp();
                      this.ParentNode.OutConnector.EndNode.shiftUp();
                  }
              }*/
            if (OutConnector.EndNode == null)
                return;
            if (this is HolderNode) //what about middleNode shift
            {
                //shifting middle node 
                if (this.ParentNode is IfNode)
                {

                    IfNode pNode = (this.ParentNode as IfNode);
                    pNode.MiddleNode.NodeLocation = new PointF(pNode.MiddleNode.connectedShape().Location.X, pNode.BackNode.NodeLocation.Y);

                } //to decide shifting middle node or not
                else if (this.ParentNode is IfElseNode)
                {
                    IfElseNode pNode = this.ParentNode as IfElseNode;
                    PointF preLocation = pNode.MiddleNode.NodeLocation;
                    pNode.balanceMiddleNode();
                    if (pNode.MiddleNode.NodeLocation.Y == preLocation.Y)
                        return; //thus don't shift the node after parent node
                }
                //shift the node after parentnode
                this.ParentNode.OutConnector.EndNode.shiftUp();
            }
            else
            {
                OutConnector.EndNode.shiftUp();
            }
        }
        public void shiftUp(float offsetY)
        {
            float tempOffset = this.NodeLocation.Y;
            if (connectedShape() != null)
                NodeLocation = new PointF(nodeLocation.X, offsetY);
            if (OutConnector.EndNode == null)
                return;
            if (this is HolderNode) //what about middleNode shift
            { 
                //shifting middle node 
                if (this.ParentNode is IfNode)
                {

                    IfNode pNode = (this.ParentNode as IfNode);
                    pNode.MiddleNode.NodeLocation = new PointF(pNode.MiddleNode.connectedShape().Location.X, pNode.BackNode.NodeLocation.Y);

                } //to decide shifting middle node or not
                else if (this.ParentNode is IfElseNode)
                {
                    IfElseNode pNode = this.ParentNode as IfElseNode;
                    PointF preLocation = pNode.MiddleNode.NodeLocation;
                    pNode.balanceMiddleNode();
                    if (pNode.MiddleNode.NodeLocation.Y == preLocation.Y)
                        return; //thus don't shift the node after parent node
                } 
               //shift the node after parentnode
                this.ParentNode.OutConnector.EndNode.shiftUp(offsetY+shiftY);
            }
            else {
                OutConnector.EndNode.shiftUp(offsetY + shiftY);
            }
        }
    }
}
