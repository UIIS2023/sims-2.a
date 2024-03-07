using SOSTeam.TravelAgency.Domain.DTO;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest1;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SOSTeam.TravelAgency.WPF.Views.Guest1
{
    /// <summary>
    /// Interaction logic for ShowAvailableDatesPage.xaml
    /// </summary>
    public partial class ShowAvailableDatesPage : Page
    {
        public ShowAvailableDatesPage(LocAccommodationDTO dto, DateTime firstDay, DateTime lastDay, int days, User user, bool enter, ChangedReservationRequest request, NavigationService service)
        {
            InitializeComponent();
            ShowAvailableDatesViewModel viewModel = new ShowAvailableDatesViewModel(dto, firstDay, lastDay, days, user, Calendar, enter, request, service);
            DataContext = viewModel;
        }
    }
}
