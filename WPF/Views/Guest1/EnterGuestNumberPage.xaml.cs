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
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace SOSTeam.TravelAgency.WPF.Views.Guest1
{
    /// <summary>
    /// Interaction logic for EnterGuestNumberPage.xaml
    /// </summary>
    public partial class EnterGuestNumberPage : Page
    {
        public EnterGuestNumberPage(AccReservationDTO item, User user, bool enterOfChange, ChangedReservationRequest request, NavigationService service)
        {
            InitializeComponent();
            EnterGuestNumberViewModel viewModel = new EnterGuestNumberViewModel(item, user, enterOfChange, request, service);
            DataContext = viewModel;
        }
    }
}
