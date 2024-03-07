using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Xml.Linq;
using SOSTeam.TravelAgency.Domain;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.WPF.Views;
using SOSTeam.TravelAgency.Repositories;
using System.Collections.ObjectModel;
using SOSTeam.TravelAgency.Domain.DTO;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class AccommodationService
    {
        private readonly IAccommodationRepository _accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();
        private readonly ILocationRepository _locationRepository = Injector.CreateInstance<ILocationRepository>();
        private readonly IAccReservationRepository _accReservationRepository = Injector.CreateInstance<IAccReservationRepository>();

        public AccommodationService() { }

        public void Delete(int id)
        {
            _accommodationRepository.Delete(id);
        }

        public List<Accommodation> GetAll()
        {
            return _accommodationRepository.GetAll();
        }

        public List<Accommodation> GetAllByUserId(int ownerId)
        { 
            return _accommodationRepository.GetAll().Where(p => p.OwnerId == ownerId).ToList();
        }

        public Accommodation GetById(int id)
        {
            return _accommodationRepository.GetById(id);
        }

        public void Save(Accommodation accommodation)
        {
            _accommodationRepository.Save(accommodation);
        }

        public Accommodation SaveAndReturn(Accommodation accommodation)
        {
            return _accommodationRepository.SaveAndReturn(accommodation);
        }

        public void Update(Accommodation accommodation)
        {
            _accommodationRepository.Update(accommodation);
        }

        

        public List<LocAccommodationDTO> ExecuteAccommodationSearch(string name, string city, string country, LocAccommodationDTO.AccommType type, int guestNumber, int daysNumber)
        {
            ObservableCollection<LocAccommodationDTO> AccommDTOsCollection = CreateAllDTOForms();
            LocAccommodationDTO dtoRequest = CreateDTORequest(name, city, country, type, guestNumber, daysNumber);
            return Search(dtoRequest, AccommDTOsCollection);
        }

        public ObservableCollection<LocAccommodationDTO> CreateAllDTOForms()
        {
            List<Accommodation> accommodations = _accommodationRepository.GetAll();
            List<Location> locations = _locationRepository.GetAll();
            ObservableCollection<LocAccommodationDTO>  AccommDTOsCollection = new ObservableCollection<LocAccommodationDTO>();
            List<LocAccommodationDTO> locAccommodationViewModels = new List<LocAccommodationDTO>();

            foreach (var accommodation in accommodations)
            {
                foreach (var location in locations)
                {
                    if (accommodation.LocationId == location.Id)
                    {
                        LocAccommodationDTO dto = CreateDTOForm(accommodation, location);
                        locAccommodationViewModels.Add(dto);
                    }
                }
            }

            locAccommodationViewModels = locAccommodationViewModels.OrderByDescending(a => a.IsSuperOwned).ToList();
            locAccommodationViewModels.ForEach(item => AccommDTOsCollection.Add(item));

            return AccommDTOsCollection;
        }

        private LocAccommodationDTO CreateDTOForm(Accommodation acc, Location loc)
        {
            SuperOwnerService superOwnerService = new SuperOwnerService();
            List<AccommodationReservation> accommodationReservations = _accReservationRepository.GetAll();

            int currentGuestNumber = 0;
            foreach (var item in accommodationReservations)
            {
                if (item.AccommodationId == acc.Id)
                {
                    DateTime today = DateTime.Today;
                    if (today >= item.FirstDay && today <= item.LastDay)
                    {
                        currentGuestNumber += item.GuestNumber;
                    }
                }
            }
            LocAccommodationDTO dto = new LocAccommodationDTO(acc.Id, acc.Name, loc.City, loc.Country, FindAccommodationType(acc), acc.MaxGuests, acc.MinDaysStay, currentGuestNumber, superOwnerService.IsSuperOwnerAccommodation(acc.Id));
            return dto;
        }

        private LocAccommodationDTO.AccommType FindAccommodationType(Accommodation acc)
        {
            if (acc.Type == Accommodation.AccommodationType.APARTMENT)
                return LocAccommodationDTO.AccommType.APARTMENT;
            else if (acc.Type == Accommodation.AccommodationType.HOUSE)
                return LocAccommodationDTO.AccommType.HOUSE;
            else if (acc.Type == Accommodation.AccommodationType.HUT)
                return LocAccommodationDTO.AccommType.HUT;
            else
                return LocAccommodationDTO.AccommType.NOTYPE;
        }

        private LocAccommodationDTO CreateDTORequest(string searchedAccName, string searchedAccCity, string searchedAccCountry, LocAccommodationDTO.AccommType searchedAccType, int searchedAccGuestsNumber, int searchedAccDaysNumber)
        {
            LocAccommodationDTO acDTO = new LocAccommodationDTO(searchedAccName, searchedAccCity, searchedAccCountry, searchedAccType,
                                                            searchedAccGuestsNumber, searchedAccDaysNumber,false);
            return acDTO;
        }

        private List<LocAccommodationDTO> Search(LocAccommodationDTO request, ObservableCollection<LocAccommodationDTO> AccommDTOsCollection)
        {
            List<LocAccommodationDTO> SearchResult = new List<LocAccommodationDTO>();
            foreach (var item in AccommDTOsCollection)
            {
                bool isCorrect = IsAppropriate(item, request);
                if (isCorrect)
                {
                    SearchResult.Add(item);
                }
            }
            return SearchResult;
        }

        private bool IsAppropriate(LocAccommodationDTO item, LocAccommodationDTO request)
        {
            bool checkName = item.AccommodationName.ToLower().Contains(request.AccommodationName.ToLower()) || request.AccommodationName.Equals(string.Empty);
            bool checkCity = item.LocationCity.ToLower().Contains(request.LocationCity.ToLower()) || request.LocationCity.Equals(string.Empty);
            bool checkCountry = item.LocationCountry.ToLower().Contains(request.LocationCountry.ToLower()) || request.LocationCountry.Equals(string.Empty);
            bool checkType = item.AccommodationType == request.AccommodationType || request.AccommodationType == LocAccommodationDTO.AccommType.NOTYPE;
            bool checkMaxGuests;
            bool checkDaysStay;
            if (request.GuestNumber == 0) checkMaxGuests = true;
            else checkMaxGuests = request.GuestNumber <= item.AccommodationMaxGuests;
            if (request.AccommodationMinDaysStay == 0) checkDaysStay = true;
            else checkDaysStay = request.AccommodationMinDaysStay >= item.AccommodationMinDaysStay;

            return checkName && checkCity && checkCountry && checkType && checkMaxGuests && checkDaysStay;
        }
    }

}
