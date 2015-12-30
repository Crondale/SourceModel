using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crondale.CodeTree
{
    public class CSharpWriter
    {
        private static void WriteNode(StreamWriter writer, StatementNode node)
        {
            writer.Write(node.Statement);
        }

        private static void WriteNode(StreamWriter writer, DeclerationNode node)
        {
            foreach (var attribute in node.Attributes)
            {
                writer.Write('[');
                writer.Write(attribute);
                writer.WriteLine(']');
            }

            writer.WriteLine(node.Decleration);
            writer.WriteLine("{");
            WriteTree(writer, node.CodeTree);
            writer.WriteLine("}");
        }

        private static void WriteTree(StreamWriter writer, CodeTree ctree)
        {
            foreach (var codeNode in ctree.Nodes)
            {
                if (codeNode is DeclerationNode)
                {
                    WriteNode(writer, (DeclerationNode)codeNode);
                }
                else if (codeNode is StatementNode)
                {
                    WriteNode(writer, (StatementNode)codeNode);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public static void Write(Stream stream, CodeTree ctree)
        {
            using (StreamWriter sw = new StreamWriter(stream))
            {
                WriteTree(sw, ctree);
            }
        }

        public static void WriteFile(string filePath, CodeTree ctree)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                Write(fileStream, ctree);
            }
        }
    }
}
