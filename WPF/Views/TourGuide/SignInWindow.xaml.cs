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
using SOSTeam.TravelAgency.WPF.ViewModels.TourGuide;

namespace SOSTeam.TravelAgency.WPF.Views.TourGuide
{
    /// <summary>
    /// Interaction logic for SignInWindow.xaml
    /// </summary>
    public partial class SignInWindow : Window
    {

        private SignInViewModel _signInViewModel;
        public SignInWindow()
        {
            InitializeComponent();
            _signInViewModel = new SignInViewModel();
            DataContext = _signInViewModel;
            _signInViewModel.OnRequestClose += (s, e) => CloseWindow();
        }

        private void CloseWindow()
        {
            if (_signInViewModel != null)
            {
                _signInViewModel.OnRequestClose -= (s, e) => CloseWindow();
                _signInViewModel.OnRequestClose += OnRequestCloseHandler;
                _signInViewModel = null;
            }
            Close();
        }
        private void OnRequestCloseHandler(object sender, EventArgs e)
        {
            CloseWindow();
        }
        private void PasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).Password = ((PasswordBox)sender).Password; }
        }
    }
}
