using System;
using System.Collections.ObjectModel;
using System.Linq;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using GuestReviewPage = SOSTeam.TravelAgency.WPF.Views.TourGuide.GuestReviewPage;
using MainWindow = SOSTeam.TravelAgency.WPF.Views.TourGuide.MainWindow;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class GuestReviewsOverviewViewModel : ViewModel
    {
        private ObservableCollection<GuestReviewCardViewModel> _guestReviewCards;

        public ObservableCollection<GuestReviewCardViewModel> GuestReviewCards
        {
            get => _guestReviewCards;
            set
            {
                if (_guestReviewCards != value)
                {
                    _guestReviewCards = value;
                    OnPropertyChanged("GuestReviewCards");
                }
            }
        }

        public string TourName { get; set; }
        public DateTime Date { get; set; }
        public RelayCommand ShowReviewDetailsCommand { get; set; }

        public GuestReviewsOverviewViewModel(TourCardViewModel selectedTour)
        {
            var guestReviewCardCreator = new GuestReviewCardCreatorViewModel();
            GuestReviewCards = guestReviewCardCreator.CreateCards(selectedTour); 

            TourName = selectedTour.Name;
            Date = selectedTour.Start;
            ShowReviewDetailsCommand = new RelayCommand(ShowGuestReviewDetails, CanExecuteMethod);
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void ShowGuestReviewDetails(object sender)
        {
            var selectedAppointment = sender as GuestReviewCardViewModel;
            GuestReviewPage guestReviewPage = new GuestReviewPage(selectedAppointment);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().MainFrame.Content = guestReviewPage;
        }
    }
}
