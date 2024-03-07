using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using SOSTeam.TravelAgency.WPF.Views.TourGuide;
using System.Linq;
using SOSTeam.TravelAgency.WPF.ViewModels.TourGuide;
using System.Windows.Controls;
using System.Windows.Threading;
using SOSTeam.TravelAgency.Domain.Models;
using System.Windows.Navigation;
using System.Reflection.Metadata;
using SOSTeam.TravelAgency.WPF.Views.TourGuide;
using SOSTeam.TravelAgency.WPF.Views.TourGuide.DemoPages;

namespace SOSTeam.TravelAgency.WPF.Navigation
{
    public class TourGuideNavigationService
    {
        private MainWindow _mainWindow;
        public MainWindow MainWindow { get => _mainWindow; set => _mainWindow = value; }

        private MainWindowViewModel _mainWindowViewModel;
        private Stack<Page> _previousPages;
        public Stack<Page> PreviousPages
        {
            get => _previousPages; set => _previousPages = value;
        }

        private readonly TourCardCreatorViewModel _tourCardCreatorViewModel;
        public TourGuideNavigationService()
        {
            _mainWindow = SetMainWindow();
            _mainWindowViewModel = (MainWindowViewModel)_mainWindow.DataContext;
            _previousPages = new Stack<Page>();
            _tourCardCreatorViewModel = new TourCardCreatorViewModel();
        }

        private MainWindow SetMainWindow()
        {
            var currentWindow = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            
            var mainWindow = (MainWindow)currentWindow;

            return mainWindow;
        }

        public void LogOut()
        {
            var currentWindow = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            var signInWindow = new WPF.Views.TourGuide.SignInWindow();
            currentWindow.Close();

            
            signInWindow.Show();
        }

