using Crainiate.Diagramming;
using Crainiate.Diagramming.Flowcharting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestingLast.CodeGeneration;
using TestingLast.Nodes;

namespace TestingLast
{
    public partial class Form1 : Form
    {
        Controller controller;
        TerminalNode terminalS;
        TerminalNode terminalE;
        
        public Form1()
        {
            
            InitializeComponent();
            
            Model model = diagram1.Model;
            controller = new Controller(model);
            diagram1.Invalidate();
            // this.Controls.Add(model);

            //initializeNodes(model);

        }

        

        private void button1_Click_1(object sender, EventArgs e)
        {
            String str = controller.getCode(Controller.Language.CPP);
            MessageBox.Show(str);

        }

      
        private void deleteBtn_Click(object sender, EventArgs e)
        {
            controller.DeleteChoosed = true;
        }

        private void diagram1_MouseClick_1(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                controller.cancelClickedButtons();
            }
        }

        private void xmlBtn_Click(object sender, EventArgs e)
        {
            controller.saveProject("F:\\", "testxml");
            
           
            //MessageBox.Show(ps.XmlString);
        }

        private void onLoad_click(object sender, EventArgs e)
        {
            //initializeNodes(BaseNode.Model);
            controller.loadProject("F:\\testxml.xml");
           
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            controller.initializeProject();
        }

        private void DialogsBtn_Click(object sender, EventArgs e)
        {
            controller.OpenDialogs = true;
        }
    }
}
