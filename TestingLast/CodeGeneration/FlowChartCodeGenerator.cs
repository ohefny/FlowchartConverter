using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingLast.Nodes;

namespace TestingLast.CodeGeneration
{
   
    public static class FlowChartCodeGenerator
    {
        public enum NodeType { WHILE, DOWHILE, FOR, IF, IFELSE, INPUT, OUTPUT, ASSIGN, DECLARE }
        public static int indentation = 0;
        public static String getCppCode(BaseNode startNode,BaseNode terminateNode) {
            StringBuilder sb = new StringBuilder();
            sb.Append("int main()");
            sb.Append(getBlockCode(startNode, terminateNode));
            //sb.Append("}");
            return sb.ToString();
        }

        private static string getBlockCode(BaseNode startBlockNode, BaseNode endBlockNode)
        {
            indentation += 4;
            BaseNode node = startBlockNode;
            StringBuilder sb = new StringBuilder(" { \n");
            
            while (node != endBlockNode) {
                if(!(node is HolderNode || node is TerminalNode))
                    sb.Append(' ', indentation);
                if (node is HolderNode || node is TerminalNode) ;
                else if (node is IfElseNode)
                {
                    IfElseNode ifNode = (IfElseNode)node;
                    sb.Append(getCppExpression(NodeType.IFELSE, ifNode.Statement));
                    sb.Append(getBlockCode(ifNode.TrueNode, ifNode.BackNode));
                    sb.Append(' ', indentation);
                    sb.Append("else");
                    sb.Append(getBlockCode(ifNode.FalseNode, ifNode.BackfalseNode));

                }
                else if (node is IfNode)
                {
                    IfNode ifNode = (IfNode)node;
                    sb.Append(getCppExpression(NodeType.IF, ifNode.Statement));
                    sb.Append(getBlockCode(ifNode.TrueNode, ifNode.BackNode));

                }
                else if (node is DoNode)
                {
                    DecisionNode loopNode = (DecisionNode)node;
                    sb.Append("Do");
                    sb.Append(getBlockCode(loopNode.TrueNode, loopNode.BackNode));
                    sb.Append(getCppExpression(NodeType.DOWHILE, node.Statement));
                    sb.Append(Environment.NewLine);
                }
                else if (node is ForNode)
                {
                    ForNode loopNode = (ForNode)node;

                    sb.Append(getCppExpression(NodeType.FOR, loopNode.Statement));
                    sb.Append(getBlockCode(loopNode.TrueNode, loopNode.BackNode));

                }
                else if (node is WhileNode) {
                    WhileNode loopNode = (WhileNode)node;

                    sb.Append(getCppExpression(NodeType.WHILE, loopNode.Statement));
                    sb.Append(getBlockCode(loopNode.TrueNode, loopNode.BackNode));

                }
                else if (node is InputNode) {
                    sb.Append(getCppExpression(NodeType.INPUT, node.Statement));
                }
                else if (node is OutputNode) {
                    sb.Append(getCppExpression(NodeType.OUTPUT, node.Statement));
                }
                else if (node is AssignNode)
                {
                    sb.Append(getCppExpression(NodeType.ASSIGN, node.Statement));
                }
                else if (node is DeclareNode)
                {
                    sb.Append(getCppExpression(NodeType.DECLARE, node.Statement));
                }
                sb.Append(Environment.NewLine);
                node = node.OutConnector.EndNode;
            }
                
           
            indentation -= 4;
            sb.Append(' ', indentation);
            sb.Append("}\n");
            
            return sb.ToString();
                
            
        }
        public static string getCppExpression(NodeType type, String statment) {
            switch (type) {
                case NodeType.ASSIGN:
                    return statment+";";
                    
                case NodeType.DECLARE:
                    return statment + ";";
                    
                case NodeType.INPUT:
                    return "cin>> " + statment + " ;";
                    
                case NodeType.OUTPUT:
                    return "cout<< " + statment + " ;";
                   
                case NodeType.IF:
                    
                    
                case NodeType.IFELSE:
                    return "if ( " + statment + " )";
                    
                case NodeType.WHILE:
                    return "while ( " + statment + " )";
                case NodeType.DOWHILE:
                    return "while ( " + statment + " );";
                    
                case NodeType.FOR:
                    return "for ( " + statment + " )";
                    

            }
            return statment;

        }
    }
}
