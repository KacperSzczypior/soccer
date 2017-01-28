using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace readyforsomesoccer
{
    public class SoccerGameWithAI : SoccerGame
    {
        protected MyHasher myHasher;
        protected MoveEvaluation bestMovesMark;
        protected List<MoveResult> bestMoves;
        protected Point crawlPoint;

        public SoccerGameWithAI(int x, int y)
            : base(x, y)
        {
        }

        public MoveResult makeMoveResult(Node from, Node to)
        {
            return new MoveResult
            {
                addX = to.x - from.x,
                addY = to.y - from.y,
                worked = true
            };
        }

        public int CountEmptyPoint()
        {
            bool[,] foundPoints = new bool[nodes.GetLength(0), nodes.GetLength(1)];
            List<Node> myNodes = new List<Node>();
            myNodes.Add(getNode(crawlPoint));
            List<Edge> edges = new List<Edge>();
            while (myNodes.Count != 0)
            {
                foreach (Edge edge in GetMovableEdges(myNodes[0]))
                {
                    edges.Add(edge);
                    if (edge.node.set == false)
                    {
                        foundPoints[edge.node.x, edge.node.y] = true;
                        edge.node.set = true;
                    }
                    edge.drawn = true;
                    myNodes.Add(edge.node);
                }
                myNodes.RemoveAt(0);
            }
            int result = 0;
            foreach (bool b in foundPoints)
            {
                if (b == true)
                    result++;
            }
            foreach (Edge edge in edges)
            {
                edge.drawn = false;
            }
            return result;
        }

        public AIMove makeIntelligentMove()
        {
            crawlPoint = new Point(currentPoint.X, currentPoint.Y);
            List<MoveResult> results = new List<MoveResult>();
            myHasher = new MyHasher();
            newCrawlv2(results);

            AIMove toReturn = new AIMove
            {
                moves = bestMoves
            };
            bestMoves = null;
            playersTurn = true;
            markMove(toReturn.moves);
            return toReturn;
        }

        public bool searchForCurrentPoint(List<Node> currentWave, List<Node> nextWave)
        {
            DebugStatistics.passedHashWasFalse++;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < currentWave.Count; i++)
            {
                Node node = currentWave[i];
                if (node.searched == false)
                {
                    foreach (Edge evalutaedEdge in node.nodes)
                    {
                        if (evalutaedEdge.drawn)
                            continue;
                        Node evalutaedNode = evalutaedEdge.node;
                        if (evalutaedNode.searched == true)
                            continue;
                        if (evalutaedNode.x == crawlPoint.X && evalutaedNode.y == crawlPoint.Y)
                        {
                            watch.Stop();
                            DebugStatistics.timeSearchingDistance += watch.Elapsed;
                            return true;
                        }
                        else
                        {
                            if (!evalutaedNode.nodeAdded)
                            {
                                evalutaedNode.nodeAdded = true;
                                if (evalutaedNode.set == false)
                                    nextWave.Add(evalutaedNode);
                                else
                                    currentWave.Add(evalutaedNode);
                            }
                        }
                    }
                }
                node.searched = true;
            }

            watch.Stop();
            DebugStatistics.timeSearchingDistance += watch.Elapsed;
            return false;
        }

        public int howFarIsIt(Point targetPoint)
        {
            int mark = 0;
            if (targetPoint.X == crawlPoint.X && targetPoint.Y == crawlPoint.Y)
                return mark;

            List<Node> currentWave = new List<Node>();
            List<Node> nextWave = new List<Node>();
            currentWave.Add(getNode(targetPoint));
            foreach (Node node in nodes)
            {
                node.searched = false;
                node.nodeAdded = false;
            }

            while (currentWave.Count != 0)
            {
                if (searchForCurrentPoint(currentWave, nextWave) == true)
                    return mark;
                else
                {
                    currentWave = nextWave;
                    nextWave = new List<Node>();
                    mark++;
                }
            }
            return 100;
        }

        public MoveEvaluation evaluateBoard()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            int distanceTargetMark = 100;
            switch (getGameState(crawlPoint))
            {
                case GameResult.botPlayerWin:
                    watch.Stop();
                    DebugStatistics.timeEvaluating += watch.Elapsed;
                    return new MoveEvaluation
                    {
                        targetDistance = 1000,
                    };
                case GameResult.topPlayerWin:
                    DebugStatistics.timeEvaluating += watch.Elapsed;
                    return new MoveEvaluation
                    {
                        targetDistance = 0
                    };
            }

            distanceTargetMark -= howFarIsIt(targetPoint);
            int distanceHome = howFarIsIt(homePoint);
            if (distanceHome == 0)
            {
                distanceTargetMark -= 100;
            }
            else if (distanceTargetMark == 0 && distanceHome == 100)
            {
                DebugStatistics.timeEvaluating += watch.Elapsed;
                return new MoveEvaluation
                {
                    targetDistance = CountEmptyPoint() % 2 == 0 ? -150 : 150
                };
            }
            if (distanceHome == 100)
            {
                distanceTargetMark += 50;
            }
            DebugStatistics.timeEvaluating += watch.Elapsed;
            return new MoveEvaluation
            {
                targetDistance = distanceTargetMark + distanceHome / 2,
                homeDistance = 10 + distanceHome
            };
        }

        public void evaluateBoardAndReplaceIfBetter(List<MoveResult> moves)
        {
            MoveEvaluation currentMark = evaluateBoard();
            if (bestMoves == null || currentMark > bestMovesMark)
            {
                bestMoves = new List<MoveResult>();
                foreach (MoveResult result in moves)
                {
                    bestMoves.Add(result);

                }
                bestMovesMark = currentMark;
            }
        }

        public void newCrawlv2(List<MoveResult> moves)
        {
            List<Node> nodes = GetMovableNodes(crawlPoint).ToList();
            if (nodes.Count == 0)
                return;
            else if (getNode(crawlPoint).set == false)
            {
                evaluateBoardAndReplaceIfBetter(moves);
            }
            else
            {
                foreach (Node node in nodes)
                {
                    moves.Add(makeMoveResult(getNode(crawlPoint), node));
                    markEdge(crawlPoint.X, crawlPoint.Y, moves[moves.Count - 1].addX, moves[moves.Count - 1].addY);
                    currentState.markOnBoardState(crawlPoint, moves[moves.Count - 1].convertToDirection());
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    if (!myHasher.Contains(currentState))
                    {
                        watch.Stop();
                        DebugStatistics.timeDuringChecking += watch.Elapsed;
                        myHasher.Add(currentState.copy());
                        crawlPoint = movePoint(crawlPoint, moves[moves.Count - 1]);
                        DebugStatistics.crawlcounter++;
                        newCrawlv2(moves);
                        crawlPoint = movePoint(crawlPoint, moves[moves.Count - 1], true);
                    }
                    else
                    {
                        DebugStatistics.passedHash++;
                        watch.Stop();
                        DebugStatistics.timeDuringChecking += watch.Elapsed;
                    }
                    currentState.markOnBoardState(crawlPoint, moves[moves.Count - 1].convertToDirection(), true);
                    markEdge(crawlPoint.X, crawlPoint.Y, moves[moves.Count - 1].addX, moves[moves.Count - 1].addY, true);
                    moves.RemoveAt(moves.Count - 1);
                }
            }
        }
    }
}
