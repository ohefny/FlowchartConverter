using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Crainiate.Diagramming;
using Crainiate.Diagramming.Forms;

namespace Crainiate.Diagramming.Testing
{
    public partial class frmGroup : Form
    {
        public frmGroup()
        {
            InitializeComponent();

            Model model = diagram1.Model;
            diagram1.Paging.Enabled = false;
            diagram1.Model.SetSize(new Size(1000, 1000));

            Shape shape = new Shape();
            shape.Location = new PointF(320, 320);
            shape.BorderColor = Color.Green;
            model.Shapes.Add(shape);
            

            Shape shape2 = new Shape();
            shape2.Location = new PointF(20, 20);
            shape2.BorderColor = Color.Blue;
            model.Shapes.Add(shape2);

            Shape shape3 = new Shape();
            shape3.Location = new PointF(200, 100);
            shape3.BorderColor = Color.Red;
            model.Shapes.Add(shape3);

            Group group = new Group();
            //group.AddElement(shape);
            
            group.Expanded = true;

            model.Shapes.Add(group);
            
            group.AddElement(shape2);

            diagram1.Refresh();
        }
    }
}