using Sportics.Model;
using Sportics.View;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class ClientScheduleInfoViewModel : BaseViewModel
    {
        public Schedule Schedule { get; set; }

        public ICommand SignUpCommand { get; }
        public ICommand LeaveReviewCommand { get; }

        public bool CanLeaveReview { get; set; }

        public ClientScheduleInfoViewModel(Schedule schedule)
        {
            Schedule = schedule;

            SignUpCommand = new RelayCommand(obj => SignUp());
            LeaveReviewCommand = new RelayCommand(obj => LeaveReview());

            EvaluateAccess();
        }

        public ClientScheduleInfoViewModel() { }

        private void EvaluateAccess()
        {
            User user = Session.CurrentUser;

            if (user == null)
                return;

            bool isEnrolled = DataWorker
                .GetAllClientSessionRecords()
                .Any(r => r.ClientId == user.Id && r.ScheduleId == Schedule.Id);

            DateTime dateTime = Schedule.Date.Date + Schedule.Time;
            DateTime now = DateTime.Now;

            CanLeaveReview = isEnrolled && now > dateTime && now <= dateTime.AddDays(1);

            if (now > dateTime)
            {
                Schedule.Status = "Завершено";
                DataWorker.UpdateSchedule(Schedule);
            }
        }

        private void SignUp()
        {
            User user = Session.CurrentUser;

            if (user == null)
                return;

            DateTime sessionDateTime = Schedule.Date.Date + Schedule.Time;
            if (DateTime.Now > sessionDateTime)
            {
                ShowMessage("Нельзя записаться на прошедшее занятие.");
                return;
            }

            // Проверка: уже записан на это занятие?
            bool alreadyEnrolled = DataWorker
                .GetAllClientSessionRecords()
                .Any(r => r.ClientId == user.Id && r.ScheduleId == Schedule.Id);

            if (alreadyEnrolled)
            {
                ShowMessage("Вы уже записаны на это занятие.");
                return;
            }

            var orders = DataWorker.GetUserMembershipOrders(user.Id);

            MembershipOrder matchingOrder = orders.FirstOrDefault(o =>
                o.Membership != null &&
                (o.Membership.Category == Schedule.Category));

            if (matchingOrder == null)
            {
                ShowMessage("У вас нет подходящего абонемента для этого занятия.");
                return;
            }

            ClientSessionRecord record = new ClientSessionRecord
            {
                ClientId = user.Id,
                ScheduleId = Schedule.Id,
                Category = Schedule.Category,
                Date = Schedule.Date,
                Time = Schedule.Time,
                MembershipId = matchingOrder.MembershipId,
                MembershipOrderId = matchingOrder.Id
            };

            CanLeaveReview = false;
            OnPropertyChanged(nameof(CanLeaveReview));

            DataWorker.SaveClientSessionRecord(record);

            RequestClose?.Invoke();
            ShowMessage("Вы успешно записались!");
        }


        private void LeaveReview()
        {
            ReviewScheduleWindow window = new ReviewScheduleWindow();
            window.DataContext = new ReviewScheduleViewModel(Schedule);
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
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

        public event Action RequestClose;
    }
}
