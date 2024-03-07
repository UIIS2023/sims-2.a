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

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class TourRequestReviewPageViewModel : ViewModel
    {
        public OrdinaryToursPageViewModel OrdinaryToursPageViewModel { get; set; }
        public ComplexToursPageViewModel ComplexToursPageViewModel { get; set; }
        public ObservableCollection<RequestViewModel> TourRequests { get; set; }
        public User LoggedInUser { get; set; }

        private readonly TourRequestService _tourRequestService;
        private readonly ComplexTourRequestService _complexTourRequestService;

        private RelayCommand _createReviewCommand;
        public RelayCommand CreateReviewCommand
        {
            get { return _createReviewCommand; }
            set
            {
                _createReviewCommand = value;
            }
        }

        private RelayCommand _closeCommand;

        public RelayCommand CloseCommand
        {
            get { return _closeCommand; }
            set
            {
                _closeCommand = value;
            }
        }
        public TourRequestReviewPageViewModel(User loggedInUser, ObservableCollection<RequestViewModel> tourRequests, OrdinaryToursPageViewModel ordinaryToursPageViewModel, ComplexToursPageViewModel complexToursPageViewModel)
        {
            OrdinaryToursPageViewModel = ordinaryToursPageViewModel;
            ComplexToursPageViewModel = complexToursPageViewModel;
            TourRequests = tourRequests;
            LoggedInUser = loggedInUser;
            _tourRequestService= new TourRequestService();
            _complexTourRequestService = new ComplexTourRequestService();
            CreateReviewCommand = new RelayCommand(Execute_CreateReviewCommand, CanExecuteMethod);
            CloseCommand = new RelayCommand(Execute_CloseCommand,CanExecuteMethod);
        }

        private void Execute_CloseCommand(object obj)
        {
            CloseWindow();
        }

        private static void CloseWindow()
        {
            var currentApp = System.Windows.Application.Current;

            foreach (Window window in currentApp.Windows)
            {
                if (window is CreateTourRequestWindow)
                {
                    window.Close();
                }
            }
        }

        private void Execute_CreateReviewCommand(object obj)
        {
            MessageBoxResult result = ConfirmRequestCreation();
            if (result == MessageBoxResult.Yes)
            {
                if(TourRequests.Count == 1)
                {
                    TourRequest tourRequest = new TourRequest(TourRequests[0].City, TourRequests[0].Country, TourRequests[0].Description, TourRequests[0].Language, TourRequests[0].MaxNumOfGuests,DateTime.Now, DateOnly.Parse(TourRequests[0].MaintenanceStartDate), DateOnly.Parse(TourRequests[0].MaintenanceEndDate),StatusType.ON_HOLD,LoggedInUser.Id);
                    _tourRequestService.Save(tourRequest);
                    OrdinaryToursPageViewModel.TourRequests.Clear();
                    OrdinaryToursPageViewModel.FillTourRequests();
                    MessageBox.Show("Uspesno ste kreirali zahtev", "Kreiranje zahteva", MessageBoxButton.OK, MessageBoxImage.Information);
                    CloseWindow();
                }
                else if(TourRequests.Count != 0)
                {
                    ComplexTourRequest complexTourRequest = new ComplexTourRequest(LoggedInUser.Id);
                    _complexTourRequestService.Save(complexTourRequest);
                    AddRequests(complexTourRequest);
                    ComplexToursPageViewModel.ComplexTourRequests.Clear();
                    ComplexToursPageViewModel.FillComplexRequests();
                    MessageBox.Show("Uspesno ste kreirali zahtev", "Kreiranje zahteva", MessageBoxButton.OK, MessageBoxImage.Information);
                    CloseWindow();
                }
            }
        }

        private void AddRequests(ComplexTourRequest complexTourRequest)
        {
            foreach (var request in TourRequests)
            {
                TourRequest tourRequest = new TourRequest(request.City, request.Country, request.Description, request.Language, request.MaxNumOfGuests, DateTime.Now, DateOnly.Parse(request.MaintenanceStartDate), DateOnly.Parse(request.MaintenanceEndDate), StatusType.ON_HOLD, LoggedInUser.Id,false,complexTourRequest.Id);
                _tourRequestService.Save(tourRequest);
            }
        }

        private MessageBoxResult ConfirmRequestCreation()
        {
            string sMessageBoxText = $"Da li ste sigurni da želite da kreirate zahtev";
            string sCaption = "Porvrda";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }
        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }
    }
}
