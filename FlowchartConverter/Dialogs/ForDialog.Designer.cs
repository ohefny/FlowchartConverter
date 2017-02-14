namespace FlowchartConverter.Dialogs
{
    partial class ForDialog
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
            Crainiate.Diagramming.Forms.Paging paging5 = new Crainiate.Diagramming.Forms.Paging();
            Crainiate.Diagramming.Forms.Margin margin5 = new Crainiate.Diagramming.Forms.Margin();
            this.ok_button = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.diagram1 = new Crainiate.Diagramming.Forms.Diagram();
            this.description_label = new System.Windows.Forms.Label();
            this.variable_text = new System.Windows.Forms.TextBox();
            this.variable_label = new System.Windows.Forms.Label();
            this.start_label = new System.Windows.Forms.Label();
            this.start_text = new System.Windows.Forms.TextBox();
            this.end_label = new System.Windows.Forms.Label();
            this.end_text = new System.Windows.Forms.TextBox();
            this.step_label = new System.Windows.Forms.Label();
            this.step_text = new System.Windows.Forms.TextBox();
            this.behaviour_label = new System.Windows.Forms.Label();
            this.behaviour_box = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // ok_button
            // 
            this.ok_button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok_button.Location = new System.Drawing.Point(128, 417);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(109, 37);
            this.ok_button.TabIndex = 1;
            this.ok_button.Text = "Ok";
            this.ok_button.UseVisualStyleBackColor = true;
            // 
            // cancel_button
            // 
            this.cancel_button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_button.Location = new System.Drawing.Point(243, 417);
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
            paging5.Enabled = true;
            margin5.Bottom = 0F;
            margin5.Left = 0F;
            margin5.Right = 0F;
            margin5.Top = 0F;
            paging5.Margin = margin5;
            paging5.Padding = new System.Drawing.SizeF(40F, 40F);
            paging5.Page = 1;
            paging5.PageSize = new System.Drawing.SizeF(793.7008F, 1122.52F);
            paging5.WorkspaceColor = System.Drawing.SystemColors.AppWorkspace;
            this.diagram1.Paging = paging5;
            this.diagram1.Size = new System.Drawing.Size(501, 391);
            this.diagram1.TabIndex = 1;
            this.diagram1.Zoom = 100F;
            // 
            // description_label
            // 
            this.description_label.BackColor = System.Drawing.SystemColors.Menu;
            this.description_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.description_label.Font = new System.Drawing.Font("Lucida Sans Unicode", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.description_label.Location = new System.Drawing.Point(140, 27);
            this.description_label.Name = "description_label";
            this.description_label.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.description_label.Size = new System.Drawing.Size(351, 47);
            this.description_label.TabIndex = 3;
            this.description_label.Text = "A for loop increments or decrements a variable through a range of values.";
            // 
            // variable_text
            // 
            this.variable_text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.variable_text.Font = new System.Drawing.Font("Lucida Sans Unicode", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.variable_text.Location = new System.Drawing.Point(15, 165);
            this.variable_text.Name = "variable_text";
            this.variable_text.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.variable_text.Size = new System.Drawing.Size(242, 28);
            this.variable_text.TabIndex = 4;
            this.variable_text.TextChanged += new System.EventHandler(this.variable_text_TextChanged);
            // 
            // variable_label
            // 
            this.variable_label.AutoSize = true;
            this.variable_label.BackColor = System.Drawing.SystemColors.Menu;
            this.variable_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.variable_label.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.variable_label.Font = new System.Drawing.Font("Lucida Sans Unicode", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.variable_label.Location = new System.Drawing.Point(15, 135);
            this.variable_label.Name = "variable_label";
            this.variable_label.Size = new System.Drawing.Size(242, 19);
            this.variable_label.TabIndex = 5;
            this.variable_label.Text = "Enter a loop variable name below:";
            // 
            // start_label
            // 
            this.start_label.AutoSize = true;
            this.start_label.BackColor = System.Drawing.SystemColors.Menu;
            this.start_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.start_label.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.start_label.Font = new System.Drawing.Font("Lucida Sans Unicode", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.start_label.Location = new System.Drawing.Point(15, 220);
            this.start_label.Name = "start_label";
            this.start_label.Size = new System.Drawing.Size(182, 19);
            this.start_label.TabIndex = 6;
            this.start_label.Text = "Enter a start value below:";
            // 
            // start_text
            // 
            this.start_text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.start_text.Font = new System.Drawing.Font("Lucida Sans Unicode", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.start_text.Location = new System.Drawing.Point(15, 250);
            this.start_text.Name = "start_text";
            this.start_text.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.start_text.Size = new System.Drawing.Size(182, 28);
            this.start_text.TabIndex = 7;
            this.start_text.TextChanged += new System.EventHandler(this.start_text_TextChanged);
            // 
            // end_label
            // 
            this.end_label.AutoSize = true;
            this.end_label.BackColor = System.Drawing.SystemColors.Menu;
            this.end_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.end_label.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.end_label.Font = new System.Drawing.Font("Lucida Sans Unicode", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.end_label.Location = new System.Drawing.Point(300, 220);
            this.end_label.Name = "end_label";
            this.end_label.Size = new System.Drawing.Size(186, 19);
            this.end_label.TabIndex = 8;
            this.end_label.Text = "Enter an end value below:";
            // 
            // end_text
            // 
            this.end_text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.end_text.Font = new System.Drawing.Font("Lucida Sans Unicode", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.end_text.Location = new System.Drawing.Point(300, 250);
            this.end_text.Name = "end_text";
            this.end_text.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.end_text.Size = new System.Drawing.Size(186, 28);
            this.end_text.TabIndex = 9;
            this.end_text.TextChanged += new System.EventHandler(this.end_text_TextChanged);
            // 
            // step_label
            // 
            this.step_label.AutoSize = true;
            this.step_label.BackColor = System.Drawing.SystemColors.Menu;
            this.step_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.step_label.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.step_label.Font = new System.Drawing.Font("Lucida Sans Unicode", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.step_label.Location = new System.Drawing.Point(15, 310);
            this.step_label.Name = "step_label";
            this.step_label.Size = new System.Drawing.Size(64, 19);
            this.step_label.TabIndex = 10;
            this.step_label.Text = "Step by:";
            // 
            // step_text
            // 
            this.step_text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.step_text.Font = new System.Drawing.Font("Lucida Sans Unicode", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.step_text.Location = new System.Drawing.Point(15, 340);
            this.step_text.Name = "step_text";
            this.step_text.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.step_text.Size = new System.Drawing.Size(153, 28);
            this.step_text.TabIndex = 11;
            this.step_text.TextChanged += new System.EventHandler(this.step_text_TextChanged);
            // 
            // behaviour_label
            // 
            this.behaviour_label.AutoSize = true;
            this.behaviour_label.BackColor = System.Drawing.SystemColors.Menu;
            this.behaviour_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.behaviour_label.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.behaviour_label.Font = new System.Drawing.Font("Lucida Sans Unicode", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.behaviour_label.Location = new System.Drawing.Point(300, 310);
            this.behaviour_label.Name = "behaviour_label";
            this.behaviour_label.Size = new System.Drawing.Size(82, 19);
            this.behaviour_label.TabIndex = 12;
            this.behaviour_label.Text = "Behaviour:";
            // 
            // behaviour_box
            // 
            this.behaviour_box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.behaviour_box.Font = new System.Drawing.Font("Lucida Sans Unicode", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.behaviour_box.FormattingEnabled = true;
            this.behaviour_box.Items.AddRange(new object[] {
            "Increment",
            "Decrement"});
            this.behaviour_box.Location = new System.Drawing.Point(300, 340);
            this.behaviour_box.Name = "behaviour_box";
            this.behaviour_box.Size = new System.Drawing.Size(96, 24);
            this.behaviour_box.TabIndex = 13;
            this.behaviour_box.SelectedIndexChanged += new System.EventHandler(this.behaviour_box_SelectedIndexChanged);
            // 
            // ForLoopDialog
            // 
            this.AcceptButton = this.ok_button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.CancelButton = this.cancel_button;
            this.ClientSize = new System.Drawing.Size(501, 461);
            this.Controls.Add(this.behaviour_box);
            this.Controls.Add(this.behaviour_label);
            this.Controls.Add(this.step_text);
            this.Controls.Add(this.step_label);
            this.Controls.Add(this.end_text);
            this.Controls.Add(this.end_label);
            this.Controls.Add(this.start_text);
            this.Controls.Add(this.start_label);
            this.Controls.Add(this.variable_label);
            this.Controls.Add(this.variable_text);
            this.Controls.Add(this.description_label);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.ok_button);
            this.Controls.Add(this.diagram1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ForLoopDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Decision Properties";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Crainiate.Diagramming.Forms.Diagram diagram1;
        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.Label description_label;
        private System.Windows.Forms.TextBox variable_text;
        private System.Windows.Forms.Label variable_label;
        private System.Windows.Forms.Label start_label;
        private System.Windows.Forms.TextBox start_text;
        private System.Windows.Forms.Label end_label;
        private System.Windows.Forms.TextBox end_text;
        private System.Windows.Forms.Label step_label;
        private System.Windows.Forms.TextBox step_text;
        private System.Windows.Forms.Label behaviour_label;
        private System.Windows.Forms.ComboBox behaviour_box;
    }
}