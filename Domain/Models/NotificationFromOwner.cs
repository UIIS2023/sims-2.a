using SOSTeam.TravelAgency.Repositories.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SOSTeam.TravelAgency.Domain.Models.ChangedReservationRequest;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class NotificationFromOwner : ISerializable
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int ReservationId { get; set; }
        public string AccommodationName { get; set; }
        public string Answer { get; set; }
        public int GuestId { get; set; }

        public NotificationFromOwner(int reservationId, Accommodation accommodation, int ownerid, int guestId)
        {
            OwnerId = ownerid;
            ReservationId = reservationId;
            AccommodationName = accommodation.Name;
            Answer = "None";
            GuestId = guestId;
        }

        public NotificationFromOwner() { }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), OwnerId.ToString(), ReservationId.ToString(), AccommodationName, Answer, GuestId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            OwnerId = Convert.ToInt32(values[i++]);
            ReservationId = Convert.ToInt32(values[i++]);
            AccommodationName = values[i++];
            Answer = values[i++];
            GuestId = Convert.ToInt32(values[i++]);
        }
    }
}
