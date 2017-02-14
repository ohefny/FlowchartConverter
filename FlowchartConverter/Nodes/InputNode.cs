using Crainiate.Diagramming;
using Crainiate.Diagramming.Flowcharting;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestingLast.Dialogs;

namespace TestingLast.Nodes
{
    class InputNode : BaseNode
    {
        public override void onShapeClicked()
        {
            base.onShapeClicked();
            if (Shape.Selected)
            {

                InputDialog db = new InputDialog();
                DialogResult dr = db.ShowDialog();
                
                if (dr == DialogResult.OK)
                {

                    Statement = db.InputVariable;

                 //   Statement = surrondExpression(Statement);



                }
                
            }
            Shape.Selected = false;
        }

        protected override void showStatment()
        {
            base.setText("Read "+Statement);
        }

        
        private String surrondExpression(String str)
        {
            return "cin>> " + str + " ;";


        }
        public InputNode()
        {
            Name = "Input";
            Shape.StencilItem = Stencil[FlowchartStencilType.Data];
            Shape.BackColor = System.Drawing.ColorTranslator.FromHtml("#0040a0");
            Shape.GradientColor = Color.Black;
            setText("Input");

            
        }
    }
}
