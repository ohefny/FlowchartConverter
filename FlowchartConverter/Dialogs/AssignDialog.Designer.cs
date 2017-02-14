namespace FlowchartConverter.Dialogs
{
    partial class AssignDialog
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
            this.ok_button = new System.Windows.Forms.Button();
            this.cancel_btton = new System.Windows.Forms.Button();
            this.diagram1 = new Crainiate.Diagramming.Forms.Diagram();
            this.description_label = new System.Windows.Forms.Label();
            this.name_text = new System.Windows.Forms.TextBox();
            this.name_label = new System.Windows.Forms.Label();
            this.expression_label = new System.Windows.Forms.Label();
            this.expression_text = new System.Windows.Forms.TextBox();
            this.equal_label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ok_button
            // 
            this.ok_button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok_button.Location = new System.Drawing.Point(121, 307);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(109, 37);
            this.ok_button.TabIndex = 1;
            this.ok_button.Text = "Ok";
            this.ok_button.UseVisualStyleBackColor = true;
            // 
            // cancel_btton
            // 
            this.cancel_btton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_btton.Location = new System.Drawing.Point(236, 307);
            this.cancel_btton.Name = "cancel_btton";
            this.cancel_btton.Size = new System.Drawing.Size(109, 37);
            this.cancel_btton.TabIndex = 2;
            this.cancel_btton.Text = "Cancel";
            this.cancel_btton.UseVisualStyleBackColor = true;
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
            paging2.Enabled = true;
            margin2.Bottom = 0F;
            margin2.Left = 0F;
            margin2.Right = 0F;
            margin2.Top = 0F;
            paging2.Margin = margin2;
            paging2.Padding = new System.Drawing.SizeF(40F, 40F);
            paging2.Page = 1;
            paging2.PageSize = new System.Drawing.SizeF(793.7008F, 1122.52F);
            paging2.WorkspaceColor = System.Drawing.SystemColors.AppWorkspace;
            this.diagram1.Paging = paging2;
            this.diagram1.Size = new System.Drawing.Size(490, 301);
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
            this.description_label.Text = "An input statement reads a value from the keyboard and stores the result in a var" +
    "iable.";
            // 
            // name_text
            // 
            this.name_text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.name_text.Font = new System.Drawing.Font("Lucida Sans Unicode", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.name_text.Location = new System.Drawing.Point(12, 200);
            this.name_text.Name = "name_text";
            this.name_text.Size = new System.Drawing.Size(115, 28);
            this.name_text.TabIndex = 4;
            this.name_text.TextChanged += new System.EventHandler(this.name_text_TextChanged);
            // 
            // name_label
            // 
            this.name_label.AutoSize = true;
            this.name_label.BackColor = System.Drawing.SystemColors.Menu;
            this.name_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.name_label.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.name_label.Font = new System.Drawing.Font("Lucida Sans Unicode", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.name_label.Location = new System.Drawing.Point(12, 170);
            this.name_label.Name = "name_label";
            this.name_label.Size = new System.Drawing.Size(112, 19);
            this.name_label.TabIndex = 5;
            this.name_label.Text = "Variable name:";
            // 
            // expression_label
            // 
            this.expression_label.AutoSize = true;
            this.expression_label.BackColor = System.Drawing.SystemColors.Menu;
            this.expression_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.expression_label.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.expression_label.Font = new System.Drawing.Font("Lucida Sans Unicode", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expression_label.Location = new System.Drawing.Point(237, 140);
            this.expression_label.Name = "expression_label";
            this.expression_label.Size = new System.Drawing.Size(132, 19);
            this.expression_label.TabIndex = 6;
            this.expression_label.Text = "Value expression:";
            // 
            // expression_text
            // 
            this.expression_text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.expression_text.Font = new System.Drawing.Font("Lucida Sans Unicode", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expression_text.Location = new System.Drawing.Point(236, 170);
            this.expression_text.Multiline = true;
            this.expression_text.Name = "expression_text";
            this.expression_text.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.expression_text.Size = new System.Drawing.Size(236, 116);
            this.expression_text.TabIndex = 7;
            this.expression_text.TextChanged += new System.EventHandler(this.expression_text_TextChanged);
            // 
            // equal_label
            // 
            this.equal_label.AutoSize = true;
            this.equal_label.BackColor = System.Drawing.Color.Transparent;
            this.equal_label.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.equal_label.Font = new System.Drawing.Font("Lucida Sans Unicode", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.equal_label.Location = new System.Drawing.Point(140, 185);
            this.equal_label.Name = "equal_label";
            this.equal_label.Size = new System.Drawing.Size(67, 48);
            this.equal_label.TabIndex = 8;
            this.equal_label.Text = " =";
            this.equal_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Assign
            // 
            this.AcceptButton = this.ok_button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.CancelButton = this.cancel_btton;
            this.ClientSize = new System.Drawing.Size(490, 355);
            this.Controls.Add(this.equal_label);
            this.Controls.Add(this.expression_text);
            this.Controls.Add(this.expression_label);
            this.Controls.Add(this.name_label);
            this.Controls.Add(this.name_text);
            this.Controls.Add(this.description_label);
            this.Controls.Add(this.cancel_btton);
            this.Controls.Add(this.ok_button);
            this.Controls.Add(this.diagram1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Assign";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Assign Properties";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Crainiate.Diagramming.Forms.Diagram diagram1;
        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.Button cancel_btton;
        private System.Windows.Forms.Label description_label;
        private System.Windows.Forms.TextBox name_text;
        private System.Windows.Forms.Label name_label;
        private System.Windows.Forms.Label expression_label;
        private System.Windows.Forms.TextBox expression_text;
        private System.Windows.Forms.Label equal_label;
    }
}