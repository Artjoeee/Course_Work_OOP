using Sportics.Model;
using Sportics.View;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class AccountViewModel: BaseViewModel
    {
        public User CurrentUser = Session.CurrentUser;
        public string UserName => Session.CurrentUser?.Name ?? "Админ";

        public ICommand ExitCommand { get; }
        public ICommand BalanceCommand { get; }

        public AccountViewModel() 
        {
            ExitCommand = new RelayCommand(obj => Exit());
            BalanceCommand = new RelayCommand(obj => Balance());
        }

        private void Exit()
        {
            Session.Logout();

            LoginWindow loginWindow = new LoginWindow();
            Application.Current.MainWindow = loginWindow;

            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is MainWindow || w is AdminWindow || w is AccountWindow)?
                .Close();

            Application.Current.MainWindow.Show();
        }

        private void Balance()
        {
            BalanceWindow balanceWindow = new BalanceWindow();
            balanceWindow.Owner = Application.Current.MainWindow;
            balanceWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            balanceWindow.ShowDialog();
        }
    }
}
