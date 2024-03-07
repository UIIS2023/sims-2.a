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
using System.Windows.Shapes;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.WPF.Views.Guest1
{
    /// <summary>
    /// Interaction logic for Guest1MainWindow.xaml
    /// </summary>
    public partial class Guest1MainWindow : Window
    {
        public static Guest1MainWindow Instance;
        public Guest1MainWindow(User loggedInUser, int notifications)
        {
            InitializeComponent();
            Instance = this;
            var navigationService = MainFrame.NavigationService;
            Guest1MainViewModel viewModel = new Guest1MainViewModel(loggedInUser, notifications, navigationService);
            DataContext = viewModel;
        }
    }
}
