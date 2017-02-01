using System;
using TestingLast.Properties;

namespace DrawShapes.Dialogs
{
    partial class PickDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.okBtn = new System.Windows.Forms.Button();
            this.forImg = new System.Windows.Forms.PictureBox();
            this.outputImg = new System.Windows.Forms.PictureBox();
            this.doImg = new System.Windows.Forms.PictureBox();
            this.assignImg = new System.Windows.Forms.PictureBox();
            this.inputImg = new System.Windows.Forms.PictureBox();
            this.ifImg = new System.Windows.Forms.PictureBox();
            this.whileImg = new System.Windows.Forms.PictureBox();
            this.declareImg = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.forImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.outputImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.doImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.assignImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inputImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ifImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.whileImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.declareImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Input / Output";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 188);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "If";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(157, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Declare / Assign";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(315, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Loop";
            // 
            // cancelBtn
            // 
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(284, 287);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(100, 37);
            this.cancelBtn.TabIndex = 13;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // okBtn
            // 
            this.okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBtn.Location = new System.Drawing.Point(146, 287);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(99, 37);
            this.okBtn.TabIndex = 14;
            this.okBtn.Text = "Ok";
            this.okBtn.UseVisualStyleBackColor = true;
            // 
            // forImg
            // 
            this.forImg.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.forImg.BackgroundImage = global::TestingLast.Properties.Resources._for;
            this.forImg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.forImg.Location = new System.Drawing.Point(284, 188);
            this.forImg.Name = "forImg";
            this.forImg.Size = new System.Drawing.Size(100, 50);
            this.forImg.TabIndex = 12;
            this.forImg.TabStop = false;
            this.forImg.Click += new System.EventHandler(this.onClick);
            // 
            // outputImg
            // 
            this.outputImg.BackgroundImage = global::TestingLast.Properties.Resources.outputD;
            this.outputImg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.outputImg.Location = new System.Drawing.Point(3, 118);
            this.outputImg.Name = "outputImg";
            this.outputImg.Size = new System.Drawing.Size(100, 50);
            this.outputImg.TabIndex = 11;
            this.outputImg.TabStop = false;
            this.outputImg.Click += new System.EventHandler(this.onClick);
            // 
            // doImg
            // 
            this.doImg.BackgroundImage = global::TestingLast.Properties.Resources.DowhileD;
            this.doImg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.doImg.Location = new System.Drawing.Point(284, 118);
            this.doImg.Name = "doImg";
            this.doImg.Size = new System.Drawing.Size(100, 50);
            this.doImg.TabIndex = 9;
            this.doImg.TabStop = false;
            this.doImg.Click += new System.EventHandler(this.onClick);
            // 
            // assignImg
            // 
            this.assignImg.BackgroundImage = global::TestingLast.Properties.Resources.assignD;
            this.assignImg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.assignImg.Location = new System.Drawing.Point(146, 188);
            this.assignImg.Name = "assignImg";
            this.assignImg.Size = new System.Drawing.Size(100, 50);
            this.assignImg.TabIndex = 8;
            this.assignImg.TabStop = false;
            this.assignImg.Click += new System.EventHandler(this.onClick);
            // 
            // inputImg
            // 
            this.inputImg.BackgroundImage = global::TestingLast.Properties.Resources.inputD;
            this.inputImg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.inputImg.Location = new System.Drawing.Point(3, 51);
            this.inputImg.Name = "inputImg";
            this.inputImg.Size = new System.Drawing.Size(100, 50);
            this.inputImg.TabIndex = 7;
            this.inputImg.TabStop = false;
            this.inputImg.Click += new System.EventHandler(this.onClick);
            // 
            // ifImg
            // 
            this.ifImg.BackgroundImage = global::TestingLast.Properties.Resources.ifD;
            this.ifImg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ifImg.Location = new System.Drawing.Point(3, 215);
            this.ifImg.Name = "ifImg";
            this.ifImg.Size = new System.Drawing.Size(100, 50);
            this.ifImg.TabIndex = 6;
            this.ifImg.TabStop = false;
            this.ifImg.Click += new System.EventHandler(this.onClick);
            // 
            // whileImg
            // 
            this.whileImg.BackgroundImage = global::TestingLast.Properties.Resources._while;
            this.whileImg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.whileImg.Location = new System.Drawing.Point(284, 51);
            this.whileImg.Name = "whileImg";
            this.whileImg.Size = new System.Drawing.Size(100, 50);
            this.whileImg.TabIndex = 5;
            this.whileImg.TabStop = false;
            this.whileImg.Click += new System.EventHandler(this.onClick);
            // 
            // declareImg
            // 
            this.declareImg.BackgroundImage = global::TestingLast.Properties.Resources.declareD;
            this.declareImg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.declareImg.Location = new System.Drawing.Point(146, 118);
            this.declareImg.Name = "declareImg";
            this.declareImg.Size = new System.Drawing.Size(100, 50);
            this.declareImg.TabIndex = 4;
            this.declareImg.TabStop = false;
            this.declareImg.Click += new System.EventHandler(this.onClick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::TestingLast.Properties.Resources.ifD;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(3, 287);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // PickDialog
            // 
            this.AcceptButton = this.okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(400, 343);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.forImg);
            this.Controls.Add(this.outputImg);
            this.Controls.Add(this.doImg);
            this.Controls.Add(this.assignImg);
            this.Controls.Add(this.inputImg);
            this.Controls.Add(this.ifImg);
            this.Controls.Add(this.whileImg);
            this.Controls.Add(this.declareImg);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PickDialog";
            this.Text = "PickDialog";
            ((System.ComponentModel.ISupportInitialize)(this.forImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.outputImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.doImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.assignImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inputImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ifImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.whileImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.declareImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

       

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox declareImg;
        private System.Windows.Forms.PictureBox whileImg;
        private System.Windows.Forms.PictureBox ifImg;
        private System.Windows.Forms.PictureBox inputImg;
        private System.Windows.Forms.PictureBox outputImg;
        private System.Windows.Forms.PictureBox doImg;
        private System.Windows.Forms.PictureBox assignImg;
        private System.Windows.Forms.PictureBox forImg;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}