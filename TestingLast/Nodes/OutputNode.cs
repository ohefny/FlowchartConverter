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
            base.onShapeClicked();
            if (Shape.Selected)
            {
                OutputBox od = new OutputBox();
                DialogResult dr = od.ShowDialog();

                if (dr == DialogResult.OK)
                {
                    Statement = od.getExpression();
                    
                  //  Statement = surrondExpression(Statement);
                }
                Shape.Selected = false;
            }
            Shape.Selected = false;
        }
        
        protected override void showStatment()
        {
            base.setText("Print " + Statement);
        }
        public OutputNode()
        {
            Name = "Output";
            Shape.StencilItem = Stencil[FlowchartStencilType.Data];
            Shape.StencilItem.BackColor = System.Drawing.Color.Green;
            Shape.StencilItem.GradientColor = Color.Black;
            setText("Output");

        }
    }
}
