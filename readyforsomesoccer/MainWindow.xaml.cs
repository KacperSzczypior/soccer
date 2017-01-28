using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace readyforsomesoccer
{
    public enum Direction
    {
        up = 0,
        upleft = 1,
        left = 2,
        downleft = 3,
        down = 4,
        downright = 5,
        right = 6,
        upright = 7
    }

    public partial class MainWindow : Window
    {
        const int squareLength = 50;
        const int BoardHeight = 10;
        const int BoardWidth = 8;
        const int margin = squareLength;
        Point currentPoint = new Point(BoardWidth / 2, BoardHeight / 2);
        public SoccerGameWithAI soccerGame;
        public bool keyboardlock = false;

        Line GetWhiteLine(double X1, double X2, double Y1, double Y2)
        {
            return new Line
            {
                X1 = X1,
                X2 = X2,
                Y1 = Y1,
                Y2 = Y2,
                Stroke = System.Windows.Media.Brushes.White,
                Visibility = System.Windows.Visibility.Visible
            };
        }

        private void DrawPitch()
        {
            Board.Children.Add(GetWhiteLine(margin, margin, margin, margin + BoardHeight * squareLength));
            Board.Children.Add(GetWhiteLine(margin, margin + (BoardWidth / 2 - 1) * squareLength, margin, margin));
            Board.Children.Add(GetWhiteLine(margin + (BoardWidth / 2 + 1) * (squareLength), margin + BoardWidth * squareLength, margin, margin));
            Board.Children.Add(GetWhiteLine(margin + BoardWidth * squareLength, margin + BoardWidth * squareLength, margin, margin + BoardHeight * squareLength));
            Board.Children.Add(GetWhiteLine(margin, margin + (BoardWidth / 2 - 1) * squareLength, margin + BoardHeight * squareLength, margin + BoardHeight * squareLength));
            Board.Children.Add(GetWhiteLine(margin + (BoardWidth / 2 + 1) * (squareLength), margin + BoardWidth * squareLength, margin + BoardHeight * squareLength, margin + BoardHeight * squareLength));
        }

        private void DrawPoints()
        {
            for (int i = 1; i < BoardHeight; i++)
            {
                for (int j = 1; j < BoardWidth; j++)
                {
                    Rectangle rect = new Rectangle
                    {
                        Stroke = Brushes.White,
                        Visibility = Visibility.Visible,
                        Width = 3,
                        Height = 3,
                        Fill = Brushes.White
                    };

                    Board.Children.Add(rect);
                    Canvas.SetLeft(rect, margin + squareLength * j - 1);
                    Canvas.SetTop(rect, margin + squareLength * i - 1);
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            soccerGame = new SoccerGameWithAI(BoardWidth, BoardHeight);
            Board.Background = Brushes.Green;
            DrawPitch();
            DrawPoints();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            /*for (int i = 0; i < 400000; i++)
            {
                soccerGame.currentState.copy();
            }*/
            watch.Stop();

            BoardState second = soccerGame.currentState.copy();
            BoardState third = soccerGame.currentState.copy();

            /*second.markOnBoardState(new System.Drawing.Point(5, 5), Direction.up);
            third.markOnBoardState(new System.Drawing.Point(5, 4), Direction.down);

            second.markOnBoardState(new System.Drawing.Point(5, 5), Direction.upleft);
            third.markOnBoardState(new System.Drawing.Point(4, 4), Direction.downright);

            second.markOnBoardState(new System.Drawing.Point(5, 5), Direction.left);
            third.markOnBoardState(new System.Drawing.Point(4, 5), Direction.right);

            second.markOnBoardState(new System.Drawing.Point(5, 5), Direction.upright);
            third.markOnBoardState(new System.Drawing.Point(6, 4), Direction.downleft);

            second.markOnBoardState(new System.Drawing.Point(5, 5), Direction.up);
            second.markOnBoardState(new System.Drawing.Point(5, 4), Direction.left);
            second.markOnBoardState(new System.Drawing.Point(4, 4), Direction.downright);
            //hasher.Add(second);
            third.markOnBoardState(new System.Drawing.Point(5, 5), Direction.upleft);
            third.markOnBoardState(new System.Drawing.Point(4, 4), Direction.right);
            third.markOnBoardState(new System.Drawing.Point(5, 4), Direction.down);
            */



            /*bool b = false;
            DebugStatistics.wholeTime = watch.Elapsed;
            if (hasher.Contains(third) == true)
            {
                b = true;
            }
            else
            {
                b = false;
            }
            DialogBox box = new DialogBox(hasher.Contains(third).ToString());*/
            //box.ShowDialog();


        }



        public void Move(Direction dir)
        {
            keyboardlock = true;
            MoveResult result = soccerGame.move(dir);
            if (result.worked == false)
            {
                keyboardlock = false;
                return;
            }
            double currentX = margin + currentPoint.X * squareLength;
            double finalX = currentX + result.addX * squareLength;
            double currentY = margin + currentPoint.Y * squareLength;
            double finalY = currentY + result.addY * squareLength;
            Board.Children.Add(GetWhiteLine(currentX + 1, finalX + 1, currentY + 1, finalY + 1));
            currentPoint = new Point(currentPoint.X + result.addX, currentPoint.Y + result.addY);
            evaluateGameState();
            if (soccerGame.playersTurn == false)
                LetAIPlay();
            else keyboardlock = false;
        }

        public void evaluateGameState()
        {
            if (soccerGame.getGameState() != GameResult.noOneWins)
            {
                DialogBox box = new DialogBox("Ktoś wygrał");
                Nullable<bool> boxresult = box.ShowDialog();
                resetGame();
            }
        }

        private async void LetAIPlay()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            AIMove moves = soccerGame.makeIntelligentMove();
            watch.Stop();
            DebugStatistics.wholeTime = watch.Elapsed;
            foreach (MoveResult result in moves.moves)
            {
                await Task.Delay(100);
                double currentX = margin + currentPoint.X * squareLength;
                double finalX = currentX + result.addX * squareLength;
                double currentY = margin + currentPoint.Y * squareLength;
                double finalY = currentY + result.addY * squareLength;
                Board.Children.Add(GetWhiteLine(currentX + 1, finalX + 1, currentY + 1, finalY + 1));
                currentPoint = new Point(currentPoint.X + result.addX, currentPoint.Y + result.addY);
            }

            DialogBox box = new DialogBox(DebugStatistics.ToString());
            DebugStatistics.resetStats();

            Nullable<bool> boxresult = box.ShowDialog();

            evaluateGameState();
            keyboardlock = false;
        }

        private void resetGame()
        {
            Board.Children.Clear();
            soccerGame = new SoccerGameWithAI(BoardWidth, BoardHeight);
            Board.Background = Brushes.Green;
            DrawPitch();
            DrawPoints();
            currentPoint = new Point(BoardWidth / 2, BoardHeight / 2);


            //Move(Direction.up);
            //Move(Direction.left);
            //Move(Direction.upright);
        }
        //bool lockdown = false;
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            /*System.Drawing.Point spare = soccerGame.homePoint;
            soccerGame.homePoint = soccerGame.targetPoint;
            soccerGame.targetPoint = spare;
            LetAIPlay();//*/
            if (keyboardlock)
                return;
            switch (e.Key)
            {
                case Key.Up:
                    Move(Direction.up);
                    break;
                case Key.Right:
                    Move(Direction.right);
                    break;
                case Key.Left:
                    Move(Direction.left);
                    break;
                case Key.Down:
                    Move(Direction.down);
                    break;
                case Key.NumPad1:
                    Move(Direction.downleft);
                    break;
                case Key.NumPad2:
                    Move(Direction.down);
                    break;
                case Key.NumPad3:
                    Move(Direction.downright);
                    break;
                case Key.NumPad4:
                    Move(Direction.left);
                    break;
                case Key.NumPad6:
                    Move(Direction.right);
                    break;
                case Key.NumPad7:
                    Move(Direction.upleft);
                    break;
                case Key.NumPad8:
                    Move(Direction.up);
                    break;
                case Key.NumPad9:
                    Move(Direction.upright);
                    break;
            }
            /*if (soccerGame.getGameState())
            {
                resetGame();
            }*/

            //*/
            //DialogBox box = new DialogBox("Ktoś wygrał");
            //Nullable<bool> result = box.ShowDialog();
        }
    }
}
