using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class NewToursViewModel : ViewModel
    {
        public User LoggedInUser { get; set; }
        public int TourId { get; set; }
        public string TextForShowing { get; set; }

        private RelayCommand _detailsCommand;

        public RelayCommand DetailsCommand
        {
            get { return _detailsCommand; }
            set
            {
                _detailsCommand = value;
            }
        }

        public NewToursViewModel(User loggedInUser, int tourId, string tourName)
        {
            LoggedInUser= loggedInUser;
            TourId= tourId;
            TextForShowing = "Tura " + tourName + " je napravljena spram nekog od Vasih neispunjenih zahteva";
            DetailsCommand = new RelayCommand(Execute_DetailsCommand,CanExecuteMethod);
        }

        private void Execute_DetailsCommand(object obj)
        {
            TourDetailsWindow window = new TourDetailsWindow(TourId);
            window.ShowDialog();
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }
    }
}
