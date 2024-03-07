using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.DTO;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class ReportFiltersViewModel
    {
        public User LoggedInUser { get; set; }
        public NavigationService NavigationService { get; set; }
        public string SelectedOption { get; set; }
        public DateTime FirstDate { get; set; }
        public DateTime LastDate { get; set; }
        public ObservableCollection<string> _comboBoxOptions { get; set; }
        public List<CancelAndMarkResDTO> _presentedReservations { get; set; }
        public RelayCommand ProcessReportCommand { get; set; }

        public ReportFiltersViewModel(User user, NavigationService service)
        {
            LoggedInUser = user;
            NavigationService = service;
            
            _comboBoxOptions = new ObservableCollection<string> { "Zakazane", "Otkazane" };
            SelectedOption = _comboBoxOptions[1];
            FirstDate = DateTime.Now;
            LastDate = DateTime.Now;
            
            _presentedReservations = new List<CancelAndMarkResDTO>();

            ProcessReportCommand = new RelayCommand(Execute_ProcessReport);
        }
        
        private void Execute_ProcessReport(object sender)
        {
            if (LastDate < FirstDate)
            {
                MessageBox.Show("Datumi opsega nisu izabrani validno!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                int k = FindSelectedType();
                if (k == 0) LoadFuturedReservations();
                else LoadCanceledReservations();
                NavigationService.Navigate(new ReportResultsPage(_presentedReservations, NavigationService, k, FirstDate, LastDate));
            }
        }
        
        private int FindSelectedType()
        {
            if (SelectedOption.Equals(_comboBoxOptions[0])) return 0;
            else return 1;
        }

        private void LoadFuturedReservations()
        {
            AccommodationService accommodationService = new AccommodationService();
            AccommodationReservationService reservationService = new AccommodationReservationService();
            ObservableCollection<LocAccommodationDTO> _locAccommodationViewModels = accommodationService.CreateAllDTOForms();
            List<AccommodationReservation> _accommodationReservations = reservationService.GetAll();

            _presentedReservations.Clear();
            foreach (var lavm in _locAccommodationViewModels)
            {
                foreach (var res in _accommodationReservations)
                {
                    if (IsValidForList(lavm, res))
                    { 
                        CancelAndMarkResDTO model = new CancelAndMarkResDTO(lavm.AccommodationName, lavm.LocationCity, lavm.LocationCountry, res.FirstDay, res.LastDay, res.Id, lavm.AccommodationId, "", res.ReservationDuration, lavm.AccommodationType);
                        _presentedReservations.Add(model);
                    }
                }
            }
        }

        private bool IsValidForList(LocAccommodationDTO lavm, AccommodationReservation res)
        {
            return lavm.AccommodationId == res.AccommodationId && res.UserId == LoggedInUser.Id && res.FirstDay >= FirstDate && res.LastDay <= LastDate;
        }

        private void LoadCanceledReservations()
        {
            AccommodationService accommodationService = new AccommodationService();
            AccommodationReservationService reservationService = new AccommodationReservationService();
            ObservableCollection<LocAccommodationDTO> _locAccommodationViewModels = accommodationService.CreateAllDTOForms();
            List<AccommodationReservation> _canceledReservations = reservationService.LoadCanceledReservations();

            _presentedReservations.Clear();
            foreach (var lavm in _locAccommodationViewModels)
            {
                foreach (var res in _canceledReservations)
                {
                    if (IsValidForList(lavm, res))
                    {
                        CancelAndMarkResDTO model = new CancelAndMarkResDTO(lavm.AccommodationName, lavm.LocationCity, lavm.LocationCountry, res.FirstDay, res.LastDay, res.Id, lavm.AccommodationId, "", res.ReservationDuration, lavm.AccommodationType);
                        _presentedReservations.Add(model);
                    }
                }
            }
        }
    }
}
