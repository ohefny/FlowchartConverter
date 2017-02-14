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
using FlowchartConverter.Nodes;

namespace FlowchartConverter
{
     partial class PickerDialog : Form
    {

        private BaseNode selectedShape;
        private Color previousColor;
        private Shape selectedImg;

        public BaseNode SelectedShape
        {
            get
            {
                return selectedShape;
            }
        }

        public PickerDialog()
        {
            InitializeComponent();
            InitializePicker();
        }

        private void InitializePicker()
        {

            Model model = diagram1.Model;
            diagram1.Model.SetSize(new Size(1000, 1000));

            FlowchartStencil stencil = (FlowchartStencil)Singleton.Instance.GetStencil(typeof(FlowchartStencil));

            //Declare
            Shape declare = new Shape();
            declare.Location = new PointF(50, 10);
            declare.Size = new SizeF(100, 100);
            declare.AllowScale = false;
            declare.AllowMove = false;
            declare.StencilItem = stencil[FlowchartStencilType.InternalStorage];
            declare.Label = new Crainiate.Diagramming.Label("Declare");
            declare.GradientColor = System.Drawing.Color.Black;
            declare.BackColor = System.Drawing.Color.LightGray;
            declare.SelectedChanged += new EventHandler(this.shape_SelectedChanged);
            declare.Selected = false;
            model.Shapes.Add("declare", declare);

            //Assign
            Shape assign = new Shape();
            assign.Location = new PointF(200, 10);
            assign.Size = new SizeF(100, 100);
            assign.AllowScale = false;
            assign.AllowMove = false;
            assign.StencilItem = stencil[FlowchartStencilType.Process];
            assign.Label = new Crainiate.Diagramming.Label("Assign");
            assign.GradientColor = System.Drawing.Color.Black;
            assign.BackColor = System.Drawing.Color.LightGreen;
            assign.SelectedChanged += new EventHandler(this.shape_SelectedChanged);
            assign.Selected = false;
            model.Shapes.Add("assign", assign);

            //Input
            Shape input = new Shape();
            input.Location = new PointF(350, 10);
            input.Size = new SizeF(100, 100);
            input.AllowScale = false;
            input.AllowMove = false;
            input.StencilItem = stencil[FlowchartStencilType.Data];
            input.Label = new Crainiate.Diagramming.Label("Input");
            input.GradientColor = System.Drawing.Color.Black;
            input.BackColor = System.Drawing.Color.Khaki;
            input.SelectedChanged += new EventHandler(this.shape_SelectedChanged);
            input.Selected = false;
            model.Shapes.Add("input", input);

            //Output
            Shape output = new Shape();
            output.Location = new PointF(50, 200);
            output.Size = new SizeF(100, 100);
            output.AllowScale = false;
            output.AllowMove = false;
            output.StencilItem = stencil[FlowchartStencilType.Data];
            output.Label = new Crainiate.Diagramming.Label("Output");
            output.GradientColor = System.Drawing.Color.Black;
            output.BackColor = System.Drawing.Color.LightBlue;
            output.SelectedChanged += new EventHandler(this.shape_SelectedChanged);
            output.Selected = false;
            model.Shapes.Add("output", output);

            //Decision
            Shape decision = new Shape();
            decision.Location = new PointF(200, 200);
            decision.Size = new SizeF(100, 100);
            decision.AllowScale = false;
            decision.AllowMove = false;
            decision.StencilItem = stencil[FlowchartStencilType.Decision];
            decision.Label = new Crainiate.Diagramming.Label("If");
            decision.GradientColor = System.Drawing.Color.Black;
            decision.BackColor = System.Drawing.Color.LightYellow;
            decision.SelectedChanged += new EventHandler(this.shape_SelectedChanged);
            decision.Selected = false;
            model.Shapes.Add("decision", decision);


            //For Loop
            Shape forLoop = new Shape();
            forLoop.Location = new PointF(350, 200);
            forLoop.Size = new SizeF(100, 100);
            forLoop.AllowScale = false;
            forLoop.AllowMove = false;
            forLoop.StencilItem = stencil[FlowchartStencilType.Preparation];
            forLoop.Label = new Crainiate.Diagramming.Label("For");
            forLoop.GradientColor = System.Drawing.Color.Black;
            forLoop.BackColor = System.Drawing.Color.Red;
            forLoop.SelectedChanged += new EventHandler(this.shape_SelectedChanged);
            forLoop.Selected = false;
            model.Shapes.Add("for", forLoop);

            //Do Loop
            /*Shape doLoop = new Shape();
            doLoop.Location = new PointF(50, 390);
            doLoop.Size = new SizeF(100, 100);
            doLoop.AllowScale = false;
            doLoop.AllowMove = false;
            doLoop.StencilItem = stencil[FlowchartStencilType.Preparation];
            doLoop.Label = new Crainiate.Diagramming.Label("Do");
            doLoop.GradientColor = System.Drawing.Color.Black;
            doLoop.BackColor = System.Drawing.Color.Chocolate;
            doLoop.SelectedChanged += new EventHandler(this.shape_SelectedChanged);
            model.Shapes.Add("do", doLoop);*/

            //While Loop
            Shape whileLoop = new Shape();
            
            whileLoop.Location = new PointF(50, 390);
            //whileLoop.Location = new PointF(350, 390);
            whileLoop.Size = new SizeF(100, 100);
            whileLoop.AllowScale = false;
            whileLoop.AllowMove = false;
            whileLoop.StencilItem = stencil[FlowchartStencilType.Preparation];
            whileLoop.Label = new Crainiate.Diagramming.Label("While");
            whileLoop.GradientColor = System.Drawing.Color.Black;
            whileLoop.BackColor = System.Drawing.Color.LightSalmon;
            whileLoop.SelectedChanged += new EventHandler(this.shape_SelectedChanged);
            model.Shapes.Add("while", whileLoop);
            whileLoop.Selected = false;
            //
            //For Loop

            Shape ifElse = new Shape();
            ifElse.Location = new PointF(200, 390);
            ifElse.Size = new SizeF(100, 100);
            ifElse.AllowScale = false;
            ifElse.AllowMove = false;
            ifElse.StencilItem = stencil[FlowchartStencilType.Decision];
            ifElse.Label = new Crainiate.Diagramming.Label("If Else");
            ifElse.GradientColor = System.Drawing.Color.Black;
            ifElse.BackColor = System.Drawing.Color.LightSalmon;
            ifElse.SelectedChanged += new EventHandler(this.shape_SelectedChanged);
            ifElse.Selected = false;
            model.Shapes.Add("ifElse", ifElse);
        }

        private void shape_SelectedChanged(object sender, EventArgs e)
        {
            if (selectedImg != null)
                selectedImg.Selected = false;
             selectedImg = (Shape)sender;
            if (selectedImg.Selected)
            {
                this.previousColor = selectedImg.BackColor;
                selectedImg.BackColor = System.Drawing.Color.Blue;
                switch (selectedImg.Label.Text)
                {
                    case "Declare":
                        this.selectedShape = new DeclareNode();
                        break;
                    case "Assign":
                        this.selectedShape = new AssignNode();
                        break;
                    case "Input":
                        this.selectedShape = new InputNode();
                        break;
                    case "Output":
                        this.selectedShape = new OutputNode();
                        break;
                    case "If":
                        this.selectedShape = new IfNode();
                        break;
                    case "For":
                        this.selectedShape = new ForNode();
                        break;
                    case "Do":
                        this.selectedShape = new DoNode();
                        break;
                    case "While":
                        this.selectedShape = new WhileNode();
                        break;
                    case "If Else":
                        this.selectedShape = new IfElseNode();
                        break;
                    default:
                        this.selectedShape = null;
                        break;
                }
            }
            else
            {
                selectedImg.BackColor = previousColor;
                this.selectedShape = null;
            }
            
        }
    }
}
