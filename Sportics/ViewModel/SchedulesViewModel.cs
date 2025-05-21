using Sportics.Helper;
using Sportics.Model;
using Sportics.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Sportics.ViewModel
{
    public class SchedulesViewModel : BaseViewModel
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

        public SchedulesViewModel()
        {
            OpenAddScheduleCommand = new RelayCommand(obj => OpenAddSchedule());
            OpenAdminCommand = new RelayCommand(obj => OpenAdmin());
            DetailsCommand = new RelayCommand(obj => GetDetails((Schedule)obj));
            ApplyFilterCommand = new RelayCommand(obj => ApplyFilter());
            OpenAccountCommand = new RelayCommand(obj => OpenAccount());

            LoadSchedules();
        }

        private void LoadSchedules()
        {
            DateTime now = DateTime.Now;

            Schedules = DataWorker.GetAllSchedules()
                .Where(s => s.Date.Date + s.Time + TimeSpan.FromDays(1) > now) // Сохраняем те, что еще не "просрочены"
                .ToList();

            FilteredSchedules = new ObservableCollection<Schedule>(Schedules);
            OnPropertyChanged(nameof(Schedules));
            OnPropertyChanged(nameof(FilteredSchedules));
        }



        private void ApplyFilter()
        {
            DateTime now = DateTime.Now;

            var filtered = Schedules.Where(s =>
                (SelectedCategory == "Все категории" || s.Category == SelectedCategory) &&
                (!SelectedDate.HasValue || s.Date.Date == SelectedDate.Value.Date) &&
                (s.Date.Date + s.Time + TimeSpan.FromDays(1) > now) // Только не старше 1 дня после окончания
            ).ToList();

            FilteredSchedules = new ObservableCollection<Schedule>(filtered);
            OnPropertyChanged(nameof(FilteredSchedules));
        }




        private void OpenAddSchedule()
        {
            AddScheduleWindow window = new AddScheduleWindow
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            AddScheduleViewModel viewModel = new AddScheduleViewModel();
            window.DataContext = viewModel;

            viewModel.RequestClose += () => window.Close();

            window.ShowDialog();
            LoadSchedules();
        }

        private void OpenAdmin()
        {
            AdminWindow adminWindow = new AdminWindow();
            Application.Current.MainWindow = adminWindow;

            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is SchedulesWindow)?
                .Close();

            Application.Current.MainWindow.Show();
        }

        private void GetDetails(Schedule schedule)
        {
            ScheduleInfoWindow window = new ScheduleInfoWindow();
            ScheduleInfoViewModel vm = new ScheduleInfoViewModel(schedule);
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
    }

}
