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

namespace TestingLast.Dialogs
{
    public partial class DeclareDialog : Form
    {
        private string _declareVariable;
        private string _declareDataType;
        private string _declareVariableType;
        private string _declareArraySize;

        public DeclareDialog()
        {
            InitializeComponent();
            InitializeDialog();
        }

        private void InitializeDialog()
        {
            Model model = diagram1.Model;
            diagram1.Model.SetSize(new Size(1000, 1000));

            FlowchartStencil stencil = (FlowchartStencil)Singleton.Instance.GetStencil(typeof(FlowchartStencil));

            //Declare
            Shape shape = new Shape();
            shape.Location = new PointF(10, 20);
            shape.Size = new SizeF(100, 100);
            shape.AllowScale = false;
            shape.AllowMove = false;
            shape.StencilItem = stencil[FlowchartStencilType.InternalStorage];
            shape.Label = new Crainiate.Diagramming.Label("Declare");
            shape.GradientColor = System.Drawing.Color.Black;
            shape.BackColor = System.Drawing.Color.LightGray;
            model.Shapes.Add("declare", shape);

            this.type_box.SelectedItem = "Single";
            this.data_box.SelectedItem = "Integer";
        }

        private void variable_box_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (comboBox.SelectedItem.Equals("Single"))
            {
                this.size_text.Enabled = false;
                this.size_text.BackColor = Color.LightGray;
                this.size_text.Text = null;
                this._declareVariableType = "Single";
            }

            else
            {
                this.size_text.Enabled = true;
                this.size_text.BackColor = Color.White;
                this._declareVariableType = "Array";
            }
            
        }

        private void name_text_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            this._declareVariable = textBox.Text;
        }

        private void size_text_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            this._declareArraySize = textBox.Text;
        }

        private void data_box_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            this._declareDataType = (string)comboBox.SelectedItem;
        }

        public string DeclareVariable
        {
            get
            {
                return _declareVariable;
            }
        }

        public string DeclareDataType
        {
            get
            {
                return _declareDataType;
            }
        }

        public string DeclareVariableType
        {
            get
            {
                return _declareVariableType;
            }
        }

        public string DeclareArraySize
        {
            get
            {
                return _declareArraySize;
            }
        }
    }
}
