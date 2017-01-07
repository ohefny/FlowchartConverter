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
        public Form1()
        {
            InitializeComponent();
            diagram1.Model.SetSize(new Size(100, 100));
            Model model = diagram1.Model;
           // this.Controls.Add(model);
           
             terminalS = new TerminalNode(TerminalNode.TerminalType.Start);
             terminalE= new TerminalNode(TerminalNode.TerminalType.End);
            BaseNode.Model = model;
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
    }
}