        public void SetMainFrame(string frameName, User loggedUser)
        {
            if (frameName == "CreateTourPage")
            {
                _mainWindowViewModel.Title = "Create tour";
                MainWindow.DataContext = _mainWindowViewModel;
                _mainWindow.MainFrame.Content = new CreateTourPage(loggedUser);
            }
            else if (frameName == "HomePage")
            {
                _mainWindowViewModel.Title = "Home";
                MainWindow.DataContext = _mainWindowViewModel;
                _mainWindow.MainFrame.Content = new HomePage(loggedUser, _mainWindowViewModel.Timer);
            }
            else if (frameName == "LiveTour")
            {
                _mainWindowViewModel.Title = "Live tour";
                MainWindow.DataContext = _mainWindowViewModel;
                _mainWindow.MainFrame.Content = new LiveTourPage(loggedUser);
            }
            else if (frameName == "Reviews")
            {
                _mainWindowViewModel.Title = "Reviews";
                MainWindow.DataContext = _mainWindowViewModel;
                _mainWindow.MainFrame.Content = new GuestReviewsByTourPage(loggedUser);
            }
            else if (frameName == "Stats")
            {
                _mainWindowViewModel.Title = "Stats";
                MainWindow.DataContext = _mainWindowViewModel;
                _mainWindow.MainFrame.Content = new StatsMenuPage(loggedUser);
            }
            else if (frameName == "StatsOverview")
            {
                _mainWindowViewModel.Title = "Tour stats";
                MainWindow.DataContext = _mainWindowViewModel;
                _mainWindow.MainFrame.Content = new StatsOverviewPage(App.LoggedUser);
            }
            else if (frameName == "StatsByTourOverviewTourOverview")
            {
                _mainWindowViewModel.Title = "Tour stats";
                MainWindow.DataContext = _mainWindowViewModel;
                _mainWindow.MainFrame.Content = new StatsByTourOverviewPage(App.LoggedUser);
            }
            else if (frameName == "TourRequestStats")
            {
                _mainWindowViewModel.Title = "Tour stats";
                MainWindow.DataContext = _mainWindowViewModel;
                _mainWindow.MainFrame.Content = new TourRequestStatsPage();
            }
            else if (frameName == "StatByTour")
            {
                _mainWindowViewModel.Title = "Tour stats";
                MainWindow.DataContext = _mainWindowViewModel;
                var statsByTourOverviewTourOverviewPage = MainWindow.MainFrame.Content as StatsByTourOverviewPage;
                var statsByTourOverviewTourOverviewViewModel =
                    (StatsPerTourOverviewViewModel)statsByTourOverviewTourOverviewPage.DataContext;
                _mainWindow.MainFrame.Content =
                    new StatByTourPage(statsByTourOverviewTourOverviewViewModel.SelectedTourCard);

            }
            else if(frameName == "Requests")
            {
                _mainWindowViewModel.Title = "Requests";
                MainWindow.DataContext = _mainWindowViewModel;
                _mainWindow.MainFrame.Content = new RequestMenuPage();
            }
            else if (frameName == "RegularRequests")
            {
                _mainWindowViewModel.Title = "Requests";
                MainWindow.DataContext = _mainWindowViewModel;
                _mainWindow.MainFrame.Content = new TourRequestPage();
            }
            else if (frameName == "TodayTours")
            {
                _mainWindowViewModel.Title = "Today tours";
                MainWindow.DataContext = _mainWindowViewModel;
                _mainWindow.MainFrame.Content = new TodayToursPage(loggedUser, _mainWindowViewModel.Timer);
            }
            else if (frameName == "AddCheckpoints")
            {
                var previousPage = PreviousPages.Pop();
                if (previousPage is CreateTourPage)
                {
                    _mainWindowViewModel.Title = "Create tour";
                    MainWindow.DataContext = _mainWindowViewModel;
                    var createTourPage = MainWindow.MainFrame.Content as CreateTourPage;
                    var createTourViewModel = (CreateTourViewModel)createTourPage.DataContext;
                    _mainWindow.MainFrame.Content = new AddCheckpointsPage(createTourViewModel.CheckpointCards);
                }
                else if(previousPage is AcceptTourRequestPage)
                {
                    _mainWindowViewModel.Title = "Accept tour";
                    MainWindow.DataContext = _mainWindowViewModel;
                    var acceptRequestTourPage = MainWindow.MainFrame.Content as AcceptTourRequestPage;
                    var acceptRequestTouModel = (AcceptTourRequestViewModel)acceptRequestTourPage.DataContext;
                    _mainWindow.MainFrame.Content = new AddCheckpointsPage(acceptRequestTouModel.CheckpointCards);
                }
                else if(previousPage is CreateSuggestedTourPage)
                {
                    _mainWindowViewModel.Title = "Create suggested tour";
                    MainWindow.DataContext = _mainWindowViewModel;
                    var createSuggestedTourPage = MainWindow.MainFrame.Content as CreateSuggestedTourPage;
                    var createSuggestedViewModel = (CreateSuggestedTourViewModel)createSuggestedTourPage.DataContext;
                    _mainWindow.MainFrame.Content = new AddCheckpointsPage(createSuggestedViewModel.CheckpointCards);

                }
                else if (previousPage is AcceptPartOfComplexTourRequestPage)
                {
                    _mainWindowViewModel.Title = "Accept tour part";
                    MainWindow.DataContext = _mainWindowViewModel;
                    var acceptPartOfComplexTourPage =
                        MainWindow.MainFrame.Content as AcceptPartOfComplexTourRequestPage;
                    var acceptPartOfComplexViewModel =
                        (AcceptPartOfComplexTourRequestViewModel)acceptPartOfComplexTourPage.DataContext;
                    _mainWindow.MainFrame.Content = new AddCheckpointsPage(acceptPartOfComplexViewModel.CheckpointCards);
                }
                PreviousPages.Push(previousPage);
            }
            else if (frameName == "AddAppointments")
            {
                var previousPage = PreviousPages.Pop();

                if (previousPage is CreateTourPage)
                {
                    _mainWindowViewModel.Title = "Create tour";
                    MainWindow.DataContext = _mainWindowViewModel;
                    var createTourPage = MainWindow.MainFrame.Content as CreateTourPage;
                    var createTourViewModel = (CreateTourViewModel)createTourPage.DataContext;
                    _mainWindow.MainFrame.Content = new AddAppointmentsPage(createTourViewModel.AppointmentCards);
                }
                else if (previousPage is CreateSuggestedTourPage)
                {
                    _mainWindowViewModel.Title = "Create suggested tour";
                    MainWindow.DataContext = _mainWindowViewModel;
                    var createSuggestedTourPage = MainWindow.MainFrame.Content as CreateSuggestedTourPage;
                    var createSuggestedViewModel = (CreateSuggestedTourViewModel)createSuggestedTourPage.DataContext;
                    _mainWindow.MainFrame.Content = new AddAppointmentsPage(createSuggestedViewModel.AppointmentCards);
                }
                
            }
            else if (frameName == "TourGallery")
            {
                var previousPage = PreviousPages.Pop();
                if (previousPage is CreateTourPage)
                {
                    _mainWindowViewModel.Title = "Create tour";
                    MainWindow.DataContext = _mainWindowViewModel;
                    var createTourPage = MainWindow.MainFrame.Content as CreateTourPage;
                    var createTourViewModel = (CreateTourViewModel)createTourPage.DataContext;
                    _mainWindow.MainFrame.Content = new TourGalleryPage(createTourViewModel.Images);
                }
                else if(previousPage is AcceptTourRequestPage)
                {
                    _mainWindowViewModel.Title = "Accept tour";
                    MainWindow.DataContext = _mainWindowViewModel;
                    var acceptRequestTourPage = MainWindow.MainFrame.Content as AcceptTourRequestPage;
                    var acceptRequestTouModel = (AcceptTourRequestViewModel)acceptRequestTourPage.DataContext;
                    _mainWindow.MainFrame.Content = new TourGalleryPage(acceptRequestTouModel.Images);
                }
                else if(previousPage is CreateSuggestedTourPage)
                {
                    _mainWindowViewModel.Title = "Create suggested tour";
                    MainWindow.DataContext = _mainWindowViewModel;
                    var createSuggestedTourPage = MainWindow.MainFrame.Content as CreateSuggestedTourPage;
                    var createSuggestedViewModel = (CreateSuggestedTourViewModel)createSuggestedTourPage.DataContext;
                    _mainWindow.MainFrame.Content = new TourGalleryPage(createSuggestedViewModel.Images);
                }
                else if (previousPage is AcceptPartOfComplexTourRequestPage)
                {
                    _mainWindowViewModel.Title = "Accept tour part";
                    MainWindow.DataContext = _mainWindowViewModel;
                    var acceptPartOfComplexTourPage =
                        MainWindow.MainFrame.Content as AcceptPartOfComplexTourRequestPage;
                    var acceptPartOfComplexViewModel =
                        (AcceptPartOfComplexTourRequestViewModel)acceptPartOfComplexTourPage.DataContext;
                    _mainWindow.MainFrame.Content = new TourGalleryPage(acceptPartOfComplexViewModel.Images);
                }
                PreviousPages.Push(previousPage);
            }
            else if(frameName == "GuestAttendances")
            {
                _mainWindowViewModel.Title = "Guest attendances";
                MainWindow.DataContext = _mainWindowViewModel;
                var liveTourPage = MainWindow.MainFrame.Content as LiveTourPage;
                var liveTourViewModel = (LiveTourViewModel)liveTourPage.DataContext;
                _mainWindow.MainFrame.Content = new GuestAttendancePage(
                    liveTourViewModel.SelectedCheckpointActivityCard, liveTourViewModel.TourName,
                    liveTourViewModel.Date);
            }
            else if(frameName == "AcceptTourRequestForm")
            {
                _mainWindowViewModel.Title = "Accept tour";
                MainWindow.DataContext = _mainWindowViewModel;
                var tourRequestsPage = MainWindow.MainFrame.Content as TourRequestPage;
                var tourRequestsViewModel = (TourRequestViewModel)tourRequestsPage.DataContext;
                _mainWindow.MainFrame.Content = new AcceptTourRequestPage(tourRequestsViewModel.SelectedTourRequestCard.Id);
            }
            else if (frameName == "CreateSuggestedTour")
            {
                _mainWindowViewModel.Title = "Create suggested tour";
                MainWindow.DataContext = _mainWindowViewModel;
                var tourRequestStatPage = MainWindow.MainFrame.Content as TourRequestStatsPage;
                var tourRequestStatViewModel = (TourRequestStatsViewModel)tourRequestStatPage.DataContext;
                _mainWindow.MainFrame.Content = new CreateSuggestedTourPage(tourRequestStatViewModel.City, tourRequestStatViewModel.Country,
                                                                                 tourRequestStatViewModel.Language, tourRequestStatViewModel.Type);
            }
            else if (frameName == "UserAccount")
            {
                _mainWindowViewModel.Title = "Account";
                MainWindow.DataContext = _mainWindowViewModel;
                _mainWindow.MainFrame.Content = new UserAccountPage();
            }
            else if (frameName == "GeneratePDFReport")
            {
                _mainWindowViewModel.Title = "PDF Report";
                MainWindow.DataContext = _mainWindowViewModel;
                _mainWindow.MainFrame.Content = new PDFReportTourGuidePage();
            }
            else if (frameName == "ShowPDFReport")
            {
                _mainWindowViewModel.Title = "PDF Report";
                MainWindow.DataContext = _mainWindowViewModel;
                var generatePDFReportPage = MainWindow.MainFrame.Content as PDFReportTourGuidePage;
                var generatePDFReportViewModel = (PDFReportTourGuideViewModel)generatePDFReportPage.DataContext;
                _mainWindow.MainFrame.Content = new ViewPDFReportPage(generatePDFReportViewModel.PDFPath);
            }
            else if (frameName == "ComplexTourRequestsOverview")
            {
                _mainWindowViewModel.Title = "Requests";
                MainWindow.DataContext = _mainWindowViewModel;
                _mainWindow.MainFrame.Content = new ComplexTourRequestsOverviewPage();
            }
            else if (frameName == "ComplexTourRequest")
            {
                _mainWindowViewModel.Title = "Requests";
                MainWindow.DataContext = _mainWindowViewModel;
                var complexTourRequestsOverviewPage = MainWindow.MainFrame.Content as ComplexTourRequestsOverviewPage;
                var complexTourRequestsOverviewViewModel =
                    (ComplexTourRequestsOverviewViewModel)complexTourRequestsOverviewPage.DataContext;
                _mainWindow.MainFrame.Content =
                    new ComplexTourRequestPage(complexTourRequestsOverviewViewModel.SelectedComplexTourRequest);
            }
            else if (frameName == "AcceptPartOfComplexTour")
            {
                _mainWindowViewModel.Title = "Accept tour part";
                MainWindow.DataContext = _mainWindowViewModel;
                var complexTourRequestPage = MainWindow.MainFrame.Content as ComplexTourRequestPage;
                var complexTourRequestViewModel = (ComplexTourRequestViewModel)complexTourRequestPage.DataContext;
                _mainWindow.MainFrame.Content = new AcceptPartOfComplexTourRequestPage(complexTourRequestViewModel.SelectedTourRequestCard.Id);
            }
            else if (_mainWindow.MainFrame.Content is CreateTourPage || _mainWindow.MainFrame.Content is CreateSuggestedTourPage
                     || _mainWindow.MainFrame.Content is AcceptTourRequestPage)
            {
                if (frameName == "CreateTourDemo")
                {
                    AddPreviousPage();
                    _mainWindowViewModel.Title = "Demo";
                    MainWindow.DataContext = _mainWindowViewModel;
                    _mainWindow.MainFrame.Content = new CreateTourDemoPage();
                }
            }

        }

