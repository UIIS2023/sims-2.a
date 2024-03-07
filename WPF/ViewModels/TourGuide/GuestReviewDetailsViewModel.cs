using System.Collections.ObjectModel;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class GuestReviewDetailsViewModel : ViewModel
    {
        private GuestReviewCardViewModel _selectedReview;

        public GuestReviewCardViewModel SelectedReview
        {
            get => _selectedReview;
            set
            {
                if (_selectedReview != value)
                {
                    _selectedReview = value;
                    OnPropertyChanged("SelectedReview");
                }
            }
        }

        private ObservableCollection<GuestAttendanceCardViewModel> _guestAttendanceCards;

        public ObservableCollection<GuestAttendanceCardViewModel> GuestAttendanceCards
        {
            get => _guestAttendanceCards;
            set
            {
                if (_guestAttendanceCards != value)
                {
                    _guestAttendanceCards = value;
                    OnPropertyChanged("GuestAttendanceCards");
                }
            }
        }

        private bool _canReport;

        public bool CanReport
        {
            get => _canReport;
            set
            {
                if (_canReport != value)
                {
                    _canReport = value;
                    OnPropertyChanged("CanReport");
                }
            }
        }

        private readonly TourReviewService _tourReviewService;

        public RelayCommand ReportReviewCommand { get; set; }

        public GuestReviewDetailsViewModel(GuestReviewCardViewModel selectedReview)
        {
            _selectedReview = selectedReview;
            var guestAttendanceCardCreator = new GuestAttendanceCardCreatorViewModel();
            GuestAttendanceCards = guestAttendanceCardCreator.CreateCheckpointCards(selectedReview);
            _tourReviewService = new TourReviewService();
            _canReport = !_tourReviewService.GetById(selectedReview.ReviewId).Reported;
            ReportReviewCommand = new RelayCommand(ReportComment, CanExecuteMethod);
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void ReportComment(object sender)
        {
            var tourReview = _tourReviewService.GetById(SelectedReview.ReviewId);
            tourReview.Reported = true;
            _tourReviewService.Update(tourReview);
            CanReport = false;
        }
    }
}
