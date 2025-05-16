

using System.Collections.Generic;

namespace Sportics.Model
{
    public class User
    {
        public List<MembershipOrder> Orders { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string Role { get; set; }
        public decimal Balance { get; set; }
        public string Status { get; set; }
    }
}
