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
using System.Windows.Threading;

namespace TetrisTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constants

        private const int HEIGHT = 20;

        private const int WIDTH = 10;

        private const int FIELD_SIZE = 10;

        #endregion

        #region Properties

        /// <summary>
        /// Pole.
        /// </summary>
        public int[,] Board { get; private set; }
        
        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            Board = new int[HEIGHT, WIDTH];
        }

        #endregion

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
                bigRect.Width = FIELD_SIZE * WIDTH + 2;
                bigRect.Height = FIELD_SIZE * HEIGHT + 2;
                bigRect.StrokeThickness = 1;
                bigRect.Stroke = Brushes.Black;
                TetrisCanvas.Children.Add(bigRect);

                // vykresleni kompletniho pole bez aktivniho prvku
                for (int i = 0; i < Board.Length; i++)
                {
                    // zjisteni koordinatu
                    x = i % WIDTH;
                    y = i / WIDTH;
                    // kontrola, jestli neni prvek prazdny
                    if (Board[y, x] == 0) continue;

                    Rectangle rect = new Rectangle();
                    rect.Width = FIELD_SIZE;
                    rect.Height = FIELD_SIZE;
                    rect.Fill = Board[y, x] == 1 ? Brushes.Red : Brushes.Green;
                    // pridani do canvasu
                    Canvas.SetTop(rect, (y * FIELD_SIZE) + 1);
                    Canvas.SetLeft(rect, (x * FIELD_SIZE) + 1);
                    TetrisCanvas.Children.Add(rect);
                }
            }));
        }

        #endregion // Events

        #region Button

        /// <summary>
        /// Fill.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //Board[17, 8] = 1;
            //Board[17, 9] = 1;
            //Board[18, 8] = 1;
            //Board[18, 9] = 1;

            Board[17, 4] = 1;
            Board[17, 5] = 1;
            Board[18, 4] = 1;
            Board[18, 5] = 1;

            Board[19, 0] = 1;
            Board[19, 1] = 1;
            //Board[19, 2] = 1;
            Board[19, 3] = 1;
            Board[19, 4] = 1;
            Board[19, 5] = 1;
            Board[19, 6] = 1;
            //Board[19, 7] = 1;
            //Board[19, 8] = 1;
            Board[19, 9] = 1;

            Board[18, 0] = 1;
            //Board[18, 1] = 1;
            Board[18, 2] = 1;
            Board[18, 3] = 1;
            //Board[18, 4] = 1;
            //Board[18, 5] = 1;
            Board[18, 6] = 1;
            //Board[18, 7] = 1;
            //Board[18, 8] = 1;
            //Board[18, 9] = 1;

            Board[17, 0] = 1;
            Board[17, 1] = 1;
            Board[17, 2] = 1;
            //Board[17, 3] = 1;
            //Board[17, 4] = 1;
            //Board[17, 5] = 1;
            Board[17, 6] = 1;
            Board[17, 7] = 1;
            //Board[17, 8] = 1;
            //Board[17, 9] = 1;
        }

        /// <summary>
        /// Redraw.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            OnRepaint(this, EventArgs.Empty);
        }

        FloodFill ff = null;

        /// <summary>
        /// FloodFill.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            ff = new FloodFill(Board);

            int x = 0;
            int y = 0;
            //for (int i = 0; i < Board.Length; i++)
            //{
            //    // zjisteni koordinatu
            //    x = i % WIDTH;
            //    y = i / WIDTH;
            //    // kontrola, jestli neni prvek prazdny
            //    if (Board[y, x] == 0) break;
            //}
            ff.ColorField(y, x, y, 0, 2);
            OnRepaint(this, EventArgs.Empty);
        }

        /// <summary>
        /// score.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            int i = 0;
            int count = 0;
            while (i<Board.Length)
            {
                if (Board[i / WIDTH, i % WIDTH] == 0) count++;
                i++;
            }

            StatusText.Text = string.Format("Number of hole is {0}, height={1}", ff.GetCoveredHoles(), 18 * 18 * 18);
        }

        #endregion // Button
    }
}
