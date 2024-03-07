using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.DTO;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class WhateverCatalogViewModel
    {
        public int Guests { get; set; }
        public int Days { get; set; }
        public WhateverSearchResultsDTO ForwardedDTO { get; set; }
        public string AccNameTextBlock { get; set; }
        public AccReservationDTO SelectedCatalogItem { get; set; }
        public List<AccReservationDTO> _appointmentCatalog { get; set; }
        public NavigationService NavigationService { get; set; }
        public RelayCommand GoBackCommand { get; set; }
        public RelayCommand ReserveCommand { get; set; }

        public WhateverCatalogViewModel(WhateverSearchResultsDTO selectedAccommodation, NavigationService service, DateTime fDate, DateTime lDate, int guests, int days)
        {
            Guests = guests;
            Days = days;
            ForwardedDTO = selectedAccommodation;
            _appointmentCatalog = selectedAccommodation.AppointmentCatalog;
            NavigationService = service;
            AccNameTextBlock = "Prikazani su pronađeni termini u smještaju " + selectedAccommodation.AccommodationName + " u periodu od " + 
                                fDate.ToShortDateString() + " do " + lDate.ToShortDateString();

            GoBackCommand = new RelayCommand(Execute_GoBack);
            ReserveCommand = new RelayCommand(Execute_Reserve);
        }

        private void Execute_Reserve(object sender)
        {
            if(SelectedCatalogItem != null)
            {
                UserService userService = new UserService();
                User LoggedInUser = userService.GetById(ForwardedDTO.GuestId);
                AccommodationReservationService reservationService = new AccommodationReservationService();
                reservationService.AddReservation(SelectedCatalogItem.ReservationFirstDay, SelectedCatalogItem.ReservationLastDay, Guests,
                                        Days, SelectedCatalogItem.AccommodationId, LoggedInUser);
                MessageBox.Show("Uspješno rezervisano.", "Potvrda", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new AccommodationBidPage(LoggedInUser, NavigationService));
            }
            else
            {
                MessageBox.Show("Niste odabrali termin za rezervaciju!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Execute_GoBack(object sender)
        {
            NavigationService.GoBack();
        }
    }
}
