using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crainiate.Diagramming;
using TestingLast.Nodes;
using TestingLast.CodeGeneration;
using TestingLast.Project_Save;
using Crainiate.Diagramming.Forms;

namespace TestingLast
{
    public class Controller
    {
        private Model model;
        private List<BaseNode> nodes = new List<BaseNode>();
        private TerminalNode terminalE;
        private TerminalNode terminalS;
        private bool deleteChoosed;
        private bool allowMove;
        private Diagram diagram;
        
        private BaseNode nodeInitiateRemoving;
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

        public bool AllowMove
        {
            get
            {
                return allowMove;
            }

            set
            {
                allowMove = value;
            }
        }

        public List<BaseNode> Nodes
        {
            get
            {
                return nodes;
            }

            set
            {
                nodes = value;
            }
        }
        
        public bool LoadingProject { get; internal set; }

        

        public  enum Language { CPP,CSHARP };

      

        public Controller(Diagram diagram1)
        {
            this.diagram = diagram1;
            this.model = diagram1.Model;
           // diagram1.Controller.Model.Elements.SetModifiable(true);
            initializeProject();
            new OutputNode();
        }

        public void initializeProject()
        {

            model.Clear();
            Nodes.Clear();
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

        internal void newProject()
        {
            initializeProject();
            diagram.Controller.Refresh();
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
                    return FlowChartCodeGenerator.getCsCode(terminalS, terminalE);
                    break;
            }
            return null;
        }

        internal void cancelClickedButtons()
        {

            DeleteChoosed = false;
            AllowMove = false;
        }

        internal void loadProject(string path)
        {
            LoadingProject = true;
            initializeProject();
            Project_Loader projectLoader = new Project_Loader(terminalS, terminalE, path);
            //redrawNodes();
            diagram.Controller.Refresh();
            LoadingProject = false;
            
        }

        internal void redrawNodes()
        {

            model.Clear();

            foreach (BaseNode node in Nodes)
            {
                if (node is HolderNode) continue;
                node.addToModel();

            }
            diagram.Controller.Refresh();
        }
        //BalanceNodes methods invoked to correct any conflicts or overlapping in the mode  
        internal void balanceNodes(BaseNode newNode)
        {
            BaseNode trackNode = null;

            do
            {
                if (trackNode == null)
                    trackNode = newNode.ParentNode;
                else
                    trackNode = trackNode.ParentNode;
                if (newNode is IfElseNode) {
                    //this is the case when adding in the false part of ifelse that is right to main track
                    if (trackNode.NodeLocation.X < newNode.NodeLocation.X
                     && (newNode as IfElseNode).FalseNode.NodeLocation.X <= trackNode.NodeLocation.X + trackNode.Shape.Width)
                    {
                        
                        addNode(newNode);
                        shiftNodesRight(newNode,false);
                        
                    }
                }
                if (newNode is DecisionNode)
                {
                    //this is the case when adding to the true part of Decision that is left to main track 
                    if (trackNode.NodeLocation.X > newNode.NodeLocation.X
                     && (newNode as DecisionNode).TrueNode.NodeLocation.X > trackNode.NodeLocation.X)
                    {
                       /* if (trackNode is IfElseNode)
                        {
                            (trackNode as IfElseNode).MoveFalsePart = false;
                            shiftNodesRight(newNode, true);
                            (trackNode as IfElseNode).MoveFalsePart = true;
                        }*/
                        shiftNodesRight(newNode, true);
                    }

                }
                else
                {
                    //this is the case when the shape is overlaping with node in it's right 
                    if (trackNode.NodeLocation.X > newNode.NodeLocation.X
                        && newNode.Shape.Width + newNode.NodeLocation.X > trackNode.NodeLocation.X)
                    {
                        shiftNodesRight(newNode,true,100);
                    }
                }
            }
            while (!(trackNode is TerminalNode)); //loop through parent and grandparents to see any conflict
        }

        internal void addToModel(BaseNode toAddNode)
        {
            if (model == null)
            {
                throw new Exception("Model should be set before calling addToModel");
            }
            if (toAddNode.OutConnector.EndNode != null)
                model.Lines.Add(toAddNode.ConnectorTag, toAddNode.OutConnector.Connector);
            if (toAddNode.Shape != null)
                model.Shapes.Add(toAddNode.ShapeTag, toAddNode.Shape);
            addNode(toAddNode);
            if (toAddNode is IfElseNode && toAddNode.NodeLocation.X < 100)
                shiftNodesRight(toAddNode,true); //to be replaced by controller
            if(toAddNode is DecisionNode)
                model.Lines.Add((toAddNode as DecisionNode).TrueConnector.Connector);
            if (toAddNode is IfElseNode)
            {
                model.Lines.Add((toAddNode as IfElseNode).FalseConnector.Connector);
            }
        }

        
        internal void removeNode(BaseNode toRemoveNode)
        {
            

            if (model == null)
            {
                throw new Exception("Model should be set before calling addToModel");
            }
            
            foreach (BaseNode node in Nodes)
            {
                BaseNode nextNode = node.OutConnector.EndNode;
                if (nextNode!=null&&nextNode.ToBeRemoved && node.OutConnector.EndNode != node.ParentNode) //problem for backnode
                {
                    node.OutConnector.EndNode = nextNode.OutConnector.EndNode;
                    node.OutConnector.EndNode.shiftUp(node.OutConnector.EndNode.NodeLocation.Y - toRemoveNode.NodeLocation.Y);
                    break;
                }
            }
           
            for (int i = 0; i < Nodes.Count; i++) {
                if (Nodes[i].ToBeRemoved) {
                    Nodes.Remove(Nodes[i]);
                    i--;
                }
            }
            redrawNodes();
           
        }

       
        
         internal void addNode(BaseNode node)
         {
             if (!nodes.Contains(node))
             {
                 nodes.Add(node);
                 if (node is IfElseNode && node.NodeLocation.X < 100)
                    shiftNodesRight(node,false,150);

             }
         }
        public void shiftNodesRight(BaseNode shiftNode,bool exculdeNode,int distance=150)
        {
            shiftNode.RightShifCaused += distance;
            foreach (BaseNode node in this.Nodes)
            {
                if (node is HolderNode) continue;
                if (exculdeNode)
                {
                    if (node.NodeLocation.X > shiftNode.NodeLocation.X)
                        node.shiftRight(distance);
                }
                else
                    if (node.NodeLocation.X >= shiftNode.NodeLocation.X)
                    node.shiftRight(distance);
            }

        }
       }
}
