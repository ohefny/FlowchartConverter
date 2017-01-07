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
    class OutputNode : BaseNode
    {
        public override void onShapeClicked()
        {
            if (Shape.Selected)
            {
                AssignmentDialog db = new AssignmentDialog();
                OutputBox od = new OutputBox();
                DialogResult dr = od.ShowDialog();
                Shape.Selected = false;
            }
        }
        public OutputNode()
        {
            Shape.StencilItem = Stencil[FlowchartStencilType.Data];
            Shape.StencilItem.BackColor =System.Drawing.ColorTranslator.FromHtml("#00a040");
            Shape.StencilItem.GradientColor = Color.Black;
            Shape.Label = new Crainiate.Diagramming.Label("Output");
            Statement = "cout<<x";
        }
    }
}
