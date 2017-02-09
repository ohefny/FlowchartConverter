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
            if (Shape.Selected && Controller.DeleteChoosed)
            {
                removeFromModel();
                Controller.DeleteChoosed = false;

            }
            else if(Shape.Selected)
            {
                OutputBox od = new OutputBox();
                DialogResult dr = od.ShowDialog();

                if (dr == DialogResult.OK)
                {
                    Statement = od.getExpression();
                    setText("Print " + Statement);
                  //  Statement = surrondExpression(Statement);
                }
                Shape.Selected = false;
            }
            Shape.Selected = false;
        }
        private String surrondExpression(String str)
        {
            return "cout<< " + str + " ;";


        }
        public OutputNode()
        {
            Name = "Output";
            Shape.StencilItem = Stencil[FlowchartStencilType.Data];
            Shape.StencilItem.BackColor = System.Drawing.Color.Green;
            Shape.StencilItem.GradientColor = Color.Black;
            Shape.Label = new Crainiate.Diagramming.Label("Output");

        }
    }
}
