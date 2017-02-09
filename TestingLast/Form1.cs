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
        TerminalNode terminalS;
        TerminalNode terminalE;
        public static bool deleteChoosed;
        public static bool openDialogs = false;
        public Form1()
        {
            InitializeComponent();
            new OutputNode();
            diagram1.Model.SetSize(new Size(100, 100));
        
            Model model = diagram1.Model;
            diagram1.Invalidate();
            // this.Controls.Add(model);

            initializeNodes(model);

        }

        private void initializeNodes(Model model)
        {

            diagram1.Model.Clear();
            BaseNode.nodes.Clear();
            terminalS = new TerminalNode(TerminalNode.TerminalType.Start);
            terminalE = new TerminalNode(TerminalNode.TerminalType.End);
            BaseNode.Model = model;
            BaseNode.Form = this;
            terminalS.attachNode(terminalE);
            terminalE.ParentNode = terminalS;
            terminalS.addToModel();
            terminalE.addToModel();
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            String str = FlowChartCodeGenerator.getCppCode(terminalS, terminalE);
            MessageBox.Show(str);

        }

      
        private void deleteBtn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Cross;
            deleteChoosed = true;
        }

        private void diagram1_MouseClick_1(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {

                deleteChoosed = false;

            }
        }

        private void xmlBtn_Click(object sender, EventArgs e)
        {
            Project_Save.Project_Saver ps = new Project_Save.Project_Saver(terminalS, terminalE, "F:\\", "testxml");
            MessageBox.Show(ps.XmlString);
        }

        private void onLoad_click(object sender, EventArgs e)
        {
            initializeNodes(BaseNode.Model);
            Project_Save.Project_Loader ps = new Project_Save.Project_Loader(terminalS, terminalE, "F:\\testxml.xml");
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            initializeNodes(BaseNode.Model);
        }

        private void DialogsBtn_Click(object sender, EventArgs e)
        {
            openDialogs = true;
        }
    }
}
