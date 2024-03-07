using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public enum NotificationType { UNKNOWN = 0, REQUESTED = 1, STATS_MADE = 2 }
    public class NewTourNotification : ISerializable
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int RequestId { get; set; }
        public int GuideId { get; set; }
        public bool IsRead { get; set; }
        public int GuestId { get; set; }
        public NotificationType Type { get; set; }

        public NewTourNotification()
        {
            Id = -1;
            AppointmentId = -1;
            RequestId = -1;
            GuideId = -1;
            IsRead = false;
            GuestId = -1;
            Type = NotificationType.UNKNOWN;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            AppointmentId = int.Parse(values[1]);
            GuideId = int.Parse(values[2]);
            RequestId = int.Parse(values[3]);
            IsRead = bool.Parse(values[4]);
            GuestId = int.Parse(values[5]);
            Type = values[6] switch
            {
                "REQUESTED" => NotificationType.REQUESTED,
                "STATS_MADE" => NotificationType.STATS_MADE,
                _ => NotificationType.UNKNOWN,
            };
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AppointmentId.ToString(),
                GuideId.ToString(),
                RequestId.ToString(),
                IsRead.ToString(),
                GuestId.ToString(),
                Type.ToString()
            };

            return csvValues;
        }
    }
}
