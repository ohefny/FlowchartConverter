﻿using Crainiate.Diagramming.Flowcharting;
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
            base.onShapeClicked();
            if (Shape.Selected)
            {
                AssignmentDialog db = new AssignmentDialog();
                DialogResult dr = db.ShowDialog();

                if (dr == DialogResult.OK)
                {
                    if (String.IsNullOrEmpty(db.AssignmentVariable) || String.IsNullOrEmpty(db.AssignmentExpression)
                        || String.IsNullOrWhiteSpace(db.AssignmentVariable) || String.IsNullOrWhiteSpace(db.AssignmentExpression))
                    {
                        MessageBox.Show("You must enter a valid assignment expression");
                        return;
                    }
                    Statement = db.AssignmentVariable + " = " + db.AssignmentExpression;
                   
                    
                }
                ///MessageBox.Show(Statement);
                
            }
            Shape.Selected = false;
        }

       
        public AssignNode()
        {
            Name = "Assign";
            Shape.StencilItem = Stencil[FlowchartStencilType.Default];
            Shape.BackColor = System.Drawing.ColorTranslator.FromHtml("#fdfd80");
            Shape.GradientColor = Color.Black;
            setText("Assing");
            
            
        }
    }
}
