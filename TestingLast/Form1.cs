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
using TestingLast.Nodes;

namespace TestingLast
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            diagram1.Model.SetSize(new Size(100, 100));
            Model model = diagram1.Model;
           // this.Controls.Add(model);
           
            TerminalNode terminalS = new TerminalNode(TerminalNode.TerminalType.Start);
            TerminalNode terminalE= new TerminalNode(TerminalNode.TerminalType.End);
            BaseNode.Model = model;
            terminalS.attachNode(terminalE);
            Connector connector = new Connector();
            // connector.
           terminalS.addToModel();
           terminalE.addToModel();

       
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (BaseNode b in BaseNode.nodes) {
                if(!(b is HolderNode))
                    b.setText("sflsfaklf lkaslksfalkasf laflsafklf");
            }
           
        }
    }
}
