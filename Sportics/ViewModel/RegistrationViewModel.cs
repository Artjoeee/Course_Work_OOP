using Sportics.Model;
using Sportics.View;
using System.Linq;
using System.Windows.Input;
using System.Windows;

namespace Sportics.ViewModel
{
    public class RegistrationViewModel : BaseViewModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public ICommand RegisterCommand { get; }

        public RegistrationViewModel()
        {
            RegisterCommand = new RelayCommand(obj => Register());
        }

        private void Register()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Phone) || string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (Password != ConfirmPassword)
            {
                MessageBox.Show("Пароли не совпадают.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (!DataWorker.CheckEmailAndPhoneNumber(Email, Phone))
            {
                MessageBox.Show("Пользователь с такой почтой или телефоном уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                DataWorker.AddUser(Name, Email, Phone, Password);

                Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is RegistrationWindow)?
                .Close();

                MessageBox.Show("Регистрация успешна!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
