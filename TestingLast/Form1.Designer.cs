namespace TestingLast
{
    partial class Form1
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
            Crainiate.Diagramming.Forms.Paging paging1 = new Crainiate.Diagramming.Forms.Paging();
            Crainiate.Diagramming.Forms.Margin margin1 = new Crainiate.Diagramming.Forms.Margin();
            this.diagram1 = new Crainiate.Diagramming.Forms.Diagram();
            this.button1 = new System.Windows.Forms.Button();
            this.deleteBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // diagram1
            // 
            this.diagram1.AllowDrop = true;
            this.diagram1.AutoScroll = true;
            this.diagram1.AutoScrollMinSize = new System.Drawing.Size(834, 1163);
            this.diagram1.AutoSize = true;
            this.diagram1.DragElement = null;
            this.diagram1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.diagram1.GridSize = new System.Drawing.Size(20, 20);
            this.diagram1.Location = new System.Drawing.Point(-12, 6);
            this.diagram1.Name = "diagram1";
            paging1.Enabled = true;
            margin1.Bottom = 0F;
            margin1.Left = 0F;
            margin1.Right = 0F;
            margin1.Top = 0F;
            paging1.Margin = margin1;
            paging1.Padding = new System.Drawing.SizeF(40F, 40F);
            paging1.Page = 1;
            paging1.PageSize = new System.Drawing.SizeF(793.7008F, 1122.52F);
            paging1.WorkspaceColor = System.Drawing.SystemColors.AppWorkspace;
            this.diagram1.Paging = paging1;
            this.diagram1.Size = new System.Drawing.Size(763, 557);
            this.diagram1.TabIndex = 0;
            this.diagram1.Zoom = 100F;
            
            this.diagram1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.diagram1_MouseClick_1);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(758, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(74, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Generate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // delBtn
            // 
            this.deleteBtn.Location = new System.Drawing.Point(757, 56);
            this.deleteBtn.Name = "delBtn";
            this.deleteBtn.Size = new System.Drawing.Size(75, 23);
            this.deleteBtn.TabIndex = 2;
            this.deleteBtn.Text = "Delete";
            this.deleteBtn.UseVisualStyleBackColor = true;
            this.deleteBtn.Click += new System.EventHandler(this.deleteBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(851, 546);
            this.Controls.Add(this.deleteBtn);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.diagram1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public  Crainiate.Diagramming.Forms.Diagram diagram1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button deleteBtn;
    }
}

