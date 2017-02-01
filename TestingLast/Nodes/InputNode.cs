using Crainiate.Diagramming;
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
    class InputNode : BaseNode
    {
        public override void onShapeClicked()
        {
            if (Shape.Selected && Form1.deleteChoosed)
            {
                removeFromModel();
                Form1.deleteChoosed = false;

            }
            else if(Shape.Selected)
            {

                InputBox db = new InputBox();
                DialogResult dr = db.ShowDialog();
                
                if (dr == DialogResult.OK)
                {

                    Statement = db.getExpression();
                    setText("Read "+Statement);
                    Statement = surrondExpression(Statement);

                    
                    
                }
                
            }
            Shape.Selected = false;
        }
        private String surrondExpression(String str)
        {
            return "cin>> " + str + " ;";


        }
        public InputNode()
        {
            Shape.StencilItem = Stencil[FlowchartStencilType.Data];
            Shape.BackColor = System.Drawing.ColorTranslator.FromHtml("#0040a0");
            Shape.GradientColor = Color.Black;
            Shape.Label = new Crainiate.Diagramming.Label("Input");

            
        }
    }
}
