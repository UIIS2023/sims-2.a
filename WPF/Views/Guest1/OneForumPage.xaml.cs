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
    /// Interaction logic for OneForumPage.xaml
    /// </summary>
    public partial class OneForumPage : Page
    {
        public OneForumPage(User user, NavigationService service, Forum forum)
        {
            InitializeComponent();
            OneForumViewModel viewModel = new OneForumViewModel(user, service, forum);
            DataContext = viewModel;
        }
    }
}
