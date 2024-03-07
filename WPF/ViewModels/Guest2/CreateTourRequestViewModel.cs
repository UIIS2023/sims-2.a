using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class CreateTourRequestViewModel : ViewModel
    {
        public NavigationService NavigationService { get; set; }
        public OrdinaryToursPageViewModel OrdinaryToursPageViewModel { get; set; }
        public ComplexToursPageViewModel ComplexToursPageViewModel { get; set; }

        public event EventHandler CloseRequested;
        public User LoggedInUser { get; set; }
        public ObservableCollection<RequestViewModel> TourRequests { get; set; }

        private RequestViewModel _tourRequest;
        public RequestViewModel TourRequest
        {
            get { return _tourRequest; }
            set
            {
                if (_tourRequest != value)
                {
                    _tourRequest = value;
                    OnPropertyChanged(); 
                }
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

        private RelayCommand _reviewCommand;
        public RelayCommand ReviewCommand
        {
            get { return _reviewCommand; }
            set
            {
                _reviewCommand = value;
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

        private RelayCommand _addMoreToursCommand;

        public RelayCommand AddMoreToursCommand
        {
            get { return _addMoreToursCommand; }
            set
            {
                _addMoreToursCommand = value;
            }
        }

        public CreateTourRequestViewModel(User loggedInUser,NavigationService navigationService,ComplexToursPageViewModel complexToursPageViewModel) 
        {
            NavigationService = navigationService;
            ComplexToursPageViewModel = complexToursPageViewModel;
            OrdinaryToursPageViewModel = new OrdinaryToursPageViewModel(loggedInUser, NavigationService);
            LoggedInUser= loggedInUser;
            TourRequest= new RequestViewModel();
            TourRequests = new ObservableCollection<RequestViewModel>();
            ReviewCommand = new RelayCommand(Execute_ReviewCommand, CanExecuteMethod);
            CloseCommand = new RelayCommand(Execute_CloseCommand,CanExecuteMethod);
            HelpCommand = new RelayCommand(Execute_HelpCommand, CanExecuteMethod);
            AddMoreToursCommand = new RelayCommand(Execute_AddMoreToursCommand, CanExecuteMethod);
        }

        private void Execute_HelpCommand(object obj)
        {
            HelpWindow window = new HelpWindow();
            window.Show();
        }

        public CreateTourRequestViewModel(User loggedInUser, NavigationService navigationService, OrdinaryToursPageViewModel ordinaryToursPageViewModel)
        {
            NavigationService = navigationService;
            OrdinaryToursPageViewModel = ordinaryToursPageViewModel;
            ComplexToursPageViewModel = new ComplexToursPageViewModel(loggedInUser);
            LoggedInUser = loggedInUser;
            TourRequest = new RequestViewModel();
            TourRequests = new ObservableCollection<RequestViewModel>();
            ReviewCommand = new RelayCommand(Execute_ReviewCommand, CanExecuteMethod);
            CloseCommand = new RelayCommand(Execute_CloseCommand, CanExecuteMethod);
            HelpCommand = new RelayCommand(Execute_HelpCommand, CanExecuteMethod);
            AddMoreToursCommand = new RelayCommand(Execute_AddMoreToursCommand, CanExecuteMethod);
        }

        private void Execute_AddMoreToursCommand(object obj)
        {
            SetDateFormat();
            if (IsDataCorrect())
            {
                TourRequests.Add(TourRequest);
                TourRequest = new RequestViewModel();
            }
        }

        private void Execute_CloseCommand(object obj)
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void Execute_ReviewCommand(object obj)
        {
            if(TourRequests.Count()!=0)
            {
                NavigationService.Navigate(new TourRequestReviewPage(LoggedInUser, TourRequests, OrdinaryToursPageViewModel, ComplexToursPageViewModel));
            }
            else if (IsDataCorrect())
            {
                SetDateFormat();
                TourRequests.Add(TourRequest);
                NavigationService.Navigate(new TourRequestReviewPage(LoggedInUser, TourRequests, OrdinaryToursPageViewModel, ComplexToursPageViewModel));
            }
        }

        private void SetDateFormat()
        {
            TourRequest.StartEndDateRange = TourRequest.MaintenanceStartDate.ToString() + " - " + TourRequest.MaintenanceEndDate.ToString();
            TourRequest.LocationFullName = TourRequest.City + ", " + TourRequest.Country;
        }

        private bool IsDataCorrect()
        {
            bool isCorrect = true;
            if (string.IsNullOrEmpty(TourRequest.City) || string.IsNullOrEmpty(TourRequest.Country) || string.IsNullOrEmpty(TourRequest.Language) || string.IsNullOrEmpty(TourRequest.Description) || string.IsNullOrEmpty(TourRequest.MaintenanceStartDate) || string.IsNullOrEmpty(TourRequest.MaintenanceEndDate))
            {
                MessageBox.Show("Nisu uneti svi podaci", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
                isCorrect = false;
            }
            else if(TourRequest.MaxNumOfGuests <= 0)
            {
                MessageBox.Show("Broj turista ne moze biti manji ili jednak nuli", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
                isCorrect = false;
            }
            else if(DateTime.Parse(TourRequest.MaintenanceStartDate) > DateTime.Parse(TourRequest.MaintenanceEndDate))
            {
                MessageBox.Show("Datum zavrsetka je pre datuma pocetka", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
                isCorrect = false;
            }
            else if(DateTime.Today >= DateTime.Parse(TourRequest.MaintenanceStartDate).AddDays(-2))
            {
                MessageBox.Show("Vodic ne moze stici da organizuje turu spram vaseg zahteva", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
                isCorrect = false;
            }
            return isCorrect;   
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }
    }
}
