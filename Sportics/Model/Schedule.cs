using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sportics.Model
{
    public class Schedule
    {
        public int Id { get; set; }
        public string Category { get; set; }

        public int CoachId { get; set; }
        public Coach Coach { get; set; }

        public TimeSpan Time { get; set; }
        public DateTime Date { get; set; }

        public string Status { get; set; }

        public List<ClientSessionRecord> ClientSessionRecords { get; set; }
    }

}
