namespace FlowchartConverter
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
            Crainiate.Diagramming.Forms.Paging paging2 = new Crainiate.Diagramming.Forms.Paging();
            Crainiate.Diagramming.Forms.Margin margin2 = new Crainiate.Diagramming.Forms.Margin();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.diagram1 = new Crainiate.Diagramming.Forms.Diagram();
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.open_button = new System.Windows.Forms.ToolStripButton();
            this.save_button = new System.Windows.Forms.ToolStripButton();
            this.toolSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.move_button = new System.Windows.Forms.ToolStripButton();
            this.clear_button = new System.Windows.Forms.ToolStripButton();
            this.delete_button = new System.Windows.Forms.ToolStripButton();
            this.toolSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.sourceCodeButton = new System.Windows.Forms.ToolStripButton();
            this.drawPanel = new System.Windows.Forms.Panel();
            this.export_button = new System.Windows.Forms.ToolStripButton();
            this.toolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // diagram1
            // 
            this.diagram1.AllowDrop = true;
            this.diagram1.AutoScroll = true;
            this.diagram1.AutoScrollMinSize = new System.Drawing.Size(6357, 8988);
            this.diagram1.AutoSize = true;
            this.diagram1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.diagram1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagram1.DragElement = null;
            this.diagram1.DragScroll = false;
            this.diagram1.GridColor = System.Drawing.Color.White;
            this.diagram1.GridSize = new System.Drawing.Size(20, 20);
            this.diagram1.Location = new System.Drawing.Point(0, 0);
            this.diagram1.Name = "diagram1";
            paging2.Enabled = true;
            margin2.Bottom = 0F;
            margin2.Left = 0F;
            margin2.Right = 0F;
            margin2.Top = 0F;
            paging2.Margin = margin2;
            paging2.Padding = new System.Drawing.SizeF(0F, 0F);
            paging2.Page = 1;
            paging2.PageSize = new System.Drawing.SizeF(6357.166F, 8987.717F);
            paging2.WorkspaceColor = System.Drawing.SystemColors.AppWorkspace;
            this.diagram1.Paging = paging2;
            this.diagram1.Size = new System.Drawing.Size(1120, 681);
            this.diagram1.TabIndex = 0;
            this.diagram1.Zoom = 100F;
            this.diagram1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.diagram1_MouseClick_1);
            // 
            // toolBar
            // 
            this.toolBar.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.toolBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolBar.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.open_button,
            this.save_button,
            this.export_button,
            this.toolSeparator2,
            this.move_button,
            this.clear_button,
            this.delete_button,
            this.toolSeparator3,
            this.sourceCodeButton});
            this.toolBar.Location = new System.Drawing.Point(1120, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.Padding = new System.Windows.Forms.Padding(5);
            this.toolBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolBar.Size = new System.Drawing.Size(64, 681);
            this.toolBar.Stretch = true;
            this.toolBar.TabIndex = 0;
            this.toolBar.Text = "Tool Bar";
            // 
            // open_button
            // 
            this.open_button.AutoSize = false;
            this.open_button.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.open_button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.open_button.Image = ((System.Drawing.Image)(resources.GetObject("open_button.Image")));
            this.open_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.open_button.Margin = new System.Windows.Forms.Padding(5, 5, 5, 15);
            this.open_button.Name = "open_button";
            this.open_button.Size = new System.Drawing.Size(40, 40);
            this.open_button.Text = "Open";
            this.open_button.Click += new System.EventHandler(this.open_button_Click);
            // 
            // save_button
            // 
            this.save_button.AutoSize = false;
            this.save_button.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.save_button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.save_button.Image = ((System.Drawing.Image)(resources.GetObject("save_button.Image")));
            this.save_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.save_button.Margin = new System.Windows.Forms.Padding(5, 5, 5, 15);
            this.save_button.Name = "save_button";
            this.save_button.Size = new System.Drawing.Size(40, 40);
            this.save_button.Text = "Save";
            this.save_button.Click += new System.EventHandler(this.save_button_Click);
            // 
            // toolSeparator2
            // 
            this.toolSeparator2.AutoSize = false;
            this.toolSeparator2.Margin = new System.Windows.Forms.Padding(5, 5, 5, 15);
            this.toolSeparator2.Name = "toolSeparator2";
            this.toolSeparator2.Size = new System.Drawing.Size(40, 10);
            // 
            // move_button
            // 
            this.move_button.AutoSize = false;
            this.move_button.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.move_button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.move_button.Image = ((System.Drawing.Image)(resources.GetObject("move_button.Image")));
            this.move_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.move_button.Margin = new System.Windows.Forms.Padding(5, 5, 5, 15);
            this.move_button.Name = "move_button";
            this.move_button.Size = new System.Drawing.Size(40, 40);
            this.move_button.Text = "Move";
            this.move_button.Click += new System.EventHandler(this.move_button_Click);
            // 
            // clear_button
            // 
            this.clear_button.AutoSize = false;
            this.clear_button.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.clear_button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.clear_button.Image = ((System.Drawing.Image)(resources.GetObject("clear_button.Image")));
            this.clear_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.clear_button.Margin = new System.Windows.Forms.Padding(5, 5, 5, 15);
            this.clear_button.Name = "clear_button";
            this.clear_button.Size = new System.Drawing.Size(44, 44);
            this.clear_button.Text = "Clear";
            this.clear_button.Click += new System.EventHandler(this.clear_button_Click);
            // 
            // delete_button
            // 
            this.delete_button.AutoSize = false;
            this.delete_button.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.delete_button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.delete_button.Image = ((System.Drawing.Image)(resources.GetObject("delete_button.Image")));
            this.delete_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.delete_button.Margin = new System.Windows.Forms.Padding(5, 5, 5, 15);
            this.delete_button.Name = "delete_button";
            this.delete_button.Size = new System.Drawing.Size(44, 44);
            this.delete_button.Text = "Delete";
            this.delete_button.Click += new System.EventHandler(this.delete_button_Click);
            // 
            // toolSeparator3
            // 
            this.toolSeparator3.AutoSize = false;
            this.toolSeparator3.Margin = new System.Windows.Forms.Padding(5, 5, 5, 15);
            this.toolSeparator3.Name = "toolSeparator3";
            this.toolSeparator3.Size = new System.Drawing.Size(40, 10);
            // 
            // sourceCodeButton
            // 
            this.sourceCodeButton.AutoSize = false;
            this.sourceCodeButton.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.sourceCodeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.sourceCodeButton.Image = ((System.Drawing.Image)(resources.GetObject("sourceCodeButton.Image")));
            this.sourceCodeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.sourceCodeButton.Margin = new System.Windows.Forms.Padding(5, 5, 5, 15);
            this.sourceCodeButton.Name = "sourceCodeButton";
            this.sourceCodeButton.Size = new System.Drawing.Size(40, 40);
            this.sourceCodeButton.Text = "Source Code";
            this.sourceCodeButton.Click += new System.EventHandler(this.sourceCodeButton_Click);
            // 
            // drawPanel
            // 
            this.drawPanel.AutoScroll = true;
            this.drawPanel.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.drawPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drawPanel.Location = new System.Drawing.Point(0, 0);
            this.drawPanel.Name = "drawPanel";
            this.drawPanel.Size = new System.Drawing.Size(1120, 681);
            this.drawPanel.TabIndex = 1;
            // 
            // export_button
            // 
            this.export_button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.export_button.Image = ((System.Drawing.Image)(resources.GetObject("export_button.Image")));
            this.export_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.export_button.Margin = new System.Windows.Forms.Padding(10, 1, 0, 15);
            this.export_button.Name = "export_button";
            this.export_button.Size = new System.Drawing.Size(33, 44);
            this.export_button.Text = "Export";
            this.export_button.Click += new System.EventHandler(this.export_button_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1184, 681);
            this.Controls.Add(this.diagram1);
            this.Controls.Add(this.drawPanel);
            this.Controls.Add(this.toolBar);
            this.Name = "Form1";
            this.Text = "Form1";
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public  Crainiate.Diagramming.Forms.Diagram diagram1;

        private System.Windows.Forms.ToolStrip toolBar;
        private System.Windows.Forms.ToolStripButton open_button;
        private System.Windows.Forms.ToolStripButton save_button;
        private System.Windows.Forms.ToolStripSeparator toolSeparator2;
        private System.Windows.Forms.ToolStripButton sourceCodeButton;
        private System.Windows.Forms.ToolStripSeparator toolSeparator3;
        private System.Windows.Forms.ToolStripButton move_button;
        private System.Windows.Forms.ToolStripButton clear_button;
        private System.Windows.Forms.ToolStripButton delete_button;
        private System.Windows.Forms.Panel drawPanel;
        private System.Windows.Forms.ToolStripButton export_button;
    }
}

