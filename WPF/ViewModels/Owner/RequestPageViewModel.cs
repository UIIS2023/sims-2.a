using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    internal class RequestPageViewModel : ViewModel
    {
        private AccommodationReservationService _accommodationReservationService;
        private UserService _userService;

        public User LoggedInUser { get; private set; }
        public RelayCommand Accept { get; private set; }
        public RelayCommand Deny { get; private set; }
        public RelayCommand ShowDenialPopup { get; private set; }
        public string Comment { get; set; }
        private bool _isCommentDropOpen;

        public bool IsCommentDropOpen
        {
            get => _isCommentDropOpen;
            set
            {
                if (_isCommentDropOpen != value)
                {
                    _isCommentDropOpen = value;
                    OnPropertyChanged(nameof(IsCommentDropOpen));
                }
            }
        }
        public ObservableCollection<RequestViewModel> Requests { get; set; }
        public RequestViewModel SelectedRequest { get; set; }

        public RequestPageViewModel()
        {
            _accommodationReservationService = new();
            _userService = new();
            LoggedInUser = App.LoggedUser;
            Requests = new();
            IsCommentDropOpen = false;
            FillObservableCollection();

            

            Accept = new RelayCommand(Execute_Accept, CanExecuteDenyorAccept);
            Deny = new RelayCommand(Execute_Deny, CanExecuteDenyorAccept);
            ShowDenialPopup = new RelayCommand(Execute_ShowDenialPopup, CanExecuteDenyorAccept);
        }

        private void FillObservableCollection()
        {
            AccommodationReservationService.ReservationsInformations reservationsInformations = _accommodationReservationService.SendRequestToOwner(LoggedInUser.Id);
            foreach (var request in reservationsInformations.requests) 
            {
                foreach (var newDates in reservationsInformations.newDates) 
                {
                    if (request.reservationId == newDates.OldReservationId) 
                    {
                        string username = _userService.GetById(request.UserId).Username;
                        Requests.Add(new RequestViewModel(request, newDates, username));
                    }
                }
            }
        }

        private void Execute_ShowDenialPopup(object obj)
        {
            IsCommentDropOpen = true;
        }
        private void Execute_Deny(object obj)
        {
            if (Comment == null) Comment = "";
            _accommodationReservationService.DeclineReservationChanges(Comment,SelectedRequest.NewDate, SelectedRequest.Request, LoggedInUser.Id);
            RefreshList();
        }
        private void Execute_Accept(object obj)
        {
            _accommodationReservationService.AcceptReservationChanges( SelectedRequest.NewDate,SelectedRequest.Request,LoggedInUser.Id);
            RefreshList();
        }

        private bool CanExecuteDenyorAccept(object obj)
        {
            return SelectedRequest != null;
        }

        private void RefreshList() 
        {
            Requests.Clear();
            FillObservableCollection();
        }
    }

    internal class RequestViewModel
    {
        public ChangedReservationRequest Request { get; private set; }
        public WantedNewDate NewDate { get; private set; }
        public string Username { get; private set; }
        public string OldReservationDates { get; private set; }
        public string NewReservationDates { get; private set; }
        public RequestViewModel( ChangedReservationRequest changedReservationRequest,WantedNewDate wantedNewDate,string username) 
        {
            Request = changedReservationRequest;
            NewDate = wantedNewDate;
            OldReservationDates = changedReservationRequest.OldFirstDay.ToShortDateString() + " - " + changedReservationRequest.OldLastDay.ToShortDateString();
            NewReservationDates = wantedNewDate.ReservationFirstDay.ToShortDateString() + " - " + wantedNewDate.ReservationLastDay.ToShortDateString();
            Username = username;
        }
    }
}
