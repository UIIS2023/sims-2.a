using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using SOSTeam.TravelAgency.WPF.ViewModels.TourGuide;

namespace SOSTeam.TravelAgency.WPF.Views.TourGuide
{
    /// <summary>
    /// Interaction logic for TourRequestsPerMonthWindow.xaml
    /// </summary>
    public partial class TourRequestsPerMonthWindow : Window
    {
        public ObservableCollection<NumOfTourRequestsPerMonthViewModel> NumOfTourRequestsPerMonth { get; set; }
        public string Year { get; set; }

        public TourRequestsPerMonthWindow(ObservableCollection<NumOfTourRequestsPerMonthViewModel> numOfRequestPerMonth, string year)
        {
            InitializeComponent();
            NumOfTourRequestsPerMonth = numOfRequestPerMonth;
            Year = year;

            DataContext = this;
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
