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
    class AssignNode : BaseNode
    {
        public override void onShapeClicked()
        {
            if (Shape.Selected)
            {
                AssignmentDialog db = new AssignmentDialog();
                DialogResult dr = db.ShowDialog();
                Shape.Selected = false;
            }
        }
        public AssignNode()
        {
            Shape.StencilItem = Stencil[FlowchartStencilType.Default];
            Shape.BackColor = System.Drawing.ColorTranslator.FromHtml("#fdfd80");
            Shape.GradientColor = Color.Black;
            Shape.Label = new Crainiate.Diagramming.Label("Assing");
            Shape.Label.Color = Color.White;
            Statement = " x = 15 ";
        }
    }
}
