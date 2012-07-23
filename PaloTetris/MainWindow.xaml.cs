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

        public IList<IModule> PageCollection { get; private set; }

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
                {
                    _tetrisGame.NextPieceGenerated += OnNextPieceGenerated;
                }
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

            PageCollection = new List<IModule>();
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
            TetrisGame = TetrisGameCollection.FirstOrDefault();

            Guid g = TryParseGuid(AppSettingsHelper.ReadProperty("TetrisAiID"), out g) ? g : Guid.Empty;
            TetrisAi = TetrisAiCollection.FirstOrDefault(t => t.UniqueID == g) ?? TetrisAiCollection.FirstOrDefault();

            // inicializace velikosti okna pro Tetris
            int temp = -1;
            MaxX = AppSettingsHelper.TryReadProperty("Width", out temp) ? temp : DEFAULT_WIDTH;
            MaxY = AppSettingsHelper.TryReadProperty("Height", out temp) ? temp : DEFAULT_HEIGHT;

            // defaultni stranka
            Navigate(typeof(GamePage));
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
            TetrisGame.Reset();
            //ActiveItem.Content = new AboutPage(this);
            Navigate(typeof(AboutPage));
        }

        /// <summary>
        /// Navigace na nastaveni.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            TetrisGame.Reset();
            // ActiveItem.Content = new SettingsPage(this);
            Navigate(typeof(SettingsPage));
        }

        /// <summary>
        /// Navigace na hru.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            TetrisGame.Reset();
            // ActiveItem.Content = new GamePage(this);
            Navigate(typeof(GamePage));
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
                new Thread(AI).Start();
            }
        }

        private void AI()
        {
            Thread.Sleep(200);
            if (TetrisAi != null && TetrisGame != null && StartAI && TetrisGame.IsRunning)
                try
                {
                    TetrisAi.Run(TetrisGame);   
                }
                catch (NullReferenceException ex)
                {
                    /// Todo upravit ukoncovani hry tak, aby toto nenastavalo
                    Console.WriteLine(ex.Message);
                }

        }

        #endregion

        #region Private methods

        private void Navigate(Type type)
        {
            bool isFirst = false;
            var page = PageCollection.FirstOrDefault(p => p.GetType() == type);
            if (page == null)
            {
                isFirst = true;
                if (type == typeof(AboutPage))
                {
                    page = new AboutPage(this);
                }
                else if (type == typeof(SettingsPage))
                {
                    page = new SettingsPage(this);
                }
                else if (type == typeof(GamePage))
                {
                    page = new GamePage(this);
                }
                else
                {
                    throw new InvalidOperationException("Navigate");
                }
                PageCollection.Add(page);
            }
            ActiveItem.Content = page;
            page.AfterLoaded(isFirst);
        }

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
