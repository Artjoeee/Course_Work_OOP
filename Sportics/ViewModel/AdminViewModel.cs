using Sportics.Helper;
using Sportics.Model;
using Sportics.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class AdminViewModel: BaseViewModel
    {
        public Membership SelectedMembership { get; set; }

        public TabItem SelectedTab { get; set; }

        public List<User> Users { get; set; }

        public List<Membership> Memberships { get; set; }

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

        public ObservableCollection<MembershipOrder> MembershipOrders { get; set; }
        public MembershipOrder SelectedOrder { get; set; }

        public ObservableCollection<Schedule> Schedules { get; set; }
        public Schedule SelectedSchedule { get; set; }

        public ObservableCollection<ClientSessionRecord> ClientSessionRecords { get; set; }
        public ClientSessionRecord SelectedClientSessionRecord { get; set; }

        public ObservableCollection<CoachReview> CoachReviews { get; set; }
        public ObservableCollection<SessionReview> SessionReviews { get; set; }


        public ICommand OpenAccountCommand { get; }

        public ICommand OpenMembershipsCommand { get; }

        public ICommand OpenCoachesCommand { get; }

        public ICommand OpenSchedulesCommand { get; }

        public ICommand DeleteMembershipCommand { get; }

        public ICommand DeleteOrderCommand { get; }

        public ICommand EditorCommand { get; }

        public AdminViewModel() 
        {
            OpenAccountCommand = new RelayCommand(obj => OpenAccount());
            OpenMembershipsCommand = new RelayCommand(obj => OpenMemberships());
            OpenCoachesCommand = new RelayCommand(obj => OpenCoaches());
            OpenSchedulesCommand = new RelayCommand(obj => OpenSchedules());
            DeleteMembershipCommand = new RelayCommand(obj => DeleteMembership());
            DeleteOrderCommand = new RelayCommand(obj => DeleteOrder());
            EditorCommand = new RelayCommand(obj => OpenEditor(SelectedMembership));
            MembershipOrders = new ObservableCollection<MembershipOrder>(DataWorker.GetAllMembershipOrders());
            Schedules = new ObservableCollection<Schedule>(DataWorker.GetAllSchedules());
            ClientSessionRecords = new ObservableCollection<ClientSessionRecord>(DataWorker.GetAllClientSessionRecords());
            CoachReviews = new ObservableCollection<CoachReview>(DataWorker.LoadCoachReviews());
            SessionReviews = new ObservableCollection<SessionReview>(DataWorker.LoadSessionReviews());
 
            AllMemberships();
            AllUsers();
            AllOrders();
        }

        private void AllUsers()
        {
            Users = DataWorker.GetAllUsers();
            OnPropertyChanged(nameof(Users));
        }

        private void AllMemberships()
        {
            Memberships = DataWorker.GetAllMemberships();
            OnPropertyChanged(nameof(Memberships));
        }

        private void AllOrders()
        {
            MembershipOrders = new ObservableCollection<MembershipOrder>(DataWorker.GetAllMembershipOrders());
            OnPropertyChanged(nameof(MembershipOrders));
        }

        private void DeleteMembership()
        {
            if (SelectedTab.Name == "Memberships")
            {
                DataWorker.DeleteMembership(SelectedMembership);
                AllMemberships();
            }
        }

        private void DeleteOrder()
        {
            if (SelectedTab.Name == "Orders")
            {
                DataWorker.DeleteOrder(SelectedOrder);
                AllOrders();
            }
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
            MembershipsWindow membershipsWindow = new MembershipsWindow();
            Application.Current.MainWindow = membershipsWindow;

            Application.Current.Windows
            .OfType<Window>()
            .FirstOrDefault(w => w is AdminWindow)?
            .Close();

            Application.Current.MainWindow.Show();
        }

        private void OpenEditor(Membership membership)
        {
            EditWindow window = new EditWindow();
            EditViewModel viewModel = new EditViewModel(membership);
            window.DataContext = viewModel;
            viewModel.RequestClose += () => window.Close();
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();

            AllMemberships();
        }

        private void OpenCoaches()
        {
            CoachesWindow coachesWindow = new CoachesWindow();
            Application.Current.MainWindow = coachesWindow;

            Application.Current.Windows
            .OfType<Window>()
            .FirstOrDefault(w => w is AdminWindow)?
            .Close();

            Application.Current.MainWindow.Show();
        }

        private void OpenSchedules()
        {
            SchedulesWindow coachesWindow = new SchedulesWindow();
            Application.Current.MainWindow = coachesWindow;

            Application.Current.Windows
            .OfType<Window>()
            .FirstOrDefault(w => w is AdminWindow)?
            .Close();

            Application.Current.MainWindow.Show();
        }
    }
}
