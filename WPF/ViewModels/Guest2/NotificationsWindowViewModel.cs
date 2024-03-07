using Microsoft.Win32;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class NotificationsWindowViewModel : ViewModel
    {
        public event EventHandler CloseRequested;
        public static User LoggedInUser { get; set; }
        public NavigationService NotificationNavigationService { get; set; }
        private RelayCommand _backCommand;

        public RelayCommand BackCommand
        {
            get { return _backCommand; }
            set
            {
                _backCommand = value;
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

        private RelayCommand _navigationCommand;
        public RelayCommand NavigationCommand
        {
            get { return _navigationCommand; }
            set
            {
                _navigationCommand = value;
            }
        }
        public NotificationsWindowViewModel(User loggedInUser,NavigationService notificationNavigationService) 
        {
            NotificationNavigationService = notificationNavigationService;
            LoggedInUser = loggedInUser;
            BackCommand = new RelayCommand(Execute_CancelCommand, CanExecuteMethod);
            HelpCommand = new RelayCommand(Execute_HelpCommand, CanExecuteMethod);
            NavigationCommand = new RelayCommand(Execute_NavigationCommand, CanExecuteMethod);
        }

        public void SetStartupPage()
        {
            Execute_NavigationCommand("FinishedTour");
        }
        private void Execute_NavigationCommand(object obj)
        {
            string nextPage = obj.ToString();

            switch (nextPage)
            {
                case "FinishedTour":
                    NotificationNavigationService.Navigate(new FinishedToursNotificationPage(LoggedInUser));
                    break;
                case "NewTour":
                    NotificationNavigationService.Navigate(new NewToursNotificationPage(LoggedInUser));
                    break;
            }
        }    

        private void Execute_HelpCommand(object obj)
        {
            HelpWindow window = new HelpWindow();
            window.Show();
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void Execute_CancelCommand(object sender)
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
