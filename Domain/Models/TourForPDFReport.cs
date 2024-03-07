using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class TourForPDFReport
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Language { get; set; }
        public int MaxNumOfGuests { get; set; }
        public string Duration { get; set; }
        public DateTime Start { get; set; }

        public TourForPDFReport() { }

        public TourForPDFReport(string name, string location, string language, int maxNumOfGuests, string duration, DateTime start)
        {
            Name = name;
            Location = location;
            Language = language;
            MaxNumOfGuests = maxNumOfGuests;
            Duration = duration;
            Start = start;
        }
    }
}
