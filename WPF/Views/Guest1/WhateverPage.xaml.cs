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
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.ViewModels.Guest1;

namespace SOSTeam.TravelAgency.WPF.Views.Guest1
{
    /// <summary>
    /// Interaction logic for WhateverPage.xaml
    /// </summary>
    public partial class WhateverPage : Page
    {
        public WhateverPage(User user, NavigationService service)
        {
            InitializeComponent();
            FirstDay.BlackoutDates.AddDatesInPast();
            LastDay.BlackoutDates.AddDatesInPast();
            WhateverViewModel viewModel = new WhateverViewModel(user, service);
            DataContext = viewModel;
        }
    }
}
