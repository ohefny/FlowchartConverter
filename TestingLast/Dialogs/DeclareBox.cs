using TestingLast.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DrawShapes.Dialogs
{
     class DeclareBox : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        public DeclareBox() {
            InitializeComponent();
        }
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cancel = new System.Windows.Forms.Button();
            this.ok = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.remove = new System.Windows.Forms.Button();
            this.variableLabel = new System.Windows.Forms.Label();
            this.variableTB = new System.Windows.Forms.TextBox();
            this.typeLabel = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.arraySizeLabel = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.arraybox = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DimGray;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(573, 72);
            this.panel1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(126, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(260, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "These are used to store data while the program runs";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(126, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(286, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "A Declare Statement is used to create variables , an array";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(38, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Declare";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = Resources.assign;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(9, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(350, 250);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(89, 32);
            this.cancel.TabIndex = 2;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(252, 250);
            this.ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(89, 32);
            this.ok.TabIndex = 3;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(2, 80);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(339, 95);
            this.listBox1.TabIndex = 4;
            // 
            // remove
            // 
            this.remove.Location = new System.Drawing.Point(369, 151);
            this.remove.Name = "remove";
            this.remove.Size = new System.Drawing.Size(70, 24);
            this.remove.TabIndex = 5;
            this.remove.Text = "Remove";
            this.remove.UseVisualStyleBackColor = true;
            // 
            // variableLabel
            // 
            this.variableLabel.AutoSize = true;
            this.variableLabel.BackColor = System.Drawing.Color.Transparent;
            this.variableLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.variableLabel.Location = new System.Drawing.Point(9, 188);
            this.variableLabel.Name = "variableLabel";
            this.variableLabel.Size = new System.Drawing.Size(52, 13);
            this.variableLabel.TabIndex = 6;
            this.variableLabel.Text = "Variable :";
            // 
            // variableTB
            // 
            this.variableTB.Location = new System.Drawing.Point(12, 205);
            this.variableTB.Name = "variableTB";
            this.variableTB.Size = new System.Drawing.Size(70, 20);
            this.variableTB.TabIndex = 7;
            // 
            // typeLabel
            // 
            this.typeLabel.AutoSize = true;
            this.typeLabel.BackColor = System.Drawing.Color.Transparent;
            this.typeLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.typeLabel.Location = new System.Drawing.Point(105, 188);
            this.typeLabel.Name = "typeLabel";
            this.typeLabel.Size = new System.Drawing.Size(41, 13);
            this.typeLabel.TabIndex = 8;
            this.typeLabel.Text = "Type : ";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(108, 205);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(90, 21);
            this.comboBox1.TabIndex = 9;
            // 
            // arraySizeLabel
            // 
            this.arraySizeLabel.AutoSize = true;
            this.arraySizeLabel.BackColor = System.Drawing.Color.Transparent;
            this.arraySizeLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.arraySizeLabel.Location = new System.Drawing.Point(278, 188);
            this.arraySizeLabel.Name = "arraySizeLabel";
            this.arraySizeLabel.Size = new System.Drawing.Size(63, 13);
            this.arraySizeLabel.TabIndex = 10;
            this.arraySizeLabel.Text = "Array Size :";
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(281, 206);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(60, 20);
            this.textBox1.TabIndex = 11;
            // 
            // arraybox
            // 
            this.arraybox.AutoSize = true;
            this.arraybox.Location = new System.Drawing.Point(204, 208);
            this.arraybox.Name = "arraybox";
            this.arraybox.Size = new System.Drawing.Size(61, 17);
            this.arraybox.TabIndex = 12;
            this.arraybox.Text = "Array ?";
            this.arraybox.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(369, 202);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(70, 24);
            this.button1.TabIndex = 13;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // OutputForm
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(457, 294);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.arraybox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.arraySizeLabel);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.typeLabel);
            this.Controls.Add(this.variableTB);
            this.Controls.Add(this.variableLabel);
            this.Controls.Add(this.remove);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.panel1);
            this.Name = "DeclareForm";
            this.Text = "Declare Properities";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button remove;
        private System.Windows.Forms.Label variableLabel;
        private System.Windows.Forms.TextBox variableTB;
        private System.Windows.Forms.Label typeLabel;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label arraySizeLabel;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox arraybox;
        private System.Windows.Forms.Button button1;
    }
}
