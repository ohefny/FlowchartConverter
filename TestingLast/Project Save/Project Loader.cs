using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TestingLast.Nodes;

namespace TestingLast.Project_Save
{
    class Pair {
        XmlNode xmlNode;
        BaseNode baseNode;
        public Pair(XmlNode xmlNode, BaseNode baseNode) {

            this.xmlNode = xmlNode;
            this.baseNode = baseNode;
        }
        public XmlNode XmlNode
        {
            get
            {
                return xmlNode;
            }

            set
            {
                xmlNode = value;
            }
        }

        public BaseNode BaseNode
        {
            get
            {
                return baseNode;
            }

            set
            {
                baseNode = value;
            }
        }
    }
    class Project_Loader
    {
        TerminalNode startNode;
        TerminalNode endNode;
        List<Pair> blockNodes = new List<Pair>();
        public Project_Loader(TerminalNode startNode, TerminalNode endNode, string filePath)
        {
            
            this.startNode = startNode;
            this.endNode = endNode;
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(filePath);

                ConnectorNode con = startNode.OutConnector;

                addBlockNodes(doc.DocumentElement.ChildNodes, startNode,startNode);
             //   foreach (Pair pair in blockNodes)
              //      setAttributes(pair.XmlNode, pair.BaseNode);

            }
            catch (Exception ex) {
                
            }
        }

   
        private void addBlockNodes(XmlNodeList list, BaseNode lastNode,BaseNode parentNode)
        {

            foreach (XmlNode node in list)
            {
                BaseNode newNode = null;
                if (node.Name.Equals("End"))
                {
                    newNode = endNode;
                }
                else if (node.Name.Equals("Start"))
                {
                    newNode = startNode;
                }
                else if (node.Name.Equals("Assign"))
                {
                    newNode = new AssignNode();


                }
                else if (node.Name.Equals("Declare"))
                {
                    newNode = new DeclareNode();


                }
                else if (node.Name.Equals("Input"))
                {
                    newNode = new InputNode();

                }
                else if (node.Name.Equals("Output"))
                {
                    newNode = new OutputNode();

                }
                else if (node.Name.Equals("If"))
                {
                    newNode = new IfNode();
                    setAttributes(node, newNode);
                    addBlockNodes(node.FirstChild.ChildNodes, ((DecisionNode)newNode).TrueNode,newNode);
                }
                else if (node.Name.Equals("IfElse"))
                {
                    newNode = new IfElseNode();
                    setAttributes(node, newNode);
                    addBlockNodes(node.FirstChild.ChildNodes, ((IfElseNode)newNode).TrueNode,newNode);
                    addBlockNodes(node.LastChild.ChildNodes, ((IfElseNode)newNode).FalseNode, newNode);

                }
                else if (node.Name.Equals("While"))
                {
                    newNode = new WhileNode();
                    setAttributes(node, newNode);
                    addBlockNodes(node.FirstChild.ChildNodes, ((DecisionNode)newNode).TrueNode, newNode);

                }
                else if (node.Name.Equals("DoWhile"))
                {
                    newNode = new DoNode();
                    setAttributes(node, newNode);
                    addBlockNodes(node.FirstChild.ChildNodes, ((DecisionNode)newNode).TrueNode, newNode);
                }
                else if (node.Name.Equals("For"))
                {
                    newNode = new ForNode();
                    setAttributes(node, newNode);
                    addBlockNodes(node.FirstChild.ChildNodes, ((DecisionNode)newNode).TrueNode, newNode);

                }
                else
                {
                }

                if (!(node.Name.Equals("Start") || node.Name.Equals("End")))
                {
                    BaseNode oldNode = lastNode.OutConnector.EndNode;
                    lastNode.OutConnector.EndNode = newNode;
                    newNode.OutConnector.EndNode = oldNode;
                    newNode.addToModel();
                    newNode.ParentNode = parentNode;   

                }
               
                lastNode = newNode;
                 blockNodes.Add(new Pair(node, newNode));
                setAttributes(node, newNode);
            }


        }

        private static void setAttributes(XmlNode node, BaseNode newNode)
        {
            string statment= node.Attributes["Statment"]?.InnerText;
            string location = node.Attributes["Location"]?.InnerText;
            string[] true_end_location = node.Attributes["True_End_Location"]?.InnerText.Split(',');
            string[] false_end_location = node.Attributes["False_End_Location"]?.InnerText.Split(',');
            string[] mid_location = node.Attributes["Mid_Location"]?.InnerText.Split(',');
            
            newNode.Statement = statment;
            
            if (location != null)
            {
                
                string[] points = location.Split(',');
                newNode.NodeLocation = new System.Drawing.PointF((float)Double.Parse(points[0]), (float)Double.Parse(points[1]));
            }
            if (newNode is IfElseNode)
            {
                IfElseNode ifElseNode = (IfElseNode)newNode;
                ifElseNode.BackNode.NodeLocation = new System.Drawing.PointF((float)Double.Parse(true_end_location[0]), (float)Double.Parse(true_end_location[1]));
                ifElseNode.BackfalseNode.NodeLocation = new System.Drawing.PointF((float)Double.Parse(false_end_location[0]), (float)Double.Parse(false_end_location[1]));
                ifElseNode.MiddleNode.NodeLocation = new System.Drawing.PointF((float)Double.Parse(mid_location[0]), (float)Double.Parse(mid_location[1]));

            }
            else if (newNode is IfNode)
            {

                IfNode ifNode = (IfNode)newNode;
                ifNode.BackNode.NodeLocation = new System.Drawing.PointF((float)Double.Parse(true_end_location[0]), (float)Double.Parse(true_end_location[1]));
                ifNode.MiddleNode.NodeLocation = new System.Drawing.PointF((float)Double.Parse(mid_location[0]), (float)Double.Parse(mid_location[1]));
            }
            else if (newNode is DecisionNode)
            {
                DecisionNode decisionNode = (DecisionNode)newNode;
                decisionNode.BackNode.NodeLocation = new System.Drawing.PointF((float)Double.Parse(true_end_location[0]), (float)Double.Parse(true_end_location[1]));

            }
            else if (newNode is DeclareNode) {
                string variable_name = node.Attributes["Variable_Name"].InnerText;
                string variable_type = node.Attributes["Variable_Type"].InnerText;
                string single = node.Attributes["Single_Variable"].InnerText;
                string size = node.Attributes["Size"].InnerText;
                DeclareNode declareNode = (DeclareNode)newNode;
                declareNode._Var.VarName = variable_name;
                declareNode._Var.Size = Int32.Parse(size);
                declareNode._Var.Single = Boolean.Parse(single);
                switch (variable_type)
                {
                    case "INTEGER":
                        declareNode._Var.VarType = DeclareNode.Variable.Data_Type.INTEGER;
                        break;
                    case "REAL":
                        declareNode._Var.VarType = DeclareNode.Variable.Data_Type.REAL;
                        break;
                    case "BOOLEAN":
                        declareNode._Var.VarType = DeclareNode.Variable.Data_Type.BOOLEAN;
                        break;
                    case "STRING":
                        declareNode._Var.VarType = DeclareNode.Variable.Data_Type.STRING;
                        break;
                }

            }
        }

    }
}
