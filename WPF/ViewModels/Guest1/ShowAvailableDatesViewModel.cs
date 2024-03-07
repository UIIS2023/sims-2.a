using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Repositories;
using SOSTeam.TravelAgency.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using System.Windows.Navigation;
using SOSTeam.TravelAgency.Domain.DTO;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class ShowAvailableDatesViewModel
    {
        public ObservableCollection<AccReservationDTO> reservationDTOList { get; set; }
        public AccommodationService accommodationService { get; set; }
        public AccommodationReservationService reservationService { get; set; }
        public AccommodationRenovationService renovationService { get; set; }
        public List<Accommodation> accommodations { get; set; }
        public List<AccommodationReservation> reservations { get; set; }
        public LocAccommodationDTO accommodationDTO { get; set; }
        public DateTime EnteredFirstDay { get; set; }
        public DateTime EnteredLastDay { get; set; }
        public DateTime[] datesArray { get; set; }
        public int DaysDuration { get; set; }
        public List<AccReservationDTO> dtoReservation { get; set; }
        public User LoggedInUser { get; set; }
        public Calendar Calendar { get; set; }
        public RelayCommand pickCommand { get; set; }
        public RelayCommand GoBackCommand { get; set; }
        public bool IsEnterOfChange { get; set; }
        public ChangedReservationRequest ChangedReservationRequest { get; set; }
        public NavigationService NavigationService { get; set; }

        public ShowAvailableDatesViewModel(LocAccommodationDTO dto, DateTime firstDay, DateTime lastDay, int days, User user, Calendar calendar, bool enter, ChangedReservationRequest request, NavigationService service)
        {
            reservationDTOList = new ObservableCollection<AccReservationDTO>();
            accommodationService = new AccommodationService();
            reservationService = new AccommodationReservationService();
            renovationService = new();
            dtoReservation = new List<AccReservationDTO>();

            datesArray = new DateTime[100];

            accommodationDTO = dto;
            EnteredFirstDay = firstDay;
            EnteredLastDay = lastDay;
            DaysDuration = days;
            LoggedInUser = user;
            Calendar = calendar;
            IsEnterOfChange = enter;
            ChangedReservationRequest = request;
            NavigationService = service;

            accommodations = accommodationService.GetAll();
            reservations = reservationService.GetAll();

            AnalyzeUnknownStatusReservations();
            MarkCalendars();
            pickCommand = new RelayCommand(ExecutePickItem);
            GoBackCommand = new RelayCommand(Execute_GoBack);
        }

        public void Execute_GoBack(object sender)
        {
            NavigationService.GoBack();
        }

        private void AnalyzeUnknownStatusReservations()
        {
            if(IsEnterOfChange)
            {
                foreach (var item in reservationService.GetProcessedReservations())
                {
                    if (!item.DefinitlyChanged && item.UserId != LoggedInUser.Id)
                        reservations.Add(item);
                }
            }
            else
            {
                foreach (var item in reservationService.GetProcessedReservations())
                {
                    if (!item.DefinitlyChanged)
                        reservations.Add(item);
                }
            }
        }

        private List<AccReservationDTO> CreateAllDTOreservations()
        {
            foreach (var accommodation in accommodations)
            {
                foreach (var reservation in reservations)
                {
                    if (accommodation.Id == reservation.AccommodationId)
                    {
                        AccReservationDTO Dto = CreateOneDTOreservation(accommodation, reservation);
                        reservationDTOList.Add(Dto);
                    }
                }
            }
            return reservationDTOList.ToList();
        }

        private AccReservationDTO CreateOneDTOreservation(Accommodation acc, AccommodationReservation res)
        {
            int currentGuestNumber = 0;
            foreach (var item in reservations)
            {
                if (item.AccommodationId == acc.Id)
                {
                    DateTime today = DateTime.Today;
                    int helpVar1 = today.DayOfYear - item.FirstDay.DayOfYear;
                    int helpVar2 = today.DayOfYear - item.LastDay.DayOfYear;
                    if (helpVar1 >= 0 && helpVar2 <= 0)
                    {
                        currentGuestNumber += item.GuestNumber;
                    }
                }
            }
            AccReservationDTO dto = new AccReservationDTO(acc.Id, acc.Name, acc.MinDaysStay, res.FirstDay, res.LastDay, res.ReservationDuration, acc.MaxGuests, currentGuestNumber);
            return dto;
        }

        private void MarkCalendars()
        {
            List<AccReservationDTO> reservationsDTO = CreateAllDTOreservations();
            var renovations = renovationService.GetAll().Where(r => r.AccommodationId == accommodationDTO.AccommodationId);
            Calendar.BlackoutDates.AddDatesInPast();
            foreach (var item in reservationsDTO)
            {
                if (item.AccommodationId == accommodationDTO.AccommodationId)
                {
                    MarkCalendar(item);
                }
            }

            foreach (var item in renovations)
            {
                
                 MarkCalendar(item);
                
            }

            CheckRequestedDates();
        }

        private void MarkCalendar(AccReservationDTO reservationDTO)
        {
            int[] ints = GetDateData(reservationDTO);
            DateTime item1 = new DateTime(ints[0], ints[1], ints[2]);
            DateTime item2 = new DateTime(ints[3], ints[4], ints[5]);
            Calendar.BlackoutDates.Add(new CalendarDateRange(item1, item2));
        }
        
        private void MarkCalendar(AccommodationRenovation renovation)
        {
            DateTime item1 = new DateTime(renovation.FirstDay.Year, renovation.FirstDay.Month, renovation.FirstDay.Day);
            DateTime item2 = new DateTime(renovation.LastDay.Year, renovation.LastDay.Month, renovation.LastDay.Day);
            Calendar.BlackoutDates.Add(new CalendarDateRange(item1, item2));
        }


        private int[] GetDateData(AccReservationDTO res)
        {
            int[] data = new int[6];
            data[0] = res.ReservationFirstDay.Year;
            data[1] = res.ReservationFirstDay.Month;
            data[2] = res.ReservationFirstDay.Day;

            data[3] = res.ReservationLastDay.Year;
            data[4] = res.ReservationLastDay.Month;
            data[5] = res.ReservationLastDay.Day;

            return data;
        }

        private void CheckRequestedDates()
        {
            int[] daysCounter = FindFreeDaysInRow(EnteredFirstDay, EnteredLastDay);
            int[] appropiatedIndexes = FindAppropiatedIndexes(daysCounter);
            int notOkeyCounter = 0;
            int okeyCounter = 0;

            foreach (int index in appropiatedIndexes)
            {
                if (index == -1)
                {
                    notOkeyCounter++;
                }
                else okeyCounter++;
            }

            if (notOkeyCounter == appropiatedIndexes.Length)
            {
                MessageBox.Show("U zadatom periodu nema slobodnih termina. Prikazaćemo Vam opcije u sledeća dva meseca.", "Potvrda", MessageBoxButton.OK, MessageBoxImage.Information);
                AnalyzeNextFiftyDays();
            }
            else
            {
                CreateFreeAppointmentsCatalog(daysCounter, appropiatedIndexes, okeyCounter, EnteredFirstDay);
            }
        }

        private void AnalyzeNextFiftyDays()
        {
            DateTime dayInFiftydDays = EnteredLastDay.AddDays(50);
            int[] alternativeDaysCounter = FindFreeDaysInRow(EnteredLastDay, dayInFiftydDays);
            int[] appropiatedIndexes = FindAppropiatedIndexes(alternativeDaysCounter);
            int notOkeyCounter = 0;
            int okeyCounter = 0;

            foreach (int index in appropiatedIndexes)
            {
                if (index == -1)
                {
                    notOkeyCounter++;
                }
                else okeyCounter++;
            }

            if (notOkeyCounter == appropiatedIndexes.Length)
            {
                MessageBox.Show("Nažalost, svi dani u traženom opsegu i dva meseca nakon njega su popunjeni.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                CreateFreeAppointmentsCatalog(alternativeDaysCounter, appropiatedIndexes, okeyCounter, EnteredLastDay);
            }
        }

        private void CreateFreeAppointmentsCatalog(int[] array, int[] appIndexes, int okCount, DateTime fDay)
        {
            int checkCounter = 0;
            int daysSum = 0;
            int j = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (checkCounter < okCount)
                {
                    if (i == appIndexes[j])
                    {
                        if (array[i] > DaysDuration)
                        {
                            int diff = array[i] - DaysDuration;
                            int daysSumCopy = daysSum;
                            for (int k = 0; k < diff; k++)
                            {
                                CreateCatalogItem(daysSumCopy, fDay);
                                daysSumCopy++;
                            }
                        }
                        else
                        {
                            CreateCatalogItem(daysSum, fDay);
                        }
                        j++;
                        checkCounter++;
                    }
                    daysSum += array[i];
                }
            }
        }

        private void CreateCatalogItem(int daysSum, DateTime fDay)
        {
            DateTime firstComponent = fDay.AddDays(daysSum);
            DateTime secondComponent = firstComponent.AddDays(DaysDuration);
            AccReservationDTO dto = new AccReservationDTO(accommodationDTO.AccommodationId, accommodationDTO.AccommodationName,
                                                          accommodationDTO.AccommodationMinDaysStay, firstComponent, secondComponent,
                                                          DaysDuration, accommodationDTO.AccommodationMaxGuests, accommodationDTO.GuestNumber);
            dtoReservation.Add(dto);
        }

        private int[] FindFreeDaysInRow(DateTime fDay, DateTime lDay)
        {
            CalendarBlackoutDatesCollection blackoutDates = Calendar.BlackoutDates;
            int[] counterArray = new int[100];
            int i = 0;
            int z = 0;
            bool flag = false;
            int j = fDay.DayOfYear;
            int k = lDay.DayOfYear;
            DateTime firstJan = new DateTime(fDay.Year, 1, 1);
            for (; j <= k; j++)
            {
                if (!blackoutDates.Contains(firstJan.AddDays(j - 1)))
                {
                    if (flag)
                    {
                        i++;
                    }
                    counterArray[i]++;
                    datesArray[z++] = firstJan.AddDays(j - 1);
                    flag = false;
                }
                else
                {
                    flag = true;
                    counterArray[++i]++;
                    datesArray[z++] = firstJan.AddDays(j - 1);
                }
            }

            return counterArray;
        }

        private int[] FindAppropiatedIndexes(int[] array)
        {
            int[] result = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = -1;
            }

            int j = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != 1)
                {
                    if (array[i] >= DaysDuration)
                    {
                        result[j++] = i;
                    }
                }
            }

            return result;
        }

        public void ExecutePickItem(object sender)
        {
            NavigationService.Navigate(new SelectReservationDatesPage(dtoReservation, LoggedInUser, IsEnterOfChange, ChangedReservationRequest, NavigationService));
        }
    }
}
