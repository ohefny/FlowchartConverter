using Crainiate.Diagramming.Flowcharting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowchartConverter.Nodes
{
    class HolderNode : BaseNode
    {
        static int hCounter=1;
        
        public HolderNode(BaseNode parentNode) {
            this.ParentNode = parentNode;
            Shape.StencilItem = Stencil[FlowchartStencilType.Connector];
            Shape.MinimumSize = new System.Drawing.SizeF(15, 15);
            
            Shape.BackColor = System.Drawing.Color.White;
            Shape.Size = new System.Drawing.SizeF(15, 15);
            Shape.GradientColor = System.Drawing.Color.White;
            ShapeTag = "Shape_Holder" + hCounter.ToString();
            hCounter++;
        }

        

        public override void onShapeClicked()
        {
           // ParentNode.onShapeClicked();
        }
        
        public override void shiftDown(float moreShift)
        {
                NodeLocation = new PointF(NodeLocation.X, NodeLocation.Y + shiftY+ moreShift);
                if (this.ParentNode is IfElseNode) //to balance Holder Nodes
                    (this.ParentNode as IfElseNode).balanceHolderNodes();

                //if the next node in main track has smaller y shift it and nodes next to it
                else if (ParentNode.OutConnector.EndNode.NodeLocation.Y < NodeLocation.Y+shiftY)
                    ((DecisionNode)ParentNode).shiftMainTrack();

                 
                
            


        }
    }
}
