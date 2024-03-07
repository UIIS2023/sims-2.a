using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Navigation;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using SOSTeam.TravelAgency.Domain.DTO;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class WhateverViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public User LoggedInUser { get; set; }
        public NavigationService NavigationService { get; set; }
        public string WhateverInfo { get; set; }
        public List<Accommodation> _potentialAccommodations {  get; set; }
        public List<WhateverSearchResultsDTO> _whateverCatalog { get; set; }
        private bool isPopupOpen;
        public bool IsPopupOpen
        {
            get { return isPopupOpen; }
            set
            {
                isPopupOpen = value;
                OnPropertyChaged("IsPopupOpen");
            }
        }
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
        private string enteredDaysNumber { get; set; }
        public string EnteredDaysNumber
        {
            get { return enteredDaysNumber; }
            set
            {
                enteredDaysNumber = value;
                OnPropertyChaged("EnteredDaysNumber");
            }
        }
        private DateTime firstDate { get; set; }
        public DateTime FirstDate
        {
            get { return firstDate; }
            set
            {
                firstDate = value;
                OnPropertyChaged("FirstDate");
            }
        }
        private DateTime lastDate { get; set; }
        public DateTime LastDate
        {
            get { return lastDate; }
            set
            {
                lastDate = value;
                OnPropertyChaged("LastDate");
            }
        }
        private bool commandsEnabled;
        public bool CommandsEnabled
        {
            get { return commandsEnabled; }
            set
            {
                commandsEnabled = value;
                OnPropertyChaged("CommandsEnabled");
            }
        }
        public RelayCommand ProcessEnteredDataCommand { get; set; }
        public RelayCommand WhateverInfoCommand { get; set; }

        public WhateverViewModel(User user, NavigationService service)
        {
            LoggedInUser = user;
            NavigationService = service;
            FirstDate = DateTime.Today;
            LastDate = DateTime.Today;
            IsPopupOpen = false;
            CommandsEnabled = true;
            _potentialAccommodations = new List<Accommodation>();
            _whateverCatalog = new List<WhateverSearchResultsDTO>();
            WhateverInfo = "Unesite broj ljudi, opseg datuma kada želite da putujete\ni na koliko dana. " 
                         + "Sistem pronalazi slobodne smještaje u zadatom opsegu datuma na bilo kojoj\nlokaciji " 
                         + "koji imaju prostora za taj broj ljudi i za onoliko dana koliko je naznačeno.\n" 
                         + "Ako ne unesete opseg datuma, onda se traže smještaji koji su slobodni bilo kada\n"
                         + "za zadati broj ljudi i broj dana. Kada pronađe slobodne smještaje, sistem Vam ih prikazuje\n" 
                         + "i nudi pronađene slobodne termine koje možete odabrati i time izvršiti rezervaciju.";

            ProcessEnteredDataCommand = new RelayCommand(Execute_ProcessData);
            WhateverInfoCommand = new RelayCommand(Execute_OpenPopup);
        }

        private void Execute_OpenPopup(object sender)
        {
            if (!IsPopupOpen)
            {
                IsPopupOpen = true;
                CommandsEnabled = false;
            }
            else
            {
                IsPopupOpen = false;
                CommandsEnabled = true;
            }
        }

        protected void OnPropertyChaged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Execute_ProcessData(object sender)
        {
            _whateverCatalog.Clear();
            if(ValidateEnteredData())
            {
                FindAppropriatedAppointments();
                NavigationService.Navigate(new WhateverResultsPage(_whateverCatalog, NavigationService, FirstDate, LastDate, int.Parse(EnteredGuestNumber), int.Parse(EnteredDaysNumber)));
            }
        }

        private void FilterAllAccommodations()
        {
            _potentialAccommodations.Clear();
            AccommodationService accommodationService = new AccommodationService();
            foreach(var accommodation in accommodationService.GetAll())
            {
                if(accommodation.MaxGuests >= int.Parse(EnteredGuestNumber) && accommodation.MinDaysStay <= int.Parse(EnteredDaysNumber)) {
                    _potentialAccommodations.Add(accommodation);
                }
            }
        }

        private void FindAppropriatedAppointments()
        {
            FilterAllAccommodations();
            if(FirstDate == DateTime.Today && LastDate == DateTime.Today)
            {
                GoToProcess(false);
                MessageBox.Show("Obzirom da niste podesili opseg datuma, prikazaćemo rezultate za naredna 3 mjeseca.", "Potvrda", MessageBoxButton.OK, MessageBoxImage.Information);
            } 
            else
            {
                GoToProcess(true);
            }
        }

        private void GoToProcess(bool isEnteredDates)
        {
            foreach(var accommodation in _potentialAccommodations)
            {
                WhateverSearchResultsDTO dto = new WhateverSearchResultsDTO(accommodation.Id, accommodation.LocationId, accommodation.Name, accommodation.MinDaysStay, accommodation.MaxGuests, LoggedInUser.Id, accommodation.Type);
                CreateAppointmentCatalog(accommodation, dto, isEnteredDates);
                if(dto.AppointmentCatalog.Count > 0)
                {
                    dto.FoundAppointmentsNumber = "Pronađenih termina: " + dto.AppointmentCatalog.Count;
                    _whateverCatalog.Add(dto);
                }
            }
        }

        private void CreateAppointmentCatalog(Accommodation acc, WhateverSearchResultsDTO dto, bool isEnteredDates)
        {
            List<DateTime> allDays = new List<DateTime>();
            if(!isEnteredDates)
            {
                LastDate = DateTime.Today.AddDays(100);
            }

            for(var day = FirstDate; day <= LastDate; day = day.AddDays(1))
            {
                allDays.Add(day);
            }

            int counter = -1;
            foreach(var day in allDays)
            {
                counter++;
                if(day.DayOfYear + int.Parse(EnteredDaysNumber) > allDays[allDays.Count-1].DayOfYear)
                {
                    break;
                }
                else
                {
                    DateTime lastDay = allDays[counter + int.Parse(EnteredDaysNumber)];
                    AccommodationReservation potentialReservation = new AccommodationReservation(day, lastDay, int.Parse(EnteredDaysNumber), int.Parse(EnteredGuestNumber), acc.Id, LoggedInUser.Id);
                    if(CheckExistingReservations(potentialReservation, acc))
                    {
                        AccReservationDTO littleDto = new AccReservationDTO(acc.Id, acc.Name, acc.MinDaysStay, day, lastDay, int.Parse(EnteredDaysNumber), int.Parse(EnteredGuestNumber), -1);
                        dto.AppointmentCatalog.Add(littleDto);
                    }
                }
            }
        }

        private bool CheckExistingReservations(AccommodationReservation potentialReservation, Accommodation acc)
        {
            List<DateTime> allPotentialReservationDays = new List<DateTime>();
            for(var day = potentialReservation.FirstDay; day <= potentialReservation.LastDay; day = day.AddDays(1))
            {
                allPotentialReservationDays.Add(day);
            }

            AccommodationReservationService reservationService = new AccommodationReservationService();
            List<AccommodationReservation> allReservations = new List<AccommodationReservation>();
            foreach(var item in reservationService.GetAll())
            {
                if(item.AccommodationId == acc.Id)
                {
                    allReservations.Add(item);
                }
            }
            foreach(var item in reservationService.GetProcessedReservations())
            {
                if(!item.DefinitlyChanged && item.AccommodationId == acc.Id)
                {
                    allReservations.Add(item);
                }
            }

            foreach(var item in allReservations)
            {
                List<DateTime> allDays = new List<DateTime>();
                for (var day = item.FirstDay; day <= item.LastDay; day = day.AddDays(1))
                {
                    allDays.Add(day);
                }
                if (!CompareLists(allPotentialReservationDays, allDays)) return false;
            }

            return true;
        }

        private bool CompareLists(List<DateTime> first, List<DateTime> second)
        {
            foreach(var item2 in second)
            {
                foreach(var item1 in first)
                {
                    if(item1.DayOfYear == item2.DayOfYear)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool ValidateEnteredData()
        {
            return ValidateEnteredGuestNumber() && ValidateEnteredDaysNumber() && ValidateEnteredDates();
        }

        private bool ValidateEnteredGuestNumber()
        {
            if (EnteredGuestNumber == null)
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

        private bool ValidateEnteredDaysNumber()
        {
            if (EnteredDaysNumber == null)
            {
                MessageBox.Show("Unesite broj dana.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (!Regex.IsMatch(EnteredDaysNumber, @"^[0-9]+$"))
            {
                MessageBox.Show("Broj dana se mora sastojati od cifara!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private bool ValidateEnteredDates()
        {
            if(FirstDate != DateTime.Today ||  LastDate != DateTime.Today)
            {
                int days = int.Parse(EnteredDaysNumber);
                int diff = LastDate.DayOfYear - FirstDate.DayOfYear;
                if (FirstDate > LastDate)
                {
                    MessageBox.Show("Nevalidan odabir datuma!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                else if (days > diff)
                {
                    MessageBox.Show("Broj dana za rezervaciju je veći od zadatog opsega.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            return true;
        }
    }
}
