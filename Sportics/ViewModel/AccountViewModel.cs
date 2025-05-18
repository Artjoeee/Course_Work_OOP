using Sportics.Model;
using Sportics.View;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class AccountViewModel : BaseViewModel
    {
        public bool IsClient => Session.CurrentUser?.Role?.ToLower() == "клиент";

        public string UserName => Session.CurrentUser?.Name ?? "Админ";

        public decimal UserBalance => Session.CurrentUser?.Balance ?? 0;

        public ICommand ExitCommand { get; }
        public ICommand BalanceCommand { get; }
        public ICommand CancelRegistrationCommand { get; }
        public ICommand CancelScheduleCommand { get; }

        private ObservableCollection<Schedule> _userSchedules;
        public ObservableCollection<Schedule> UserSchedules
        {
            get => _userSchedules;
            set
            {
                _userSchedules = value;
                OnPropertyChanged();
            }
        }


        private Schedule _selectedSchedule;
        public Schedule SelectedSchedule
        {
            get => _selectedSchedule;
            set
            {
                _selectedSchedule = value;
                OnPropertyChanged();
            }
        }

        public AccountViewModel()
        {
            ExitCommand = new RelayCommand(_ => Exit());
            BalanceCommand = new RelayCommand(_ => OpenBalance());
            CancelScheduleCommand = new RelayCommand(obj => CancelSchedule(obj));

            Session.BalanceUpdated += OnBalanceUpdated;

            LoadUserSchedules();
        }

        private void LoadUserSchedules()
        {
            var userId = Session.CurrentUser?.Id ?? 0;
            var schedules = DataWorker.LoadUserSchedules(userId);
            UserSchedules = new ObservableCollection<Schedule>(schedules);
            OnPropertyChanged(nameof(UserSchedules));
        }

        private void CancelSchedule(object obj)
        {
            if (obj is Schedule schedule)
            {
                bool success = DataWorker.CancelUserSchedule(Session.CurrentUser.Id, schedule.Id);
                if (success)
                {
                    UserSchedules.Remove(schedule);
                    OnPropertyChanged(nameof(UserSchedules));
                    ShowMessage("Запись отменена");
                }
                else
                {
                    ShowMessage("Не удалось отменить запись");
                }
            }
        }


        private void OnBalanceUpdated()
        {
            OnPropertyChanged(nameof(UserBalance));
        }

        private void Exit()
        {
            Session.Logout();

            LoginWindow loginWindow = new LoginWindow();
            Application.Current.MainWindow = loginWindow;

            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is MainWindow || w is AdminWindow || w is AccountWindow 
                    || w is CoachesWindow || w is MembershipsWindow || w is SchedulesWindow || w is ClientCoachesWindow
                    || w is ClientMembershipsWindow || w is ClientSchedulesWindow)?
                .Close();

            Application.Current.MainWindow.Show();
        }

        private void OpenBalance()
        {
            BalanceWindow balanceWindow = new BalanceWindow();
            BalanceViewModel balanceViewModel = new BalanceViewModel();
            balanceWindow.DataContext = balanceViewModel;
            balanceViewModel.RequestClose += () => balanceWindow.Close();
            balanceWindow.Owner = Application.Current.MainWindow;
            balanceWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            balanceWindow.ShowDialog();
        }

        private void ShowMessage(string message)
        {
            var messageWindow = new MessageWindow();
            var viewModel = new MessageViewModel(message);
            messageWindow.DataContext = viewModel;
            viewModel.RequestClose += () => messageWindow.Close();
            messageWindow.Owner = Application.Current.MainWindow;
            messageWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            messageWindow.ShowDialog();
        }

        ~AccountViewModel()
        {
            Session.BalanceUpdated -= OnBalanceUpdated;
        }
    }
}
