using Sportics.Model;
using Sportics.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Runtime.InteropServices.ComTypes;

namespace Sportics.ViewModel
{
    public class ClientMembershipInfoViewModel: BaseViewModel
    {
        public Membership Membership { get; set; }

        public bool CanBuy => Session.CurrentUser?.Balance >= Membership?.Price;

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

        private void BuyMembership()
        {
            User user = Session.CurrentUser;

            // Пытаемся списать баланс
            bool success = DataWorker.DeductBalance(user.Id, Membership.Price);

            Session.CurrentUser.Balance -= Membership.Price;

            DateTime startDate = DateTime.Today;
            DateTime endDate = startDate.AddDays(Membership.DurationInDays);

            // Добавляем заказ
            DataWorker.SaveOrder(user.Id, user.Name, Membership.Id, Membership.FullName, endDate);

            RequestClose?.Invoke();
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
    }
}
