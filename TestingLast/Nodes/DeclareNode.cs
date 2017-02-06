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
            if (Shape.Selected && Form1.deleteChoosed)
            {

                removeFromModel();
                Form1.deleteChoosed = false;
            }
            else if(Shape.Selected)
            {
                TextBox textBox = new TextBox();
                textBox.Location = new Point( (int) Shape.Location.X, (int) Shape.Location.Y);
                textBox.Width = (int) Shape.Width;
                DeclareBox db = new DeclareBox();
                DialogResult dr = db.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    if (String.IsNullOrEmpty(db.DeclareDataType) || String.IsNullOrEmpty(db.DeclareVariableType)
                        || String.IsNullOrWhiteSpace(db.DeclareDataType) || String.IsNullOrWhiteSpace(db.DeclareVariableType)
                        || String.IsNullOrEmpty(db.DeclareVariable) || String.IsNullOrWhiteSpace(db.DeclareVariable))
                    {
                        MessageBox.Show("You must enter a valid declare expression");
                        return;
                    }

                    if (db.DeclareVariableType.Equals("Array") && (String.IsNullOrEmpty(db.DeclareArraySize) || String.IsNullOrWhiteSpace(db.DeclareArraySize)))
                    {
                        MessageBox.Show("You must enter a valid declare expression");
                        return;
                    }
                    if (db.DeclareVariableType.Equals("Array"))
                    {
                        string dataType = "";
                        if (db.DeclareDataType.Equals("Integer"))
                            dataType = "int";
                        else if (db.DeclareDataType.Equals("Float"))
                            dataType = "float";
                        else if (db.DeclareDataType.Equals("Bool"))
                            dataType = "bool";
                        else
                            dataType = "char";
                        Statement = dataType + "[" + db.DeclareArraySize + "] " + db.DeclareVariable;
                    }
                    else
                    {
                        string dataType = "";
                        if (db.DeclareDataType.Equals("Integer"))
                            dataType = "int";
                        else if (db.DeclareDataType.Equals("Float"))
                            dataType = "float";
                        else if (db.DeclareDataType.Equals("Bool"))
                            dataType = "bool";
                        else
                            dataType = "String";
                        Statement = dataType + " " + db.DeclareVariable;
                    }

                    setText(Statement);
                    //Statement += " ;";
                }
                // MessageBox.Show(Statement);
            
            }
            Shape.Selected = false;
        }
        public DeclareNode()
        {
            Name = "Declare";
            Shape.StencilItem = Stencil[FlowchartStencilType.InternalStorage];
            Shape.BackColor = System.Drawing.ColorTranslator.FromHtml("#e3810c");
            Shape.GradientColor = Color.White;
            Shape.Label = new Crainiate.Diagramming.Label("Declare");
            Statement = "int x";

        }
    }
}
