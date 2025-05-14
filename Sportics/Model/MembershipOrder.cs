using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sportics.Model
{
    public class MembershipOrder
    {
        public int Id { get; set; }

        // Явно сохраняем нужные поля
        public int MembershipId { get; set; }
        public string MembershipName { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }

        // Навигационные свойства (необязательно, но удобно)
        public Membership Membership { get; set; }

        public User Client { get; set; }
    }
}

