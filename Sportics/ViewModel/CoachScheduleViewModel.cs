using Sportics.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class CoachScheduleViewModel : BaseViewModel
    {
        public ObservableCollection<Schedule> CoachSchedules { get; set; }

        public ICommand SignUpCommand { get; }

        private Coach _coach;

        public CoachScheduleViewModel(Coach coach)
        {
            _coach = coach;

            var allSchedules = DataWorker.GetAllSchedules();
            var filtered = allSchedules
                .Where(s => s.Coach != null && s.Coach.Id == coach.Id)
                .ToList();

            CoachSchedules = new ObservableCollection<Schedule>(filtered);

            SignUpCommand = new RelayCommand(SignUpForSession);
        }

        private void SignUpForSession(object obj)
        {
            if (obj is Schedule schedule)
            {
                var user = Session.CurrentUser;
                if (user == null)
                {
                    MessageBox.Show("Вы не авторизованы.");
                    return;
                }

                // Проверка: не записан ли уже пользователь на это занятие
                bool alreadyEnrolled = DataWorker
                    .GetAllClientSessionRecords()
                    .Any(r => r.ClientId == user.Id && r.ScheduleId == schedule.Id);

                if (alreadyEnrolled)
                {
                    MessageBox.Show("Вы уже записаны на это занятие.");
                    return;
                }

                // Получение всех заказов пользователя
                var userOrders = DataWorker.GetUserMembershipOrders(user.Id);

                // Поиск подходящего абонемента
                var matchingOrder = userOrders
                    .Where(order => order.Membership != null &&
                                    (order.Membership.Category == schedule.Category ||
                                     order.Membership.ShortName == schedule.Category)) // по необходимости
                    .FirstOrDefault();

                if (matchingOrder == null)
                {
                    MessageBox.Show("У вас нет подходящего абонемента для этой тренировки.");
                    return;
                }

                // Запись на занятие
                var record = new ClientSessionRecord
                {
                    ClientId = user.Id,
                    ScheduleId = schedule.Id,
                    Category = schedule.Category,
                    Date = schedule.Date,
                    Time = schedule.Time,
                    MembershipId = matchingOrder.MembershipId,
                    MembershipOrderId = matchingOrder.Id
                };

                DataWorker.SaveClientSessionRecord(record);

                MessageBox.Show("Вы успешно записались!");
            }
        }


        public event Action RequestClose;
    }
}
