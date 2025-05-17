using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sportics.Model
{
    public static class Session
    {
        public static User CurrentUser { get; set; }

        public static event Action BalanceUpdated;

        public static void RaiseBalanceUpdated()
        {
            BalanceUpdated?.Invoke();
        }

        public static void Logout()
        {
            CurrentUser = null;
        }
    }
}
