using SOSTeam.TravelAgency.Domain.DTO;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for EnterReservationPage.xaml
    /// </summary>
    public partial class EnterReservationPage : Page
    {
        public EnterReservationPage(LocAccommodationDTO dto, User user, bool enter, NavigationService service)
        {
            InitializeComponent();
            EnterReservationViewModel viewModel = new EnterReservationViewModel(dto, user, enter, Days, FirstDay, LastDay, service);
            DataContext = viewModel;
        }

        public EnterReservationPage(LocAccommodationDTO dto, User user, bool enter, ChangedReservationRequest request, NavigationService service)
        {
            InitializeComponent();
            EnterReservationViewModel viewModel = new EnterReservationViewModel(dto, user, enter, Days, FirstDay, LastDay, service, request);
            DataContext = viewModel;
        }

        private void TestEnteredText(object sender, RoutedEventArgs e)
        {
            if (Days.Text.Equals(""))
            {
                MessageBox.Show("Unesite broj dana.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!Regex.IsMatch(Days.Text, @"^[0-9]+$"))
            {
                MessageBox.Show("Broj dana se mora sastojati od cifara!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                Days.Focus();
                return;
            }
        }
    }
}
