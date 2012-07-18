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
    /// Interaction logic for AboutPage.xaml
    /// </summary>
    public partial class AboutPage : UserControl, IModule
    {
        public IShell Shell { get; private set; }

        public AboutPage(IShell shell)
        {
            Shell = shell;
            InitializeComponent();
        }

        public void AfterLoaded(bool isFirstTime)
        {
            if (isFirstTime)
            {
                AuthorName.Text = AppSettingsHelper.ReadProperty("AuthorName");
                EmailAddress.Text = AppSettingsHelper.ReadProperty("EmailAddress");
                ProjectSiteAddress.Text = AppSettingsHelper.ReadProperty("ProjectSiteAddress");
                VersionNumber.Text = AppSettingsHelper.ReadProperty("VersionNumber");
            }
        }
    }
}
