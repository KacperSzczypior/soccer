using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace readyforsomesoccer
{
    public struct MoveResult
    {
        public int addX;
        public int addY;
        public bool worked;

        public Direction convertToDirection()
        {
            if (addY == 1 && addX == 0)
                return Direction.down;
            if (addY == 1 && addX == -1)
                return Direction.downleft;
            if(addY ==0 && addX == -1)
                return Direction.left;
            if(addY==-1&&addX==-1)
                return Direction.upleft;
            if(addY==-1 && addX == 0)
                return Direction.up;
            if(addY==-1 && addX==1)
                return Direction.upright;
            if(addY==0 && addX == 1)
                return Direction.right;        
            else return Direction.downright;
        }
    }
}
