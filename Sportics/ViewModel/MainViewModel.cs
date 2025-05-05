using Sportics.Model;
using Sportics.View;
using System.Windows;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public RelayCommand OpenLoginCommand { get; }

        //public MainViewModel()
        //{
        //    OpenLoginCommand = new RelayCommand(obj => OpenLogin());
        //}

        //private void OpenLogin()
        //{
        //    LoginWindow loginWindow = new LoginWindow();
        //    loginWindow.Owner = Application.Current.MainWindow;
        //    loginWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        //    loginWindow.ShowDialog();
        //}
    }
}
