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
                if (node is HolderNode || node is TerminalNode);  
                else if (node is DecisionNode) {
                    if (node is IfNode)
                    {
                        IfNode ifNode = (IfNode)node;                     
                        sb.Append(ifNode.Statement);
                        sb.Append(getBlockCode(ifNode.TrueNode,ifNode.BackNode));
                        if (!ifNode.isEmptyElse()) {
                            // sb.Append("\n");  
                            sb.Append(' ', indentation);
                            sb.Append("else");
                            sb.Append(getBlockCode(ifNode.FalseNode, ifNode.BackfalseNode));
                        }
                    }
                    else if (node is DoNode) {
                        DecisionNode loopNode = (DecisionNode)node;                      
                        sb.Append("Do");
                        sb.Append(getBlockCode(loopNode.TrueNode, loopNode.BackNode));                
                        sb.Append(node.Statement);
                        sb.Append(Environment.NewLine);
                    }
                    else
                    {
                        DecisionNode loopNode = (DecisionNode)node;
                        
                        sb.Append(loopNode.Statement);
                        sb.Append(getBlockCode(loopNode.TrueNode, loopNode.BackNode));

                    }
                }
                else {
                       sb.Append(node.Statement);
                      sb.Append(Environment.NewLine);
                }
                
                node = node.OutConnector.EndNode;
            }
            indentation -= 4;
            sb.Append(' ', indentation);
            sb.Append("}\n");
            
            return sb.ToString();
                
            
        }
    }
}
