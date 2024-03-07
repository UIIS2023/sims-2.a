using System;
using System.Windows;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Navigation;
using SOSTeam.TravelAgency.WPF.Views.TourGuide;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class SignInViewModel : ViewModel
    {
        private readonly UserService _userService;

        private string _username;

        private string _password;

        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged("Username");
                }
                
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                if (value != _password)
                {
                    _password = value;
                    OnPropertyChanged("Password");
                }
                
            }
        }

        public RelayCommand SignInCommand { get; set; }

        public event EventHandler OnRequestClose;

        public SignInViewModel()
        {
            _userService = new UserService();
            SignInCommand = new RelayCommand(OpenMainWindow, CanExecuteMethod);
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void OpenMainWindow(object sender)
        {
            User user = _userService.GetByUsername(Username);
            if (user != null)
            {
                if (user.Password == Password && user.Role == Roles.VODIC && user.IsJobQuit != true)
                {
                    MainWindow mainWindow = new MainWindow(user);
                    //System.Windows.Application.Current.MainWindow = mainWindow;
                    mainWindow.Show();
                    OnRequestClose(this, new EventArgs());
                    App.TourGuideNavigationService = new TourGuideNavigationService();
                    App.LoggedUser = user;
                }
                else
                {
                    MessageBox.Show("Wrong password!");
                }
            }
            else
            {
                MessageBox.Show("Wrong username!");
            }
        }
    }
}
