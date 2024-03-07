using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using SOSTeam.TravelAgency.WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using System.Reflection.Metadata;
using SOSTeam.TravelAgency.Application.Services;
using System.Windows.Navigation;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class RequestsStatusViewModel
    {
        public User LoggedInUser { get; set; }
        public NavigationService NavigationService { get; set; }
        public NavigationService ProfilleService { get; set; }
        public string UsernameTextBlock { get; set; }
        public int Notifications { get; set; }
        public bool Report { get; set; }
        public string WindowNameTextBlock { get; set; }
        public RelayCommand NavigationButtonCommand { get; set; }

        public RequestsStatusViewModel(User user, NavigationService service, int notifications, bool report, NavigationService profilleService)
        {
            LoggedInUser = user;
            NavigationService = service;
            ProfilleService = profilleService;
            UsernameTextBlock = user.Username;
            Notifications = notifications;
            Report = report;

            FillTitleBlock();

            NavigationButtonCommand = new RelayCommand(Execute_NavigationButtonCommand);
        }
        
        private void FillTitleBlock()
        {
            if (!Report) WindowNameTextBlock = "Pregled zahtjeva";
            else WindowNameTextBlock = "Izvještaj";
        }
        
        public void SetStartupPage()
        {
            if (!Report)
            {
                NavigationService.Navigate(new AllStatusesPage(LoggedInUser, NavigationService));
            }
            else
            {
                NavigationService.Navigate(new ReportFiltersPage(LoggedInUser, NavigationService));
            }
        }
        
        public void Execute_NavigationButtonCommand(object parameter)
        {
            string nextPage = parameter.ToString();

            switch (nextPage)
            {
                case "Profille":
                    NotificationFromOwnerService service = new NotificationFromOwnerService();
                    ProfilleService.Navigate(new UserProfillePage(LoggedInUser, service.TestInboxCharge(LoggedInUser.Id), ProfilleService));
                    break;
                case "Search":
                    break;
                case "Bid":
                    break;
                case "Whatever":
                    break;
                case "LogOut":
                    SignInForm form = new SignInForm();
                    Guest1MainWindow.Instance.Close();
                    form.ShowDialog();
                    break;
                default:
                    break;
            }
            return;
        }
    }
}
