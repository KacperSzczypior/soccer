using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace readyforsomesoccer
{
    public class Node
    {
        public int x;
        public int y;
        public bool set = false;
        public List<Edge> nodes = new List<Edge>();
        public bool searched = false;
        public bool nodeAdded = false;
    }
}
