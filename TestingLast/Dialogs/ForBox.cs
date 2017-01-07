using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestingLast.Dialogs
{
    public partial class ForBox : Form
    {
        bool close = true;
        public enum Direction { Increasing,Decreasing};

        public ForBox()
        {
            InitializeComponent();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            checkFields();
            if (!close) {
                this.AcceptButton = okBtn;
            }
        }
       
        public String getVar() {
            return varBox.Text.Trim();
        }
        public String getStartVal() {
            return startValBox.Text.Trim();
        }
        public String getEndVal() {
            return endValBox.Text.Trim();
        }
        public String getStepByVal() {
            return stepByBox.Text.Trim(); 
        }
        public Direction getDirection() {
            if (directionBox.SelectedIndex == 0)
                return Direction.Increasing;
            else
                return Direction.Decreasing;
        }
        

        private void checkFields()
        {
            close = true;
            if (String.IsNullOrWhiteSpace(varBox.Text.Trim()))
            {
                close = false;
                showError("Fill Variable Field Before Clickin Ok");
            }
            else if (String.IsNullOrWhiteSpace(startValBox.Text.Trim()))
            {
                close = false;
                showError("Fill Start Value Field Before Clickin Ok");
            }
            else if (String.IsNullOrWhiteSpace(endValBox.Text.Trim()))
            {
                close = false;
                showError("Fill End Value Field Before Clickin Ok");
            }
            else if (String.IsNullOrWhiteSpace(stepByBox.Text.Trim()))
            {
                close = false;
                showError("Fill Step By Field Before Clickin Ok");

            }
            else if (directionBox.SelectedIndex < 0) {
                close = false;
                showError("Choose Direction Before Clickin Ok");
            }
        }

      /*  public String getExpression() {

        }*/

        private void showError(string v)
        {
            MessageBox.Show(v, "Error",
          MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
       

        private void ForBox_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (!close)
            {
                // Cancel the Closing event from closing the form.
                e.Cancel = true;
                // Call method to save file...

            }

        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            close = true;
        }
    }
}
