using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestingLast
{
    class Compiler
    {
        private static Compiler instance;

        private Compiler() { }

        public static Compiler getInstance()
        {
            if (instance == null)
                instance = new Compiler();
            return instance;
        }

        public bool CompileCSharpFromSource(String source, String output, string icon = null, string[] resources = null)
        {
            // We declare the new compiler parameters variable
            // that will contain all settings for the compilation.
            CompilerParameters CSharpParams = new CompilerParameters();

            // We want an executable file on disk.
            CSharpParams.GenerateExecutable = true;
            // This is where the compiled file will be saved into.
            CSharpParams.OutputAssembly = output;

            // Save the assembly as a physical file.
            CSharpParams.GenerateInMemory = true;

            // We need these compiler options, we will use code optimization,
            // compile as a x86 process and our target is a windows form.
            // The unsafe keyword is used because the stub contains pointers and
            // unsafe blocks of code.
            string options = "/optimize+ /platform:x86 /unsafe";
            // If the icon is not null (as we initialize it), add the corresponding option.
            if (icon != null)
                options += " /win32icon:\"" + icon + "\"";

            // Set the options.
            CSharpParams.CompilerOptions = options;
            // We don't care about warnings, we don't need them to show as errors.
            CSharpParams.TreatWarningsAsErrors = false;

            // Add the references to the libraries we use so we can have access
            // to their namespaces.
            CSharpParams.ReferencedAssemblies.Add("System.dll");
            CSharpParams.ReferencedAssemblies.Add("System.Data.dll");


            // Check if the user specified any resource files.
            // If yes, add then to the stub's resources.
            if (resources != null && resources.Length > 0)
            {
                // Loop through all resource files specified in the Resources[] array.
                foreach (string res in resources)
                {
                    // Add each resource file to the compiled stub.
                    CSharpParams.EmbeddedResources.Add(res);
                }
            }

            // Dictionary variable is used to tell the compiler that we want
            // our file to be compiled for .NET v2
            Dictionary<string, string> ProviderOptions = new Dictionary<string, string>();
            ProviderOptions.Add("CompilerVersion", "v2.0");

            // Now, we compile the code and get the result back in the "Results" variable
            CompilerResults Results = new CSharpCodeProvider(ProviderOptions).CompileAssemblyFromSource(CSharpParams, source);

            // Check if any errors occured while compiling.
            if (Results.Errors.Count > 0)
            {
                // Errors occured, notify the user.
                MessageBox.Show(string.Format("The compiler has encountered {0} errors",
                    Results.Errors.Count), "Errors while compiling", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                // Now loop through all errors and show them to the user.
                foreach (CompilerError Err in Results.Errors)
                {
                    MessageBox.Show(string.Format("{0}\nLine: {1} - Column: {2}\nFile: {3}", Err.ErrorText,
                        Err.Line, Err.Column, Err.FileName), "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;

            }
            else
            {
                // No error was found, return true.
                return true;
            }
        }

        public bool CompileCPlusFromSource(String source, String output, string icon = null, string[] resources = null)
        {
            // We declare the new compiler parameters variable
            // that will contain all settings for the compilation.
            CompilerParameters CPlusParams = new CompilerParameters();

            // We want an executable file on disk.
            CPlusParams.GenerateExecutable = true;
            // This is where the compiled file will be saved into.
            CPlusParams.OutputAssembly = output;

            // Save the assembly as a physical file.
            CPlusParams.GenerateInMemory = true;

            // We need these compiler options, we will use code optimization,
            // compile as a x86 process and our target is a windows form.
            // The unsafe keyword is used because the stub contains pointers and
            // unsafe blocks of code.
            string options = "/optimize+ /platform:x86 /unsafe";
            // If the icon is not null (as we initialize it), add the corresponding option.
            if (icon != null)
                options += " /win32icon:\"" + icon + "\"";

            // Set the options.
            CPlusParams.CompilerOptions = options;
            // We don't care about warnings, we don't need them to show as errors.
            CPlusParams.TreatWarningsAsErrors = false;

            // Add the references to the libraries we use so we can have access
            // to their namespaces.
            CPlusParams.ReferencedAssemblies.Add("System.dll");


            // Check if the user specified any resource files.
            // If yes, add then to the stub's resources.
            if (resources != null && resources.Length > 0)
            {
                // Loop through all resource files specified in the Resources[] array.
                foreach (string res in resources)
                {
                    // Add each resource file to the compiled stub.
                    CPlusParams.EmbeddedResources.Add(res);
                }
            }
            CodeDomProvider pro = CodeDomProvider.CreateProvider("cpp");

            // Now, we compile the code and get the result back in the "Results" variable
            CompilerResults Results = pro.CompileAssemblyFromSource(CPlusParams, source);

            // Check if any errors occured while compiling.
            if (Results.Errors.Count > 0)
            {
                // Errors occured, notify the user.
                MessageBox.Show(string.Format("The compiler has encountered {0} errors",
                    Results.Errors.Count), "Errors while compiling", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                // Now loop through all errors and show them to the user.
                foreach (CompilerError Err in Results.Errors)
                {
                    MessageBox.Show(string.Format("{0}\nLine: {1} - Column: {2}\nFile: {3}", Err.ErrorText,
                        Err.Line, Err.Column, Err.FileName), "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;

            }
            else
            {
                // No error was found, return true.
                return true;
            }
        }
    }
}
