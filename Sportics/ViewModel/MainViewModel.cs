using Sportics.Helper;
using Sportics.Model;
using Sportics.View;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<string> Languages { get; } = new ObservableCollection<string> { "RU", "EN" };

        private string _selectedLanguage = "RU";
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (_selectedLanguage != value)
                {
                    _selectedLanguage = value;
                    OnPropertyChanged();
                    LocalizationManager.ChangeCulture(value);
                }
            }
        }

        private readonly ThemeService ThemeService = ThemeService.Instance;

        public bool IsDarkTheme
        {
            get => ThemeService.IsDarkTheme;
            set
            {
                if (value)
                    ThemeService.SetDarkTheme();
                else
                    ThemeService.SetLightTheme();

                OnPropertyChanged();
            }
        }

        public ICommand OpenAccountCommand { get; }

        public ICommand OpenMembershipsCommand { get; }

        public ICommand OpenCoachesCommand { get; }

        public ICommand OpenSchedulesCommand { get; }

        public MainViewModel()
        {
            OpenAccountCommand = new RelayCommand(obj => OpenAccount());
            OpenMembershipsCommand = new RelayCommand(obj => OpenMemberships());
            OpenCoachesCommand = new RelayCommand(obj => OpenCoaches());
            OpenSchedulesCommand = new RelayCommand(obj => OpenSchedules());
        }

        private void OpenAccount()
        {
            AccountWindow accountWindow = new AccountWindow();
            accountWindow.Owner = Application.Current.MainWindow;
            accountWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            accountWindow.ShowDialog();
        }

        private void OpenMemberships()
        {
            ClientMembershipsWindow membershipsWindow = new ClientMembershipsWindow();
            Application.Current.MainWindow = membershipsWindow;

            Application.Current.Windows
            .OfType<Window>()
            .FirstOrDefault(w => w is MainWindow)?
            .Close();

            Application.Current.MainWindow.Show();
        }

        private void OpenCoaches()
        {
            ClientCoachesWindow window = new ClientCoachesWindow();
            Application.Current.MainWindow = window;

            Application.Current.Windows
            .OfType<Window>()
            .FirstOrDefault(w => w is MainWindow)?
            .Close();

            Application.Current.MainWindow.Show();
        }

        private void OpenSchedules()
        {
            ClientSchedulesWindow window = new ClientSchedulesWindow();
            Application.Current.MainWindow = window;

            Application.Current.Windows
            .OfType<Window>()
            .FirstOrDefault(w => w is MainWindow)?
            .Close();

            Application.Current.MainWindow.Show();
        }
    }
}
