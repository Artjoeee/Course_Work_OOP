using Sportics.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class EditScheduleViewModel : BaseViewModel, IDataErrorInfo
    {
        public Schedule Schedule { get; set; }

        public List<string> Categories { get; } = new List<string>
        {
            "Фитнес", "Йога", "Бассейн", "Тренажерный зал", "Танцы"
        };

        public List<string> CoachNames { get; }

        private string category;
        private DateTime? date;
        private string timeString;
        private string selectedCoach;
        private bool isValidationActive;

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

        public DateTime? Date
        {
            get => date;
            set
            {
                date = value;
                OnPropertyChanged(nameof(Date));
                IsValidationActive = false;
            }
        }

        public string TimeString
        {
            get => timeString;
            set
            {
                timeString = value;
                OnPropertyChanged(nameof(TimeString));
                IsValidationActive = false;
            }
        }

        public string SelectedCoach
        {
            get => selectedCoach;
            set
            {
                selectedCoach = value;
                OnPropertyChanged(nameof(SelectedCoach));
                IsValidationActive = false;
            }
        }

        public ICommand SaveCommand { get; }

        public event Action RequestClose;

        public bool IsValidationActive
        {
            get => isValidationActive;
            set
            {
                isValidationActive = value;
                OnPropertyChanged(nameof(IsValidationActive));
            }
        }

        public EditScheduleViewModel(Schedule schedule)
        {
            Schedule = schedule;

            CoachNames = DataWorker.GetCoachNames(); // Метод должен вернуть список имён тренеров

            Category = schedule.Category;
            Date = schedule.Date;
            TimeString = schedule.Time.ToString(@"hh\:mm");
            SelectedCoach = schedule.Coach?.Name;

            SaveCommand = new RelayCommand(_ => ExecuteSave());
        }

        public EditScheduleViewModel() { }

        private void ExecuteSave()
        {
            IsValidationActive = true;

            OnPropertyChanged(nameof(Category));
            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(TimeString));
            OnPropertyChanged(nameof(SelectedCoach));

            if (!string.IsNullOrEmpty(this[nameof(Category)]) ||
                !string.IsNullOrEmpty(this[nameof(Date)]) ||
                !string.IsNullOrEmpty(this[nameof(TimeString)]) ||
                !string.IsNullOrEmpty(this[nameof(SelectedCoach)]))
                return;

            Coach selected = DataWorker.GetCoachByName(SelectedCoach);
            if (selected == null)
            {
                MessageBox.Show("Тренер не найден.");
                return;
            }

            Schedule.Category = Category;
            Schedule.Date = Date.Value;
            Schedule.Time = TimeSpan.Parse(TimeString);
            Schedule.Coach = selected;

            DataWorker.EditSchedule(Schedule, Category, selected.Id, Schedule.Date, Schedule.Time);
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
                    case nameof(TimeString):
                        if (string.IsNullOrWhiteSpace(TimeString) || !Regex.IsMatch(TimeString, @"^\d{1,2}:\d{2}$"))
                            return "Введите корректное время (чч:мм)";
                        break;

                    case nameof(SelectedCoach):
                        if (string.IsNullOrWhiteSpace(SelectedCoach))
                            return "Выберите тренера";
                        break;
                }

                return null;
            }
        }
    }
}
