using LiveCharts;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.DTO;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Navigation;
using System.Xml.Linq;
using Xceed.Wpf.AvalonDock.Controls;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class GuestInboxViewModel
    {
        public User LoggedInUser { get; set; }
        public NavigationService ProfilleNavigationService { get; set; }
        public ObservableCollection<CancelAndMarkResDTO> _reservationsForMark { get; set; }
        public List<CancelAndMarkResDTO> _changedReservations { get; set; }
        public ObservableCollection<CancelAndMarkResDTO> _ratingsFromOwner { get; set; }
        public RelayCommand ShowMenuCommand { get; set; }
        public RelayCommand GoBackCommand { get; set; }
        public RelayCommand MarkAccommodationCommand { get; set; }

        public GuestInboxViewModel(User user, ListBox list, ListBox newList, ListBox ratings, NavigationService userProfille)
        {
            LoggedInUser = user;
            ProfilleNavigationService = userProfille;

            _reservationsForMark = new ObservableCollection<CancelAndMarkResDTO>();
            _changedReservations = new List<CancelAndMarkResDTO>();
            _ratingsFromOwner = new ObservableCollection<CancelAndMarkResDTO>();

            ShowOwnerAnswers();
            ShowGuestRatings();
            PrepareMarkReservationList();
            ShowMarkingNotifications();

            list.ItemsSource = _reservationsForMark;
            newList.ItemsSource = _changedReservations;
            ratings.ItemsSource = _ratingsFromOwner;

            ShowMenuCommand = new RelayCommand(Execute_ShowMenu);
            GoBackCommand = new RelayCommand(Execute_GoBack);
            MarkAccommodationCommand = new RelayCommand(Execute_MarkAccommodation);
        }

        private void Execute_GoBack(object sender)
        {
            NotificationFromOwnerService service = new NotificationFromOwnerService();
            ProfilleNavigationService.Navigate(new UserProfillePage(LoggedInUser, service.TestInboxCharge(LoggedInUser.Id), ProfilleNavigationService));
        }

        private void Execute_ShowMenu(object sender)
        {
            NotificationFromOwnerService service = new NotificationFromOwnerService();
            ProfilleNavigationService.Navigate(new SearchAccommodationPage(LoggedInUser, ProfilleNavigationService, service.TestInboxCharge(LoggedInUser.Id)));
        }

        private void PrepareMarkReservationList()
        {
            AccommodationService accommodationService = new AccommodationService();
            ObservableCollection<LocAccommodationDTO> _locAccommodationViewModels = accommodationService.CreateAllDTOForms();
            
            AccommodationReservationService reservationService = new AccommodationReservationService();
            List<AccommodationReservation> _accommodationReservations = reservationService.GetAll();

            foreach (var reserv in _accommodationReservations)
            {
                int diff = DateTime.Today.DayOfYear - reserv.LastDay.DayOfYear;
                foreach (var lavm in _locAccommodationViewModels)
                {
                    if (IsValidCandidate(reserv, diff, lavm))
                    {
                        CancelAndMarkResDTO crModel = new CancelAndMarkResDTO(lavm.AccommodationName, lavm.LocationCity, lavm.LocationCountry, reserv.FirstDay, reserv.LastDay, reserv.Id, lavm.AccommodationId);
                        _reservationsForMark.Add(crModel);
                           
                    }
                }
            }
        }

        private bool IsValidCandidate(AccommodationReservation reserv, int diff, LocAccommodationDTO lavm)
        {
            return reserv.AccommodationId == lavm.AccommodationId && diff > 0 && !reserv.ReadMarkNotification && reserv.UserId == LoggedInUser.Id;
        }

        private void Execute_MarkAccommodation(object sender)
        {
            CancelAndMarkResDTO? selected = sender as CancelAndMarkResDTO;
            ProfilleNavigationService.Navigate(new MarkAccommodationPage(LoggedInUser, selected, _reservationsForMark, _ratingsFromOwner, ProfilleNavigationService));
        }

        private void ShowMarkingNotifications()
        {
            AccommodationReservationService service = new AccommodationReservationService();
            List<AccommodationReservation> finishedReservations = service.LoadFinishedReservations();
            foreach (var item in finishedReservations)
            {
                int diff = DateTime.Today.DayOfYear - item.LastDay.DayOfYear;
                if (diff > 5)
                {
                    RemoveDeadlineReservation(item.Id);
                } 
                else
                {
                    SetMarkingDays(diff, item.Id);
                }
            }
        }

        private void SetMarkingDays(int days, int resId)
        {
            foreach (var item in _reservationsForMark)
            {
                if (resId == item.ReservationId)
                {
                    int diff = 6 - days;
                    if(diff == 1)
                    {
                        item.NotificationShape += diff.ToString() + " dan. Link za ocjenjivanje :";
                    }
                    else
                    {
                        item.NotificationShape += diff.ToString() + " dana. Link za ocjenjivanje :";
                    }
                    break;
                }
            }
        }

        private void RemoveDeadlineReservation(int resId)
        {
            List<CancelAndMarkResDTO> local = new List<CancelAndMarkResDTO>();
            foreach(var item in _reservationsForMark)
            {
                local.Add(item);
            }
            foreach(var item in local)
            {
                if(resId == item.ReservationId)
                {
                    _reservationsForMark.Remove(item);
                    break;
                }
            }
        }

        private void ShowOwnerAnswers()
        {
            NotificationFromOwnerService service = new NotificationFromOwnerService();
            UserService userService = new UserService();
            List<NotificationFromOwner> localList = service.GetAll();

            if (localList.Count > 0)
            {
                foreach (var item in localList)
                {
                    if (item.GuestId == LoggedInUser.Id)
                    {
                        User user = userService.GetById(item.OwnerId);
                        string shape = "Vlasnik " + user.Username + " je odgovorio na Vaš zahtjev za pomjeranje rezervacije u smještaju " +
                                        item.AccommodationName + " sa: " + item.Answer + ".";
                        CancelAndMarkResDTO model = new CancelAndMarkResDTO();
                        model.NotificationShape = shape;
                        _changedReservations.Add(model);
                        service.Delete(item.Id);
                    }
                }
            }
        }

        private void ShowGuestRatings()
        {
            GuestReviewService reviewService = new GuestReviewService();
            UserService userService = new UserService();
            AccommodationService accommodationService = new AccommodationService();
            AccommodationReservationService reservationService = new AccommodationReservationService();

            List<GuestReview> guestReviews = reviewService.GetAll();

            if(guestReviews.Count > 0)
            {
                foreach(var item in guestReviews)
                {
                    AccommodationReservation reservation = reservationService.GetById(item.ReservationId);
                    if (item.GuestId == LoggedInUser.Id && reservation.ReadMarkNotification)
                    {
                        User owner = userService.GetById(item.OwnerId);
                        Accommodation accommodation = accommodationService.GetById(item.AccommodationId);
                        string shape = "Vlasnik " + owner.Username + " je ocijenio Vašu čistoću u smještaju " + accommodation.Name + 
                                       " u periodu od " + reservation.FirstDay.ToShortDateString() + " do " + reservation.LastDay.ToShortDateString() + 
                                       " ocjenom " + item.CleanlinessGrade + ", a poštovanje pravila sa " + item.RespectGrade + ". ";
                        if (item.Comment.Equals("")) shape += "Nije ostavljao dodatni komentar.";
                        else shape += "Ostavio je i dodatni komentar: " + item.Comment;
                        CancelAndMarkResDTO model = new CancelAndMarkResDTO();
                        model.NotificationShape = shape;
                        _ratingsFromOwner.Add(model);
                    }
                }
            }
        }
    }
}
