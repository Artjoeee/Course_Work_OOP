using Sportics.Helper;
using Sportics.Model;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Sportics
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            CultureInfo culture = new CultureInfo("RU");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            LocalizationManager.ChangeCulture("RU");

            DataWorker.CleanupOldSchedules();

            base.OnStartup(e);
        }
    }
}
