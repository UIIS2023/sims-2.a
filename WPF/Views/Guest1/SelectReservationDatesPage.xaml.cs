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
    /// Interaction logic for SelectReservationDatesPage.xaml
    /// </summary>
    public partial class SelectReservationDatesPage : Page
    {
        public SelectReservationDatesPage(List<AccReservationDTO> list, User user, bool enter, ChangedReservationRequest request, NavigationService service)
        {
            InitializeComponent();
            SelectResDatesViewModel viewModel = new SelectResDatesViewModel(list, user, enter, request, service);
            DataContext = viewModel;
        }
    }
}
