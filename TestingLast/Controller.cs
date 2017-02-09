using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crainiate.Diagramming;
using TestingLast.Nodes;
using TestingLast.CodeGeneration;
using TestingLast.Project_Save;

namespace TestingLast
{
    public class Controller
    {
        private Model model;
        private List<BaseNode> nodes = new List<BaseNode>();
        private TerminalNode terminalE;
        private TerminalNode terminalS;
        private bool deleteChoosed;
        private bool openDialogs;

        public bool DeleteChoosed
        {
            get
            {
                return deleteChoosed;
            }

            set
            {
                deleteChoosed = value;
            }
        }

        public bool OpenDialogs
        {
            get
            {
                return openDialogs;
            }

            set
            {
                openDialogs = value;
            }
        }

        public  enum Language { CPP,CSHARP };

        public Controller(Model model)
        {
            this.model = model;
            initializeProject();
            new OutputNode();
            
        }
        public void initializeProject()
        {

            model.Clear();
            nodes.Clear();
            BaseNode.Controller = this;
            BaseNode.Model = model;
            ConnectorNode.Controller = this;
            terminalS = new TerminalNode(TerminalNode.TerminalType.Start);
            terminalE = new TerminalNode(TerminalNode.TerminalType.End);
            terminalS.attachNode(terminalE);
            terminalE.ParentNode = terminalS;
            terminalS.addToModel();
            terminalE.addToModel();
        }

        internal void saveProject(string path, string fileName)
        {
            Project_Saver ps = new Project_Saver(terminalS, terminalE, path, fileName);
        }

        public string getCode(Language lang)
        {
            switch (lang) {
                case Language.CPP:
                    return FlowChartCodeGenerator.getCppCode(terminalS, terminalE);
                    break;
                case Language.CSHARP:
                    return "";
                    break;
            }
            return null;
        }

        internal void cancelClickedButtons()
        {

            DeleteChoosed = false;
            OpenDialogs = false;
        }

        internal void loadProject(string path)
        {
            initializeProject();
            Project_Loader projectLoader = new Project_Loader(terminalS, terminalE, path);
        }

        
    }
}
