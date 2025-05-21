using Sportics.Model;
using Sportics.View;
using System.Linq;
using System.Windows.Input;
using System.Windows;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Sportics.ViewModel
{
    public class LoginViewModel : BaseViewModel, IDataErrorInfo
    {
        private string email;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
                IsValidationActive = false;
            }
        }

        private string password;
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
                IsValidationActive = false;
            }
        }

        private bool isValidationActive = false;
        public bool IsValidationActive
        {
            get => isValidationActive;
            set
            {
                isValidationActive = value;
                OnPropertyChanged(nameof(IsValidationActive));
            }
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (!IsValidationActive) return null;

                switch (columnName)
                {
                    case nameof(Email):
                        if (string.IsNullOrWhiteSpace(Email))
                            return "Введите почту";
                        if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                            return "Неверный формат почты.";
                        if (Email.Length > 50)
                            return "Длина строки превышает максимум.";
                        break;

                    case nameof(Password):
                        if(string.IsNullOrWhiteSpace(Password))  
                            return "Введите пароль";
                        if (Password.Length > 50)
                            return "Длина строки превышает максимум.";
                        break;
                }
                return null;
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand OpenRegisterCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(obj => Login());
            OpenRegisterCommand = new RelayCommand(obj => OpenRegister());
        }

        private void Login()
        {
            IsValidationActive = true;
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Password));

            if (!string.IsNullOrWhiteSpace(this[nameof(Email)]) || !string.IsNullOrWhiteSpace(this[nameof(Password)]))
                return;

            if (DataWorker.CheckUser(Email, Password))
            {
                User user = DataWorker.SelectUser(Email, Password);

                if (user.Status == "Заблокирован")
                {
                    ShowMessage("Ваша учетная запись заблокирована.");
                    return;
                }

                Session.CurrentUser = user;

                if (user.Role == "Администратор")
                {
                    AdminWindow adminWindow = new AdminWindow();
                    Application.Current.MainWindow = adminWindow;

                    Application.Current.Windows
                        .OfType<Window>()
                        .FirstOrDefault(w => w is LoginWindow)?
                        .Close();

                    Application.Current.MainWindow.Show();
                }
                else
                {
                    MainWindow mainWindow = new MainWindow();
                    Application.Current.MainWindow = mainWindow;

                    Application.Current.Windows
                        .OfType<Window>()
                        .FirstOrDefault(w => w is LoginWindow)?
                        .Close();

                    Application.Current.MainWindow.Show();
                }
            }
            else
            {
                ShowMessage("Неверный email или пароль");
            }
        }

        private void OpenRegister()
        {
            RegistrationWindow regWindow = new RegistrationWindow();
            Application.Current.MainWindow = regWindow;

            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is LoginWindow)?
                .Close();

            Application.Current.MainWindow.Show();
        }

        private void ShowMessage(string message)
        {
            MessageWindow messageWindow = new MessageWindow();
            MessageViewModel viewModel = new MessageViewModel(message);
            messageWindow.DataContext = viewModel;
            viewModel.RequestClose += () => messageWindow.Close();
            messageWindow.Owner = Application.Current.MainWindow;
            messageWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            messageWindow.ShowDialog();
        }

    }
}
