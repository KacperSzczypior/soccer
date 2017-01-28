using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace readyforsomesoccer
{
    public class MoveEvaluation
    {
        public int targetDistance;
        public int homeDistance;
        public static bool operator <(MoveEvaluation x, MoveEvaluation y)
        {
            if (x.targetDistance != y.targetDistance)
                return x.targetDistance < y.targetDistance;
            return x.homeDistance < y.homeDistance;
        }
        public static bool operator >(MoveEvaluation x, MoveEvaluation y)
        {
            return !(x < y);
        }
    }
}
