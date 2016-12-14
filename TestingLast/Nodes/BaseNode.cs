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

    abstract class BaseNode : Crainiate.Diagramming.OnShapeClickListener
    {
        public static List<BaseNode> nodes = new List<BaseNode>();      
        static Model model;
        ConnectorNode outConnector;
        Form dialog;
        String statement;
        Shape shape;
        FlowchartStencil stencil;
        protected String shapeTag;
        String connectorTag;
        static int counter;
        protected int shiftY = 90;
        protected PointF nodeLocation;
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

        }
        virtual public void addToModel()
        {
            if (model == null)
            {
                throw new Exception("Model should be set before calling addToModel");
            }
            if (Shape != null)
                model.Shapes.Add(shapeTag, shape);
            if (OutConnector.EndNode != null)
                model.Lines.Add(connectorTag, OutConnector.Connector);
         //   nodes.Add(this);
        }
        public void removeFromModel(Model model)
        {
            model.Shapes.Remove(shapeTag);
            model.Lines.Remove(connectorTag);
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
            if (this.NodeLocation.X == oldOutNode.NodeLocation.X)
            {
              
                newNode.NodeLocation = oldOutNode.NodeLocation;
                if (newNode is LoopNode)
                    oldOutNode.shiftDown(0);
                else
                    oldOutNode.shiftDown(0);
             //   newNode.OutConnector.Connector.CalculateRoute();
             //   oldOutNode.OutConnector.Connector.CalculateRoute();
             //   this.OutConnector.Connector.CalculateRoute();
               
            }
          /*  else
            {
                newNode.NodeLocation = this.NodeLocation;
                
                //MessageBox.Show(newNode.NodeLocation, this.NodeLocation);
                newNode.shiftDown();
                newNode.OutConnector.Connector.CalculateRoute();
                newNode.OutConnector.Connector.Invalidate();
                //    MessageBox.Show(newNode.NodeLocation.ToString()+" "+ this.NodeLocation.ToString());
            }*/
            //newNode.addToModel();

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
            
            if (OutConnector.EndNode != null)
                OutConnector.EndNode.shiftDown(moreShift);
        }
        public void shiftUp()
        {

        }

    }
}
