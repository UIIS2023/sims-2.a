using SOSTeam.TravelAgency.WPF.Creators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class ComplexTourRequestViewModel: ViewModel
    {
        private ObservableCollection<TourRequestCardViewModel> _tourRequestCards;

        public ObservableCollection<TourRequestCardViewModel> TourRequestCards
        {
            get => _tourRequestCards;
            set
            {
                if (_tourRequestCards != value)
                {
                    _tourRequestCards = value;
                    OnPropertyChanged("TourRequestCards");
                }
            }
        }

        public RelayCommand ShowAcceptPartOfComplexTourCommand { get; set; }

        public ComplexTourRequestViewModel(ComplexTourRequest selectedComplexTourRequest)
        {
            var tourRequestCardCreator = new TourRequestCardCreatorViewModel();
            _tourRequestCards = tourRequestCardCreator.CreateComplexTourRequestCards(selectedComplexTourRequest.Id);
            ShowAcceptPartOfComplexTourCommand = new RelayCommand(ShowAcceptPartOfComplexTour, CanExecuteMethod);
        }

        public TourRequestCardViewModel SelectedTourRequestCard { get; set; }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void ShowAcceptPartOfComplexTour(object sender)
        {
            SelectedTourRequestCard = sender as TourRequestCardViewModel;
            App.TourGuideNavigationService.AddPreviousPage();
            App.TourGuideNavigationService.SetMainFrame("AcceptPartOfComplexTour", App.LoggedUser);
        }
    }
}
