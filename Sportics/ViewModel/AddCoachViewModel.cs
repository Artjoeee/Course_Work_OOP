using Microsoft.Win32;
using Sportics.Model;
using Sportics.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class AddCoachViewModel : BaseViewModel, IDataErrorInfo
    {
        public List<string> Specializations { get; } = new List<string>
        {
            "Фитнес",
            "Йога",
            "Бассейн",
            "Тренажерный зал",
            "Танцы"
        };

        public string Name
        {
            get => name;
            set { name = value; OnPropertyChanged(); IsValidationActive = false; }
        }

        public string Specialization
        {
            get => specialization;
            set { specialization = value; OnPropertyChanged(); IsValidationActive = false; }
        }

        public string Phone
        {
            get => phone;
            set { phone = value; OnPropertyChanged(); IsValidationActive = false; }
        }

        public string Email
        {
            get => email;
            set { email = value; OnPropertyChanged(); IsValidationActive = false; }
        }

        public string Information
        {
            get => information;
            set { information = value; OnPropertyChanged(); IsValidationActive = false; }
        }

        public byte[] PhotoData
        {
            get => photoData;
            set { photoData = value; OnPropertyChanged(); IsValidationActive = false; }
        }

        private string name;
        private string specialization;
        private string phone;
        private string email;
        private string information;
        private byte[] photoData;

        private bool isValidationActive = false;
        public bool IsValidationActive
        {
            get => isValidationActive;
            set { isValidationActive = value; OnPropertyChanged(); }
        }

        public ICommand SelectPhotoCommand { get; }
        public ICommand AddCoachCommand { get; }

        public AddCoachViewModel()
        {
            SelectPhotoCommand = new RelayCommand(obj => ExecuteSelectPhoto());
            AddCoachCommand = new RelayCommand(obj => ExecuteAdd());
        }

        private void ExecuteSelectPhoto()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg;*.png)|*.jpg;*.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                PhotoData = File.ReadAllBytes(openFileDialog.FileName);
            }
        }

        public event Action RequestClose;

        private void ExecuteAdd()
        {
            IsValidationActive = true;

            // Trigger validation
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Specialization));
            OnPropertyChanged(nameof(Phone));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Information));
            OnPropertyChanged(nameof(PhotoData));

            if (!string.IsNullOrEmpty(this[nameof(Name)]) ||
                !string.IsNullOrEmpty(this[nameof(Specialization)]) ||
                !string.IsNullOrEmpty(this[nameof(Phone)]) ||
                !string.IsNullOrEmpty(this[nameof(Email)]) ||
                !string.IsNullOrEmpty(this[nameof(Information)]) ||
                !string.IsNullOrEmpty(this[nameof(PhotoData)]))
            {
                return;
            }

            DataWorker.AddCoach(Name, Specialization, Phone, Email, Information , PhotoData);
            RequestClose?.Invoke();
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (!IsValidationActive) return null;

                switch (columnName)
                {
                    case nameof(Name):
                        if (string.IsNullOrWhiteSpace(Name) || Name.Length < 5)
                            return "Минимум 5 символов";
                        break;
                    case nameof(Specialization):
                        if (string.IsNullOrWhiteSpace(Specialization))
                            return "Выберите специализацию";
                        break;
                    case nameof(Phone):
                        if (string.IsNullOrWhiteSpace(Phone) || !Regex.IsMatch(Phone, @"^\+?\d{10,15}$"))
                            return "Некорректный номер телефона";
                        break;
                    case nameof(Email):
                        if (string.IsNullOrWhiteSpace(Email) || !Regex.IsMatch(Email, @"^[\w\.-]+@[\w\.-]+\.\w{2,4}$"))
                            return "Некорректный email";
                        break;
                    case nameof(Information):
                        if (string.IsNullOrWhiteSpace(Information) || Information.Length <= 10)
                            return "Минимум 5 символов";
                        break;
                    case nameof(PhotoData):
                        if (PhotoData == null || PhotoData.Length == 0)
                            return "Выберите фото";
                        break;
                }

                return null;
            }
        }
    }
}
