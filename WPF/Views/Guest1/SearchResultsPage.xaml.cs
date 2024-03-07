using SOSTeam.TravelAgency.Domain.DTO;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest1;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for SearchResultsPage.xaml
    /// </summary>
    public partial class SearchResultsPage : Page
    {
        public SearchResultsPage(List<LocAccommodationDTO> Results, User user, NavigationService service)
        {
            InitializeComponent();
            SearchResultsViewModel viewModel = new SearchResultsViewModel(Results, user, service);
            DataContext = viewModel;
        }
    }
}
