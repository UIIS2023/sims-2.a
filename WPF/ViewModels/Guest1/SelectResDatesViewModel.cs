using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.DTO;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class SelectResDatesViewModel
    {
        public User LoggedInUser { get; set; }
        public NavigationService NavigationService { get; set; }
        public bool IsEnterOfChange { get; set; }
        public string AccNameTextBlock { get; set; }
        public AccReservationDTO selectedCatalogItem { get; set; }
        public ChangedReservationRequest ChangedReservationRequest { get; set; }
        public List<AccReservationDTO> suggestCatalog { get; set; }
        public RelayCommand ContinueReserveCommand { get; set; }
        public RelayCommand GoBackCommand { get; set; }

        public SelectResDatesViewModel(List<AccReservationDTO> list, User user, bool isEnterOfChange, ChangedReservationRequest request, NavigationService service)
        {
            suggestCatalog = list;
            LoggedInUser = user;
            IsEnterOfChange = isEnterOfChange;
            ChangedReservationRequest = request;
            NavigationService = service;

            FillTextBlock(list);

            ContinueReserveCommand = new RelayCommand(Execute_CountinueReservation);
            GoBackCommand = new RelayCommand(Execute_GoBack);
        }

        private void Execute_GoBack(object sender)
        {
            NavigationService.GoBack();
        }

        private void FillTextBlock(List<AccReservationDTO> list)
        {
            foreach (var item in list)
            {
                AccNameTextBlock = "Pronađeni termini u smještaju " + item.AccommodationName;
                break;
            }
        }

        public void Execute_CountinueReservation(object sender)
        {
            if (selectedCatalogItem == null)
            {
                MessageBox.Show("Odaberite smeštaj koji želite rezervisati.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                NavigationService.Navigate(new EnterGuestNumberPage(selectedCatalogItem, LoggedInUser, IsEnterOfChange, ChangedReservationRequest, NavigationService));
            }
        }
    }
}
