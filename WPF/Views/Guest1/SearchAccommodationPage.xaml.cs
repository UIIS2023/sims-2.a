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
    /// Interaction logic for SearchAccommodationPage.xaml
    /// </summary>
    public partial class SearchAccommodationPage : Page
    {
        public SearchAccommodationPage(User user, NavigationService profille, int notifications)
        {
            InitializeComponent();
            var service = MainMenuFrame.NavigationService;
            SearchAccommodationViewModel viewModel = new SearchAccommodationViewModel(user, service, profille, notifications);
            DataContext = viewModel;
            viewModel.SetStartupPage();
        }
    }
}
