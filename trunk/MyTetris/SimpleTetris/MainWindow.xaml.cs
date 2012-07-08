using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Windows.Threading;

namespace SimpleTetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        private const int FIELD_SIZE = 10;

        #endregion // Fields

        #region Properties

        /// <summary>
        /// Reference na tetris.
        /// </summary>
        public Tetris Tetris { get; private set; }

        public int MaxX { get; set; }

        public int MaxY { get; set; }

        #endregion // Properties

        #region Constructor

        /// <summary>
        /// Konstruktor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            // registrace key handleru
            KeyDown += MainWindow_KeyUp;
            // Inicializace tetrisu
            MaxX = 20;
            MaxY = 50;

            TetrisCanvas.Width = FIELD_SIZE * MaxX + 2;
            TetrisCanvas.Height = FIELD_SIZE * MaxY + 2;

            Tetris = new Tetris(MaxX, MaxY);
            Tetris.Repaint += OnRepaint;
        }

        #endregion // Constructor

        #region Events

        /// <summary>
        /// Prekresleni.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnRepaint(object sender, EventArgs e)
        {

            Dispatcher.Invoke(
            DispatcherPriority.Normal,
            (Action)(() =>
            {
                int x, y;
                // smazeme vse
                TetrisCanvas.Children.RemoveRange(0, TetrisCanvas.Children.Count);

                // ohraniceni kolem pole
                Rectangle bigRect = new Rectangle();
                bigRect.Width = FIELD_SIZE * MaxX + 2;
                bigRect.Height = FIELD_SIZE * MaxY + 2;
                bigRect.StrokeThickness = 1;
                bigRect.Stroke = Brushes.Black;
                TetrisCanvas.Children.Add(bigRect);

                //// vykresleni policek
                for (int i = 0; i < 16; i++)
                {
                    if (Tetris.pieces[Tetris.ActivePiece][Tetris.ActiveR][i] == 0) continue;
                    x = i % 4;
                    y = i / 4;

                    Rectangle rect = new Rectangle();
                    rect.Width = FIELD_SIZE;
                    rect.Height = FIELD_SIZE;
                    rect.Fill = Brushes.Red; // Tetris.Board[i]
                    // pridani do canvasu
                    Canvas.SetTop(rect, ((y + Tetris.ActiveY) * FIELD_SIZE) + 1);
                    Canvas.SetLeft(rect, ((x + Tetris.ActiveX) * FIELD_SIZE) + 1);
                    TetrisCanvas.Children.Add(rect);
                }

                // vykresleni kompletniho pole bez aktivniho prvku
                for (int i = 0; i < Tetris.Board.Length; i++)
                {
                    // zjisteni koordinatu
                    x = i % MaxX;
                    y = i / MaxX;
                    // kontrola, jestli neni prvek prazdny
                    if (Tetris.Board[y, x] == 0) continue;

                    Rectangle rect = new Rectangle();
                    rect.Width = FIELD_SIZE;
                    rect.Height = FIELD_SIZE;
                    rect.Fill = Brushes.Blue; // Tetris.Board[i]
                    // pridani do canvasu
                    Canvas.SetTop(rect, (y * FIELD_SIZE) + 1);
                    Canvas.SetLeft(rect, (x * FIELD_SIZE) + 1);
                    TetrisCanvas.Children.Add(rect);
                }
            }));
        }

        /// <summary>
        /// Reakce na stisk klaves.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                //
                Tetris.Left();
            }
            else if (e.Key == Key.Right)
            {
                //
                Tetris.Right();
            }
            else if (e.Key == Key.Up)
            {
                //
                Tetris.Rotate();
            }
            else if (e.Key == Key.Down)
            {
                //
                Tetris.Drop();
            }
            else if (e.Key == Key.S)
            {
                Tetris.Start();
            }
            else
            {
            }
        }

        #endregion // Events
    }
}
