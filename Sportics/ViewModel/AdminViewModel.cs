using Sportics.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class AdminViewModel: BaseViewModel
    {
        public List<User> Users { get; set; }

        public ICommand AllUsersCommand { get; }

        public AdminViewModel() 
        {
            AllUsersCommand = new RelayCommand(obj => AllUsers());
            AllUsers();
        }

        private void AllUsers()
        {
            Users = DataWorker.GetAllUsers();
        }
    }
}
