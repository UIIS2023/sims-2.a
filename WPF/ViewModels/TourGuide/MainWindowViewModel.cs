using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.TourGuide;
using SOSTeam.TravelAgency.WPF.Views.TourGuide.DemoPages;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class MainWindowViewModel : ViewModel
    {
        private readonly DispatcherTimer _timer;
        public DispatcherTimer Timer => _timer;
        
        private readonly User _loggedUser;

        private string _title;

        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged("Title");
                }
            }
        }

        private string _username;

        public string Username
        {
            get => _username;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged("Username");
                }
            }
        }

        #region Services

        private readonly AppointmentService _appointmentService;
        private readonly CreateSuperGuideService _createSuperGuideService;

        #endregion

        #region Commands
        public RelayCommand ShowCreateTourFormCommand { get; set; }
        public RelayCommand ShowHomePageCommand { get; set; }
        public RelayCommand ShowLiveTourCommand { get; set; }

        //Burger menu commands
        public RelayCommand ShowTourReviewCommand { get; set; }
        public RelayCommand ShowStatsMenuCommand { get; set; }
        public RelayCommand ShowRequestsMenuCommand { get; set; }
        public RelayCommand NavigationButtonCommand { get; set; }
        public RelayCommand ShowUserAccountCommand { get; set; }
        public RelayCommand ShowGeneratePDFReportCommand { get; set; }
        public RelayCommand LogOutCommand { get; set; }
        public RelayCommand ShowDemoCommand { get; set; }

        #endregion

        public MainWindowViewModel(User loggedUser)
        {
            _loggedUser = loggedUser;
            _appointmentService = new AppointmentService();
            _createSuperGuideService = new CreateSuperGuideService();
            _title = "Home";
            _username = loggedUser.Username;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(1)
            };
            Timer.Tick += UpdateAppointments;
            Timer.Start();

            NavigationButtonCommand = new RelayCommand(ExecuteNavigationButtonCommand, CanExecuteMethod);
            ShowCreateTourFormCommand = new RelayCommand(ShowCreateTourForm, CanExecuteMethod);
            ShowHomePageCommand = new RelayCommand(ShowHomePage, CanExecuteMethod);
            ShowLiveTourCommand = new RelayCommand(ShowLiveTourPage, CanExecuteMethod);
            ShowTourReviewCommand = new RelayCommand(ShowTourReviews, CanExecuteMethod);
            ShowStatsMenuCommand = new RelayCommand(ShowStatsMenu, CanExecuteMethod);
            ShowRequestsMenuCommand = new RelayCommand(ShowRequestsMenu, CanExecuteMethod);
            ShowUserAccountCommand = new RelayCommand(ShowUserAccount, CanExecuteMethod);
            ShowGeneratePDFReportCommand = new RelayCommand(ShowGeneratePDFReport, CanExecuteMethod);
            LogOutCommand = new RelayCommand(LogOut, CanExecuteMethod);
            ShowDemoCommand = new RelayCommand(ShowDemo, CanExecuteMethod);
        }

        private void UpdateAppointments(object sender, EventArgs e)
        {
            _appointmentService.SetExpiredAppointments(_loggedUser.Id);
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        void ShowCreateTourForm(object sender)
        {
            App.TourGuideNavigationService.AddPreviousPage();
            App.TourGuideNavigationService.SetMainFrame("CreateTourPage", _loggedUser);
        }

        private void ShowHomePage(object sender)
        {
            App.TourGuideNavigationService.AddPreviousPage();
            App.TourGuideNavigationService.SetMainFrame("HomePage", _loggedUser);
        }

        private void ShowLiveTourPage(object sender)
        {
            App.TourGuideNavigationService.AddPreviousPage();
            App.TourGuideNavigationService.SetMainFrame("LiveTour", _loggedUser);
        }

        private void ShowTourReviews(object sender)
        {
            App.TourGuideNavigationService.AddPreviousPage();
            App.TourGuideNavigationService.SetMainFrame("Reviews", _loggedUser);
        }

        private void ShowStatsMenu(object sender)
        {
            App.TourGuideNavigationService.AddPreviousPage();
            App.TourGuideNavigationService.SetMainFrame("Stats", _loggedUser);
        }

        private void ShowRequestsMenu(object sender)
        {
            App.TourGuideNavigationService.AddPreviousPage();
            App.TourGuideNavigationService.SetMainFrame("Requests", _loggedUser);
        }

        private void ExecuteNavigationButtonCommand(object parameter)
        {
            var button = parameter as string;
            App.TourGuideNavigationService.GoBack(button);
        }

        private void ShowUserAccount(object sender)
        {
            _createSuperGuideService.CreateSuperTourGuide();
            App.TourGuideNavigationService.AddPreviousPage();
            App.TourGuideNavigationService.SetMainFrame("UserAccount", _loggedUser);
        }

        private void ShowGeneratePDFReport(object sender)
        {
            App.TourGuideNavigationService.AddPreviousPage();
            App.TourGuideNavigationService.SetMainFrame("GeneratePDFReport", _loggedUser);
        }

        private void LogOut(object sender)
        {
            App.TourGuideNavigationService.LogOut();
        }

        private void ShowDemo(object sender)
        {
            App.TourGuideNavigationService.SetMainFrame("CreateTourDemo", _loggedUser);
            
            //var currentWindow = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            //var mainWindow = (MainWindow)currentWindow;

            //mainWindow.MainFrame.Content = new CreateTourDemoPage();
            //mainWindow.MainFrame.Content = new AddCheckpointsDemoPage();
            //mainWindow.MainFrame.Content = new AddAppointmentsDemoPage();
        }

    }
}
