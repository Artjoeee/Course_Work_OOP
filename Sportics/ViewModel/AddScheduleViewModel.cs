using Sportics.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class AddScheduleViewModel : BaseViewModel, IDataErrorInfo
    {
        public List<string> Categories { get; } = new List<string>
        {
            "Фитнес", "Йога", "Танцы", "Бассейн", "Тренажерный зал"
        };

        public List<string> Coaches { get; } = DataWorker.GetCoachNames();

        public string Category
        {
            get => category;
            set { category = value; OnPropertyChanged(); IsValidationActive = false; }
        }

        public DateTime? Date
        {
            get => date;
            set { date = value; OnPropertyChanged(); IsValidationActive = false; }
        }

        public string Time
        {
            get => time;
            set { time = value; OnPropertyChanged(); IsValidationActive = false; }
        }

        public string Coach
        {
            get => coach;
            set { coach = value; OnPropertyChanged(); IsValidationActive = false; }
        }

        private string category;
        private DateTime? date;
        private string time;
        private string coach;

        private bool isValidationActive = false;
        public bool IsValidationActive
        {
            get => isValidationActive;
            set { isValidationActive = value; OnPropertyChanged(); }
        }

        public ICommand AddScheduleCommand { get; }

        public event Action RequestClose;

        public AddScheduleViewModel()
        {
            AddScheduleCommand = new RelayCommand(obj => ExecuteAdd());
        }

        private void ExecuteAdd()
        {
            IsValidationActive = true;

            // Триггер валидации
            OnPropertyChanged(nameof(Category));
            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(Time));
            OnPropertyChanged(nameof(Coach));

            if (!string.IsNullOrEmpty(this[nameof(Category)]) ||
                !string.IsNullOrEmpty(this[nameof(Date)]) ||
                !string.IsNullOrEmpty(this[nameof(Time)]) ||
                !string.IsNullOrEmpty(this[nameof(Coach)]))
            {
                return;
            }

            var parsedTime = TimeSpan.Parse(Time);
            DataWorker.AddSchedule(Category, Date.Value, parsedTime, Coach);
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
                    case nameof(Category):
                        if (string.IsNullOrWhiteSpace(Category))
                            return "Выберите категорию";
                        break;
                    case nameof(Date):
                        if (Date == null)
                            return "Выберите дату";

                        if (Date.Value.Date < DateTime.Today)
                            return "Дата не может быть в прошлом";

                        string datePattern = @"^(0[1-9]|[12][0-9]|3[01])[\./-](0[1-9]|1[0-2])[\./-](20\d{2})$";
                        string dateAsString = Date.Value.ToString("dd.MM.yyyy");
                        if (!Regex.IsMatch(dateAsString, datePattern))
                            return "Формат даты: ДД.ММ.ГГГГ";
                        break;
                    case nameof(Time):
                        if (string.IsNullOrWhiteSpace(Time) || !Regex.IsMatch(Time, @"^\d{2}:\d{2}$"))
                            return "Формат времени: чч:мм";
                        var parsedTime = TimeSpan.Parse(Time);
                        if (Date.Value.Date < DateTime.Today && parsedTime < DateTime.Now.TimeOfDay)
                            return "Не может быть прошедшее время";
                        break;
                    case nameof(Coach):
                        if (string.IsNullOrWhiteSpace(Coach))
                            return "Выберите тренера";
                        break;
                }

                return null;
            }
        }
    }
}
