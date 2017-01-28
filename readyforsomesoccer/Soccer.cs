using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace readyforsomesoccer
{
 
    public enum GameResult
    {
        topPlayerWin,
        botPlayerWin,
        noOneWins,
        nolegitMove
    }

    public class SoccerGame : SoccerBoard
    {
        public bool playersTurn = true;
        protected Point currentPoint;
        public Point targetPoint;
        public Point homePoint;
        public BoardState currentState;//todo private

        protected Point movePoint(Point toMove, MoveResult moveToMake, bool revert = false)
        {
            if(revert)
                return new Point(toMove.X - moveToMake.addX, toMove.Y - moveToMake.addY);
            return new Point(toMove.X + moveToMake.addX, toMove.Y + moveToMake.addY);
        }

        public GameResult getGameState()
        {
            return getGameState(currentPoint);
        }

        public GameResult getGameState(Point somePoint)
        {
            if (somePoint.X == targetPoint.X && somePoint.Y == targetPoint.Y)
                return GameResult.botPlayerWin;
            if (somePoint.X == homePoint.X && somePoint.Y == homePoint.Y)
                return GameResult.topPlayerWin;
            if (GetMovableEdges(getNode(somePoint)).ToList().Count == 0)
                return GameResult.nolegitMove;
            return GameResult.noOneWins;
        }

        public IEnumerable<Node> GetMovableNodes(Point point)
        {
            Node node = getNode(point);
            foreach (Edge edge in GetMovableEdges(node))
            {
                yield return edge.node;
            }
        }

        public IEnumerable<Edge> GetMovableEdges(Node node)
        {
            foreach (Edge someEdge in node.nodes)
            {
                if (someEdge.drawn == true)
                    continue;
                yield return someEdge;
            }
        }

        public void markMove(List<MoveResult> moves)
        {
            foreach (MoveResult move in moves)
            {
                currentState.markOnBoardState(currentPoint, move.convertToDirection());
                markEdge(currentPoint.X, currentPoint.Y, move.addX, move.addY);
                currentPoint = movePoint(currentPoint, move);
                getNode(currentPoint).set = true;
            }
        }

        public SoccerGame(int x, int y) : base(x, y)
        {
            currentPoint = new Point(x / 2, y / 2 + 1);
            getNode(x / 2, y / 2 + 1).set = true;
            targetPoint = new Point(x / 2, nodes.GetLength(1) - 1);
            homePoint = new Point(x / 2, 0);
            currentState = new BoardState(nodes.GetLength(1) + 1, nodes.GetLength(0) + 1);
        }

        public void markEdge(int x, int y, int addX, int addY, bool reset = false)
        {
            foreach (Edge edge in getNode(x, y).nodes)
            {
                if (edge.node != getNode(x + addX, y + addY))
                    continue;
                else
                {
                    edge.drawn = !reset;
                    break;
                }
            }
            foreach (Edge edge in getNode(x + addX, y + addY).nodes)
            {
                if (edge.node != getNode(x, y))
                    continue;
                else
                {
                    edge.drawn = !reset;
                    break;
                }
            }
        }

        public MoveResult move(Direction dir)
        {
            int addX = 0;
            int addY = 0;

            switch (dir)
            {
                case Direction.down:
                    addY = 1;
                    break;
                case Direction.downleft:
                    addY = 1;
                    addX = -1;
                    break;
                case Direction.left:
                    addX = -1;
                    break;
                case Direction.upleft:
                    addX = -1;
                    addY = -1;
                    break;
                case Direction.up:
                    addY = -1;
                    break;
                case Direction.upright:
                    addY = -1;
                    addX = 1;
                    break;
                case Direction.right:
                    addX = 1;
                    break;
                case Direction.downright:
                    addX = 1;
                    addY = 1;
                    break;
            }

            Edge edge = getNode(currentPoint).nodes.Where(u => u.node == getNode(currentPoint.X + addX, currentPoint.Y + addY)).FirstOrDefault();
            if (edge == null || edge.drawn == true)
            {
                return new MoveResult
                {
                    addX = 0,
                    addY = 0,
                    worked = false
                };
            }
            else
            {
                edge.drawn = true;
                Edge secondEdge = getNode(currentPoint.X + addX, currentPoint.Y + addY).nodes.Where(u => u.node == getNode(currentPoint)).FirstOrDefault();
                secondEdge.drawn = true;
                currentState.markOnBoardState(currentPoint, dir);
                currentPoint = new Point(currentPoint.X + addX, currentPoint.Y + addY);
                if (getNode(currentPoint).set == false)
                {
                    getNode(currentPoint).set = true;
                    playersTurn = false;
                }
                return new MoveResult
                {
                    addX = addX,
                    addY = addY,
                    worked = true
                };
            }
        }
    }
}
