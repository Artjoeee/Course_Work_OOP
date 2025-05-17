using Sportics.Model;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class BalanceViewModel : BaseViewModel, IDataErrorInfo
    {
        public User CurrentUser => Session.CurrentUser;

        private string money;
        public string Money
        {
            get => money;
            set
            {
                money = value;
                OnPropertyChanged(nameof(Money));
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

                if (columnName == nameof(Money))
                {
                    if (string.IsNullOrWhiteSpace(Money))
                        return "Введите сумму";

                    if (!Regex.IsMatch(Money, @"^\d+(\,\d{1,2})?$"))
                        return "Допустимы только цифры";

                    if (!decimal.TryParse(Money, out decimal result))
                        return "Недопустимое значение";

                    if (result <= 0)
                        return "Сумма должна быть больше 0";

                    if (result > 5000)
                        return "Максимальная сумма: 5000";
                }

                return null;
            }
        }

        public ICommand AddBalanceCommand { get; }

        public BalanceViewModel()
        {
            AddBalanceCommand = new RelayCommand(obj => AddBalance());
        }

        public event Action RequestClose;

        private void AddBalance()
        {
            IsValidationActive = true;
            OnPropertyChanged(nameof(Money));

            if (!string.IsNullOrEmpty(this[nameof(Money)]))
                return;

            decimal value = decimal.Parse(Money);
            DataWorker.AddBalance(CurrentUser.Id, value);
            CurrentUser.Balance += value;

            Session.RaiseBalanceUpdated();
            RequestClose?.Invoke();
        }
    }
}
