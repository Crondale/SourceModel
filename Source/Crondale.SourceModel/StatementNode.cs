using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crondale.CodeTree
{
    public class StatementNode:CodeNode
    {
        
        public StatementNode(string statement)
        {
            Statement = statement;
        }

        public string Statement { get; set; }



    }
}
