using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace readyforsomesoccer
{
    public class MyHasher
    {
        Dictionary<int, HashSet<BoardState>> states = new Dictionary<int, HashSet<BoardState>>();
        public bool Contains(BoardState second)
        {
            if (!states.ContainsKey(second.secondHashCode))
                return false;
            return states[second.secondHashCode].Contains(second);
        }

        public MyHasher() { }

        public void Add(BoardState newBoard)
        {
            if (!states.ContainsKey(newBoard.secondHashCode))
                states.Add(newBoard.secondHashCode, new HashSet<BoardState>());
            states[newBoard.secondHashCode].Add(newBoard);
        }
    }
}
