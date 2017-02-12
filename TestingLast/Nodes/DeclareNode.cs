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
    public class DeclareNode : BaseNode
    {
        
        public class Variable {
            public enum Data_Type {INTEGER,STRING,REAL,BOOLEAN,CHAR }
            String varName;
            Data_Type varType;
            bool single;
            int size;

            public string VarName
            {
                get
                {
                    return varName;
                }

                set
                {
                    varName = value;
                }
            }

            public Data_Type VarType
            {
                get
                {
                    return varType;
                }

                set
                {
                    varType = value;
                }
            }

            public bool Single
            {
                get
                {
                    return single;
                }

                set
                {
                    single = value;
                }
            }

            public int Size
            {
                get
                {
                    return size;
                }

                set
                {
                    size = value;
                }
            }

            

        }
        Variable variable;

        public Variable _Var
        {
            get
            {
                return variable;
            }

            set
            {
                variable = value;
            }
        }

        public override void onShapeClicked()
        {
            if (Shape.Selected && Controller.DeleteChoosed)
            {

                removeFromModel();
                Controller.DeleteChoosed = false;
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
                    initializeVariable(db);
                    makeStatment();
                    
                    //Statement += " ;";
                }
                // MessageBox.Show(Statement);

            }
            Shape.Selected = false;
        }

        private void makeStatment()
        {
            if(variable.Single)
                Statement = variable.VarType + "  " + variable.VarName;
            else
                Statement = variable.VarType + " Array " + variable.VarName + "[" + variable.Size+ "]";
            
        }

        private void initializeVariable(DeclareBox db)
        {
            variable.VarName = db.DeclareVariable;
            if (db.DeclareDataType.Equals("Integer"))
                variable.VarType = Variable.Data_Type.INTEGER;
            else if (db.DeclareDataType.Equals("Float"))
                variable.VarType = Variable.Data_Type.REAL;
            else if (db.DeclareDataType.Equals("Bool"))
                variable.VarType = Variable.Data_Type.BOOLEAN;
            else
                variable.VarType = Variable.Data_Type.STRING;


            if (db.DeclareVariableType.Equals("Array"))
            {
                variable.Single = false;
                
                variable.Size = Int32.Parse(db.DeclareArraySize);
            }
            else
            {
                variable.Single = true;
                
            }
        }

        protected override void showStatment()
        {
            setText(Statement);
        }

        public DeclareNode()
        {
            Name = "Declare";
            Shape.StencilItem = Stencil[FlowchartStencilType.InternalStorage];
            Shape.BackColor = System.Drawing.ColorTranslator.FromHtml("#e3810c");
            Shape.GradientColor = Color.Black;
            setText("Declare");
            
            variable = new Variable();
        }
    }
}
