using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sportics.Model
{
    public class CoachReview
    {
        public int Id { get; set; }

        public int CoachId { get; set; }
        public Coach Coach { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int Rating { get; set; }
        public string Comment { get; set; }

        public DateTime Date { get; set; }

        public string AdminReply { get; set; }
    }

}
