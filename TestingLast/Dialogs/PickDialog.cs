using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawShapes.Dialogs
{
    public partial class PickDialog : Form
    {
       // PictureBox prePB
        private int selectedShape;
        PictureBox prePB;
        public PickDialog()
        {
            InitializeComponent();
            selectedShape = -1;
        }

        private void onClick(object sender, EventArgs e)
        {
            
            PictureBox pb = (PictureBox)sender;
            if (prePB == null)
                prePB = pb;
            prePB.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            if (pb.Name == "declareImg")
                selectedShape = 0;
            else if (pb.Name == "assignImg")
                selectedShape = 1;
            else if (pb.Name == "whileImg")
                selectedShape = 2;
            else if (pb.Name == "forImg")
                selectedShape = 3;
            else if (pb.Name == "doImg")
                selectedShape = 4;
            else if (pb.Name == "ifImg")
                selectedShape = 5;
            else if (pb.Name == "inputImg")
                selectedShape = 6;
            else if (pb.Name == "outputImg")
                selectedShape = 7;
            else if (pb.Name == "pictureBox1")
                selectedShape = 8;
            else
                selectedShape = -1;
            pb.BackColor = Color.Black;
            prePB = pb;
        }

        public int getSelectedShape()
        {
            return selectedShape;
        }

       
    }
}
