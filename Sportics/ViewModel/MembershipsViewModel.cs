using Sportics.Helper;
using Sportics.Model;
using Sportics.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class MembershipsViewModel : BaseViewModel, IDataErrorInfo
    {
        public List<Membership> Memberships { get; set; }
        public ObservableCollection<Membership> FilteredMemberships { get; set; }

        public List<string> Categories { get; set; } = new List<string>
        {
            "Все категории", "Фитнес", "Йога", "Бассейн", "Тренажерный зал", "Танцы"
        };

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


        private string priceFrom;
        public string PriceFrom
        {
            get => priceFrom;
            set
            {
                priceFrom = value;
                OnPropertyChanged(nameof(PriceFrom));
                IsValidationActive = false;
            }
        }

        private string priceTo;
        public string PriceTo
        {
            get => priceTo;
            set
            {
                priceTo = value;
                OnPropertyChanged(nameof(PriceTo));
                IsValidationActive = false;
            }
        }

        private string selectedCategory = "Все категории";
        public string SelectedCategory
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }

        private bool isValidationActive = false;
        public bool IsValidationActive
        {
            get => isValidationActive;
            set
            {
                isValidationActive = value;
                OnPropertyChanged(nameof(IsValidationActive));
            }
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (!IsValidationActive) return null;

                switch (columnName)
                {
                    case nameof(PriceFrom):
                        if (!string.IsNullOrWhiteSpace(PriceFrom) && !Regex.IsMatch(PriceFrom, @"^\d+$"))
                            return "Введите положительное число";
                        break;
                    case nameof(PriceTo):
                        if (!string.IsNullOrWhiteSpace(PriceTo) && !Regex.IsMatch(PriceTo, @"^\d+$"))
                            return "Введите положительное число";
                        break;
                }

                return null;
            }
        }

        public ICommand OpenAccountCommand { get; }
        public ICommand OpenAddMembershipCommand { get; }
        public ICommand OpenAdminCommand { get; }
        public ICommand DetailsCommand { get; }
        public ICommand ApplyFilterCommand { get; }

        public MembershipsViewModel()
        {
            OpenAccountCommand = new RelayCommand(obj => OpenAccount());
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
            IsValidationActive = true;
            OnPropertyChanged(nameof(PriceFrom));
            OnPropertyChanged(nameof(PriceTo));

            if (!string.IsNullOrEmpty(this[nameof(PriceFrom)]) || !string.IsNullOrEmpty(this[nameof(PriceTo)]))
                return;

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
            AddWindow window = new AddWindow
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            window.ShowDialog();

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

        private void OpenAccount()
        {
            AccountWindow accountWindow = new AccountWindow();
            accountWindow.Owner = Application.Current.MainWindow;
            accountWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            accountWindow.ShowDialog();
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
