using Crainiate.Diagramming.Flowcharting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingLast.Nodes
{
    class TerminalNode : BaseNode
    {
        public  enum TerminalType {Start,End}
        public override void onShapeClicked()
        {
        }
        public TerminalNode(TerminalType termType) {
            Shape.StencilItem = Stencil[FlowchartStencilType.Terminator];
            Shape.BackColor =Color.Magenta;
            Shape.StencilItem.GradientColor = Color.Black;
            //Shape.Size = new SizeF(40, 40);
            if (termType == TerminalType.Start) {
                setText("Start");
                Name = "Start";
               // Shape.Label = new Crainiate.Diagramming.Label("Start");
                NodeLocation = new PointF(80, 10);
                ParentNode = this;
                
            }
            else {
                setText("End");
                Name = "End";
                //  Shape.Label = new Crainiate.Diagramming.Label("End");
            }

        }
    }
}
