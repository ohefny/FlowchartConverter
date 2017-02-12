using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingLast.Nodes;
using TestingLast.Properties;

namespace TestingLast.CodeGeneration
{
   
    public static class FlowChartCodeGenerator
    {
        const int CSHARP = 2;
        const int CPP = 1;
        static int choosenCode=CPP;
       
        public enum NodeType { WHILE, DOWHILE, FOR, IF, IFELSE, INPUT, OUTPUT, ASSIGN, DECLARE }
        public static int indentation = 0;
        public static String getCppCode(BaseNode startNode,BaseNode terminateNode) {
            choosenCode = CPP;  
            StringBuilder sb = new StringBuilder();
            sb.Append("#include <iostream> \r\n using namespace std; \r\n int main()\r\n");
            sb.Append(getBlockCode(startNode, terminateNode));
            //sb.Append("}");
            return sb.ToString();
        }
        public static String getCsCode(BaseNode startNode, BaseNode terminateNode)
        {
            choosenCode = CSHARP;
            string line = "";
            int count = 0;
            StringBuilder sb = new StringBuilder();
            indentation = 8;
            string[] seperators = new String[] { "\r\n" };
            string[] lines = Resources.cscode.Split(seperators, StringSplitOptions.None);
            for (int i = 0; i < 5; i++)
            {
                sb.Append(lines[i]);
                sb.Append("\r\n");
            }         
          
            sb.Append(getBlockCode(startNode, terminateNode));
            for (int i = 5; i < lines.Length; i++)
            {
                sb.Append(lines[i]);
                sb.Append("\r\n");
            }
            //sb.Append("}");

            File.WriteAllText(@"D:\code.txt", sb.ToString());
            return sb.ToString();
        }

        private static string getBlockCode(BaseNode startBlockNode, BaseNode endBlockNode)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" \r\n");
            sb.Append(' ', indentation);
            sb.Append("{");
            sb.Append(" \r\n");
            indentation += 4;
            BaseNode node = startBlockNode;
            
            
            while (node != endBlockNode) {
                if(!(node is HolderNode || node is TerminalNode))
                    sb.Append(' ', indentation);
                if (node is HolderNode || node is TerminalNode) ;
                else if (node is IfElseNode)
                {
                    IfElseNode ifNode = (IfElseNode)node;
                    sb.Append(getExpression(NodeType.IFELSE, ifNode));
                    sb.Append(getBlockCode(ifNode.TrueNode, ifNode.BackNode));
                    sb.Append(' ', indentation);
                    sb.Append("else");
                    sb.Append(getBlockCode(ifNode.FalseNode, ifNode.BackfalseNode));

                }
                else if (node is IfNode)
                {
                    IfNode ifNode = (IfNode)node;
                    sb.Append(getExpression(NodeType.IF, ifNode));
                    sb.Append(getBlockCode(ifNode.TrueNode, ifNode.BackNode));

                }
                else if (node is DoNode)
                {
                    DecisionNode loopNode = (DecisionNode)node;
                    sb.Append("Do");
                    sb.Append(getBlockCode(loopNode.TrueNode, loopNode.BackNode));
                    sb.Append(getExpression(NodeType.DOWHILE, node));
                    sb.Append(Environment.NewLine);
                }
                else if (node is ForNode)
                {
                    ForNode loopNode = (ForNode)node;

                    sb.Append(getExpression(NodeType.FOR, loopNode));
                    sb.Append(getBlockCode(loopNode.TrueNode, loopNode.BackNode));

                }
                else if (node is WhileNode) {
                    WhileNode loopNode = (WhileNode)node;

                    sb.Append(getExpression(NodeType.WHILE, loopNode));
                    sb.Append(getBlockCode(loopNode.TrueNode, loopNode.BackNode));

                }
                else if (node is InputNode) {
                    sb.Append(getExpression(NodeType.INPUT, node));
                }
                else if (node is OutputNode) {
                    sb.Append(getExpression(NodeType.OUTPUT, node));
                }
                else if (node is AssignNode)
                {
                    sb.Append(getExpression(NodeType.ASSIGN, node));
                }
                else if (node is DeclareNode)
                {
                    sb.Append(getExpression(NodeType.DECLARE, node));
                }
                sb.Append(Environment.NewLine);
                node = node.OutConnector.EndNode;
            }
                
           
            indentation -= 4;
            sb.Append(' ', indentation);
            sb.Append("}\r\n");
            
