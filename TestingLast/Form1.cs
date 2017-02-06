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
        public Form1()
        {
            InitializeComponent();
            new OutputNode();
            diagram1.Model.SetSize(new Size(100, 100));
            Model model = diagram1.Model;
            diagram1.Invalidate();
           // this.Controls.Add(model);
           
             terminalS = new TerminalNode(TerminalNode.TerminalType.Start);
             terminalE= new TerminalNode(TerminalNode.TerminalType.End);
            BaseNode.Model = model;
            BaseNode.Form = this;
            terminalS.attachNode(terminalE);
            Connector connector = new Connector();
            // connector.
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
            BaseNode.nodes.Clear();
            terminalS = new TerminalNode(TerminalNode.TerminalType.Start);
            terminalE = new TerminalNode(TerminalNode.TerminalType.End);
            diagram1.Model.Clear();
            diagram1.Model.OnModelInvalid();
            BaseNode.nodes.Clear();
            terminalS.attachNode(terminalE);
           // Connector connector = new Connector();
            // connector.
            terminalS.addToModel();
            terminalE.addToModel();
            Project_Save.Project_Loader ps = new Project_Save.Project_Loader(terminalS, terminalE, "F:\\testxml.xml");
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            terminalS = new TerminalNode(TerminalNode.TerminalType.Start);
            terminalE = new TerminalNode(TerminalNode.TerminalType.End);
            diagram1.Model.Clear();
            BaseNode.nodes.Clear();
           /* diagram1.StopEdit();
            diagram1.Suspend();
            diagram1.Refresh();
            diagram1.Resume();
            diagram1.Invalidate();
            //diagram1.InitiateClick(new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
            /*diagram1.Model.OnModelInvalid();
            diagram1.Refresh();
            diagram1.Render.Layers = diagram1.Model.Layers;
            diagram1.Render.Elements = diagram1.Model.Elements;
            diagram1.Render.RenderDiagram(new Rectangle(0, 0, diagram1.Width, diagram1.Height), diagram1.Paging);*/

            // diagram1.DrawDiagram(new Rectangle(0, 0, this.Width, this.Height));
            terminalS.attachNode(terminalE);
            // Connector connector = new Connector();
            // connector.
            terminalS.addToModel();
            terminalE.addToModel();
        }
    }
}
