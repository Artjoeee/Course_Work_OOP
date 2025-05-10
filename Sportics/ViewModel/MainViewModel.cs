using Sportics.Model;
using Sportics.View;
using System.Windows;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand OpenLoginCommand { get; }
        public ICommand OpenAccountCommand { get; }

        public MainViewModel()
        {
            //OpenLoginCommand = new RelayCommand(obj => OpenLogin());
            OpenAccountCommand = new RelayCommand(obj => OpenAccount());
        }

        //private void OpenLogin()
        //{
        //    LoginWindow loginWindow = new LoginWindow();
        //    loginWindow.Owner = Application.Current.MainWindow;
        //    loginWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        //    loginWindow.ShowDialog();
        //}

        private void OpenAccount()
        {
            AccountWindow accountWindow = new AccountWindow();
            accountWindow.Owner = Application.Current.MainWindow;
            accountWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            accountWindow.ShowDialog();
        }
    }
}
