using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SOSTeam.TravelAgency.Domain.Models.Accommodation;
using System.Xml.Linq;
using SOSTeam.TravelAgency.Repositories.Serializer;
using System.Windows.Controls;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class AccommodationReservation : ISerializable
    {
        public int Id { get; set; }
        public DateTime FirstDay { get; set; }
        public DateTime LastDay { get; set; }
        public int ReservationDuration { get; set; }
        public int GuestNumber { get; set; }
        public int AccommodationId { get; set; }
        public int UserId { get; set; }
        public bool ReadMarkNotification { get; set; }
        public bool DefinitlyChanged { get; set; }

        public AccommodationReservation() 
        {
            Id = -1;
            FirstDay = new DateTime();
            LastDay = new DateTime();
            ReservationDuration = -1;
            AccommodationId = -1;
            UserId = -1;
            ReadMarkNotification = false;
        }

        public AccommodationReservation(int id, DateTime firstDay, DateTime lastDay, int reservationDuration, int accommodationId, int uId)
        {
            Id = id;
            FirstDay = firstDay;
            LastDay = lastDay;
            ReservationDuration = reservationDuration;
            AccommodationId = accommodationId;
            UserId = uId;
        }

        public AccommodationReservation(DateTime firstDay, DateTime lastDay, int reservationDuration, int guestNumber, int accommodationId, int uid, bool read = false, bool def = false)
        {
            FirstDay = firstDay;
            LastDay = lastDay;
            ReservationDuration = reservationDuration;
            GuestNumber = guestNumber;
            AccommodationId = accommodationId;
            UserId= uid;
            ReadMarkNotification = read;
            DefinitlyChanged = def;
        }
        
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), FirstDay.ToString(), LastDay.ToString(), ReservationDuration.ToString(), GuestNumber.ToString(), AccommodationId.ToString(), UserId.ToString(), ReadMarkNotification.ToString(), DefinitlyChanged.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            FirstDay = Convert.ToDateTime(values[i++]);
            LastDay = Convert.ToDateTime(values[i++]);
            ReservationDuration = Convert.ToInt32(values[i++]);
            GuestNumber = Convert.ToInt32(values[i++]);
            AccommodationId = Convert.ToInt32(values[i++]);
            UserId = Convert.ToInt32(values[i++]);
            if (values[i++].Equals("False"))
                ReadMarkNotification = false;
            else 
                ReadMarkNotification = true;
            if (values[i++].Equals("False"))
                DefinitlyChanged = false;
            else
                DefinitlyChanged = true;
        }
    }
}
