using Sportics.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sportics.ViewModel
{
    public class MessageViewModel: BaseViewModel
    {
        public string Message { get; set; }

        public ICommand CloseCommand { get; }

        public MessageViewModel(string message) 
        {
            Message = message;
            CloseCommand = new RelayCommand(obj => Close()); 
        }

        public MessageViewModel() { }

        public event Action RequestClose;

        private void Close()
        {
            RequestClose?.Invoke();
        }
    }
}
