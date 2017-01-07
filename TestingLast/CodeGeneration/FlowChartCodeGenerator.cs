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
        public static String getCppCode(BaseNode startNode,BaseNode terminateNode) {
            StringBuilder sb = new StringBuilder();
            sb.Append(" int main()");
            sb.Append(getBlockCode(startNode, terminateNode));
            //sb.Append("}");
            return sb.ToString();
        }

        private static string getBlockCode(BaseNode startBlockNode, BaseNode endBlockNode)
        {
            BaseNode node = startBlockNode;
            StringBuilder sb = new StringBuilder("{ \n");
            while (node != endBlockNode) {
                if (node is HolderNode || node is TerminalNode);
                else if (node is DecisionNode) {
                    if (node is IfNode)
                    {
                        sb.Append("if(x>2){ \n }");
                    }
                    else {
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
            sb.Append("}");
            return sb.ToString();
                
            
        }
    }
}
