using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest1;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class Guest1MainViewModel
    {
        public Guest1MainViewModel(User loggedInUser, int notifications, NavigationService navigationService) 
        {
            navigationService.Navigate(new UserProfillePage(loggedInUser, notifications, navigationService));
        }

    }
}
