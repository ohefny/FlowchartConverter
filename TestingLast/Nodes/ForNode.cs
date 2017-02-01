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
    class ForNode : WhileNode
    {
        String var, startVal, endVal, stepBy;
        ForBox.Direction direction;
        public override void onShapeClicked()
        {
            if (Shape.Selected && Form1.deleteChoosed)
            {
                while (!(TrueNode.OutConnector.EndNode is HolderNode))
                {
                    TrueNode.OutConnector.EndNode.removeFromModel();
                }
                removeFromModel();
                Form1.deleteChoosed = false;
            }
            else if(Shape.Selected)
            {
                ForBox forBox = new ForBox();
                if (forBox.ShowDialog() == DialogResult.OK)
                {

                    setExpVariables(forBox);
                    Statement = surrondExpression(generateEx());

                    setText(Statement);
                    
                    //MessageBox.Show(Statement);
                }
            }
            Shape.Selected = false;
        }

        private string generateEx()
        {
            return "int " + var + " = "
                + startVal + " ; " + var + getDirectionCondition()
                + endVal + " ; " + var + getDirectionOperator() 
                + stepBy;

        }

        private void setExpVariables(ForBox forBox)
        {
            var = forBox.getVar();
            startVal = forBox.getStartVal();
            endVal = forBox.getEndVal();
            stepBy = forBox.getStepByVal();
            direction = forBox.getDirection();
        }

        private string surrondExpression(string str)
        {
            return "for ( " + str + " )";
        }
        private string getDirectionOperator()
        {
            if (direction == ForBox.Direction.Increasing)
                return "+=";
            return "-=";
        }

        private string getDirectionCondition()
        {
            if (direction == ForBox.Direction.Increasing)
                return "<";
            return ">";
        }
        public ForNode()
        {
            //base();
            // Shape.Label = new Crainiate.Diagramming.Label("For");
            setText("For");
            trueConnector.Connector.Label = new Crainiate.Diagramming.Label("Next");
            OutConnector.Connector.Label = new Crainiate.Diagramming.Label("Done");
            Statement=("for(int i=0 ;i<0;i++)");
        }
    }
}
