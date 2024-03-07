using LiveCharts;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using System.Windows.Navigation;
using SOSTeam.TravelAgency.Domain.DTO;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class AccommodationBidViewModel
    {
        public User LoggedInUser { get; set; }
        public NavigationService NavigationService { get; set; }
        public ObservableCollection<LocAccommodationDTO> _accommDTOsCollection { get; set; }
        public LocAccommodationDTO SelectedAccommodationDTO { get; set; }
        public RelayCommand ReserveCommand { get; set; }

        private AccommodationRenovationService _accommodationRenovationService ;
        
        public AccommodationBidViewModel(User user, NavigationService service)
        {
            LoggedInUser = user;
            NavigationService = service;

            AccommodationService accommodationService = new AccommodationService();
            _accommDTOsCollection = accommodationService.CreateAllDTOForms();
            _accommodationRenovationService = new();
            CheckLastRenovations();

            ReserveCommand = new RelayCommand(Execute_ReserveAccommodation);
        }

        private void CheckLastRenovations()
        {
            foreach (var dto in _accommDTOsCollection)
            {
                var renovations = _accommodationRenovationService.GetAll();
                if (renovations.Any(r => r.AccommodationId == dto.AccommodationId && r.LastDay > DateTime.Today.AddYears(-1) && r.LastDay < DateTime.Today ) )
                {
                    dto.IsRenovatedInLastYear = true;
                }
            }
        }

        public void Execute_ReserveAccommodation(object sender)
        {
            if (SelectedAccommodationDTO != null)
            {
                NavigationService.Navigate(new EnterReservationPage(SelectedAccommodationDTO, LoggedInUser, false, NavigationService));
            }
            else
            {
                MessageBox.Show("Morate da odaberete smeštaj za rezervaciju.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
