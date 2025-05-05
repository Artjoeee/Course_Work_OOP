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
                if (DataWorker.SelectUser(Email, Password).Role == "Администратор")
                {
                    AdminWindow adminWindow = new AdminWindow();
                    Application.Current.MainWindow = adminWindow;

                    Application.Current.Windows
                    .OfType<Window>()
                    .FirstOrDefault(w => w is LoginWindow)?
                    .Close();

                    Application.Current.MainWindow.Show();
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
            RegistrationWindow registrationWindow = new RegistrationWindow();
            Application.Current.MainWindow = registrationWindow;

            Application.Current.Windows
            .OfType<Window>()
            .FirstOrDefault(w => w is LoginWindow)?
            .Close();

            Application.Current.MainWindow.Show();
        }
    }
}
