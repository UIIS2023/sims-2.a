using SOSTeam.TravelAgency.Application.Services;
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
    public class TourDetailsViewModel : ViewModel
    {
        public Tour TourForShowing { get; set; }
        public string TourForShowingLocation { get; set; }
        public string TourForShowingImage { get; set; }

        private readonly TourService _tourService;
        private readonly ImageService _imageService;
        private readonly LocationService _locationService;

        private RelayCommand _closeCommand;

        public RelayCommand CloseCommand
        {
            get { return _closeCommand; }
            set
            {
                _closeCommand = value;
            }
        }
        public TourDetailsViewModel(int tourId)
        {
            _tourService = new TourService();
            _imageService = new ImageService();
            _locationService = new LocationService();
            TourForShowing = _tourService.GetById(tourId);
            TourForShowingLocation = _locationService.GetFullName(_locationService.GetById(TourForShowing.LocationId));
            TourForShowingImage = (_imageService.GetTourCover(tourId)).Path;
            CloseCommand = new RelayCommand(Execute_CloseCommand, CanExecuteMethod);
        }

        private void Execute_CloseCommand(object obj)
        {
            var currentApp = System.Windows.Application.Current;

            foreach (Window window in currentApp.Windows)
            {
                if (window is TourDetailsWindow)
                {
                    window.Close();
                    break;
                }
            }
        }

        private bool CanExecuteMethod(object obj)
        {
            return true;
        }
    }
}
