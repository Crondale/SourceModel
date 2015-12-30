using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crondale.CodeTree
{
    public class CSharpReader
    {

        private static CodeTree ReadTree(StreamReader sr)
        {
            CodeTree tree = new CodeTree();
            
            List<string> attributeBuffer = new List<string>();
            StringBuilder buffer = new StringBuilder();
            int paranthesesLevel = 0;
            bool inQuotes = false;
            bool inCommentStart = false;
            bool inLineComment = false;

            while (!sr.EndOfStream)
            {
                char c = (char)sr.Read();

                if (inQuotes)
                {
                    buffer.Append(c);

                    if (c == '"')
                    {
                        inQuotes = false;
                    }
                }
                else if (inLineComment)
                {
                    buffer.Append(c);

                    if (c == '\n')
                    {
                        tree.Nodes.Add(new StatementNode(buffer.ToString()));
                        buffer.Clear();
                        inLineComment = false;
                    }
                }
                else
                {
                    switch (c)
                    {
                        case ' ':
                            //ignore spaces
                            goto default;
                        case '[':
                            break;
                        case ']':
                            attributeBuffer.Add(buffer.ToString().Trim());
                            buffer.Clear();
                            break;
                        case '/':
                            if (inCommentStart)
                            {
                                inLineComment = true;
                                inCommentStart = false;
                            }
                            else
                            {
                                inCommentStart = true;
                            }
                            goto default;
                        case '\n':
                            goto default;
                        case ';':
                            buffer.Append(';');
                            tree.Nodes.Add(new StatementNode(buffer.ToString()));
                            buffer.Clear();
                            break;
                        case '"':
                            inQuotes = true;
                            goto default;
                        case '(':
                            paranthesesLevel++;
                            goto default;
                        case ')':
                            paranthesesLevel--;
                            goto default;
                        case '{':
                            if (paranthesesLevel == 0)
                            {
                                DeclerationNode dn = new DeclerationNode();

                                var dec = buffer.ToString().Trim();
                                dn.Decleration = dec;

                                var m = Regex.Match(dec, @"((?<mod>\S*)\s)*(?<name>[^\(\s]*)\s*(?<args>\([^\)]*\))");

                                if (m.Success)
                                {
                                    dn.Name = m.Groups["name"].Value;
                                }

                                //dn.Name = dn.Decleration.Split(' ').Last();

                                dn.Attributes.AddRange(attributeBuffer);
                                CodeTree t = ReadTree(sr);
                                dn.CodeTree = t;
                                
                                tree.Nodes.Add(dn);
                                attributeBuffer.Clear();
                                buffer.Clear();
                            }
                            break;
                        case '}':
                            if (paranthesesLevel == 0)
                            {
                                return tree;
                            }
                            break;
                        default:
                            buffer.Append(c);
                            break;
                    }
                }
            }

            return tree;
        }

        public static CodeTree ReadFile(string path)
        {
            
            using (StreamReader sr = new StreamReader(new FileStream(path, FileMode.Open)))
            {

                return ReadTree(sr);
            }
            
        }


    }
}
