using TestingLast.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DrawShapes.Dialogs
{
    class IfBox :Form
    {
        public String getExpression()
        {
            return conditionBox.Text.Trim();
        }
        public void setExpression(string str) {
            if (String.IsNullOrEmpty(str))
                return;
            conditionBox.Text = str;
        }
        public IfBox() {
                InitializeComponent();
            }
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
                this.cancel = new System.Windows.Forms.Button();
                this.ok = new System.Windows.Forms.Button();
                this.conditionBox = new System.Windows.Forms.TextBox();
                this.conditionLabel = new System.Windows.Forms.Label();
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
                this.panel1.Size = new System.Drawing.Size(751, 72);
                this.panel1.TabIndex = 0;
                // 
                // label3
                // 
                this.label3.AutoSize = true;
                this.label3.Location = new System.Drawing.Point(126, 29);
                this.label3.Name = "label3";
                this.label3.Size = new System.Drawing.Size(254, 13);
                this.label3.TabIndex = 5;
                this.label3.Text = "executes a true or false branch based on the result";
                // 
                // label2
                // 
                this.label2.AutoSize = true;
                this.label2.Location = new System.Drawing.Point(126, 9);
                this.label2.Name = "label2";
                this.label2.Size = new System.Drawing.Size(249, 13);
                this.label2.TabIndex = 4;
                this.label2.Text = "An If Statement checks a Boolean expression then";
                // 
                // label1
                // 
                this.label1.AutoSize = true;
                this.label1.BackColor = System.Drawing.Color.Transparent;
                this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
                this.label1.Location = new System.Drawing.Point(51, 29);
                this.label1.Name = "label1";
                this.label1.Size = new System.Drawing.Size(15, 13);
                this.label1.TabIndex = 4;
                this.label1.Text = "If";
            // 
            // pictureBox1
            // 
                this.pictureBox1.BackgroundImage = Resources.clicked_if;
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
                this.cancel.Location = new System.Drawing.Point(294, 234);
                this.cancel.Name = "cancel";
                this.cancel.Size = new System.Drawing.Size(89, 32);
                this.cancel.TabIndex = 2;
                this.cancel.Text = "Cancel";
                this.cancel.UseVisualStyleBackColor = true;
            // 
            // ok
            // 
            this.ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok.Location = new System.Drawing.Point(190, 233);
                this.ok.Name = "ok";
                this.ok.Size = new System.Drawing.Size(89, 32);
                this.ok.TabIndex = 3;
                this.ok.Text = "OK";
                this.ok.UseVisualStyleBackColor = true;
                // 
                // conditionBox
                // 
                this.conditionBox.Location = new System.Drawing.Point(12, 107);
                this.conditionBox.Multiline = true;
                this.conditionBox.Name = "conditionBox";
                this.conditionBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
                this.conditionBox.Size = new System.Drawing.Size(362, 120);
                this.conditionBox.TabIndex = 4;
                // 
                // conditionLabel
                // 
                this.conditionLabel.AutoSize = true;
                this.conditionLabel.BackColor = System.Drawing.Color.Transparent;
                this.conditionLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
                this.conditionLabel.Location = new System.Drawing.Point(12, 91);
                this.conditionLabel.Name = "conditionLabel";
                this.conditionLabel.Size = new System.Drawing.Size(151, 13);
                this.conditionLabel.TabIndex = 6;
                this.conditionLabel.Text = "Enter Conditional Expression :";
                // 
                // IfForm
                // 
                this.AcceptButton = this.ok;
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.CancelButton = this.cancel;
                this.ClientSize = new System.Drawing.Size(388, 269);
                this.Controls.Add(this.conditionLabel);
                this.Controls.Add(this.conditionBox);
                this.Controls.Add(this.ok);
                this.Controls.Add(this.cancel);
                this.Controls.Add(this.panel1);
                this.Name = "IfForm";
                this.Text = "IF Properities";
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
            private System.Windows.Forms.TextBox conditionBox;
            private System.Windows.Forms.Label conditionLabel;
        
    }
}
