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
using PaloTetris.Core;

namespace PaloTetris
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : UserControl
    {
        #region Fields

        private bool _isloaded;

        #endregion // Fields

        #region Properties

        public IShell Shell { get; private set; }

        #endregion // Properties

        #region Constructors

        public SettingsPage(IShell shell)
        {
            Shell = shell;

            InitializeComponent();
        }

        #endregion

        #region AfterLoad

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // nastaveni AI
            cbTetrisAI.ItemsSource = Shell.TetrisAiCollection;
            cbTetrisAI.SelectedItem = Shell.TetrisAi;

            // nastaveni hry
            cbTetrisGame.ItemsSource = Shell.TetrisGameCollection;
            cbTetrisGame.SelectedItem = Shell.TetrisGame;

            // prebrani rozmeru
            tbHeight.Text = Shell.MaxY.ToString();
            tbWidth.Text = Shell.MaxX.ToString();
        }

        #endregion // AfterLoad

        #region Button

        /// <summary>
        /// Ukladani nastaveni do aplikace.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // ulozeni nastaveni do hlavniho okna
            Shell.TetrisGame = (ITetrisGame)cbTetrisGame.SelectedItem;
            Shell.TetrisAi = (ITetrisAI)cbTetrisAI.SelectedItem;
            Shell.MaxX = Int16.Parse( tbWidth.Text);
            Shell.MaxY = Int16.Parse(tbHeight.Text);

            // zapis do konfigu
            AppSettingsHelper.SetProperty(Constants.TetrisGameID, ((ITetrisGame)cbTetrisGame.SelectedItem).UniqueID.ToString());
            AppSettingsHelper.SetProperty(Constants.TetrisAiID, ((ITetrisAI)cbTetrisAI.SelectedItem).UniqueID.ToString());
            AppSettingsHelper.SetProperty(Constants.Width, tbWidth.Text);
            AppSettingsHelper.SetProperty(Constants.Height, tbHeight.Text);
        }

        #endregion // Button
    }
}
