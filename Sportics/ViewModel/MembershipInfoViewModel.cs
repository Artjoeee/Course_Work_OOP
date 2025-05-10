using Sportics.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sportics.ViewModel
{
    public class MembershipInfoViewModel: BaseViewModel
    {
        public Membership Membership { get; set; }

        public MembershipInfoViewModel(Membership membership)
        {
            Membership = membership;
        }

        public MembershipInfoViewModel() { }
    }
}
