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
    class ForNode : WhileNode
    {
        String var, startVal, endVal, stepBy,direction;

        public string Var
        {
            get
            {
                return var;
            }

            set
            {
                var = value;
            }
        }

        public string StartVal
        {
            get
            {
                return startVal;
            }

            set
            {
                startVal = value;
            }
        }

        public string EndVal
        {
            get
            {
                return endVal;
            }

            set
            {
                endVal = value;
            }
        }

        public string StepBy
        {
            get
            {
                return stepBy;
            }

            set
            {
                stepBy = value;
            }
        }

        public string Direction
        {
            get
            {
                return direction;
            }

            set
            {
                direction = value;
            }
        }

        //ForDialog.Direction direction;

        public override void onShapeClicked()
        {
            if (Shape.Selected && Controller.DeleteChoosed)
            {

                removeFromModel();
                Controller.DeleteChoosed = false;
                Shape.Selected = false;

            }
            if (Shape.Selected)
            {
                ForDialog forBox = new ForDialog();
                if (forBox.ShowDialog() == DialogResult.OK)
                {

                    setExpVariables(forBox);
                    Statement = generateEx();

                    
                    
                    //MessageBox.Show(Statement);
                }
            }
            Shape.Selected = false;
        }
        protected override void showStatment()
        {
            base.setText(Var+" = "+StartVal+" to "+EndVal+ " "+Direction);
        }

        private string generateEx()
        {
            return "int " + Var + " = "
                + StartVal + " ; " + Var + getDirectionCondition()
                +" "+ EndVal + " ; " + Var + getDirectionOperator() 
                + StepBy;

        }

        private void setExpVariables(ForDialog forBox)
        {
            Var = forBox.LoopVariable;
            StartVal = forBox.LoopStart;
            EndVal = forBox.LoopEnd;
            StepBy = forBox.LoopStep;
            Direction = forBox.LoopBehaviour;
        }

        private string surrondExpression(string str)
        {
            return "for ( " + str + " )";
        }
        private string getDirectionOperator()
        {
            if (Direction.Equals("Increment"))
                return "+=";
            return "-=";
        }

        private string getDirectionCondition()
        {
            if (Direction.Equals("Increment"))
                return "<";
            return ">";
        }
        public ForNode()
        {
            //base();
            // Shape.Label = new Crainiate.Diagramming.Label("For");
            Name = "For";
           // Statement = ("false");
            setText("For");
            TrueConnector.Connector.Label = new Crainiate.Diagramming.Label("Next");
            OutConnector.Connector.Label = new Crainiate.Diagramming.Label("Done");
            
        }
    }
}
