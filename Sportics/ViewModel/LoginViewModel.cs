using Sportics.Model;
using Sportics.View;
using System.Linq;
using System.Windows.Input;
using System.Windows;

namespace Sportics.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public ICommand LoginCommand { get; }

        public ICommand OpenRegisterCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(obj => Login());
            OpenRegisterCommand = new RelayCommand(obj => OpenRegister());
        }

        private void Login()
        {
            if (DataWorker.CheckUser(Email, Password))
            {
                Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is LoginWindow)?
                .Close();

                MessageBox.Show($"Добро пожаловать, {Email}!", "Успешный вход", MessageBoxButton.OK, MessageBoxImage.Information);

                if (DataWorker.SelectUser(Email, Password).Role == "Администратор")
                {
                    AdminWindow adminnWindow = new AdminWindow();
                    adminnWindow.Owner = Application.Current.MainWindow;
                    adminnWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    adminnWindow.ShowDialog();

                    Application.Current.Windows
                    .OfType<Window>()
                    .FirstOrDefault(w => w is MainWindow)?
                    .Close();
                }
            }
            else if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Пожалуйста, введите email и пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                MessageBox.Show($"Неверный email или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }

        private void OpenRegister()
        {
            Application.Current.Windows
            .OfType<Window>()
            .FirstOrDefault(w => w is LoginWindow)?
            .Close();

            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Owner = Application.Current.MainWindow;
            registrationWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            registrationWindow.ShowDialog();
        }
    }
}
