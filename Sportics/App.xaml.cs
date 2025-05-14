using Sportics.Helper;
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

            base.OnStartup(e);

            // Загружаем курсор
            Stream cursorStream = GetResourceStream(
                new Uri("pack://application:,,,/Resources/cursor.cur"))?.Stream;

            if (cursorStream != null)
            {
                Cursor customCursor = new Cursor(cursorStream);

                Mouse.OverrideCursor = customCursor;

                // Применяем ко всем окнам
                this.Dispatcher.Invoke(() =>
                {
                    EventManager.RegisterClassHandler(typeof(Window),
                        Window.LoadedEvent,
                        new RoutedEventHandler((s, args) =>
                        {
                            if (s is Window window)
                            {
                                window.Cursor = customCursor;
                            }
                        }));
                });
            }
        }
    }
}
