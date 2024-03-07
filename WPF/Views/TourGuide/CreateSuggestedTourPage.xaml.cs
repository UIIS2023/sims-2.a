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

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    /// <summary>
    /// Interaction logic for CreateSuggestedTourPage.xaml
    /// </summary>
    public partial class CreateSuggestedTourPage : Page
    {
        public CreateSuggestedTourPage(string city, string country, string language, string type)
        {
            InitializeComponent();
            DataContext = new CreateSuggestedTourViewModel(city, country, language, type);
        }
    }
}
