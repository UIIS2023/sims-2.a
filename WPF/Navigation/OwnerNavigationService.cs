using SOSTeam.TravelAgency.WPF.Views.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace SOSTeam.TravelAgency.WPF.Navigation
{
    public class OwnerNavigationService
    {
        private NavigationService _navigationService;
        private MainWindow _mainWindow;

        public OwnerNavigationService(NavigationService navigationService, MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            _navigationService = navigationService;
        }

        public void NavigateMainWindow(object parameter)
        {
            string nextPage = parameter.ToString();

            switch (nextPage)
            {
                case "Home":
                    _navigationService.Navigate(new HomePage());
                    break;
                case "Accommodation":
                    _navigationService.Navigate(new AccommodationsPage());
                    break;
                case "AccommodationAdd":
                    _navigationService.Navigate(new AddAccommodationPage());
                    break;
                case "Request":
                    _navigationService.Navigate(new RequestPage());
                    break;
                case "Review":
                    _navigationService.Navigate(new OwnerReviewPage());
                    break;
                case "User":
                    _navigationService.Navigate(new UserPage());
                    break;
                case "Renovation":
                    _navigationService.Navigate(new RenovationPage());
                    break;
                case "RenovationAdd":
                    _navigationService.Navigate(new RenovationAddPage());
                    break;
                case "Suggestion":
                    _navigationService.Navigate(new SuggestionPage());
                    break;
                case "Forum":
                    _navigationService.Navigate(new ForumPage());
                    break;
                /*
                
                */

                default:
                    break;
            }
            return;


        }

        public void SetStartupPage()
        {
            NavigateMainWindow("Home");
        }

        internal void SetPage(object root)
        {
            _navigationService.Navigate(root);
        }

        internal void CloseWindow() 
        {
            _mainWindow.Close();
        }
    }
}
