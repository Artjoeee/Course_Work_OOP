

using System.Collections.Generic;

namespace Sportics.Model
{
    public class Membership
    {
        public List<MembershipOrder> Orders { get; set; }

        public int Id { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public byte[] Photo { get; set; }
        public int DurationInDays { get; set; }
        public bool IsWeeklyOffer { get; set; }

    }
}
