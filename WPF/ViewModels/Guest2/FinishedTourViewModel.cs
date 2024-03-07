using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class FinishedTourViewModel : ViewModel
    {
        public User LoggedInUser { get; set; }
        public int AppointmentId { get; set; }
        public int ReservationId { get; set; }
        public string TextForShowing { get; set; }

        private RelayCommand _reviewCommand;

        public RelayCommand ReviewCommand
        {
            get { return _reviewCommand; }
            set
            {
                _reviewCommand = value;
            }
        }

        public FinishedTourViewModel(int reservationId, int appointmentId,User loggedInUser, string tourName)
        {
            LoggedInUser = loggedInUser;
            ReservationId = reservationId;
            AppointmentId = appointmentId;
            TextForShowing = "Tura " + tourName + " je zavrsena,\r\n mozete oceniti turu i vodica";
            ReviewCommand = new RelayCommand(Execute_ReviewPageCommand, CanExecuteMethod);
        }

        private void Execute_ReviewPageCommand(object obj)
        {
            var currentApp = System.Windows.Application.Current;

            foreach (Window window in currentApp.Windows)
            {
                if (window is NotificationsWindow)
                {
                    var navigationService = ((NotificationsWindow)window).ReviewFrame.NavigationService;
                    navigationService.Navigate(new ReviewPage(LoggedInUser, AppointmentId, ReservationId));
                    ((NotificationsWindow)window).MainFrame.Navigate(null);
                    break;
                }
            }
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }
    }
}
