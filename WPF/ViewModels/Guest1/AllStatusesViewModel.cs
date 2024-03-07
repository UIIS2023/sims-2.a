using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon.Primitives;
using System.Windows.Navigation;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.DTO;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.WPF.Views;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using static System.Net.Mime.MediaTypeNames;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class AllStatusesViewModel
    {
        public User LoggedInUser { get; set; }
        public NavigationService NavigationService { get; set; }    
        public ChangedReservationRequest SelectedChangedReservation { get; set; }
        public CancelAndMarkResDTO SelectedCanceledReservation { get; set; }
        public ObservableCollection<CancelAndMarkResDTO> _reservationCancelOptions { get; set; }
        public ObservableCollection<ChangedReservationRequest> _changedReservationRequests { get; set; }
        public List<AccommodationReservation> _accommodationReservations { get; set; }
        public RelayCommand FindNewDateCommand { get; set; }
        public RelayCommand CancelReservationCommand { get; set; }

        public AllStatusesViewModel(User user, NavigationService service) 
        { 
            LoggedInUser = user;
            NavigationService = service;

            _reservationCancelOptions = new ObservableCollection<CancelAndMarkResDTO>();
            _changedReservationRequests = new ObservableCollection<ChangedReservationRequest>();

            AccommodationReservationService reservationService = new AccommodationReservationService();
            _accommodationReservations = reservationService.GetAll();

            AddExistingChangedItems();
            PrepareAccommodationReservationsList();
            PrepareReservationsForChangeRequest();
            PrepareCancelReservationList();

            FindNewDateCommand = new RelayCommand(Execute_FindNewDate);
            CancelReservationCommand = new RelayCommand(Execute_CancelReservation);
             
        }

        private void AddExistingChangedItems()
        {
            ChangedReservationRequestService requestService = new ChangedReservationRequestService();
            List<ChangedReservationRequest> helpList = requestService.GetAll();
            if (helpList.Count > 0)
            {
                foreach (var item in helpList)
                {
                    if (LoggedInUser.Id == item.UserId)
                    {
                        _changedReservationRequests.Add(item);
                    }
                }
            }
        }

        private void Execute_CancelReservation(object sender)
        {
            if(SelectedCanceledReservation != null)
            {
                CancelReservation(SelectedCanceledReservation);
            }
            else
            {
                MessageBox.Show("Odaberite rezervaciju koju želite otkazati.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void CancelReservation(CancelAndMarkResDTO selectedReservation)
        {
            AccommodationService accommodationService = new AccommodationService();
            Accommodation accommodation = accommodationService.GetById(selectedReservation.AccommodationId);
            int difference = selectedReservation.FirstDay.DayOfYear - DateTime.Today.DayOfYear;

            if (difference > 1)
            {
                if (difference >= accommodation.MinDaysForCancelation)
                {
                    AccommodationReservationService reservationService = new AccommodationReservationService();
                    reservationService.CancelReservation(selectedReservation);
                    _reservationCancelOptions.Remove(selectedReservation); 
                    ChangedReservationRequest request = FindRequest(selectedReservation);
                    _changedReservationRequests.Remove(request);
                    MessageBox.Show("Uspješno otkazano!", "Potvrda", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Odabrana rezervacija se ne može otkazati zbog postavljenog vlasnikovog ograničenja za otkazivanje od " +
                                    accommodation.MinDaysForCancelation.ToString() + " dana do početka rezervacije.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Odabrana rezervacija se ne može otkazati jer počinje sutra.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private ChangedReservationRequest FindRequest(CancelAndMarkResDTO model)
        {
            foreach(var request in _changedReservationRequests)
            {
                if(request.reservationId == model.ReservationId)
                {
                    return request;
                }
            }

            return null;
        }

        private void PrepareCancelReservationList()
        {
            AccommodationService accommodationService = new AccommodationService();
            ObservableCollection<LocAccommodationDTO> _locAccommodationViewModels = accommodationService.CreateAllDTOForms();

            foreach (var reserv in _accommodationReservations)
            {
                foreach(var lavm in _locAccommodationViewModels)
                {
                    bool isInFuture = reserv.AccommodationId == lavm.AccommodationId && reserv.FirstDay > DateTime.Today && reserv.UserId == LoggedInUser.Id;
                    if (isInFuture)
                    {
                        CancelAndMarkResDTO crModel = new CancelAndMarkResDTO(lavm.AccommodationName, lavm.LocationCity, lavm.LocationCountry, reserv.FirstDay, reserv.LastDay, reserv.Id, lavm.AccommodationId);
                        _reservationCancelOptions.Add(crModel);
                    }
                }
            }
        }

        private void PrepareAccommodationReservationsList()
        {
            List<AccommodationReservation> accommodationReservationsCopy = new List<AccommodationReservation>();
            foreach(var res in _accommodationReservations)
            {
                accommodationReservationsCopy.Add(res);
            }

            foreach(var res in accommodationReservationsCopy)
            {
                foreach(var ch in _changedReservationRequests)
                {
                    if(HaveToBeDeleted(res, ch))
                    {
                        AccommodationReservationService reservationService = new AccommodationReservationService();
                        reservationService.Delete(res.Id);
                    }
                }
            }
        }

        private bool HaveToBeDeleted(AccommodationReservation res, ChangedReservationRequest ch)
        {
            return res.Id == ch.reservationId && ch.status != ChangedReservationRequest.Status.ACCEPTED && ch.status != ChangedReservationRequest.Status.REFUSED;
        }

        private void Execute_FindNewDate(object sender)
        {
            if(SelectedChangedReservation != null)
            {
                if (SelectedChangedReservation.status == ChangedReservationRequest.Status.NOT_REQUIRED)
                {
                    PrepareReservationCSV();
                    LocAccommodationDTO model = FindModel(SelectedChangedReservation);
                    NavigationService.Navigate(new EnterReservationPage(model, LoggedInUser, true, SelectedChangedReservation, NavigationService));
                }
                else
                {
                    MessageBox.Show("Odabrana rezervacija je već procesuirana!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Odaberite rezervaciju za procesuiranje.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void PrepareReservationCSV()
        {
            AccommodationReservationService reservationService = new AccommodationReservationService();
            List<AccommodationReservation> accommodationReservations = reservationService.GetAll();
            foreach(var res in accommodationReservations)
            {
                if(res.Id == SelectedChangedReservation.reservationId)
                {
                    reservationService.SaveToOtherCSV(res);
                    reservationService.Delete(res.Id);
                    break;
                }
            }
        }
        
        private LocAccommodationDTO FindModel(ChangedReservationRequest model)
        {
            AccommodationService accommodationService = new AccommodationService();
            ObservableCollection<LocAccommodationDTO> _locAccommodationViewModels = accommodationService.CreateAllDTOForms();

            foreach(var dto in _locAccommodationViewModels)
            {
                if(dto.AccommodationId == model.AccommodationId)
                {
                    return dto;
                }
            }
            return null;
        }

        private void PrepareReservationsForChangeRequest()
        {
            AccommodationService accommodationService = new AccommodationService();
            ObservableCollection<LocAccommodationDTO> _locAccommodationViewModels = accommodationService.CreateAllDTOForms();

            foreach (var res in _accommodationReservations)
            {
                foreach(var model in _locAccommodationViewModels)
                {
                    if(IsValid(res, model)) 
                    {
                        ChangedReservationRequest request = new ChangedReservationRequest(res.Id, res.AccommodationId, model.AccommodationName, model.LocationCity, model.LocationCountry, res.FirstDay, res.LastDay, res.GuestNumber, res.UserId);
                        _changedReservationRequests.Add(request);
                    }
                }
            }
        }

        private bool IsValid(AccommodationReservation res, LocAccommodationDTO model)
        {
            foreach(var request in _changedReservationRequests)
            {
                bool notValid = request.reservationId == res.Id && request.status != ChangedReservationRequest.Status.ACCEPTED && request.status != ChangedReservationRequest.Status.REFUSED;
                if (notValid)
                    return false;
            }

            return res.AccommodationId == model.AccommodationId && res.UserId == LoggedInUser.Id && res.FirstDay > DateTime.Today;
        }
    }
}
