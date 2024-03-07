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
using System.Windows.Navigation;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class OrdinaryToursPageViewModel : ViewModel
    {
        public NavigationService NavigationService { get; set; }
        public static User LoggedInUser { get; set; }
        private ObservableCollection<RequestViewModel> _tourRequests;
        public ObservableCollection<RequestViewModel> TourRequests
        {
            get { return _tourRequests; }
            set
            {
                if (value != _tourRequests)
                {
                    _tourRequests = value;
                    OnPropertyChanged();
                }
            }
        }

        private readonly TourRequestService _tourRequestService;

        private RelayCommand _statisticsCommand;

        public RelayCommand StatisticsCommand
        {
            get { return _statisticsCommand; }
            set
            {
                _statisticsCommand = value;
            }
        }

        private RelayCommand _createCommand;

        public RelayCommand CreateCommand
        {
            get { return _createCommand; }
            set
            {
                _createCommand = value;
            }
        }

        public OrdinaryToursPageViewModel(User loggedInUser, NavigationService navigationService)
        {
            LoggedInUser = loggedInUser;
            _tourRequestService = new TourRequestService();
            StatisticsCommand = new RelayCommand(Execute_StatisticsCommand, CanExecuteMethod);
            CreateCommand = new RelayCommand(Execute_CreateCommand, CanExecuteMethod);
            TourRequests = new ObservableCollection<RequestViewModel>();
            UpdateTourRequests();
            FillTourRequests();
            NavigationService = navigationService;
        }

        public void FillTourRequests()
        {
            foreach(var request in _tourRequestService.GetAll())
            {
                if(request.UserId == LoggedInUser.Id && request.ComplexTourRequestId == -1)
                {
                    TourRequests.Add(new RequestViewModel(LoggedInUser.Id, request.City, request.Country, request.Description, request.Language, request.MaxNumOfGuests, request.MaintenanceStartDate, request.MaintenanceEndDate, request.Status));
                }
            }
        }

        private void UpdateTourRequests()
        {
            foreach(var request in _tourRequestService.GetAll())
            {
                if(DateOnly.FromDateTime(DateTime.Today) >= request.MaintenanceStartDate.AddDays(-2) && request.Status == StatusType.ON_HOLD)
                {
                    request.Status = StatusType.INVALID;
                    _tourRequestService.Update(request);
                }
            }
        }
        private void Execute_StatisticsCommand(object obj)
        {
            NavigationService.Navigate(new StatisticsPage(LoggedInUser,this));
        }

        private void Execute_CreateCommand(object obj)
        {
            CreateTourRequestWindow window = new CreateTourRequestWindow(LoggedInUser,this);
            window.ShowDialog();
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }
    }
}
