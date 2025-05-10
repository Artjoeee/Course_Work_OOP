using Sportics.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sportics.ViewModel
{
    public class EditViewModel : BaseViewModel
    {
        public Membership Membership { get; set; }

        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public byte[] Photo { get; set; }

        public EditViewModel(Membership membership)
        {
            Membership = membership;
        }

        public EditViewModel() 
        {

        }

        private void EditMembership()
        {
            DataWorker.EditMembership(Membership, FullName, ShortName, Description, Category, Price, Photo);

        }


    }
}
