using Crainiate.Diagramming;
using Crainiate.Diagramming.Flowcharting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowchartConverter.Dialogs
{
    public partial class WhileDialog : Form
    {
        private string _loopExpression;

        public WhileDialog()
        {
            InitializeComponent();
            InitializeDialog();
        }

        private void InitializeDialog()
        {
            Model model = diagram1.Model;
            diagram1.Model.SetSize(new Size(1000, 1000));

            FlowchartStencil stencil = (FlowchartStencil)Singleton.Instance.GetStencil(typeof(FlowchartStencil));

            //While
            Shape shape = new Shape();
            shape.Location = new PointF(10, 10);
            shape.Size = new SizeF(100, 100);
            shape.AllowScale = false;
            shape.AllowMove = false;
            shape.StencilItem = stencil[FlowchartStencilType.Preparation];
            shape.Label = new Crainiate.Diagramming.Label("While");
            shape.GradientColor = System.Drawing.Color.Black;
            shape.BackColor = System.Drawing.Color.LightSalmon;
            model.Shapes.Add("while", shape);
        }

        private void expression_text_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            this._loopExpression = textBox.Text;
        }

        public string LoopExpression
        {
            get
            {
                return _loopExpression;
            }
        }
    }
}
