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
using System.ComponentModel.Composition;
using PaloTetris.Core;
using System.Threading;

namespace PaloTetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 
    /// Hlavni okno aplikaci. 
    /// Implementace jednoducheho strankovani a sdilenych dat mezi okny.
    /// </summary>
    public partial class MainWindow : Window, IShell
    {
        #region Fields

        private const int DEFAULT_WIDTH = 10;

        private const int DEFAULT_HEIGHT = 20;

        private bool _startAi;

        private ITetrisGame _tetrisGame;

        #endregion // Fields

        #region Properties

        [ImportMany]
        public IEnumerable<ITetrisGame> TetrisGameCollection { get; private set; }

        [ImportMany]
        public IEnumerable<ITetrisAI> TetrisAiCollection { get; private set; }

        public ITetrisGame TetrisGame
        {
            get { return _tetrisGame; }
            set
            {
                _tetrisGame = value;
                if (_tetrisGame != null)
                    _tetrisGame.NextPieceGenerated += OnNextPieceGenerated;
            }
        }

        public ITetrisAI TetrisAi { get; set; }

        public int MaxX { get; set; }

        public int MaxY { get; set; }

        public bool StartAI
        {
            get { return _startAi; }
            set 
            { 
                _startAi = value;
                if (_startAi) AI();
            }
        }

        #endregion // Properties

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();

            // registrace key handleru
            KeyDown += MainWindow_KeyUp;
        }

        #endregion

        #region AfterLoad

        /// <summary>
        /// Reakce po nacteni obsahu okna.
        /// Nacteni defaultnich hodnot.
        /// </summary>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // kontrola nactenych assemblies
            if (TetrisGameCollection == null || TetrisGameCollection.Count() == 0)
            {
                MessageBox.Show("No tetris game found. Application will be terminated", "No game class found", MessageBoxButton.OK, MessageBoxImage.Stop);
                Application.Current.Shutdown();
            }

            if (TetrisAiCollection == null || TetrisAiCollection.Count() == 0)
            {
                MessageBox.Show("No tetris AI found. Application will be terminated", "No AI class found", MessageBoxButton.OK, MessageBoxImage.Stop);
                Application.Current.Shutdown();
            }

            // nastaveni AI a hry
            Guid g = TryParseGuid(AppSettingsHelper.ReadProperty("TetrisGameID"), out g) ? g : Guid.Empty;
            TetrisGame = TetrisGameCollection.FirstOrDefault(t => t.UniqueID == g) ?? TetrisGameCollection.FirstOrDefault();

            g = TryParseGuid(AppSettingsHelper.ReadProperty("TetrisAiID"), out g) ? g : Guid.Empty;
            TetrisAi = TetrisAiCollection.FirstOrDefault(t => t.UniqueID == g) ?? TetrisAiCollection.FirstOrDefault();

            // inicializace velikosti okna pro Tetris
            int temp = -1;
            MaxX = AppSettingsHelper.TryReadProperty("Width", out temp) ? temp : DEFAULT_WIDTH;
            MaxY = AppSettingsHelper.TryReadProperty("Height", out temp) ? temp : DEFAULT_HEIGHT;

            ActiveItem.Content = new GamePage(this);
        }

        #endregion

        #region Menu navigation

        /// <summary>
        /// Navigace na About stranku.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ActiveItem.Content = new AboutPage(this);
        }

        /// <summary>
        /// Navigace na nastaveni.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            ActiveItem.Content = new SettingsPage(this);
        }

        /// <summary>
        /// Navigace na hru.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            ActiveItem.Content = new GamePage(this);
        }

        #endregion // Menu navigation

        #region Key events

        /// <summary>
        /// Reakce na stisk klaves.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (!TetrisGame.IsRunning || StartAI) return;

            if (e.Key == Key.Left)
            {
                //
                TetrisGame.Left();
            }
            else if (e.Key == Key.Right)
            {
                //
                TetrisGame.Right();
            }
            else if (e.Key == Key.Up)
            {
                //
                TetrisGame.Rotate();
            }
            else if (e.Key == Key.Down)
            {
                //
                TetrisGame.Drop();
            }
            else
            {
            }
        }

        #endregion // Key events

        #region AI events

        protected void OnNextPieceGenerated(object sender, EventArgs e)
        {
            if (StartAI)
            {
                Thread t = new Thread(AI);
                t.Start();
            }
        }

        private void AI()
        {
            if (TetrisAi == null) return;
            Thread.Sleep(200);
            TetrisAi.Run(TetrisGame);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Pomocna metoda, ktera se snazi parsovat unikatni ID ze stringu.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        private bool TryParseGuid(string str, out Guid guid)
        {
            try
            {
                guid = new Guid(str);
                return true;
            }
            catch (Exception)
            {
                guid = Guid.Empty;
                return false;
            }
        }

        #endregion
    }
}
