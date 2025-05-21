using Sportics.Model;
using Sportics.View;
using System.Linq;
using System.Windows.Input;
using System.Windows;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System;

namespace Sportics.ViewModel
{
    public class RegistrationViewModel : BaseViewModel, IDataErrorInfo
    {
        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
                IsValidationActive = false;
            }
        }

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

        private string phone;
        public string Phone
        {
            get => phone;
            set
            {
                phone = value;
                OnPropertyChanged(nameof(Phone));
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

        private string confirmPassword;
        public string ConfirmPassword
        {
            get => confirmPassword;
            set
            {
                confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
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

        public ICommand RegisterCommand { get; }

        public ICommand OpenLoginCommand { get; }

        public RegistrationViewModel()
        {
            RegisterCommand = new RelayCommand(obj => Register());
            OpenLoginCommand = new RelayCommand(obj => OpenLogin());
        }

        private void Register()
        {
            IsValidationActive = true;

            // Обновляем свойства, чтобы сработала IDataErrorInfo
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Phone));
            OnPropertyChanged(nameof(Password));
            OnPropertyChanged(nameof(ConfirmPassword));

            // Проверка всех полей на ошибки
            var propertiesToCheck = new[] { nameof(Name), nameof(Email), nameof(Phone), nameof(Password), nameof(ConfirmPassword) };
            bool hasErrors = propertiesToCheck.Any(prop => !string.IsNullOrWhiteSpace(this[prop]));

            if (hasErrors)
                return;

            if (!DataWorker.CheckEmailAndPhoneNumber(Email, Phone))
            {
                ShowMessage("Почта (телефон) уже зарегистрирована");
                return;
            }

            DataWorker.AddUser(Name, Email, Phone, Password);

            User user = DataWorker.SelectUser(Email, Password);
            Session.CurrentUser = user;

            MainWindow mainWindow = new MainWindow();
            Application.Current.MainWindow = mainWindow;

            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is RegistrationWindow)?
                .Close();

            Application.Current.MainWindow.Show();
        }


        private void OpenLogin()
        {
            LoginWindow window = new LoginWindow();
            Application.Current.MainWindow = window;

            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is RegistrationWindow)?
                .Close();

            Application.Current.MainWindow.Show();
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (!isValidationActive)
                    return null;

                switch (columnName)
                {
                    case nameof(Name):
                        if (string.IsNullOrWhiteSpace(Name))
                            return "Имя обязательно для заполнения.";
                        if (Name.Length > 50)
                            return "Длина строки превышает максимум.";

                        if (Regex.IsMatch(Name, @"\d"))
                            return "Имя не должно содержать цифры.";

                        var nameParts = Name.Trim().Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
                        if (nameParts.Length < 2 || nameParts.Length > 3)
                            return "Введите имя и фамилию (отчество — при необходимости), разделённые пробелами.";

                        var nameWithoutSpaces = string.Concat(nameParts);
                        if (nameWithoutSpaces.Length < 4)
                            return "Имя должно содержать не менее 4 символов (без учёта пробелов).";
                        break;
                    case nameof(Email):
                        if (string.IsNullOrWhiteSpace(Email))
                            return "Почта обязательна.";
                        if (Email.Length > 50)
                            return "Длина строки превышает максимум.";
                        if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                            return "Неверный формат почты.";
                        break;
                    case nameof(Phone):
                        if (string.IsNullOrWhiteSpace(Phone))
                            return "Телефон обязателен.";
                        if (Phone.Length > 50)
                            return "Длина строки превышает максимум.";
                        if (!Regex.IsMatch(Phone, @"^\+?\d{10,15}$"))
                            return "Неверный формат телефона.";
                        break;
                    case nameof(Password):
                        if (string.IsNullOrWhiteSpace(Password))
                            return "Пароль обязателен.";
                        if (Password.Length > 50)
                            return "Длина строки превышает максимум.";

                        if (!Regex.IsMatch(Password, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$"))
                            return "Пароль должен содержать 1 цифру, строчную и заглавную букву, длину не меньше 6.";
                        break;
                    case nameof(ConfirmPassword):
                        if (ConfirmPassword != Password)
                            return "Пароли не совпадают.";
                        break;
                }

                return null;
            }
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
