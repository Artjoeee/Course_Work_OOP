using Sportics.Model;
using Sportics.View;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class AccountViewModel: BaseViewModel
    {
        public string UserName => Session.CurrentUser?.Name ?? "Админ";

        public decimal UserBalance => Session.CurrentUser?.Balance ?? 0;

        public ICommand ExitCommand { get; }
        public ICommand BalanceCommand { get; }

        public AccountViewModel() 
        {
            ExitCommand = new RelayCommand(obj => Exit());
            BalanceCommand = new RelayCommand(obj => OpenBalance());

            Session.BalanceUpdated += OnBalanceUpdated;
        }

        private void OnBalanceUpdated()
        {
            OnPropertyChanged(nameof(UserBalance)); // Обновит текст в интерфейсе
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

        private decimal _balance;
        public decimal Balance
        {
            get => _balance;
            set
            {
                _balance = value;
                OnPropertyChanged();
            }
        }

        ~AccountViewModel()
        {
        Session.BalanceUpdated -= OnBalanceUpdated;
        }
    }
}
