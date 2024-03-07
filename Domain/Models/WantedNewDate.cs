using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Repositories.Serializer;
using SOSTeam.TravelAgency.WPF.ViewModels;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest1;
using static SOSTeam.TravelAgency.Domain.Models.ChangedReservationRequest;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class WantedNewDate : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public string AccommodationName { get; set; }
        public int AccommodationMinDaysStay { get; set; }
        public DateTime ReservationFirstDay { get; set; }
        public DateTime ReservationLastDay { get; set; }
        public int ReservationDuration { get; set; }
        public int AccommodationMaxGuests { get; set; }
        public int CurrentGuestNumber { get; set; }
        public int UserId { get; set; }
        public int OldReservationId { get; set; }

        public WantedNewDate(int id, string name, int days, DateTime fDay, DateTime lDay, int duration, int maxGuests, int currGuests, int userId, int oldReservationId)
        {
            AccommodationId = id;
            AccommodationName = name;
            AccommodationMinDaysStay = days;
            ReservationFirstDay = fDay;
            ReservationLastDay = lDay;
            ReservationDuration = duration;
            AccommodationMaxGuests = maxGuests;
            CurrentGuestNumber = currGuests;
            UserId = userId;
            OldReservationId = oldReservationId;
        }

        public WantedNewDate() { }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), AccommodationId.ToString(), AccommodationName, AccommodationMinDaysStay.ToString(), 
                                    ReservationFirstDay.ToString(), ReservationLastDay.ToString(), 
                                    ReservationDuration.ToString(), AccommodationMaxGuests.ToString(), CurrentGuestNumber.ToString(), 
                                    UserId.ToString(), OldReservationId.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            AccommodationId = Convert.ToInt32(values[i++]);
            AccommodationName = values[i++];
            AccommodationMinDaysStay = Convert.ToInt32(values[i++]);
            ReservationFirstDay = Convert.ToDateTime(values[i++]);
            ReservationLastDay = Convert.ToDateTime(values[i++]);
            ReservationDuration = Convert.ToInt32(values[i++]);
            AccommodationMaxGuests = Convert.ToInt32(values[i++]);
            CurrentGuestNumber = Convert.ToInt32(values[i++]);
            UserId = Convert.ToInt32(values[i++]);
            OldReservationId = Convert.ToInt32(values[i++]);
        }
    }
}
