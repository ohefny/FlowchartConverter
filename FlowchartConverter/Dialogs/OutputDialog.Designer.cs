namespace FlowchartConverter.Dialogs
{
    partial class OutputDialog
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
            this.ok_button = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.diagram1 = new Crainiate.Diagramming.Forms.Diagram();
            this.description_label = new System.Windows.Forms.Label();
            this.expression_text = new System.Windows.Forms.TextBox();
            this.expression_label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ok_button
            // 
            this.ok_button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok_button.Location = new System.Drawing.Point(120, 307);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(109, 37);
            this.ok_button.TabIndex = 1;
            this.ok_button.Text = "Ok";
            this.ok_button.UseVisualStyleBackColor = true;
            // 
            // cancel_button
            // 
            this.cancel_button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_button.Location = new System.Drawing.Point(235, 307);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(109, 37);
            this.cancel_button.TabIndex = 2;
            this.cancel_button.Text = "Cancel";
            this.cancel_button.UseVisualStyleBackColor = true;
            // 
            // diagram1
            // 
            this.diagram1.AllowDrop = true;
            this.diagram1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.diagram1.Dock = System.Windows.Forms.DockStyle.Top;
            this.diagram1.DragElement = null;
            this.diagram1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.diagram1.GridSize = new System.Drawing.Size(15, 15);
            this.diagram1.GridStyle = Crainiate.Diagramming.GridStyle.Pixel;
            this.diagram1.Location = new System.Drawing.Point(0, 0);
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
            this.diagram1.Size = new System.Drawing.Size(484, 287);
            this.diagram1.TabIndex = 1;
            this.diagram1.Zoom = 100F;
            // 
            // description_label
            // 
            this.description_label.BackColor = System.Drawing.SystemColors.Menu;
            this.description_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.description_label.Font = new System.Drawing.Font("Lucida Sans Unicode", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.description_label.Location = new System.Drawing.Point(121, 27);
            this.description_label.Name = "description_label";
            this.description_label.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.description_label.Size = new System.Drawing.Size(351, 55);
            this.description_label.TabIndex = 3;
            this.description_label.Text = "An output statement evaluates an expression and then displays the result to the s" +
    "creen.\r\n";
            // 
            // expression_text
            // 
            this.expression_text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.expression_text.Font = new System.Drawing.Font("Lucida Sans Unicode", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expression_text.Location = new System.Drawing.Point(12, 172);
            this.expression_text.Multiline = true;
            this.expression_text.Name = "expression_text";
            this.expression_text.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.expression_text.Size = new System.Drawing.Size(460, 103);
            this.expression_text.TabIndex = 4;
            this.expression_text.TextChanged += new System.EventHandler(this.expression_text_TextChanged);
            // 
            // expression_label
            // 
            this.expression_label.AutoSize = true;
            this.expression_label.BackColor = System.Drawing.SystemColors.Menu;
            this.expression_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.expression_label.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.expression_label.Font = new System.Drawing.Font("Lucida Sans Unicode", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expression_label.Location = new System.Drawing.Point(15, 140);
            this.expression_label.Name = "expression_label";
            this.expression_label.Size = new System.Drawing.Size(196, 19);
            this.expression_label.TabIndex = 5;
            this.expression_label.Text = "Enter an expression below:";
            // 
            // OutputDialog
            // 
            this.AcceptButton = this.ok_button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.CancelButton = this.cancel_button;
            this.ClientSize = new System.Drawing.Size(484, 351);
            this.Controls.Add(this.expression_label);
            this.Controls.Add(this.expression_text);
            this.Controls.Add(this.description_label);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.ok_button);
            this.Controls.Add(this.diagram1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OutputDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Output Properties";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Crainiate.Diagramming.Forms.Diagram diagram1;
        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.Label description_label;
        private System.Windows.Forms.TextBox expression_text;
        private System.Windows.Forms.Label expression_label;
    }
}