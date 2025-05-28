using Microsoft.Win32;
using Sportics.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class EditCoachViewModel : BaseViewModel, IDataErrorInfo
    {
        public Coach Coach { get; set; }

        public List<string> Specializations { get; } = new List<string>
        {
            "Фитнес",
            "Йога",
            "Бассейн",
            "Тренажерный зал",
            "Танцы"
        };

        private string name;
        private string specialization;
        private string phoneNumber;
        private string email;
        private string information;
        private byte[] photoData;

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

        public string Specialization
        {
            get => specialization;
            set
            {
                specialization = value;
                OnPropertyChanged(nameof(Specialization));
                IsValidationActive = false;
            }
        }

        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
                IsValidationActive = false;
            }
        }

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

        public string Information
        {
            get => information;
            set
            {
                information = value;
                OnPropertyChanged(nameof(Information));
                IsValidationActive = false;
            }
        }

        public byte[] PhotoData
        {
            get => photoData;
            set
            {
                photoData = value;
                OnPropertyChanged(nameof(PhotoData));
                IsValidationActive = false;
            }
        }

        public ICommand SelectPhotoCommand { get; }
        public ICommand EditCommand { get; }

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
                    case nameof(Name):
                        if (string.IsNullOrWhiteSpace(Name) || Name.Length < 5)
                            return "Минимум 5 символов";
                        break;
                    case nameof(Specialization):
                        if (string.IsNullOrWhiteSpace(Specialization))
                            return "Введите специализацию";
                        break;
                    case nameof(PhoneNumber):
                        if (string.IsNullOrWhiteSpace(PhoneNumber) || !Regex.IsMatch(PhoneNumber, @"^\+?\d{10,15}$"))
                            return "Некорректный номер";
                        break;
                    case nameof(Email):
                        if (string.IsNullOrWhiteSpace(Email) || !Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                            return "Некорректный Email";
                        break;
                    case nameof(Information):
                        if (string.IsNullOrWhiteSpace(Information) || Information.Length < 10)
                            return "Введите не менее 10 символов";
                        break;
                    case nameof(PhotoData):
                        if (PhotoData == null || PhotoData.Length == 0)
                            return "Выберите фото";
                        break;
                }

                return null;
            }
        }

        public EditCoachViewModel(Coach coach)
        {
            Coach = coach;

            Name = coach.Name;
            Specialization = coach.Specialization;
            PhoneNumber = coach.PhoneNumber;
            Email = coach.Email;
            Information = coach.Information;
            PhotoData = coach.Photo;

            SelectPhotoCommand = new RelayCommand(obj => ExecuteSelectPhoto());
            EditCommand = new RelayCommand(obj => ExecuteEdit());
        }

        public EditCoachViewModel() { }

        public event Action RequestClose;

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

        private void ExecuteEdit()
        {
            IsValidationActive = true;

            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Specialization));
            OnPropertyChanged(nameof(PhoneNumber));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Information));
            OnPropertyChanged(nameof(PhotoData));

            if (!string.IsNullOrEmpty(this[nameof(Name)]) ||
                !string.IsNullOrEmpty(this[nameof(Specialization)]) ||
                !string.IsNullOrEmpty(this[nameof(PhoneNumber)]) ||
                !string.IsNullOrEmpty(this[nameof(Email)]) ||
                !string.IsNullOrEmpty(this[nameof(PhotoData)]))
                return;

            DataWorker.EditCoach(Coach, Name, Email, PhoneNumber, Specialization, Information, PhotoData);
            RequestClose?.Invoke();
        }
    }
}
