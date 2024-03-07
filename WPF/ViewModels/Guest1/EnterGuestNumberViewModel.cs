using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.DTO;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class EnterGuestNumberViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public User LoggedInUser { get; set; }
        public NavigationService NavigationService { get; set; }
        public int guestNumber { get; set; }
        public string MaximalGuestsNumber { get; set; }
        private string enteredGuestNumber { get; set; }
        public string EnteredGuestNumber
        {
            get { return enteredGuestNumber; }
            set
            {
                enteredGuestNumber = value;
                OnPropertyChaged("EnteredGuestNumber");
            }
        }
        public AccReservationDTO forwardedItem { get; set; }
        public ChangedReservationRequest ChangedReservationRequest { get; set; }
        public RelayCommand finishReserveCommand { get; set; }
        public RelayCommand GoBackCommand { get; set; }
        
        public EnterGuestNumberViewModel(AccReservationDTO item, User user, bool enterOfChange, ChangedReservationRequest request, NavigationService service)
        {
            forwardedItem = item;
            MaximalGuestsNumber = "*Maksimalan broj gostiju je " + forwardedItem.AccommodationMaxGuests;
            LoggedInUser = user;
            ChangedReservationRequest = request;
            NavigationService = service;
            EnteredGuestNumber = string.Empty;

            if (!enterOfChange)
                finishReserveCommand = new RelayCommand(ExecuteReserveAccommodation);
            else
                finishReserveCommand = new RelayCommand(ExecuteSendRequestForChange);

            GoBackCommand = new RelayCommand(Execute_GoBack);
        }

        protected void OnPropertyChaged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Execute_GoBack(object sender)
        {
            NavigationService.GoBack();
        }

        private void ExecuteSendRequestForChange(object sender)
        {
            if(Validate())
            {
                guestNumber = int.Parse(EnteredGuestNumber);
                ExecuteSendRequestForChange(forwardedItem, guestNumber);
            }
        }

        private void ExecuteSendRequestForChange(AccReservationDTO item, int forwadedGuestNumber)
        {
            AccommodationReservationService accResService = new AccommodationReservationService();
            if (forwadedGuestNumber > 0)
            {
                if (forwadedGuestNumber > item.AccommodationMaxGuests)
                {
                    MessageBox.Show("Prekoračen je maksimalni broj gostiju za ovaj smeštaj. Pokušajte ponovo.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    accResService.ProcessReservation(item, LoggedInUser, ChangedReservationRequest);
                    MessageBox.Show("Zahtjev za pomjeranje rezervacije je uspješno poslat vlasniku.", "Potvrda", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.Navigate(new AllStatusesPage(LoggedInUser, NavigationService));
                }
            }
            else
            {
                MessageBox.Show("Ne možete izvršiti rezervaciju za 0 osoba.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteReserveAccommodation(object sender)
        {
            if (Validate())
            {
                guestNumber = int.Parse(EnteredGuestNumber);
                ExecuteReserveAccommodation(forwardedItem, guestNumber);
            }
        }

        private bool Validate()
        {
            if (EnteredGuestNumber.Equals(""))
            {
                MessageBox.Show("Unesite broj gostiju.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (!Regex.IsMatch(EnteredGuestNumber, @"^[0-9]+$"))
            {
                MessageBox.Show("Broj gostiju se mora sastojati od cifara!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void ExecuteReserveAccommodation(AccReservationDTO item, int forwadedGuestNumber)
        {
            AccommodationReservationService accResService = new AccommodationReservationService();
            if (forwadedGuestNumber > 0)
            {
                if (forwadedGuestNumber > item.AccommodationMaxGuests)
                {
                    MessageBox.Show("Prekoračen je maksimalni broj gostiju za ovaj smeštaj. Pokušajte ponovo.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    accResService.AddReservation(item.ReservationFirstDay, item.ReservationLastDay, forwadedGuestNumber,
                                    item.ReservationDuration, item.AccommodationId, LoggedInUser);
                    MessageBox.Show("Uspješno rezervisano.", "Potvrda", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.Navigate(new AccommodationBidPage(LoggedInUser, NavigationService));
                }
            }
            else
            {
                MessageBox.Show("Ne možete izvršiti rezervaciju za 0 osoba.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
