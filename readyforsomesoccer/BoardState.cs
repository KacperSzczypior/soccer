using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace readyforsomesoccer
{
    public static class HashesBoardState{
        public static int[, , , ] numbers = null;
    }

    public class BoardState
    {
        public int hashCode = 0;
        public int secondHashCode = 0;
        public bool[, ,] board;
        public BoardState(int height, int width)
        {
            board = new bool[width, height, 4];
            if (HashesBoardState.numbers == null)
            {
                Random rand = new Random(123);
                HashesBoardState.numbers = new int[board.GetLength(0), board.GetLength(1), board.GetLength(2), 2];
                for (int x = 0; x < board.GetLength(0); x++)
                {
                    for (int y = 0; y < board.GetLength(1); y++)
                    {
                        for (int z = 0; z < board.GetLength(2); z++)
                        {
                            for (int v = 0; v < 2;v++ )
                                HashesBoardState.numbers[x, y, z, v] = rand.Next();
                        }
                    }
                }
            }
        }
        public BoardState copy()
        {
            BoardState toReturn = new BoardState(board.GetLength(1), board.GetLength(0));
            toReturn.hashCode = hashCode;
            toReturn.secondHashCode = secondHashCode;
            Array.Copy(board, 0, toReturn.board, 0, board.Length);
            return toReturn;
        }

        public void markOnBoardState(Point startingPoint, Direction dir, bool reset = false)
        {
            switch (dir)
            {
                case Direction.down:
                    mark(startingPoint.X, startingPoint.Y, (int)BoardStateInternalDirection.down, reset);
                    break;
                case Direction.downleft:
                    mark(startingPoint.X - 1, startingPoint.Y, (int)BoardStateInternalDirection.downleft, reset);
                    break;
                case Direction.left:
                    mark(startingPoint.X - 1, startingPoint.Y, (int)BoardStateInternalDirection.right, reset);
                    break;
                case Direction.upleft:
                    mark(startingPoint.X - 1, startingPoint.Y - 1, (int)BoardStateInternalDirection.downright, reset);
                    break;
                case Direction.up:
                    mark(startingPoint.X, startingPoint.Y - 1, (int)BoardStateInternalDirection.down, reset);
                    break;
                case Direction.upright:
                    mark(startingPoint.X, startingPoint.Y - 1, (int)BoardStateInternalDirection.downleft, reset);
                    break;
                case Direction.right:
                    mark(startingPoint.X, startingPoint.Y, (int)BoardStateInternalDirection.right, reset);
                    break;
                case Direction.downright:
                    mark(startingPoint.X, startingPoint.Y, (int)BoardStateInternalDirection.downright, reset);
                    break;
            }
        }

        private void mark(int x, int y, int z, bool reset)
        {
            board[x, y, z] = !reset;
            hashCode = hashCode ^ HashesBoardState.numbers[x, y, z, 0];
            secondHashCode = secondHashCode ^ HashesBoardState.numbers[x, y, z, 1];
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            BoardState second = obj as BoardState;
            if (second == null)
                return false;
            return this.Equals(second);
        }

        public bool Equals(BoardState second)
        {
            DebugStatistics.enteredCheck++;
            if (second == null)
                return false;
            if (board.GetLength(0) != second.board.GetLength(0) || board.GetLength(1) != second.board.GetLength(1))
                return false;
            if (hashCode != second.hashCode)
                return false;
            //Stopwatch watch = new Stopwatch();
            //watch.Start();
            

            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    for (int z = 0; z < board.GetLength(2); z++)
                    {
                        if (second.board[x, y, z] != board[x, y, z])
                        {
                            //watch.Stop();
                            //DebugStatistics.timeDuringChecking += watch.Elapsed;
                            DebugStatistics.passedHashWasFalse++;
                            return false;
                        }
                    }
                }
            }
            //DebugStatistics.timeDuringChecking += watch.Elapsed;
            return true;
        }

        public override int GetHashCode()
        {
            return hashCode;
        }
    }

}
