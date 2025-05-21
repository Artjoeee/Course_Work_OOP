using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sportics.Model
{
    public class ClientSessionRecord
    {
        public int Id { get; set; }

        public int ClientId { get; set; }
        public User Client { get; set; }

        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }

        public string Category { get; set; }
        public TimeSpan Time { get; set; }
        public DateTime Date { get; set; }

        public int MembershipOrderId { get; set; }

        public MembershipOrder MembershipOrder { get; set; }

        public int MembershipId { get; set; }
        public Membership Membership { get; set; }
    }

}
