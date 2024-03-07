using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SOSTeam.TravelAgency.Domain.DTO.LocAccommodationDTO;

namespace SOSTeam.TravelAgency.Domain.DTO
{
    public class WhateverSearchResultsDTO
    {
        public int AccommodationId { get; set; }
        public string AccommodationName { get; set; }
        public int AccommodationMinDaysStay { get; set; }
        public int AccommodationMaxGuests { get; set; }
        public int GuestId { get; set; }
        public List<AccReservationDTO> AppointmentCatalog { get; set; }
        public Image Cover { get; set; }
        public string AccommodationType { get; set; }
        public string FullLocation { get; set; }
        public string MaxGuests { get; set; }
        public string MinDaysString { get; set; }
        public string FoundAppointmentsNumber { get; set; }

        public WhateverSearchResultsDTO(int accommodationId, int locationId, string accommodationName, int accommodationMinDaysStay, int accommodationMaxGuests, int guestId, Accommodation.AccommodationType accommodationType)
        {
            AccommodationId = accommodationId;
            AccommodationName = accommodationName;
            AccommodationMinDaysStay = accommodationMinDaysStay;
            AccommodationMaxGuests = accommodationMaxGuests;
            GuestId = guestId;
            AppointmentCatalog = new List<AccReservationDTO>();
            ImageService imageService = new ImageService();
            Cover = imageService.GetAccommodationCover(accommodationId);
            if (Cover == null)
            {
                Cover = new Image();
                Cover.Path = "/Resources/Images/UnknownPhoto.png";
            }
            if (accommodationType == Accommodation.AccommodationType.APARTMENT) AccommodationType = "APARTMAN";
            else if (accommodationType == Accommodation.AccommodationType.HOUSE) AccommodationType = "KUĆA";
            else AccommodationType = "KOLIBA";
            LocationService locationService = new LocationService();
            Location location = locationService.GetById(locationId);
            FullLocation = location.City + ", " + location.Country;
            MaxGuests = "  Maksimalno gostiju: " + accommodationMaxGuests.ToString();
            MinDaysString = "   Minimalno dana: " + accommodationMinDaysStay.ToString();
        }
    }
}
