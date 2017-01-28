using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace readyforsomesoccer
{
    public class Edge
    {
        public Node node;
        public bool drawn = false;
        public Edge(Node node)
        {
            this.node = node;
        }

        public Edge() { }
    }
}
