using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Repositories;
using SOSTeam.TravelAgency.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.WPF.Views.Owner;
using System.Windows.Controls;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using SOSTeam.TravelAgency.WPF.Views.TourGuide;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Drawing;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class SearchAccommodationViewModel
    {
        public User LoggedInUser { get; set; }
        public NavigationService MenuNavigationService { get; set; }
        public NavigationService ProfilleNavigationService { get; set; }
        public Window UserProfilleWindow { get; set; }
        public Frame ThisFrame { get; set; }
        public int Notifications { get; set; }
        public string UserNameTextBlock { get; set; }
        public RelayCommand NavigationButtonCommand { get; set; }

        public SearchAccommodationViewModel(User user, NavigationService menu, NavigationService profille, int notifications)
        {
            LoggedInUser = user;
            MenuNavigationService = menu;
            ProfilleNavigationService = profille;
            UserNameTextBlock = user.Username;
            Notifications = notifications;

            NavigationButtonCommand = new RelayCommand(Execute_NavigationButtonCommand);
        }

        public void SetStartupPage()
        {
            Execute_NavigationButtonCommand("Bid");
        }

        public void Execute_NavigationButtonCommand(object parameter)
        {
            string nextPage = parameter.ToString();

            switch (nextPage)
            {
                case "Profille":
                    NotificationFromOwnerService service = new NotificationFromOwnerService();
                    ProfilleNavigationService.Navigate(new UserProfillePage(LoggedInUser, service.TestInboxCharge(LoggedInUser.Id), ProfilleNavigationService));
                    break;
                case "Search":
                    MenuNavigationService.Navigate(new SearchPage(LoggedInUser, MenuNavigationService));
                    break;
                case "Bid":
                    MenuNavigationService.Navigate(new AccommodationBidPage(LoggedInUser, MenuNavigationService));
                    break;
                case "Whatever":
                    MenuNavigationService.Navigate(new WhateverPage(LoggedInUser, MenuNavigationService));
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
