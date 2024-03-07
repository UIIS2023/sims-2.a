using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.TourGuide;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class StatsMenuViewModel
    {
        public RelayCommand ShowToursOverviewStatsCommand { get; set; }

        public RelayCommand ShowStatByToursCommand { get; set; }
        public RelayCommand ShowTourRequestStatsCommand { get; set; }

        public User LoggedUser { get; set; }

        public StatsMenuViewModel(User loggedUser)
        {
            LoggedUser = loggedUser;
            ShowToursOverviewStatsCommand = new RelayCommand(ShowToursOverviewStats, CanExecuteMethod);
            ShowStatByToursCommand = new RelayCommand(ShowStatByTours, CanExecuteMethod);
            ShowTourRequestStatsCommand = new RelayCommand(ShowTourRequestStats, CanExecuteMethod);
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void ShowToursOverviewStats(object sender)
        {
            App.TourGuideNavigationService.AddPreviousPage();
            App.TourGuideNavigationService.SetMainFrame("StatsOverview", App.LoggedUser);
        }

        private void ShowStatByTours(object sender)
        {
            App.TourGuideNavigationService.AddPreviousPage();
            App.TourGuideNavigationService.SetMainFrame("StatsByTourOverviewTourOverview", App.LoggedUser);
            
        }

        private void ShowTourRequestStats(object sender)
        {
            App.TourGuideNavigationService.AddPreviousPage();
            App.TourGuideNavigationService.SetMainFrame("TourRequestStats", App.LoggedUser);
        }

    }
}
