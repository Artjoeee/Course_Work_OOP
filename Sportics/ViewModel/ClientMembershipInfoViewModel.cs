using Sportics.Model;
using Sportics.View;
using System;
using System.Windows.Input;
using System.Windows;

namespace Sportics.ViewModel
{
    public class ClientMembershipInfoViewModel: BaseViewModel
    {
        public Membership Membership { get; set; }

        public ICommand BuyMembershipCommand { get; }

        public ICommand ReviewCommand { get; }

        public ClientMembershipInfoViewModel(Membership membership)
        {
            Membership = membership;
            BuyMembershipCommand = new RelayCommand(obj => BuyMembership());
            ReviewCommand = new RelayCommand(obj => OpenReview(Membership));
            OnPropertyChanged(nameof(CanBuy));
        }

        public ClientMembershipInfoViewModel() { }

        public event Action RequestClose;

        public bool CanBuy =>
            Session.CurrentUser?.Balance >= Membership?.Price &&
            !DataWorker.HasActiveMembership(Session.CurrentUser.Id, Membership.Id);

        private void BuyMembership()
        {
            User user = Session.CurrentUser;

            if (DataWorker.HasActiveMembership(user.Id, Membership.Id))
            {
                ShowMessage("У вас уже есть активный абонемент этого типа.");
                return;
            }

            if (user.Balance < Membership.Price)
            {
                ShowMessage("Недостаточно средств для покупки абонемента.");
                return;
            }

            // Пытаемся списать баланс
            bool success = DataWorker.DeductBalance(user.Id, Membership.Price);
            if (!success)
            {
                ShowMessage("Ошибка при списании средств.");
                return;
            }

            user.Balance -= Membership.Price;

            DateTime startDate = DateTime.Today;
            DateTime endDate = startDate.AddDays(Membership.DurationInDays);

            // Добавляем заказ
            DataWorker.SaveOrder(user.Id, user.Name, Membership.Id, Membership.FullName, endDate);
            RequestClose?.Invoke();

            ShowMessage("Абонемент успешно оформлен");
        }


        private void OpenReview(Membership membership)
        {
            ReviewWindow window = new ReviewWindow();
            ReviewViewModel viewModel = new ReviewViewModel();
            window.DataContext = viewModel;
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
    }
}
