namespace FlowchartConverter.Dialogs
{
    partial class DeclareDialog
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
            Crainiate.Diagramming.Forms.Paging paging3 = new Crainiate.Diagramming.Forms.Paging();
            Crainiate.Diagramming.Forms.Margin margin3 = new Crainiate.Diagramming.Forms.Margin();
            this.ok_button = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.diagram1 = new Crainiate.Diagramming.Forms.Diagram();
            this.description_label = new System.Windows.Forms.Label();
            this.name_text = new System.Windows.Forms.TextBox();
            this.name_label = new System.Windows.Forms.Label();
            this.data_label = new System.Windows.Forms.Label();
            this.type_label = new System.Windows.Forms.Label();
            this.data_box = new System.Windows.Forms.ComboBox();
            this.type_box = new System.Windows.Forms.ComboBox();
            this.size_label = new System.Windows.Forms.Label();
            this.size_text = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ok_button
            // 
            this.ok_button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok_button.Location = new System.Drawing.Point(126, 304);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(109, 37);
            this.ok_button.TabIndex = 1;
            this.ok_button.Text = "Ok";
            this.ok_button.UseVisualStyleBackColor = true;
            // 
            // cancel_button
            // 
            this.cancel_button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_button.Location = new System.Drawing.Point(241, 304);
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
            paging3.Enabled = true;
            margin3.Bottom = 0F;
            margin3.Left = 0F;
            margin3.Right = 0F;
            margin3.Top = 0F;
            paging3.Margin = margin3;
            paging3.Padding = new System.Drawing.SizeF(40F, 40F);
            paging3.Page = 1;
            paging3.PageSize = new System.Drawing.SizeF(793.7008F, 1122.52F);
            paging3.WorkspaceColor = System.Drawing.SystemColors.AppWorkspace;
            this.diagram1.Paging = paging3;
            this.diagram1.Size = new System.Drawing.Size(512, 291);
            this.diagram1.TabIndex = 1;
            this.diagram1.Zoom = 100F;
            // 
            // description_label
            // 
            this.description_label.BackColor = System.Drawing.SystemColors.Menu;
            this.description_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.description_label.Font = new System.Drawing.Font("Lucida Sans Unicode", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.description_label.Location = new System.Drawing.Point(125, 28);
            this.description_label.Name = "description_label";
            this.description_label.Padding = new System.Windows.Forms.Padding(2);
            this.description_label.Size = new System.Drawing.Size(377, 53);
            this.description_label.TabIndex = 3;
            this.description_label.Text = "A declare statement is used to create single variables and arrays. These variable" +
    " are used to store data.";
            // 
            // name_text
            // 
            this.name_text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.name_text.Font = new System.Drawing.Font("Lucida Sans Unicode", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.name_text.Location = new System.Drawing.Point(15, 160);
            this.name_text.Name = "name_text";
            this.name_text.Size = new System.Drawing.Size(487, 28);
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
            this.name_label.Location = new System.Drawing.Point(15, 130);
            this.name_label.Name = "name_label";
            this.name_label.Size = new System.Drawing.Size(207, 19);
            this.name_label.TabIndex = 5;
            this.name_label.Text = "Enter a variable name below:";
            // 
            // data_label
            // 
            this.data_label.AutoSize = true;
            this.data_label.BackColor = System.Drawing.SystemColors.Menu;
            this.data_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.data_label.Font = new System.Drawing.Font("Lucida Sans Unicode", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.data_label.Location = new System.Drawing.Point(15, 220);
            this.data_label.Name = "data_label";
            this.data_label.Size = new System.Drawing.Size(82, 19);
            this.data_label.TabIndex = 6;
            this.data_label.Text = "Data Type:";
            // 
            // type_label
            // 
            this.type_label.AutoSize = true;
            this.type_label.BackColor = System.Drawing.SystemColors.Menu;
            this.type_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.type_label.Font = new System.Drawing.Font("Lucida Sans Unicode", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.type_label.Location = new System.Drawing.Point(175, 220);
            this.type_label.Name = "type_label";
            this.type_label.Size = new System.Drawing.Size(107, 19);
            this.type_label.TabIndex = 7;
            this.type_label.Text = "Variable Type:";
            // 
            // data_box
            // 
            this.data_box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.data_box.Font = new System.Drawing.Font("Lucida Sans Unicode", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.data_box.FormattingEnabled = true;
            this.data_box.Items.AddRange(new object[] {
            "Integer",
            "Float",
            "String",
            "Bool"});
            this.data_box.Location = new System.Drawing.Point(15, 250);
            this.data_box.Name = "data_box";
            this.data_box.Size = new System.Drawing.Size(82, 24);
            this.data_box.TabIndex = 8;
            this.data_box.SelectedIndexChanged += new System.EventHandler(this.data_box_SelectedIndexChanged);
            // 
            // type_box
            // 
            this.type_box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.type_box.Font = new System.Drawing.Font("Lucida Sans Unicode", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.type_box.FormattingEnabled = true;
            this.type_box.Items.AddRange(new object[] {
            "Single",
            "Array"});
            this.type_box.Location = new System.Drawing.Point(175, 250);
            this.type_box.Name = "type_box";
            this.type_box.Size = new System.Drawing.Size(107, 24);
            this.type_box.TabIndex = 9;
            this.type_box.SelectedIndexChanged += new System.EventHandler(this.variable_box_SelectedIndexChanged);
            // 
            // size_label
            // 
            this.size_label.AutoSize = true;
            this.size_label.BackColor = System.Drawing.SystemColors.Menu;
            this.size_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.size_label.Font = new System.Drawing.Font("Lucida Sans Unicode", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.size_label.Location = new System.Drawing.Point(355, 220);
            this.size_label.Name = "size_label";
            this.size_label.Size = new System.Drawing.Size(83, 19);
            this.size_label.TabIndex = 10;
            this.size_label.Text = "Array Size:";
            // 
            // size_text
            // 
            this.size_text.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.size_text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.size_text.Enabled = false;
            this.size_text.Font = new System.Drawing.Font("Lucida Sans Unicode", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.size_text.Location = new System.Drawing.Point(355, 250);
            this.size_text.Name = "size_text";
            this.size_text.Size = new System.Drawing.Size(83, 28);
            this.size_text.TabIndex = 11;
            this.size_text.TextChanged += new System.EventHandler(this.size_text_TextChanged);
            // 
            // DeclareDialog
            // 
            this.AcceptButton = this.ok_button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.CancelButton = this.cancel_button;
            this.ClientSize = new System.Drawing.Size(512, 353);
            this.Controls.Add(this.size_text);
            this.Controls.Add(this.size_label);
            this.Controls.Add(this.type_box);
            this.Controls.Add(this.data_box);
            this.Controls.Add(this.type_label);
            this.Controls.Add(this.data_label);
            this.Controls.Add(this.name_label);
            this.Controls.Add(this.name_text);
            this.Controls.Add(this.description_label);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.ok_button);
            this.Controls.Add(this.diagram1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeclareDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Declare Properties";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Crainiate.Diagramming.Forms.Diagram diagram1;
        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.Label description_label;
        private System.Windows.Forms.TextBox name_text;
        private System.Windows.Forms.Label name_label;
        private System.Windows.Forms.Label data_label;
        private System.Windows.Forms.Label type_label;
        private System.Windows.Forms.ComboBox data_box;
        private System.Windows.Forms.ComboBox type_box;
        private System.Windows.Forms.Label size_label;
        private System.Windows.Forms.TextBox size_text;
    }
}