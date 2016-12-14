
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestingLast.Properties;

namespace DrawShapes.Dialogs
{
        public class AssignmentDialog :Form
        {
        public AssignmentDialog() {
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
                this.removeBtn = new System.Windows.Forms.Button();
                this.variableLabel = new System.Windows.Forms.Label();
                this.typeLabel = new System.Windows.Forms.Label();
                this.variablesBox = new System.Windows.Forms.ComboBox();
                this.arraySizeLabel = new System.Windows.Forms.Label();
                this.expressionBox = new System.Windows.Forms.TextBox();
                this.addBtn = new System.Windows.Forms.Button();
                this.indexBox = new System.Windows.Forms.TextBox();
                this.assignmentLB = new System.Windows.Forms.ListBox();
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
                this.label3.Size = new System.Drawing.Size(193, 13);
                this.label3.TabIndex = 5;
                this.label3.Text = "and then stores the result in a variable";
                // 
                // label2
                // 
                this.label2.AutoSize = true;
                this.label2.Location = new System.Drawing.Point(126, 9);
                this.label2.Name = "label2";
                this.label2.Size = new System.Drawing.Size(254, 13);
                this.label2.TabIndex = 4;
                this.label2.Text = "An Assignment Statment calcualates an expression ";
                // 
                // label1
                // 
                this.label1.AutoSize = true;
                this.label1.BackColor = System.Drawing.Color.Transparent;
                this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
                this.label1.Location = new System.Drawing.Point(30, 29);
                this.label1.Name = "label1";
                this.label1.Size = new System.Drawing.Size(41, 13);
                this.label1.TabIndex = 4;
                this.label1.Text = "Assignment";
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
                this.cancel.Location = new System.Drawing.Point(651, 280);
                this.cancel.Name = "cancel";
                this.cancel.Size = new System.Drawing.Size(89, 32);
                this.cancel.TabIndex = 2;
                this.cancel.Text = "Cancel";
                this.cancel.UseVisualStyleBackColor = true;
                // 
                // ok
                // 
                this.ok.Location = new System.Drawing.Point(529, 280);
                this.ok.Name = "ok";
                this.ok.Size = new System.Drawing.Size(89, 32);
                this.ok.TabIndex = 3;
                this.ok.Text = "OK";
                this.ok.UseVisualStyleBackColor = true;
                this.ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            // 
            // removeBtn
            // 
            this.removeBtn.Location = new System.Drawing.Point(670, 120);
                this.removeBtn.Name = "removeBtn";
                this.removeBtn.Size = new System.Drawing.Size(70, 24);
                this.removeBtn.TabIndex = 5;
                this.removeBtn.Text = "Remove";
                this.removeBtn.UseVisualStyleBackColor = true;
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
                // typeLabel
                // 
                this.typeLabel.AutoSize = true;
                this.typeLabel.BackColor = System.Drawing.Color.Transparent;
                this.typeLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
                this.typeLabel.Location = new System.Drawing.Point(114, 188);
                this.typeLabel.Name = "typeLabel";
                this.typeLabel.Size = new System.Drawing.Size(45, 13);
                this.typeLabel.TabIndex = 8;
                this.typeLabel.Text = "Index : ";
                // 
                // variablesBox
                // 
                this.variablesBox.FormattingEnabled = true;
                this.variablesBox.Location = new System.Drawing.Point(12, 206);
                this.variablesBox.Name = "variablesBox";
                this.variablesBox.Size = new System.Drawing.Size(90, 21);
                this.variablesBox.TabIndex = 9;
                // 
                // arraySizeLabel
                // 
                this.arraySizeLabel.AutoSize = true;
                this.arraySizeLabel.BackColor = System.Drawing.Color.Transparent;
                this.arraySizeLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
                this.arraySizeLabel.Location = new System.Drawing.Point(249, 186);
                this.arraySizeLabel.Name = "arraySizeLabel";
                this.arraySizeLabel.Size = new System.Drawing.Size(66, 13);
                this.arraySizeLabel.TabIndex = 10;
                this.arraySizeLabel.Text = "Expression :";
                // 
                // expressionBox
                // 
                this.expressionBox.Enabled = true;
                this.expressionBox.Location = new System.Drawing.Point(252, 202);
                this.expressionBox.Multiline = true;
                this.expressionBox.Name = "expressionBox";
                this.expressionBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
                this.expressionBox.Size = new System.Drawing.Size(392, 72);
                this.expressionBox.TabIndex = 11;
                // 
                // addBtn
                // 
                this.addBtn.Location = new System.Drawing.Point(670, 199);
                this.addBtn.Name = "addBtn";
                this.addBtn.Size = new System.Drawing.Size(70, 24);
                this.addBtn.TabIndex = 13;
                this.addBtn.Text = "Add";
                this.addBtn.UseVisualStyleBackColor = true;
                // 
                // indexBox
                // 
                this.indexBox.Enabled = false;
                this.indexBox.Location = new System.Drawing.Point(117, 206);
                this.indexBox.Name = "indexBox";
                this.indexBox.Size = new System.Drawing.Size(60, 20);
                this.indexBox.TabIndex = 14;
                // 
                // assignmentLB
                // 
                this.assignmentLB.FormattingEnabled = true;
                this.assignmentLB.Location = new System.Drawing.Point(2, 80);
                this.assignmentLB.Name = "assignmentLB";
                this.assignmentLB.ScrollAlwaysVisible = true;
                this.assignmentLB.Size = new System.Drawing.Size(642, 95);
                this.assignmentLB.TabIndex = 4;
                // 
                // OutputForm
                // 
                this.AcceptButton = this.ok;
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.CancelButton = this.cancel;
                this.ClientSize = new System.Drawing.Size(752, 324);
                this.Controls.Add(this.indexBox);
                this.Controls.Add(this.addBtn);
                this.Controls.Add(this.expressionBox);
                this.Controls.Add(this.arraySizeLabel);
                this.Controls.Add(this.variablesBox);
                this.Controls.Add(this.typeLabel);
                this.Controls.Add(this.variableLabel);
                this.Controls.Add(this.removeBtn);
                this.Controls.Add(this.assignmentLB);
                this.Controls.Add(this.ok);
                this.Controls.Add(this.cancel);
                this.Controls.Add(this.panel1);
                this.Name = "Assignment Form";
                this.Text = "Assign Properities";
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
            private System.Windows.Forms.Button removeBtn;
            private System.Windows.Forms.Label variableLabel;
            private System.Windows.Forms.Label typeLabel;
            private System.Windows.Forms.ComboBox variablesBox;
            private System.Windows.Forms.Label arraySizeLabel;
            private System.Windows.Forms.TextBox expressionBox;
            private System.Windows.Forms.Button addBtn;
            private System.Windows.Forms.TextBox indexBox;
            private System.Windows.Forms.ListBox assignmentLB;
        }
    

}
