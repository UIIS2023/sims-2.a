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
    public class GuestAccommodationMark : ISerializable
    {
        public int Id { get; set; }
        public int CleanMark { get; set; }
        public int OwnerMark { get; set; }
        public string GuestComment { get; set; }
        public string UrlGuestImage { get; set; }
        public int GuestId { get; set; }
        public int AccommodationId { get; set; }
        public string RenovationMark { get; set; }
        public string GuestSuggest { get; set; }

        public GuestAccommodationMark() { }

        public GuestAccommodationMark(int cleanMark, int ownerMark, string guestComment, string urlGuestImage, int guestId, int accommodationId, string renovationMark, string suggest)
        {
            CleanMark = cleanMark;
            OwnerMark = ownerMark;
            GuestComment = guestComment;
            UrlGuestImage = urlGuestImage;
            GuestId = guestId;
            AccommodationId = accommodationId;
            RenovationMark = renovationMark;
            GuestSuggest = suggest;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), CleanMark.ToString(), OwnerMark.ToString(), 
                                   GuestComment, UrlGuestImage, GuestId.ToString(), AccommodationId.ToString(), 
                                   RenovationMark, GuestSuggest };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            CleanMark = Convert.ToInt32(values[i++]);
            OwnerMark = Convert.ToInt32(values[i++]);
            GuestComment = values[i++];
            UrlGuestImage = values[i++];
            GuestId = Convert.ToInt32(values[i++]);
            AccommodationId = Convert.ToInt32(values[i++]);
            RenovationMark = values[i++];
            GuestSuggest = values[i++];
        }
    }
}
