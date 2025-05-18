using Microsoft.Win32;
using Sportics.Model;
using Sportics.View;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class EditViewModel : BaseViewModel, IDataErrorInfo
    {
        public Membership Membership { get; set; }

        private string fullName;
        private string shortName;
        private string category;
        private string description;
        private string priceText;
        private byte[] photoData;
        private string durationInDays;

        public string FullName
        {
            get => fullName;
            set
            {
                fullName = value;
                OnPropertyChanged(nameof(FullName));
                IsValidationActive = false;
            }
        }

        public string ShortName
        {
            get => shortName;
            set
            {
                shortName = value;
                OnPropertyChanged(nameof(ShortName));
                IsValidationActive = false;
            }
        }

        public string Category
        {
            get => category;
            set
            {
                category = value;
                OnPropertyChanged(nameof(Category));
                IsValidationActive = false;
            }
        }

        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
                IsValidationActive = false;
            }
        }

        public string Price
        {
            get => priceText;
            set
            {
                priceText = value;
                OnPropertyChanged(nameof(Price));
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

        public string DurationInDays
        {
            get => durationInDays;
            set
            {
                durationInDays = value;
                OnPropertyChanged(nameof(DurationInDays));
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
                    case nameof(FullName):
                        if (string.IsNullOrWhiteSpace(FullName) || FullName.Length < 10)
                            return "Минимум 10 символов";
                        break;
                    case nameof(ShortName):
                        if (string.IsNullOrWhiteSpace(ShortName) || ShortName.Length < 5)
                            return "Минимум 5 символов";
                        break;
                    case nameof(Description):
                        if (string.IsNullOrWhiteSpace(Description) || Description.Length < 10)
                            return "Минимум 10 символов";
                        break;
                    case nameof(Category):
                        if (string.IsNullOrWhiteSpace(Category))
                            return "Выберите категорию";
                        break;
                    case nameof(Price):
                        if (string.IsNullOrWhiteSpace(Price))
                            return "Введите цену";

                        if (!Regex.IsMatch(Price, @"^\d+([.,]\d{1,2})?$"))
                            return "Введите корректное число (до 2 знаков после запятой)";

                        if (!decimal.TryParse(Price, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal priceVal))
                            return "Неверный формат цены";

                        if (priceVal <= 0)
                            return "Цена должна быть больше 0";

                        if (priceVal > 1500)
                            return "Максимум 1500";

                        break;

                    case nameof(PhotoData):
                        if (PhotoData == null || PhotoData.Length == 0)
                            return "Выберите фото";
                        break;
                    case nameof(DurationInDays):
                        if (int.Parse(DurationInDays) <= 0)
                            return "Длительность должна быть больше нуля";
                        if (int.Parse(DurationInDays) >= 366)
                            return "Слишком большая длительность";
                        break;
                }

                return null;
            }
        }

        public ICommand SelectPhotoCommand { get; }
        public ICommand EditCommand { get; }

        public EditViewModel(Membership membership)
        {
            Membership = membership;

            FullName = membership.FullName;
            ShortName = membership.ShortName;
            Category = membership.Category;
            Description = membership.Description;
            Price = membership.Price.ToString();
            PhotoData = membership.Photo;
            DurationInDays = membership.DurationInDays.ToString();

            SelectPhotoCommand = new RelayCommand(obj => ExecuteSelectPhoto());
            EditCommand = new RelayCommand(obj => ExecuteEdit());
        }

        public EditViewModel() { }

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

            OnPropertyChanged(nameof(FullName));
            OnPropertyChanged(nameof(ShortName));
            OnPropertyChanged(nameof(Category));
            OnPropertyChanged(nameof(Description));
            OnPropertyChanged(nameof(Price));
            OnPropertyChanged(nameof(PhotoData));
            OnPropertyChanged(nameof(DurationInDays));

            if (!string.IsNullOrEmpty(this[nameof(FullName)]) ||
                !string.IsNullOrEmpty(this[nameof(ShortName)]) ||
                !string.IsNullOrEmpty(this[nameof(Category)]) ||
                !string.IsNullOrEmpty(this[nameof(Description)]) ||
                !string.IsNullOrEmpty(this[nameof(Price)]) ||
                !string.IsNullOrEmpty(this[nameof(DurationInDays)]))
                return;

            if (!string.IsNullOrEmpty(this[nameof(PhotoData)]))
            {
                string message = "Выберите фото";

                var messageWindow = new MessageWindow();
                var viewModel = new MessageViewModel(message);
                messageWindow.DataContext = viewModel;
                viewModel.RequestClose += () => messageWindow.Close();
                messageWindow.Owner = System.Windows.Application.Current.MainWindow;
                messageWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                messageWindow.ShowDialog();

                return;
            }

            DataWorker.EditMembership(Membership, FullName, ShortName, Description, Category, decimal.Parse(Price), PhotoData, int.Parse(DurationInDays));
            RequestClose?.Invoke();
        }
    }
}
