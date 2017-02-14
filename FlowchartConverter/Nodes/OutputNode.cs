using Crainiate.Diagramming.Flowcharting;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlowchartConverter.Dialogs;

namespace FlowchartConverter.Nodes
{
    class OutputNode : BaseNode
    {
        public override void onShapeClicked()
        {
            base.onShapeClicked();
            if (Shape.Selected)
            {
                OutputDialog od = new OutputDialog();
                DialogResult dr = od.ShowDialog();

                if (dr == DialogResult.OK)
                {
                    Statement = od.OutputExpression;
                    
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
