using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using static SOSTeam.TravelAgency.Domain.DTO.LocAccommodationDTO;

namespace SOSTeam.TravelAgency.Domain.DTO
{
    public class CancelAndMarkResDTO
    {
        public string AccommodationName { get; set; }
        public string AccommodationCity { get; set; }
        public string AccommodationCountry { get; set; }
        public DateTime FirstDay { get; set; }
        public string FirstDayStr { get; set; }
        public DateTime LastDay { get; set; }
        public string LastDayStr { get; set; }
        public int ReservationId { get; set; }
        public int AccommodationId { get; set; }
        public string DaysForMarking { get; set; }
        public AccommType AccommodationType { get; set; }
        public int DaysDuration { get; set; }
        public string NotificationShape { get; set; }
        public bool IsSuperOwned { get; set; }
        public string TypeString { get; set; }

        public CancelAndMarkResDTO() { }

        public CancelAndMarkResDTO(string accommodationName, string accommodationCity, string accommodationCountry, DateTime firstDay, DateTime lastDay, int reservationId, int accommodationId, string daysForMarking = "", int daysDuration = -1, AccommType type = AccommType.NOTYPE)
        {
            AccommodationName = accommodationName;
            AccommodationCity = accommodationCity;
            AccommodationCountry = accommodationCountry;
            FirstDay = firstDay;
            FirstDayStr = firstDay.ToShortDateString();
            LastDay = lastDay;
            LastDayStr = lastDay.ToShortDateString();
            ReservationId = reservationId;
            AccommodationId = accommodationId;
            DaysForMarking = daysForMarking;
            DaysDuration = daysDuration;
            AccommodationType = type;
            if (type == AccommType.APARTMENT) TypeString = "APARTMAN";
            else if (type == AccommType.HOUSE) TypeString = "KUĆA";
            else TypeString = "KOLIBA";
            NotificationShape = "Vaša rezervacija u smještaju " + AccommodationName + " (" + AccommodationCity + ", " +
                                AccommodationCountry + ") za period " + FirstDay.ToShortDateString() + " - " + LastDay.ToShortDateString() +
                                " je završena. Za eventualno ocjenjivanje ovog smještaja Vam je ostalo još ";
            IsSuperOwned = false;
        }
    }
}
