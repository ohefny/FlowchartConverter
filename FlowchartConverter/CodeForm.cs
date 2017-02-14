using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.CodeDom;
namespace TestingLast
{
    public partial class CodeForm : Form
    {
        private string cppStr;
        private string cSharpStr;
        private string selectedCode;
        private string path;
        private Compiler compiler;

        public CodeForm()
        {
            InitializeComponent();

            this.code_combo.SelectedItem = "C++";
            this.selectedCode = "C++";
            compiler = Compiler.getInstance();
        }

        public void setMeta(String cppStr, String cSharpStr)
        {
            this.cppStr = cppStr;
            this.cSharpStr = cSharpStr;
        }

        public void set(string str)
        {
            
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.ColumnCount = 0;
            int row = 0;

            using (StringReader reader = new StringReader(str))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    this.tableLayoutPanel1.RowCount++;
                    System.Windows.Forms.Label label = new System.Windows.Forms.Label();

                    label.Text = line;
                    label.Anchor = AnchorStyles.Left;
                    label.AutoSize = true;
                    label.Font = new System.Drawing.Font(label.Font.FontFamily.Name, 18);
                    this.tableLayoutPanel1.Controls.Add(label, 0, row);
                    row++;
                }
            }
            Console.ReadLine();
        }


        private void save_button_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if(selectedCode == "C++")
                saveFileDialog.Filter = "C++ Source (*.cpp)|*.cpp";
            else
                saveFileDialog.Filter = "C# Source (*.cs)|*.cs";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (selectedCode == "C++")
                {
                    using (Stream stream = File.Open(saveFileDialog.FileName, FileMode.CreateNew))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(stream))
                            streamWriter.Write(this.cppStr);
                    }
                }
                else {

                    using (Stream stream = File.Open(saveFileDialog.FileName, FileMode.CreateNew))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(stream))
                            streamWriter.Write(this.cSharpStr);
                    }
                }
            }
        }

        private void compile_button_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();

            if ( fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                path = fbd.SelectedPath;

                if (selectedCode == "C#")
                {
                    if (compiler.CompileCSharpFromSource(cSharpStr, path + "output.exe"))
                    {
                        MessageBox.Show("Compiled Successfully!");
                    }
                }

            }
            
        }

        private void run_button_Click(object sender, EventArgs e)
        {
            if (selectedCode == "C#")
            {
                string cmd = "/C output.exe";
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.WorkingDirectory = path;
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.Arguments = cmd;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.Start();
                //MessageBox.Show(proc.StandardOutput.ReadToEnd());
                proc.OutputDataReceived += (object senderr, DataReceivedEventArgs ee) =>
                MessageBox.Show(ee.Data, "Outputs");
                proc.BeginOutputReadLine();

                proc.ErrorDataReceived += (object senderr, DataReceivedEventArgs ee) =>
                    MessageBox.Show(ee.Data, "Errors");
                proc.BeginErrorReadLine();

                proc.WaitForExit();

                Console.WriteLine("ExitCode: {0}", proc.ExitCode);
                proc.Close();
            }
        }
    }
}
