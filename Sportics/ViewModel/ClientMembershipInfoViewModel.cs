using Sportics.Model;
using Sportics.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        public ClientMembershipInfoViewModel() { }

        public event Action RequestClose;

        private void BuyMembership()
        {
            RequestClose?.Invoke();
        }

        private void OpenReview(Membership membership)
        {
            EditWindow window = new EditWindow();
            EditViewModel viewModel = new EditViewModel(membership);
            window.DataContext = viewModel;
            viewModel.RequestClose += () => window.Close();
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }
    }
}
