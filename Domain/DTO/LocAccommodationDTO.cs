using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Repositories;
using SOSTeam.TravelAgency.Repositories.Serializer;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.Domain.DTO
{
    public class LocAccommodationDTO
    {
        public enum AccommType
        {
            APARTMENT, HOUSE, HUT, NOTYPE
        }

        public int AccommodationId { get; set; }
        public string AccommodationName { get; set; }
        public string LocationCity { get; set; }
        public string LocationCountry { get; set; }
        public AccommType AccommodationType { get; set; }
        public int AccommodationMaxGuests { get; set; }
        public int AccommodationMinDaysStay { get; set; }
        public int GuestNumber { get; set; }
        public bool IsSuperOwned { get; set; }
        public Image Cover { get; set; }
        public string FullLocation { get; set; }
        public string CurrentGuests { get; set; }
        public string MinDaysString { get; set; }
        public string TypeString { get; set; }
        public bool IsRenovatedInLastYear { get; set; }

        public LocAccommodationDTO(int id, string name, string city, string country, AccommType type, int guests, int days, int guestNumber, bool isSuperOwned)

        {
            AccommodationId = id;
            AccommodationName = name;
            LocationCity = city;
            LocationCountry = country;
            FullLocation = city + ", " + country;
            AccommodationType = type;
            if (type == AccommType.APARTMENT) TypeString = "APARTMAN";
            else if (type == AccommType.HOUSE) TypeString = "KUĆA";
            else TypeString = "KOLIBA";
            AccommodationMaxGuests = guests;
            AccommodationMinDaysStay = days;
            GuestNumber = guestNumber;
            CurrentGuests = "  Trenutno gostiju: " + guestNumber.ToString();
            IsSuperOwned = isSuperOwned;
            MinDaysString = "   Minimalno dana: " + days.ToString();
            ImageService imageService = new ImageService();
            Cover = imageService.GetAccommodationCover(id);
            if (Cover == null)
            {
                Cover = new Image();
                Cover.Path = "/Resources/Images/UnknownPhoto.png";
            }
            IsRenovatedInLastYear = false;
        }

        public LocAccommodationDTO(string name, string city, string country, AccommType type, int guestNumber, int days, bool isSuperOwned)
        {
            AccommodationName = name;
            LocationCity = city;
            LocationCountry = country;
            AccommodationType = type;
            AccommodationMinDaysStay = days;
            GuestNumber = guestNumber;
            IsSuperOwned = isSuperOwned;
            IsRenovatedInLastYear = false;
        }
    }
}
