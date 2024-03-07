using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class GuestAttendanceCardViewModel
    {
        public string UserName { get; set; }
        public string CheckpointName { get; set; }
        public string StatusImage { get; set; }
        public string Background { get; set; }

        public GuestAttendanceCardViewModel()
        {
            UserName = string.Empty;
            CheckpointName = string.Empty;
            StatusImage = string.Empty;
            Background = string.Empty;
        }

        public void SetStatusImageAndBackground(GuestAttendance guestAttendance)
        {
            if (guestAttendance.Presence == GuestPresence.YES)
            {
                StatusImage = "/Resources/Icons/checkpoint_yes.png";
                Background = "#C7FFC2";
            }
            else if (guestAttendance.Presence == GuestPresence.NO)
            {
                StatusImage = "/Resources/Icons/checkpoint_no.png";
                Background = "#FFC7C8";
            }
            else
            {
                StatusImage = "/Resources/Icons/checkpoint_unknown.png";
                Background = "#F8FFB7";
            }
        }

    }
}
