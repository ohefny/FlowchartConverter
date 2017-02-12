using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingLast.Nodes;

namespace TestingLast.Project_Save
{
    class Project_Saver
    {
        StringBuilder stringBuilder = new StringBuilder();
        public static int indentation = 0;
        const string begin= "<?xml version=\"1.0\" encoding=\"utf-8\"?> \n <FlowChart>";
        const string endStr = "</FlowChart>";
        string xmlString = "";

        public string XmlString
        {
            get
            {
                return xmlString;
            }

            set
            {
                xmlString = value;
            }
        }

        public Project_Saver(TerminalNode start, TerminalNode end, String path,String filename) {
            
            stringBuilder.Append(begin);
            stringBuilder.Append(getBlockXML(start, null));
            stringBuilder.Append(endStr);
            XmlString = stringBuilder.ToString();
            try
            {
                File.WriteAllText(path + '\\' + filename + ".xml", xmlString);

            }
            catch (Exception ex) {
                

            }
        }

        private string getBlockXML(BaseNode node, BaseNode endBlockNode)
        {
            indentation += 4;
            StringBuilder sb = new StringBuilder("\r\n");

            while (node != endBlockNode)
            {
                if (!(node is HolderNode )) {
                    sb.Append("\r\n");
                    sb.Append(' ', indentation);
                    sb.Append("<" + node.Name + putAttributes(node)+ ">");
                }
                if (node is HolderNode );
                else if (node is DecisionNode)
                 {
                     if (node is IfElseNode)
                     {
                         IfElseNode ifNode = (IfElseNode)node;
                         indentation += 4;
                         sb.Append("\r\n");
                         sb.Append(' ', indentation);
                         sb.Append("<True>");
                         sb.Append(getBlockXML(ifNode.TrueNode, ifNode.BackNode));
                         sb.Append("</True>");
                         sb.Append("\r\n");
                         sb.Append(' ', indentation);
                         sb.Append("<False>");
                         sb.Append(getBlockXML(ifNode.FalseNode, ifNode.BackfalseNode));
                         sb.Append("</False>");
                         indentation -= 4;
                        sb.Append("\r\n");
                    }
                    
                     else
                     {
                         DecisionNode loopNode = (DecisionNode)node;
                        indentation += 4;
                        sb.Append("\r\n");
                        sb.Append(' ', indentation);
                        sb.Append("<True>");
                        sb.Append(getBlockXML(loopNode.TrueNode, loopNode.BackNode));
                        sb.Append("</True>");
                        indentation -= 4;
                        sb.Append("\r\n");
                    }
                 }
                 else
                 {
                   
                     sb.Append("\r\n");
                 }

                if (!(node is HolderNode )) {
                    sb.Append(' ', indentation);
                    sb.Append("</" + node.Name + "> \n");
                }
                node = node.OutConnector.EndNode;

            }
            sb.Append("\r\n");
            indentation -= 4;
            sb.Append(' ', indentation);
            
          //  sb.Append("</" + node.Name + "> \n");

            return sb.ToString();
            
        }

        private string putAttributes(BaseNode node)
        {
            StringBuilder sb = new StringBuilder();
            if (node is DeclareNode) {
                DeclareNode declareNode = (DeclareNode)node;
                sb.Append(" Variable_Name = \"" + declareNode._Var.VarName + "\"");
                sb.Append(" Variable_Type = \"" + declareNode._Var.VarType + "\"");
                sb.Append(" Single_Variable = \"" + declareNode._Var.Single + "\"");
                sb.Append(" Size = \"" + declareNode._Var.Size + "\"");
               

            }
            if (node is IfElseNode)
            {
                IfElseNode ifElseNode = (IfElseNode)node;
                sb.Append(" True_End_Location = \"" + ifElseNode.BackNode.NodeLocation.X.ToString() + "," + ifElseNode.BackNode.NodeLocation.Y.ToString() + "\" ");
                sb.Append(" False_End_Location = \"" + ifElseNode.BackfalseNode.NodeLocation.X.ToString() + "," + ifElseNode.BackfalseNode.NodeLocation.Y.ToString() + "\" ");
                sb.Append(" Mid_Location = \"" + ifElseNode.MiddleNode.NodeLocation.X.ToString() + "," + ifElseNode.MiddleNode.NodeLocation.Y.ToString() + "\" ");
            }
            else if (node is IfNode)
            {
                IfNode ifNode = (IfNode)node;
                sb.Append(" True_End_Location = \"" + ifNode.BackNode.NodeLocation.X.ToString() + "," + ifNode.BackNode.NodeLocation.Y.ToString() + "\" ");
                sb.Append(" Mid_Location = \"" + ifNode.MiddleNode.NodeLocation.X.ToString() + "," + ifNode.MiddleNode.NodeLocation.Y.ToString() + "\" ");
            }
            else if (node is DecisionNode) {
                DecisionNode decisionNode = (DecisionNode)node;
                sb.Append(" True_End_Location = \"" + decisionNode.BackNode.NodeLocation.X.ToString() + "," + decisionNode.BackNode.NodeLocation.Y.ToString() + "\" ");

            }
            sb.Append(" Statment = \"" + node.Statement +"\" ");
            sb.Append(" Location = \"" + node.NodeLocation.X.ToString()+","+ node.NodeLocation.Y.ToString() + "\" ");
            return sb.ToString();
        }
    }
}
