using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    public class RenovationAddPageViewModel : ViewModel
    {
        public User LoggedInUser { get; private set; }

        public ObservableCollection<Accommodation> Accommodations { get; private set; }
        public ObservableCollection<DatesRangeViewModel> PossibleDateRanges { get; set; }
        public ObservableCollection<DateTime> BlackoutDates { get; set; }

        public DatesRangeViewModel SelectedDateRange { get; set; }

        public RelayCommand Cancel { get; private set; }
        public RelayCommand SearchDates { get; private set; }
        public RelayCommand ResetCalendar { get; private set; }
        public RelayCommand AddRenovation { get; private set; }
        public string Comment { get; set; }

        public Accommodation SelectedAccommodation
        {
            get { return _selectedAccommodation; }
            set
            {
                if (_selectedAccommodation != value)
                {
                    _selectedAccommodation = value;
                    if(SelectedAccommodation == null) IsCalendarEnabled = false;
                    SetBlackoutDates();
                    OnPropertyChanged(nameof(SelectedAccommodation));
                }
            }
        } 

        public string StartDateString
        {
            get { return _startDateString; }
            set
            {
                if (_startDateString != value)
                {
                    _startDateString = value;
                    OnPropertyChanged(nameof(StartDateString));
                }
            }
        }

        public System.Windows.Visibility ChooseDateVisibility
        {
            get { return _chooseDateVisibility; }
            set
            {
                if (_chooseDateVisibility != value)
                {
                    _chooseDateVisibility = value;
                    OnPropertyChanged(nameof(ChooseDateVisibility));
                }
            }
        }
        public System.Windows.Visibility PickDateVisibility
        {
            get { return _pickDateVisibility; }
            set
            {
                if (_pickDateVisibility != value)
                {
                    _pickDateVisibility = value;
                    OnPropertyChanged(nameof(PickDateVisibility));
                }
            }
        }

        public string EndDateString
        {
            get { return _endDateString; }
            set
            {
                if (_endDateString != value)
                {
                    _endDateString = value;
                    OnPropertyChanged(nameof(EndDateString));
                }
            }
        }

        public int RenovationDuration
        {
            get { return _renovationDuration; }
            set
            {
                if (_renovationDuration != value)
                {
                    _renovationDuration = value;
                    OnPropertyChanged(nameof(RenovationDuration));
                }
            }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    SelectedDateChange(_selectedDate);
                    OnPropertyChanged(nameof(SelectedDate));
                }
            }
        }
        public bool IsCalendarEnabled
        {
            get { return _isCalendarEnabled; }
            set
            {
                if (_isCalendarEnabled != value)
                {
                    _isCalendarEnabled = value;
                    OnPropertyChanged(nameof(IsCalendarEnabled));
                }
            }
        }




        private DateTime _startDate;
        private DateTime _endDate;
        private DateTime _selectedDate;
        private string _startDateString;
        private string _endDateString;
        private bool _isCalendarEnabled;
        private int _datePickCounter;
        private int _renovationDuration;
        private System.Windows.Visibility _chooseDateVisibility;
        private System.Windows.Visibility _pickDateVisibility;
        private Accommodation _selectedAccommodation;



        private AccommodationService _accommodationService;
        private AccommodationRenovationService _accommodationRenovationService;
        private AccommodationReservationService _accommodationReservationService;






        public RenovationAddPageViewModel()
        {
            LoggedInUser = App.LoggedUser;
            _accommodationService = new();
            _accommodationRenovationService = new();
            _accommodationReservationService = new();
            Accommodations = new ObservableCollection<Accommodation>(_accommodationService.GetAllByUserId(LoggedInUser.Id));
            SelectedAccommodation = Accommodations.First();
            PossibleDateRanges = new();
            BlackoutDates = new();
            ChooseDateVisibility = System.Windows.Visibility.Visible;
            PickDateVisibility = System.Windows.Visibility.Collapsed;
            RenovationDuration = 1;
            RefreshCalendar();

            Cancel = new RelayCommand(Execute_Cancel, CanExecuteCancel);
            SearchDates = new RelayCommand(Execute_SearchDates, CanExecuteSearchDates);
            ResetCalendar = new RelayCommand(Execute_SetupCalendar, CanExecuteSetupCalendar);
            AddRenovation = new RelayCommand(Execute_AddRenovation, CanExecuteAddRenovation);

        }

        private void Execute_AddRenovation(object obj)
        {
            if (Comment == null) Comment = "";
            _accommodationRenovationService.Save(new AccommodationRenovation(SelectedAccommodation.Id, SelectedDateRange.StartDate, SelectedDateRange.EndDate,Comment));
            App.OwnerNavigationService.NavigateMainWindow("Renovation");
        }

        private bool CanExecuteAddRenovation(object obj)
        {
            return SelectedDateRange != null && SelectedAccommodation != null;
        }

        private bool CanExecuteSearchDates(object obj)
        {
            return _datePickCounter >= 2 && SelectedAccommodation != null;
        }

        public void RefreshCalendar()
        {
            Execute_SetupCalendar(null);
        }

        private void Execute_Cancel(object obj)
        {
            App.OwnerNavigationService.NavigateMainWindow("Renovation");
        }

        private bool CanExecuteCancel(object obj)
        {
            return true;
        }

        private void Execute_SetupCalendar(object obj)
        {
            StartDate = DateTime.Now;
            SelectedDate = DateTime.Now;
            EndDate = DateTime.Now.AddYears(5);
            _datePickCounter = 0;

            IsCalendarEnabled = true;
            StartDateString = "";
            EndDateString = "";
        }

        private void SetBlackoutDates()
        {
            if (BlackoutDates == null) return;
            BlackoutDates.Clear();
            var reservations = _accommodationReservationService.GetAll().Where(r => SelectedAccommodation.Id == r.AccommodationId);
            reservations = reservations.Where(r => r.LastDay >= DateTime.Today);
            foreach (var reservation in reservations)
            {
                BlackoutDates.AddRange(GetDatesBetween(reservation.FirstDay, reservation.LastDay));
            }
            OnPropertyChanged(nameof(BlackoutDates));
        }

        private List<DateTime> GetDatesBetween(DateTime start,DateTime end) 
        {
            List <DateTime> dates = new();
            if (start <= DateTime.Today) start = DateTime.Today.AddDays(1);
            while(start.CompareTo(end) <= 0) 
            {
                dates.Add(start);
                start = start.AddDays(1);
            }
            return dates;
        }

        private void SelectedDateChange(DateTime selectedDate)
        {
            _datePickCounter++;
            if (_datePickCounter == 2)
            {
                EndDate = selectedDate;
                EndDateString = EndDate.ToString("dd/MM/yyyy");
            }
            if (_datePickCounter == 1)
            {
                StartDate = selectedDate;
                StartDateString = StartDate.ToString("dd/MM/yyyy");
            }

        }


        private void Execute_SearchDates(object obj)
        {
            ChooseDateVisibility = System.Windows.Visibility.Collapsed;
            PickDateVisibility = System.Windows.Visibility.Visible;

            DateTime end = EndDate.AddDays(-(RenovationDuration-1));
            DateTime temp = StartDate;
            var dateRanges = new List<DatesRangeViewModel>();
            while (temp.CompareTo(end) <= 0)
            {
                dateRanges.Add(new DatesRangeViewModel(temp, temp.AddDays(RenovationDuration - 1)));
                temp = temp.AddDays(1);
            }
            var blackoutRanges = dateRanges.Where(d => !(BlackoutDates.Any(b => b <= d.EndDate && b >= d.StartDate)));
            PossibleDateRanges.AddRange(
                blackoutRanges.Where(d => !(BlackoutDates.Any(b => CompareDayMonthYear(b, d.EndDate) || CompareDayMonthYear(b, d.StartDate))))
                );
        }

        private bool CompareDayMonthYear(DateTime one,DateTime two) 
        {
            return one.Day == two.Day && one.Month == two.Month && one.Year == two.Year;


        }

        private bool CanExecuteSetupCalendar(object obj)
        {
            return true;
        }

        
    }

    public class DatesRangeViewModel
    {
        public DateTime StartDate { get; set; }
        public string StartDateString { get; set; }
        public DateTime EndDate { get; set; }
        public string EndDateString { get; set; }
        public DatesRangeViewModel(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            StartDateString = StartDate.ToString("dd/MM/yyyy");
            EndDate = endDate;
            EndDateString = EndDate.ToString("dd/MM/yyyy");
        }
    }

}
