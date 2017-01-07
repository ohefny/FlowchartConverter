namespace TestingLast.Dialogs
{
    partial class ForBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.directionBox = new System.Windows.Forms.ComboBox();
            this.varBox = new System.Windows.Forms.TextBox();
            this.startValBox = new System.Windows.Forms.TextBox();
            this.endValBox = new System.Windows.Forms.TextBox();
            this.stepByBox = new System.Windows.Forms.TextBox();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(433, 79);
            this.panel1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(120, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "replacement for a While Loop";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(120, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(234, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "a range of values. This is a common and useful ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(120, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(280, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "A For Loop increments or de crements a variable through";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::TestingLast.Properties.Resources._for;
            this.pictureBox1.Location = new System.Drawing.Point(10, 17);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(97, 39);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Variable :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 141);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Start Value :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(228, 141);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "End Value :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 202);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Direction :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(228, 202);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Step By :";
            // 
            // directionBox
            // 
            this.directionBox.FormattingEnabled = true;
            this.directionBox.Items.AddRange(new object[] {
            "Increasing",
            "Decreasing"});
            this.directionBox.Location = new System.Drawing.Point(14, 225);
            this.directionBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.directionBox.Name = "directionBox";
            this.directionBox.Size = new System.Drawing.Size(159, 21);
            this.directionBox.TabIndex = 6;
            // 
            // varBox
            // 
            this.varBox.Location = new System.Drawing.Point(14, 114);
            this.varBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.varBox.Name = "varBox";
            this.varBox.Size = new System.Drawing.Size(159, 20);
            this.varBox.TabIndex = 7;
            // 
            // startValBox
            // 
            this.startValBox.Location = new System.Drawing.Point(14, 171);
            this.startValBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.startValBox.Name = "startValBox";
            this.startValBox.Size = new System.Drawing.Size(159, 20);
            this.startValBox.TabIndex = 8;
            // 
            // endValBox
            // 
            this.endValBox.Location = new System.Drawing.Point(231, 171);
            this.endValBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.endValBox.Name = "endValBox";
            this.endValBox.Size = new System.Drawing.Size(168, 20);
            this.endValBox.TabIndex = 9;
            // 
            // stepByBox
            // 
            this.stepByBox.Location = new System.Drawing.Point(231, 225);
            this.stepByBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.stepByBox.Name = "stepByBox";
            this.stepByBox.Size = new System.Drawing.Size(168, 20);
            this.stepByBox.TabIndex = 10;
            // 
            // okBtn
            // 
            this.okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBtn.Location = new System.Drawing.Point(231, 258);
            this.okBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(64, 29);
            this.okBtn.TabIndex = 11;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(333, 258);
            this.cancelBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(64, 29);
            this.cancelBtn.TabIndex = 12;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // ForBox
            // 
            this.AcceptButton = this.okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(438, 297);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.stepByBox);
            this.Controls.Add(this.endValBox);
            this.Controls.Add(this.startValBox);
            this.Controls.Add(this.varBox);
            this.Controls.Add(this.directionBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ForBox";
            this.Text = "ForBox";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ForBox_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox directionBox;
        private System.Windows.Forms.TextBox varBox;
        private System.Windows.Forms.TextBox startValBox;
        private System.Windows.Forms.TextBox endValBox;
        private System.Windows.Forms.TextBox stepByBox;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
    }
}