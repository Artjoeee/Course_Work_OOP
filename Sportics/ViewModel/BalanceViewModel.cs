using Sportics.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class BalanceViewModel: BaseViewModel
    {
        public User CurrentUser = Session.CurrentUser;

        public decimal Money { get; set; }

        public ICommand AddBalanceCommand { get; }

        public BalanceViewModel()
        {
            AddBalanceCommand = new RelayCommand(obj => AddBalance());
        }

        private void AddBalance()
        {
            DataWorker.AddBalance(CurrentUser.Id, Money);
        }
    }
}
