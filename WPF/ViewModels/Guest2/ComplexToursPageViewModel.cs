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

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class ComplexToursPageViewModel : ViewModel
    {
        public static User LoggedInUser { get; set; }

        private ObservableCollection<ComplexTourRequestViewModel> _complexTourRequests;

        private readonly TourRequestService _tourRequestService;
        private readonly ComplexTourRequestService _complexTourRequestService;
        public int Counter { get; set; }
        public ObservableCollection<ComplexTourRequestViewModel> ComplexTourRequests
        {
            get { return _complexTourRequests; }
            set
            {
                if(value != _complexTourRequests)
                {
                    _complexTourRequests = value;
                    OnPropertyChanged();
                }
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

        public ComplexToursPageViewModel(User loggedInUser)
        {
            LoggedInUser= loggedInUser;
            _tourRequestService= new TourRequestService();
            _complexTourRequestService= new ComplexTourRequestService();
            ComplexTourRequests = new ObservableCollection<ComplexTourRequestViewModel>();
            CreateCommand = new RelayCommand(Execute_CreateCommand, CanExecuteMethod);
            UpdateInvalidStatus();
            UpdateAcceptedStatus();
            FillComplexRequests();
        }

        private void UpdateAcceptedStatus()
        {
            int counter = 0;
            foreach (var complexRequest in _complexTourRequestService.GetAll())
            {
                foreach (var request in _tourRequestService.GetComplexRequestParts(complexRequest.Id))
                {
                    if(request.Status == StatusType.ACCEPTED && request.UserId == LoggedInUser.Id) counter++;
                    if(counter == _tourRequestService.GetComplexRequestParts(complexRequest.Id).Count)
                    {
                        complexRequest.Status = StatusType.ACCEPTED;
                        _complexTourRequestService.Update(complexRequest);
                    }
                }
            }
        }

        private void UpdateInvalidStatus()
        {
            foreach(var complexRequest in _complexTourRequestService.GetAll())
            {
                foreach(var request in _tourRequestService.GetComplexRequestParts(complexRequest.Id))
                {
                    UpdateInvalidRequests(complexRequest, request);
                }
            }
        }

        private void UpdateInvalidRequests(ComplexTourRequest complexRequest, TourRequest request)
        {
            if (request.Status == StatusType.INVALID)
            {
                complexRequest.Status = StatusType.INVALID;
                _complexTourRequestService.Update(complexRequest);
            }
        }

        public void FillComplexRequests()
        {
            foreach(var complexRequest in _complexTourRequestService.GetAll())
            {
                if(complexRequest.UserId == LoggedInUser.Id)
                {
                    Counter++;
                    ObservableCollection<RequestViewModel> complexTourRequestParts = FillComplexRequestPartsList(complexRequest);
                    ComplexTourRequests.Add(new ComplexTourRequestViewModel(LoggedInUser.Id, complexRequest.Status, complexTourRequestParts, Counter));
                }
            }
        }

        private ObservableCollection<RequestViewModel> FillComplexRequestPartsList(ComplexTourRequest complexTourRequest)
        {
            ObservableCollection<RequestViewModel> complexTourRequestParts = new ObservableCollection<RequestViewModel>();
            foreach (var request in _tourRequestService.GetAll())
            {
                if(request.ComplexTourRequestId == complexTourRequest.Id)
                {
                    complexTourRequestParts.Add(new RequestViewModel(LoggedInUser.Id, request.City, request.Country, request.Description, request.Language, request.MaxNumOfGuests, request.MaintenanceStartDate, request.MaintenanceEndDate, request.Status));
                }
            }
            return complexTourRequestParts;
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
