using System;
using System.Windows.Forms;
using System.CodeDom;
namespace TestingLast
{
    partial class CodeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.save_button = new System.Windows.Forms.ToolStripButton();
            this.run_button = new System.Windows.Forms.ToolStripButton();
            this.compile_button = new System.Windows.Forms.ToolStripButton();
            this.code_combo = new System.Windows.Forms.ToolStripComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.save_button,
            this.run_button,
            this.compile_button,
            this.code_combo});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(675, 50);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // save_button
            // 
            this.save_button.AutoSize = false;
            this.save_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.save_button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.save_button.Image = ((System.Drawing.Image)(resources.GetObject("save_button.Image")));
            this.save_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.save_button.Margin = new System.Windows.Forms.Padding(5, 5, 25, 5);
            this.save_button.Name = "save_button";
            this.save_button.Size = new System.Drawing.Size(40, 40);
            this.save_button.Text = "Save";
            this.save_button.Click += new System.EventHandler(this.save_button_Click);
            // 
            // run_button
            // 
            this.run_button.AutoSize = false;
            this.run_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.run_button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.run_button.Image = ((System.Drawing.Image)(resources.GetObject("run_button.Image")));
            this.run_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.run_button.Margin = new System.Windows.Forms.Padding(5, 5, 25, 5);
            this.run_button.Name = "run_button";
            this.run_button.Size = new System.Drawing.Size(40, 40);
            this.run_button.Text = "Run";
            this.run_button.Click += new System.EventHandler(this.run_button_Click);
            // 
            // compile_button
            // 
            this.compile_button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.compile_button.Image = ((System.Drawing.Image)(resources.GetObject("compile_button.Image")));
            this.compile_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.compile_button.Margin = new System.Windows.Forms.Padding(0, 1, 25, 2);
            this.compile_button.Name = "compile_button";
            this.compile_button.Size = new System.Drawing.Size(44, 47);
            this.compile_button.Text = "Compile";
            this.compile_button.Click += new System.EventHandler(this.compile_button_Click);
            // 
            // code_combo
            // 
            this.code_combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.code_combo.Font = new System.Drawing.Font("Lucida Sans Unicode", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.code_combo.Items.AddRange(new object[] {
            "C#",
            "C++"});
            this.code_combo.Margin = new System.Windows.Forms.Padding(1, 0, 25, 0);
            this.code_combo.Name = "code_combo";
            this.code_combo.Size = new System.Drawing.Size(121, 50);
            this.code_combo.SelectedIndexChanged += new System.EventHandler(this.code_combo_SelectedIndexChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 50);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(675, 567);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // CodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(675, 617);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "CodeForm";
            this.Text = "CodeForm";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void code_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cppStr == null)
                return;
            ToolStripComboBox comboBox = (ToolStripComboBox)sender;
            string code = (string)comboBox.SelectedItem;
            if (code.Equals("C#"))
            {
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.RowStyles.Clear();
                selectedCode = "C#";
                this.set(this.cSharpStr);
            }
            else
            {

                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.RowStyles.Clear();
                selectedCode = "C++";
                this.set(this.cppStr);
            }
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton save_button;
        private System.Windows.Forms.ToolStripButton run_button;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStripButton compile_button;
        private System.Windows.Forms.ToolStripComboBox code_combo;
    }
}