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
    /// Interaction logic for AllStatusesPage.xaml
    /// </summary>
    public partial class AllStatusesPage : Page
    {
        public AllStatusesPage(User user, NavigationService service)
        {
            InitializeComponent();
            AllStatusesViewModel viewModel = new AllStatusesViewModel(user, service);
            DataContext = viewModel;
        }

        private void TestFocus(object sender, RoutedEventArgs e)
        {
            if (Tabs.SelectedIndex == 0)
            {
                ChangeButton.IsEnabled = true;
                CancelResButton.IsEnabled = false;
            }
            else
            {
                ChangeButton.IsEnabled = false;
                CancelResButton.IsEnabled = true;
            }
        }
    }
}
