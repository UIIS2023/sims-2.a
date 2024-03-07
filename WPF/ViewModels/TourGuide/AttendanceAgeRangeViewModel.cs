using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class AttendanceAgeRangeViewModel
    {
        public string AgeGroup { get; set; }
        public int NumOfGuests { get; set; }

        public AttendanceAgeRangeViewModel()
        {
            NumOfGuests = 0;
        }
    }
}
