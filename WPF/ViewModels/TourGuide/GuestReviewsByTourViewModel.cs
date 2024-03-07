using System.Collections.ObjectModel;
using System.Linq;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.TourGuide;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class GuestReviewsByTourViewModel : ViewModel
    {
        private ObservableCollection<TourCardViewModel> _tourCards;

        public ObservableCollection<TourCardViewModel> TourCards
        {
            get => _tourCards;
            set
            {
                if (_tourCards != value)
                {
                    _tourCards = value;
                    OnPropertyChanged("TourCards");
                }
            }
        }

        public RelayCommand ShowGuestReviewsCommand { get; set; }

        public GuestReviewsByTourViewModel(User loggedUser)
        {
            var tourCardCreator = new TourCardCreatorViewModel();
            TourCards = tourCardCreator.CreateCards(loggedUser, CreationType.FINISHED);

            ShowGuestReviewsCommand = new RelayCommand(ShowGuestReviews, CanExecuteMethod);
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void ShowGuestReviews(object sender)
        {
            var selectedTourCard = sender as TourCardViewModel;
            var guestReviewOverviewPage = new GuestReviewOverviewPage(selectedTourCard);
            System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault().MainFrame.Content = guestReviewOverviewPage;
        }
    }
}
