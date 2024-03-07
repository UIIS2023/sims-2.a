using SOSTeam.TravelAgency.Repositories.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class ChangedReservationRequest : ISerializable
    {
        public enum Status { ACCEPTED, REFUSED, ON_HOLD, NOT_REQUIRED }
        public int Id { get; set; }
        public int reservationId { get; set; }
        public int AccommodationId { get; set; }
        public string AccommodationName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime OldFirstDay { get; set; }
        public string OldReservationString { get; set; }
        public string NewReservationString { get; set; }
        public DateTime OldLastDay { get; set; }
        public DateTime? NewFirstDay { get; set; }
        public DateTime? NewLastDay { get; set; }
        public int GuestNumber { get; set; }
        public int UserId { get; set; }
        public Status status { get; set; }
        public string StatusString { get; set; }
        public string ownerComment { get; set; }
        public bool IsSuperOwned { get; set; }

        public ChangedReservationRequest()
        {

        }

        public ChangedReservationRequest(int resId, int accId, string accName, string city, string country, DateTime fDay, DateTime lDay, int guests, int userId)
        {
            reservationId = resId;
            AccommodationId = accId;
            AccommodationName = accName;
            City = city;
            Country = country;
            OldFirstDay = fDay;
            OldLastDay = lDay;
            OldReservationString = OldFirstDay.ToShortDateString() + " - " + OldLastDay.ToShortDateString(); 
            NewFirstDay = null;
            NewLastDay = null;
            NewReservationString = "";
            GuestNumber = guests;
            UserId = userId;
            status = Status.NOT_REQUIRED;
            StatusString = "BEZ ZAHTJEVA";
            ownerComment = "Nema komentara";
            IsSuperOwned = false;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), reservationId.ToString(), AccommodationId.ToString(), AccommodationName, City, Country, 
                                    OldFirstDay.ToString(), OldLastDay.ToString(), NewFirstDay.ToString(), NewLastDay.ToString(),GuestNumber.ToString(), UserId.ToString(), status.ToString(), ownerComment };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            reservationId = Convert.ToInt32(values[i++]);
            AccommodationId = Convert.ToInt32(values[i++]);
            AccommodationName = values[i++];
            City = values[i++];
            Country = values[i++];

            OldFirstDay = Convert.ToDateTime(values[i++]);
            OldLastDay = Convert.ToDateTime(values[i++]);
            NewFirstDay = Convert.ToDateTime(values[i++]);
            NewLastDay = Convert.ToDateTime(values[i++]);

            OldReservationString = OldFirstDay.ToShortDateString() + " - " + OldLastDay.ToShortDateString();
            NewReservationString = NewFirstDay?.ToShortDateString() + " - " + NewLastDay?.ToShortDateString();

            GuestNumber = Convert.ToInt32(values[i++]);
            UserId = Convert.ToInt32(values[i++]);
            status = FindStatus(values[i++]);
            StatusString = FindStatusString(status);
            ownerComment = values[i++];
        }

        private Status FindStatus(string str)
        {
            if (str.Equals("ACCEPTED"))
                return Status.ACCEPTED;
            else if (str.Equals("REFUSED"))
                return Status.REFUSED;
            else if (str.Equals("ON_HOLD"))
                return Status.ON_HOLD;
            else
                return Status.NOT_REQUIRED;
        }
        private string FindStatusString(Status status)
        {
            if (status == Status.ACCEPTED)
                return "PRIHVAĆENO";
            else if (status == Status.REFUSED)
                return "ODBIJENO";
            else if (status == Status.ON_HOLD)
                return "NA ČEKANjU";
            else
                return "BEZ ZAHTJEVA";
        }
    }
}
