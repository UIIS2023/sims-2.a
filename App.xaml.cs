using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Windows;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Navigation;

namespace SOSTeam.TravelAgency
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public static TourGuideNavigationService TourGuideNavigationService { get; set; }
        public static OwnerNavigationService OwnerNavigationService { get; set; }
        public static User LoggedUser { get; set; }
    }
}
