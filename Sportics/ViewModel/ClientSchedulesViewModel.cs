using Sportics.Helper;
using Sportics.Model;
using Sportics.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows;

namespace Sportics.ViewModel
{
    public class ClientSchedulesViewModel : BaseViewModel
    {
        public List<Schedule> Schedules { get; set; }
        public ObservableCollection<Schedule> FilteredSchedules { get; set; }

        public List<string> Categories { get; set; } = new List<string>
        {
            "Все категории", "Фитнес", "Йога", "Танцы", "Плавание", "Тренажерный зал"
        };

        public ObservableCollection<string> Languages { get; } = new ObservableCollection<string> { "RU", "EN" };

        private string _selectedLanguage = "RU";
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                _selectedLanguage = value;
                OnPropertyChanged();
                LocalizationManager.ChangeCulture(value);
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

        private string selectedCategory = "Все категории";
        public string SelectedCategory
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;
                OnPropertyChanged();
            }
        }

        private DateTime? selectedDate;
        public DateTime? SelectedDate
        {
            get => selectedDate;
            set
            {
                selectedDate = value;
                OnPropertyChanged();
            }
        }

        public ICommand OpenAdminCommand { get; }
        public ICommand OpenAddScheduleCommand { get; }
        public ICommand DetailsCommand { get; }
        public ICommand ApplyFilterCommand { get; }
        public ICommand OpenAccountCommand { get; }
        public ICommand OpenMembershipsCommand { get; }
        public ICommand OpenCoachesCommand { get; }
        public ICommand OpenMainCommand { get; }

        public ClientSchedulesViewModel()
        {
            OpenMainCommand = new RelayCommand(obj => OpenMain());
            OpenMembershipsCommand = new RelayCommand(obj => OpenMemberships());
            OpenCoachesCommand = new RelayCommand(obj => OpenCoaches());
            DetailsCommand = new RelayCommand(obj => GetDetails((Schedule)obj));
            ApplyFilterCommand = new RelayCommand(obj => ApplyFilter());
            OpenAccountCommand = new RelayCommand(obj => OpenAccount());

            LoadSchedules();
        }

        private void LoadSchedules()
        {
            Schedules = DataWorker.GetAllSchedules();
            FilteredSchedules = new ObservableCollection<Schedule>(Schedules);
            OnPropertyChanged(nameof(Schedules));
            OnPropertyChanged(nameof(FilteredSchedules));
        }

        private void ApplyFilter()
        {
            var filtered = Schedules.Where(s =>
                (SelectedCategory == "Все категории" || s.Category == SelectedCategory) &&
                (!SelectedDate.HasValue || s.Date.Date == SelectedDate.Value.Date)
            ).ToList();

            FilteredSchedules = new ObservableCollection<Schedule>(filtered);
            OnPropertyChanged(nameof(FilteredSchedules));
        }

        private void GetDetails(Schedule schedule)
        {
            ClientScheduleInfoWindow window = new ClientScheduleInfoWindow();
            ClientScheduleInfoViewModel vm = new ClientScheduleInfoViewModel(schedule);
            window.DataContext = vm;
            vm.RequestClose += () => window.Close();
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();

            LoadSchedules();
        }

        private void OpenAccount()
        {
            AccountWindow accountWindow = new AccountWindow
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            accountWindow.ShowDialog();
        }

        private void OpenMain()
        {
            MainWindow mainWindow = new MainWindow();
            Application.Current.MainWindow = mainWindow;

            Application.Current.Windows
            .OfType<Window>()
            .FirstOrDefault(w => w is ClientSchedulesWindow)?
            .Close();

            Application.Current.MainWindow.Show();
        }

        private void OpenMemberships()
        {
            ClientMembershipsWindow membershipsWindow = new ClientMembershipsWindow();
            Application.Current.MainWindow = membershipsWindow;

            Application.Current.Windows
            .OfType<Window>()
            .FirstOrDefault(w => w is ClientSchedulesWindow)?
            .Close();

            Application.Current.MainWindow.Show();
        }

        private void OpenCoaches()
        {
            ClientCoachesWindow window = new ClientCoachesWindow();
            Application.Current.MainWindow = window;

            Application.Current.Windows
            .OfType<Window>()
            .FirstOrDefault(w => w is ClientSchedulesWindow)?
            .Close();

            Application.Current.MainWindow.Show();
        }
    }

}
