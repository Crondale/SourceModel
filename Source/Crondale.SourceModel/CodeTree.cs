using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crondale.CodeTree
{
    public class CodeTree
    {

        public List<CodeNode> Nodes { get; } = new List<CodeNode>();


        public CodeTree(params CodeNode[] nodes)
        {
            Nodes.AddRange(nodes);
        }
    }
}
