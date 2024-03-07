using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest2;
using SOSTeam.TravelAgency.WPF.Views.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class MyToursPageViewModel : ViewModel
    {
        public NavigationService NavigationService { get; set; }
        private RelayCommand _backCommand;
        public static User LoggedInUser { get; set; }
        public RelayCommand BackCommand
        {
            get { return _backCommand; }
            set
            {
                _backCommand = value;
            }
        }

        private RelayCommand _navigationCommand;
        public RelayCommand NavigationCommand
        {
            get { return _navigationCommand; }
            set
            {
                _navigationCommand = value;
            }
        }

        private RelayCommand _helpCommand;

        public RelayCommand HelpCommand
        {
            get { return _helpCommand; }
            set
            {
                _helpCommand = value;
            }
        }
        public MyToursPageViewModel(User loggedInUser, NavigationService navigationService) 
        {
            BackCommand = new RelayCommand(Execute_BackCommand,CanExecuteMethod);
            NavigationCommand = new RelayCommand(Execute_NavigationCommand, CanExecuteMethod);
            HelpCommand = new RelayCommand(Execute_HelpCommand, CanExecuteMethod);
            LoggedInUser = loggedInUser;
            NavigationService= navigationService;
        }

        private void Execute_HelpCommand(object obj)
        {
            HelpWindow helpWindow = new HelpWindow();
            helpWindow.Show();
        }

        public void SetStartupPage()
        {
            Execute_NavigationCommand("ActiveTour");
        }
        private void Execute_NavigationCommand(object obj)
        {
            string nextPage = obj.ToString();

            switch(nextPage)
            {
                case "ActiveTour":
                    NavigationService.Navigate(new ActiveTourPage(LoggedInUser));
                    break;
                case "Reservations":
                    NavigationService.Navigate(new MyReservationsPage(LoggedInUser));
                    break;
            }
        }

        private void Execute_BackCommand(object obj)
        {
            var currentApp = System.Windows.Application.Current;

            foreach (Window window in currentApp.Windows)
            {
                if (window is ToursOverviewWindow)
                {
                    ToursOverviewWindow mainWindow = new ToursOverviewWindow(LoggedInUser);
                    mainWindow.Show();
                    window.Close();
                }
            }
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }
    }
}
