using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace readyforsomesoccer
{
    public class RandomSoccer : SoccerGame
    {
        public RandomSoccer(int x, int y)
            : base(x, y)
        {

        }

        public AIMove makeWholeMove()
        {
            AIMove result = new AIMove();
            while (!playersTurn)
                result.moves.Add(makeMove());
            return result;
        }

        public MoveResult makeMove()
        {
            List<Node> nodes = GetMovableNodes(currentPoint).ToList();
            if (nodes.Count == 0)
            {
                playersTurn = true;
                return new MoveResult
                {
                    addX = 0,
                    addY = 0,
                    worked = false
                };
            }
            var r = new Random();
            Node togo = nodes[r.Next(nodes.Count - 1)];
            if (togo.set == false)
            {
                togo.set = true;
                playersTurn = true;
            }
            MoveResult result = new MoveResult { addX = togo.x - currentPoint.X, addY = togo.y - currentPoint.Y };
            markEdge(currentPoint.X, currentPoint.Y, togo.x - currentPoint.X, togo.y - currentPoint.Y);
            currentPoint = new Point(togo.x, togo.y);

            return result;
        }
    }
}
