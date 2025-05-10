using Sportics.Model;
using Sportics.View;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class AccountViewModel: BaseViewModel
    {
        public ICommand ExitCommand { get; }

        public AccountViewModel() 
        {
            ExitCommand = new RelayCommand(obj => Exit());
        }

        private void Exit()
        {
            LoginWindow loginWindow = new LoginWindow();
            Application.Current.MainWindow = loginWindow;

            Application.Current.Windows
            .OfType<Window>()
            .FirstOrDefault(w => w is MainWindow || w is AdminWindow)?
            .Close();

            Application.Current.MainWindow.Show();
        }
    }
}