        public void AddPreviousPage()
        {
            _previousPages.Push(_mainWindow.MainFrame.Content as Page);
        }

        public void GoBack(string buttonName)
        {
            var navigationService = _mainWindow.MainFrame.NavigationService;

            switch (buttonName)
            {
                case "Back":
                    
                    if (_mainWindow.MainFrame.Content is HomePage)
                    {
                        YesNoMessageBox yesNoMessageBox = CreateYesNoMessageBox("Are you sure you want to exit the application?");
                        var result = yesNoMessageBox.ShowDialog();
                        if (result == true)
                        {
                            _mainWindow.Close();
                        }
                    }
                    else
                    {
                        var previousPage = PreviousPages.Pop();
                        navigationService.GoBack();


                        if (previousPage is HomePage)
                        {
                             var homePage = (HomePage)previousPage;
                             var homePageViewModel = homePage.DataContext as HomePageViewModel;
                             homePageViewModel.TourCards = _tourCardCreatorViewModel.CreateCards(homePageViewModel.LoggedUser, CreationType.ALL);
                            _mainWindowViewModel.Title = "Home";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is StatsMenuPage)
                        {
                            _mainWindowViewModel.Title = "Stats";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is RequestMenuPage)
                        {
                            _mainWindowViewModel.Title = "Requests";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is TourRequestPage)
                        {
                            _mainWindowViewModel.Title = "Requests";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is LiveTourPage)
                        {
                            _mainWindowViewModel.Title = "Live tour";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is GuestReviewsByTourPage)
                        {
                            _mainWindowViewModel.Title = "Reviews";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is CreateTourPage)
                        {
                            _mainWindowViewModel.Title = "Create tour";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is TodayToursPage)
                        {
                            var todayToursPage = (TodayToursPage)previousPage;
                            var todayToursPageViewModel = todayToursPage.DataContext as TodayToursViewModel;
                            todayToursPageViewModel.TodayTourCards = _tourCardCreatorViewModel.CreateCards(todayToursPageViewModel.LoggedUser, CreationType.TODAY);
                            _mainWindowViewModel.Title = "Today tours";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }
                        
                        if (previousPage is CreateTourPage)
                        {
                            _mainWindowViewModel.Title = "Create tour";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is AddCheckpointsPage)
                        {
                            _mainWindowViewModel.Title = "Create tour";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }
                        if (previousPage is AddAppointmentsPage)
                        {
                            _mainWindowViewModel.Title = "Create tour";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }
                        if (previousPage is TourGalleryPage)
                        {
                            _mainWindowViewModel.Title = "Create tour";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is GuestAttendancePage)
                        {
                            _mainWindowViewModel.Title = "Guest attendances";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is TourRequestPage)
                        {
                            _mainWindowViewModel.Title = "Tour requests";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is CreateSuggestedTourPage)
                        {
                            _mainWindowViewModel.Title = "Create suggested tour";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is UserAccountPage)
                        {
                            _mainWindowViewModel.Title = "Account";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is PDFReportTourGuidePage)
                        {
                            _mainWindowViewModel.Title = "PDF Report";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is ViewPDFReportPage)
                        {
                            _mainWindowViewModel.Title = "PDF Report";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is ComplexTourRequestsOverviewPage)
                        {
                            _mainWindowViewModel.Title = "Requests";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is ComplexTourRequestPage)
                        {
                            _mainWindowViewModel.Title = "Requests";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is AcceptPartOfComplexTourRequestPage)
                        {
                            _mainWindowViewModel.Title = "Accept tour part";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is StatsOverviewPage)
                        {
                            _mainWindowViewModel.Title = "Tour stats";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is StatsByTourOverviewPage)
                        {
                            _mainWindowViewModel.Title = "Tour stats";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is TourRequestStatsPage)
                        {
                            _mainWindowViewModel.Title = "Tour stats";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is StatByTourPage)
                        {
                            _mainWindowViewModel.Title = "Tour stats";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }

                        if (previousPage is AcceptTourRequestPage)
                        {
                            _mainWindowViewModel.Title = "Accept tour";
                            _mainWindow.DataContext = _mainWindowViewModel;
                        }
                    }

                    break;
            }
        }

        public void CreateOkMessageBox(string message, string title = "Alert",
            string imagePath = "/Resources/Icons/warning.png")
        {
            var messageBoxViewModel = new OkMessageBoxViewModel(title, imagePath, message);
            var okMessageBox = new OkMessageBox();
            okMessageBox.DataContext = messageBoxViewModel;
            okMessageBox.ShowDialog();
        }

        private YesNoMessageBox CreateYesNoMessageBox(string message, string title = "Alert", string imagePath = "/Resources/Icons/warning.png")
        {
            var messageBoxViewModel = new YesNoMessageBoxViewModel(title, imagePath, message);
            var messageBoxWindow = new YesNoMessageBox
            {
                DataContext = messageBoxViewModel
            };
            return messageBoxWindow;
        }

        public bool? GetMessageBoxResult(string massage)
        {
            var messageBox = CreateYesNoMessageBox(massage);
            return messageBox.ShowDialog();
        }
    }
}
