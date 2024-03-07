using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class RequestsMenuViewModel
    {
        public RelayCommand ShowRegularRequestsCommand { get; set; }
        public RelayCommand ShowComplexTourRequestsCommand { get; set; }

        public RequestsMenuViewModel()
        {
            ShowRegularRequestsCommand = new RelayCommand(ShowRegularRequests, CanExecuteMethod);
            ShowComplexTourRequestsCommand = new RelayCommand(ShowComplexTourRequests, CanExecuteMethod);
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void ShowRegularRequests(object parameter)
        {
            App.TourGuideNavigationService.AddPreviousPage();
            App.TourGuideNavigationService.SetMainFrame("RegularRequests", App.LoggedUser);
        }

        private void ShowComplexTourRequests(object sender)
        {
            App.TourGuideNavigationService.AddPreviousPage();
            App.TourGuideNavigationService.SetMainFrame("ComplexTourRequestsOverview", App.LoggedUser);
        }

    }
}
