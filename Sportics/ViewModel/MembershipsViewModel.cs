using MaterialDesignThemes.Wpf;
using Sportics.Helper;
using Sportics.Model;
using Sportics.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class MembershipsViewModel : BaseViewModel
    {
        public List<Membership> Memberships { get; set; }
        public ObservableCollection<Membership> FilteredMemberships { get; set; }

        public List<string> Categories { get; set; } = new List<string>
        {
            "Все категории", "Фитнес", "Йога", "Бассейн", "Тренажерный зал", "Танцы"
        };

        public string SelectedCategory { get; set; } = "Все категории";
        public string PriceFrom { get; set; }
        public string PriceTo { get; set; }

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

        public ICommand OpenAddMembershipCommand { get; }
        public ICommand OpenAdminCommand { get; }
        public ICommand DetailsCommand { get; }
        public ICommand ApplyFilterCommand { get; }

        public MembershipsViewModel()
        {
            OpenAddMembershipCommand = new RelayCommand(obj => OpenAddMembership());
            OpenAdminCommand = new RelayCommand(obj => OpenAdmin());
            DetailsCommand = new RelayCommand(obj => GetDetails((Membership)obj));
            ApplyFilterCommand = new RelayCommand(obj => ApplyFilter());

            AllMemberships();
        }

        private void AllMemberships()
        {
            Memberships = DataWorker.GetAllMemberships();
            FilteredMemberships = new ObservableCollection<Membership>(Memberships);
            OnPropertyChanged(nameof(Memberships));
            OnPropertyChanged(nameof(FilteredMemberships));
        }

        private void ApplyFilter()
        {
            decimal.TryParse(PriceFrom, out decimal from);
            decimal.TryParse(PriceTo, out decimal to);

            List<Membership> filtered = Memberships.Where(m =>
                (string.IsNullOrEmpty(SelectedCategory) || SelectedCategory == "Все категории" || m.Category == SelectedCategory) &&
                (string.IsNullOrWhiteSpace(PriceFrom) || m.Price >= from) &&
                (string.IsNullOrWhiteSpace(PriceTo) || m.Price <= to)).ToList();

            FilteredMemberships = new ObservableCollection<Membership>(filtered);
            OnPropertyChanged(nameof(FilteredMemberships));
        }

        private void OpenAddMembership()
        {
            AddWindow addWindow = new AddWindow
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            addWindow.ShowDialog();

            AllMemberships();
        }

        private void OpenAdmin()
        {
            AdminWindow adminWindow = new AdminWindow();
            Application.Current.MainWindow = adminWindow;

            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is MembershipsWindow)?
                .Close();

            Application.Current.MainWindow.Show();
        }

        private void GetDetails(Membership membership)
        {
            MembershipInfoWindow window = new MembershipInfoWindow();
            MembershipInfoViewModel viewModel = new MembershipInfoViewModel(membership);
            window.DataContext = viewModel;
            viewModel.RequestClose += () => window.Close();
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();

            AllMemberships();
        }
    }
}
