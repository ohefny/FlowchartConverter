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
    public partial class ForDialog : Form
    {
        private string _loopVariable;
        private string _loopStart;
        private string _loopEnd;
        private string _loopStep;
        private string _loopBehaviour;

        public ForDialog()
        {
            InitializeComponent();
            InitializeDialog();
        }

        private void InitializeDialog()
        {
            Model model = diagram1.Model;
            diagram1.Model.SetSize(new Size(1000, 1000));

            FlowchartStencil stencil = (FlowchartStencil)Singleton.Instance.GetStencil(typeof(FlowchartStencil));

            //For
            Shape shape = new Shape();
            shape.Location = new PointF(10, 10);
            shape.Size = new SizeF(100, 100);
            shape.AllowScale = false;
            shape.AllowMove = false;
            shape.StencilItem = stencil[FlowchartStencilType.Preparation];
            shape.Label = new Crainiate.Diagramming.Label("For");
            shape.GradientColor = System.Drawing.Color.Black;
            shape.BackColor = System.Drawing.Color.LightCoral;
            model.Shapes.Add("for", shape);

            this.behaviour_box.SelectedItem = "Increment";
        }

        private void variable_text_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            this._loopVariable = textBox.Text;
        }

        private void start_text_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            this._loopStart = textBox.Text;
        }

        private void end_text_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            this._loopEnd = textBox.Text;
        }

        private void step_text_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            this._loopStep = textBox.Text;
        }

        private void behaviour_box_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            this._loopBehaviour = (string) comboBox.SelectedItem;
        }

        public string LoopVariable
        {
            get
            {
                return _loopVariable;
            }
        }

        public string LoopStart
        {
            get
            {
                return _loopStart;
            }
        }

        public string LoopEnd
        {
            get
            {
                return _loopEnd;
            }
        }

        public string LoopStep
        {
            get
            {
                return _loopStep;
            }
        }

        public string LoopBehaviour
        {
            get
            {
                return _loopBehaviour;
            }
        }
    }
}
