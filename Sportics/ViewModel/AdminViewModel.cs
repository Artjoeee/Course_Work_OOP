using Sportics.Helper;
using Sportics.Model;
using Sportics.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

        private bool _isWeeklyOffer;
        public bool IsWeeklyOffer
        {
            get => _isWeeklyOffer;
            set
            {
                if (_isWeeklyOffer != value)
                {
                    _isWeeklyOffer = value;
                    OnPropertyChanged(nameof(IsWeeklyOffer));

                    DataWorker.UpdateMembership(SelectedMembership);
                }
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

        public ObservableCollection<Coach> Coaches { get; set; }
        public Coach SelectedCoach { get; set; }

        private User _selectedUser;
        public User SelectedUser
        {
            get => _selectedUser;
            set { _selectedUser = value; OnPropertyChanged(); }
        }

        public ICommand DeleteCoachCommand { get; }

        public ICommand EditCoachCommand { get; }

        public ICommand BlockUserCommand { get; }

        public ICommand OpenAccountCommand { get; }

        public ICommand OpenMembershipsCommand { get; }

        public ICommand OpenCoachesCommand { get; }

        public ICommand OpenSchedulesCommand { get; }

        public ICommand DeleteMembershipCommand { get; }

        public ICommand DeleteOrderCommand { get; }

        public ICommand EditorCommand { get; }

        public ICommand EditScheduleCommand { get; }

        public ICommand DeleteScheduleCommand { get; }

        public ICommand DeleteClientSessionRecordCommand { get; }

        public ICommand SetAsWeeklyOfferCommand { get; }

        public AdminViewModel() 
        {
            OpenAccountCommand = new RelayCommand(obj => OpenAccount());
            OpenMembershipsCommand = new RelayCommand(obj => OpenMemberships());
            OpenCoachesCommand = new RelayCommand(obj => OpenCoaches());
            OpenSchedulesCommand = new RelayCommand(obj => OpenSchedules());

            DeleteMembershipCommand = new RelayCommand(obj => DeleteMembership());
            DeleteOrderCommand = new RelayCommand(obj => DeleteOrder());
            DeleteCoachCommand = new RelayCommand(obj => DeleteCoach());
            DeleteScheduleCommand = new RelayCommand(obj => DeleteSchedule());
            DeleteClientSessionRecordCommand = new RelayCommand(obj => DeleteClientSessionRecord());

            EditorCommand = new RelayCommand(obj => OpenEditor(SelectedMembership));
            EditCoachCommand = new RelayCommand(obj => EditCoach(SelectedCoach));
            EditScheduleCommand = new RelayCommand(obj => EditSchedule(SelectedSchedule));

            Coaches = new ObservableCollection<Coach>(DataWorker.GetAllCoaches());
            MembershipOrders = new ObservableCollection<MembershipOrder>(DataWorker.GetAllMembershipOrders());
            Schedules = new ObservableCollection<Schedule>(DataWorker.GetAllSchedules());
            ClientSessionRecords = new ObservableCollection<ClientSessionRecord>(DataWorker.GetAllClientSessionRecords());
            CoachReviews = new ObservableCollection<CoachReview>(DataWorker.LoadCoachReviews());
            SessionReviews = new ObservableCollection<SessionReview>(DataWorker.LoadSessionReviews());

            BlockUserCommand = new RelayCommand(obj => BlockUser());

            SetAsWeeklyOfferCommand = new RelayCommand(obj => SetAsWeeklyOffer(SelectedMembership));

            AllMemberships();
            AllUsers();
            AllOrders();
            AllCoaches();
            AllSchedules();
            AllClientSessionRecords();

            DeleteExpiredOrders();
        }

        private void AllUsers()
        {
            Users = DataWorker.GetAllUsers();
            OnPropertyChanged(nameof(Users));
        }

        private void AllCoaches()
        {
            Coaches = new ObservableCollection<Coach>(DataWorker.GetAllCoaches());
            OnPropertyChanged(nameof(Coaches));
        }

        private void AllSchedules()
        {
            Schedules = new ObservableCollection<Schedule>(DataWorker.GetAllSchedules());
            OnPropertyChanged(nameof(Schedules));
        }

        private void AllClientSessionRecords()
        {
            ClientSessionRecords = new ObservableCollection<ClientSessionRecord>(DataWorker.GetAllClientSessionRecords());
            OnPropertyChanged(nameof(ClientSessionRecords));
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

        private void BlockUser()
        {
            if (SelectedUser != null)
            {
                if (SelectedUser.Status == "Активен") 
                {
                    SelectedUser.Status = "Заблокирован";
                    DataWorker.UpdateUser(SelectedUser);
                }
                else
                {
                    SelectedUser.Status = "Активен";
                    DataWorker.UpdateUser(SelectedUser);
                }

                AllUsers();
            }
        }

        private void DeleteCoach()
        {
            if (SelectedCoach != null && (SelectedCoach.Schedules?.Count ?? 0) == 0)
            {
                DataWorker.DeleteCoach(SelectedCoach);
                Coaches = new ObservableCollection<Coach>(DataWorker.GetAllCoaches());
                OnPropertyChanged(nameof(Coaches));
                AllCoaches();
            }
            else
            {
                ShowMessage("У тренера ещё запланированы занятия");
            }
        }

        private void DeleteSchedule()
        {
            if (SelectedSchedule != null && (SelectedSchedule.ClientSessionRecords?.Count ?? 0) == 0)
            {
                DataWorker.DeleteSchedule(SelectedSchedule);
                Schedules = new ObservableCollection<Schedule>(DataWorker.GetAllSchedules());
                OnPropertyChanged(nameof(Schedules));
                AllSchedules();
                AllCoaches();
            }
            else
            {
                ShowMessage("На занятие ещё записаны люди");
            }
        }

        private void DeleteClientSessionRecord()
        {
            if (SelectedClientSessionRecord != null)
            {
                DataWorker.DeleteClientSessionRecord(SelectedClientSessionRecord);
                ClientSessionRecords = new ObservableCollection<ClientSessionRecord>(DataWorker.GetAllClientSessionRecords());
                OnPropertyChanged(nameof(ClientSessionRecords));
                AllClientSessionRecords();
                AllOrders();
            }
        }

        private void DeleteMembership()
        {
            if (SelectedMembership != null)
            {
                DataWorker.DeleteMembership(SelectedMembership);
                AllMemberships();
            }
        }

        private void DeleteOrder()
        {
            if (SelectedOrder != null && (SelectedOrder.ClientSession?.Count ?? 0) == 0)
            {
                DataWorker.DeleteOrder(SelectedOrder);
                AllOrders();
            }
            else
            {
                ShowMessage("Пользователь ещё записан на занятия");
            }
        }

        private void DeleteExpiredOrders()
        {
            DataWorker.DeleteExpiredOrders();
            AllOrders();
        }


        private void EditCoach(Coach coach)
        {
            if (SelectedCoach != null)
            {
                if (SelectedTab.Name == "Coaches")
                {
                    EditCoachWindow window = new EditCoachWindow();
                    EditCoachViewModel viewModel = new EditCoachViewModel(coach);
                    window.DataContext = viewModel;
                    viewModel.RequestClose += () => window.Close();
                    window.Owner = Application.Current.MainWindow;
                    window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    window.ShowDialog();

                    AllMemberships();
                }

                Coaches = new ObservableCollection<Coach>(DataWorker.GetAllCoaches());
                OnPropertyChanged(nameof(Coaches));
                AllCoaches();
            }
        }


        private void EditSchedule(Schedule schedule)
        {
            if (SelectedSchedule != null)
            {
                if (SelectedTab.Name == "Schedule")
                {
                    EditScheduleWindow window = new EditScheduleWindow();
                    EditScheduleViewModel viewModel = new EditScheduleViewModel(schedule);
                    window.DataContext = viewModel;
                    viewModel.RequestClose += () => window.Close();
                    window.Owner = Application.Current.MainWindow;
                    window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    window.ShowDialog();
                }

                Schedules = new ObservableCollection<Schedule>(DataWorker.GetAllSchedules());
                OnPropertyChanged(nameof(Schedules));
                AllSchedules();
            }
        }

        private void SetAsWeeklyOffer(Membership membership)
        {
            if (membership == null)
                return;

            if (!membership.IsWeeklyOffer)
            {
                int currentOffers = Memberships.Count(m => m.IsWeeklyOffer);
                if (currentOffers >= 3)
                {
                    ShowMessage("Максимум 3 предложения недели могут быть активны одновременно.");
                    return;
                }

                membership.IsWeeklyOffer = true;
                DataWorker.UpdateMembership(membership);
                ShowMessage($"{membership.ShortName} добавлен в предложения недели.");
            }
            else
            {
                membership.IsWeeklyOffer = false;
                DataWorker.UpdateMembership(membership);
                ShowMessage($"{membership.ShortName} удалён из предложений недели.");
            }

            AllMemberships();
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
            if (SelectedTab != null)
            {
                if (SelectedTab.Name == "Memberships")
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
            }
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

        private void ShowMessage(string message)
        {
            var messageWindow = new MessageWindow();
            var viewModel = new MessageViewModel(message);
            messageWindow.DataContext = viewModel;
            viewModel.RequestClose += () => messageWindow.Close();
            messageWindow.Owner = System.Windows.Application.Current.MainWindow;
            messageWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            messageWindow.ShowDialog();
        }
    }
}
