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

        public int MembershipId { get; set; }
        public string MembershipName { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string Category { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime EndDate { get; set; }

        public Membership Membership { get; set; }
        public User Client { get; set; }
        public List<ClientSessionRecord> ClientSession { get; set; }
    }
}

