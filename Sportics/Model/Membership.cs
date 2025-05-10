

namespace Sportics.Model
{
    public class Membership
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public byte[] Photo { get; set; }
    }
}
