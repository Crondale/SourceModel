using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crondale.CodeTree
{
    public class DeclerationNode:CodeNode
    {
        [Obsolete]
        public string Decleration { get; set; }

        public List<string> Attributes { get; } = new List<string>();

        public CodeTree CodeTree { get; set; }

        public object Name { get; set; }


    }
}
