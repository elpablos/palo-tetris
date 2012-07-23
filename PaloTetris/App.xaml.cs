using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Windows.Threading;

namespace PaloTetris
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DispatcherUnhandledException += Application_DispatcherUnhandledException;

            // nacteme vsechny assembly ve slozce vcetne teto assembly
            var catalog = new AggregateCatalog(new DirectoryCatalog(System.AppDomain.CurrentDomain.BaseDirectory), 
                new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly()));
            // katalogr pridame do kontajneru
            var container = new CompositionContainer(catalog);

            // vytahneme z kontajneru okno a zapneme jej
            var window = container.GetExport<IShell>();
            window.Value.Show();
        }

        /// <summary>
        /// Zachytavani neosetrenych vyjimek.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "Critical error", MessageBoxButton.OK, MessageBoxImage.Error);

            e.Handled = true;

            if (Application.Current != null)
                Application.Current.Shutdown();
        }
    }
}
