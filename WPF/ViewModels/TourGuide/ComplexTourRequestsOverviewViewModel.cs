using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.TourGuide;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class ComplexTourRequestsOverviewViewModel : ViewModel
    {
        private readonly ComplexTourRequestService _complexTourRequestService;

        private ObservableCollection<ComplexTourRequest> _complexTourRequestCards;

        public ObservableCollection<ComplexTourRequest> ComplexTourRequestCards
        {
            get => _complexTourRequestCards;
            set
            {
                if (_complexTourRequestCards != value)
                {
                    _complexTourRequestCards = value;
                    OnPropertyChanged("ComplexTourRequestCards");
                }
            }
        }

        public ComplexTourRequest SelectedComplexTourRequest { get; set; }
        

        public RelayCommand ShowComplexTourCommand { get; set; }

        public ComplexTourRequestsOverviewViewModel()
        {
            _complexTourRequestService = new ComplexTourRequestService();
            _complexTourRequestCards = new ObservableCollection<ComplexTourRequest>();
            CreateComplexTourCards();

            ShowComplexTourCommand = new RelayCommand(ShowComplexTour, CanExecuteMethod);
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }


        private void CreateComplexTourCards()
        {
            foreach (var complexRequest in _complexTourRequestService.GetAllNotAcceptedOnePart())
            {
                ComplexTourRequestCards.Add(complexRequest);
            }
        }

        private void ShowComplexTour(object sender)
        {
            var selectedComplexTourRequest = sender as ComplexTourRequest;
            SelectedComplexTourRequest = selectedComplexTourRequest;

            App.TourGuideNavigationService.AddPreviousPage();
            App.TourGuideNavigationService.SetMainFrame("ComplexTourRequest", App.LoggedUser);

        }

    }
}
