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

namespace PaloTetris
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : UserControl, IModule
    {
        #region Fields

        private const int FIELD_SIZE = 25;
        private const string START_GAME_TOOLTIP = "Start game";
        private const string START_AI_TOOLTIP = "Activate AI";
        private const string STOP_GAME_TOOLTIP = "Stop game";
        private const string STOP_AI_TOOLTIP = "deactivate AI";

        private int _correctDimension = 1;

        #endregion // Fields

        #region Properties

        /// <summary>
        /// Reference na hlavni okno aplikace.
        /// </summary>
        public IShell Shell { get; private set; }

        public ITetrisGame Game { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="shell"></param>
        public GamePage(IShell shell)
        {
            Shell = shell;

            InitializeComponent();
        }

        #endregion

        #region AfterLoad

        public void AfterLoaded(bool isFirstTime)
        {
            if (isFirstTime)
            {
                btnStartAI.Content = new Image() { Source = new BitmapImage(new Uri("/PaloTetris;component/Resources/user_anonymous_64.png", UriKind.Relative)) };
                btnStartAI.ToolTip = START_AI_TOOLTIP;
            }

            if (Game != Shell.TetrisGame)
            {
                // zaregistrujeme se na prekreslovani
                Game = Shell.TetrisGame;
                Game.Repaint += OnRepaint;

                // nastavime rozmery
                Game.Width = Shell.MaxX;
                Game.Height = Shell.MaxY;
            }
            SwitchButton();
        }

        #endregion

        #region Repaint event

        /// <summary>
        /// Prekresleni.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnRepaint(object sender, EventArgs e)
        {
            if (!Shell.TetrisGame.IsRunning) return;

            Dispatcher.Invoke(
            DispatcherPriority.Normal,
            (Action)(() =>
            {
                int x, y;
                // smazeme vse
                TetrisCanvas.Children.RemoveRange(0, TetrisCanvas.Children.Count);

                // ohraniceni kolem pole
                Rectangle bigRect = new Rectangle();
                bigRect.Width = _correctDimension * Shell.MaxX + 2;
                bigRect.Height = _correctDimension * Shell.MaxY + 2;
                bigRect.StrokeThickness = 1;
                bigRect.Stroke = Brushes.Black;
                TetrisCanvas.Children.Add(bigRect);

                // vykresleni policek
                if (Shell.TetrisGame.ActivePiece != null)
                    for (int i = 0; i < 16; i++)
                    {
                        if (Shell.TetrisGame.ActivePiece.Pieces[i] == 0) continue;
                        x = i % 4;
                        y = i / 4;

                        Rectangle rect = new Rectangle();
                        rect.Width = _correctDimension;
                        rect.Height = _correctDimension;
                        rect.Fill = BaseField.PieceColor(Shell.TetrisGame.ActivePiece.Pieces[i]);
                        // pridani do canvasu
                        Canvas.SetTop(rect, ((y + Shell.TetrisGame.ActiveY) * _correctDimension) + 1);
                        Canvas.SetLeft(rect, ((x + Shell.TetrisGame.ActiveX) * _correctDimension) + 1);
                        TetrisCanvas.Children.Add(rect);
                    }

                // vykresleni kompletniho pole bez aktivniho prvku
                for (int i = 0; i < Shell.TetrisGame.Board.Length; i++)
                {
                    // zjisteni koordinatu
                    x = i % Shell.MaxX;
                    y = i / Shell.MaxX;

                    // kontrola, jestli neni prvek prazdny
                    if (Shell.TetrisGame.Board[y, x] == 0) continue;

                    Rectangle rect = new Rectangle();
                    rect.Width = _correctDimension;
                    rect.Height = _correctDimension;
                    rect.Fill = BaseField.PieceColor(Shell.TetrisGame.Board[y, x]);

                    // pridani do canvasu
                    Canvas.SetTop(rect, (y * _correctDimension) + 1);
                    Canvas.SetLeft(rect, (x * _correctDimension) + 1);
                    TetrisCanvas.Children.Add(rect);
                }
            }));
        }

        #endregion // Repaint event

        #region Button events

        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            if (Shell.TetrisGame.IsRunning)
            {
                Shell.TetrisGame.Reset();
            }
            else
            {
                Shell.TetrisGame.Start();
            }
            SwitchButton(); ;
        }

        private void btnPauseGame_Click(object sender, RoutedEventArgs e)
        {
           //
        }

        private void btnStartAI_Click(object sender, RoutedEventArgs e)
        {
            Shell.StartAI = !Shell.StartAI;
            SwitchButton();
        }

        #endregion // Button events

        #region Helper methods

        private void SwitchButton()
        {
            if (Shell.TetrisGame.IsRunning)
            {
                btnStartGame.Content = new Image() { Source = new BitmapImage(new Uri("/PaloTetris;component/Resources/stop_64.png", UriKind.Relative)) };
                btnStartGame.ToolTip = STOP_GAME_TOOLTIP;
            }
            else
            {
                btnStartGame.Content = new Image() { Source = new BitmapImage(new Uri("/PaloTetris;component/Resources/paly_64.png", UriKind.Relative)) };
                btnStartGame.ToolTip = START_GAME_TOOLTIP;
            }
            if (Shell.StartAI)
            {
                btnStartAI.Content = new Image() { Source = new BitmapImage(new Uri("/PaloTetris;component/Resources/user1_64.png", UriKind.Relative)) };
                btnStartAI.ToolTip = STOP_AI_TOOLTIP;
            }
            else
            {
                btnStartAI.Content = new Image() { Source = new BitmapImage(new Uri("/PaloTetris;component/Resources/user_anonymous_64.png", UriKind.Relative)) };
                btnStartAI.ToolTip = START_AI_TOOLTIP;
            }
        }

        #endregion

        #region Resize event

        private void TetrisCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double width = TetrisCanvas.ActualWidth / Shell.MaxX;
            double height = TetrisCanvas.ActualHeight / Shell.MaxY;
            int temp = Convert.ToInt32(width < height ? width : height);
            _correctDimension = temp > 1 ? temp - 1 : 1;
        }

        #endregion
    }
}
