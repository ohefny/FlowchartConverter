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
    class DeclareNode : BaseNode
    {
        public override void onShapeClicked()
        {
            if (Shape.Selected)
            {
                DeclareBox db = new DeclareBox();
                DialogResult dr = db.ShowDialog();
                Shape.Selected = false;
            }
        }
        public DeclareNode()
        {
            Shape.StencilItem = Stencil[FlowchartStencilType.InternalStorage];
            Shape.BackColor = System.Drawing.ColorTranslator.FromHtml("#e3810c");
            Shape.GradientColor = Color.White;
            Shape.Label = new Crainiate.Diagramming.Label("Declare");
            Statement = "int x";

        }
    }
}
