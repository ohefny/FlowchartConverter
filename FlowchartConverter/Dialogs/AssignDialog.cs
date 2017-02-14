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
    public partial class AssignDialog : Form
    {
        private string _assignmentVariable;
        private string _assignmentExpression;

        public AssignDialog()
        {
            InitializeComponent();
            InitializeDialog();
        }

        private void InitializeDialog()
        {
            Model model = diagram1.Model;
            diagram1.Model.SetSize(new Size(1000, 1000));

            FlowchartStencil stencil = (FlowchartStencil)Singleton.Instance.GetStencil(typeof(FlowchartStencil));

            //Assign
            Shape shape = new Shape();
            shape.Location = new PointF(10, 10);
            shape.Size = new SizeF(100, 100);
            shape.AllowScale = false;
            shape.AllowMove = false;
            shape.Selected = false;
            shape.StencilItem = stencil[FlowchartStencilType.Process];
            shape.Label = new Crainiate.Diagramming.Label("Assign");
            shape.GradientColor = System.Drawing.Color.Black;
            shape.BackColor = System.Drawing.Color.LightGreen;
            model.Shapes.Add("assign", shape);
        }

        private void name_text_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            _assignmentVariable = textBox.Text;
        }

        private void expression_text_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            _assignmentExpression = textBox.Text;
        }

        public string AssignmentVariable
        {
            get
            {
                return _assignmentVariable;
            }
        }

        public string AssignmentExpression
        {
            get
            {
                return _assignmentExpression;
            }
        }
    }
}
