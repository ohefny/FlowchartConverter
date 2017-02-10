using Crainiate.Diagramming.Flowcharting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingLast.Nodes
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
            this.moreShift = moreShift ;
            if (OutConnector.EndNode != ParentNode)
                base.shiftDown(moreShift);
            else
            {
                NodeLocation = new PointF(NodeLocation.X, NodeLocation.Y + shiftY+ moreShift);
                if (ParentNode is DecisionNode) {

                    if (ParentNode.OutConnector.EndNode!=null&& ParentNode.OutConnector.EndNode.NodeLocation.Y < NodeLocation.Y) 
                        ((DecisionNode)ParentNode).shiftMainTrack();
                
                //this shifts down main track even if it will be shifted
                }
            }


        }
    }
}