            return sb.ToString();
                
            
        }
        public static string getExpression(NodeType type, BaseNode node) {
            return choosenCode == CPP ? getCppExpression(type, node) : getCsExpression(type, node); 
        }
        public static string getCppExpression(NodeType type, BaseNode node) {
            switch (type)
            {
                case NodeType.ASSIGN:
                    return node.Statement + ";";

                case NodeType.DECLARE:
                    return getDeclareCppCode((node as DeclareNode)._Var) + ";";
                //return getDeclareCsCode((node as DeclareNode)._Var) + ";";

                case NodeType.INPUT:
                    return "cin>> " + node.Statement + " ;";

                case NodeType.OUTPUT:
                    return "cout<< " + node.Statement + " ;";

                case NodeType.IF:


                case NodeType.IFELSE:
                    return "if ( " + node.Statement + " )";

                case NodeType.WHILE:
                    return "while ( " + node.Statement + " )";
                case NodeType.DOWHILE:
                    return "while ( " + node.Statement + " );";

                case NodeType.FOR:
                    return "for ( " + node.Statement + " )";


            }
            return node.Statement;

        }
        public static string getCsExpression(NodeType type, BaseNode node)
        {
            switch (type)
            {
                case NodeType.ASSIGN:
                    return node.Statement + ";";

                case NodeType.DECLARE:
                    return getDeclareCsCode((node as DeclareNode)._Var) + ";";

                case NodeType.INPUT:
                    return "input(" + node.Statement + ") ;";

                case NodeType.OUTPUT:
                    return "Console.WriteLine(" + node.Statement + ") ;";

                case NodeType.IF:


                case NodeType.IFELSE:
                    return "if ( " + node.Statement + " )";

                case NodeType.WHILE:
                    return "while ( " + node.Statement + " )";
                case NodeType.DOWHILE:
                    return "while ( " + node.Statement + " );";

                case NodeType.FOR:
                    return "for ( " + node.Statement + " )";


            }
            return node.Statement;

        }

        private static string getDeclareCppCode(DeclareNode.Variable variable)
        {
            string str = "";
            switch (variable.VarType) {
                case DeclareNode.Variable.Data_Type.INTEGER:
                    str += "int ";
                    break;
                case DeclareNode.Variable.Data_Type.REAL:
                    str += "float ";
                    break;
                case DeclareNode.Variable.Data_Type.BOOLEAN:
                    str += "bool ";
                    break;
                case DeclareNode.Variable.Data_Type.STRING:
                    str += "string ";
                    break;
            }
            str += variable.VarName;
            if (!variable.Single) {
                str += "[" + variable.Size + "]";
            }
            return str;
          
        }
        private static string getDeclareCsCode(DeclareNode.Variable variable)
        {
            string str = "";  
            string type = "";
            switch (variable.VarType)
            {
                case DeclareNode.Variable.Data_Type.INTEGER:
                    type += "int ";
                    break;
                case DeclareNode.Variable.Data_Type.REAL:
                    type += "float ";
                    break;
                case DeclareNode.Variable.Data_Type.BOOLEAN:
                    type += "bool ";
                    break;
                case DeclareNode.Variable.Data_Type.STRING:
                    type += "string ";
                    break;
            }
            str += type;
            if (!variable.Single)
            {
                str += "[" + "] "+ variable.VarName + " = new "+type+"["+variable.Size+"]";
            }
            else
                str += variable.VarName;
            return str;

        }
    }
}
