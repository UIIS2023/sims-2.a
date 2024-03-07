using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    public class UserPageViewModel
    {
        public string Username { get; private set; }
        public User LoggedInUser { get; private set; }
        public RelayCommand LogOut { get; private set; }
        public UserPageViewModel()
        {
            LoggedInUser = App.LoggedUser;
            Username = LoggedInUser.Username;
            LogOut = new RelayCommand(Execute_LogOut, CanExecuteLogOut);
        }

        internal void Execute_LogOut(object obj)
        {
            SignInForm form = new SignInForm();
            App.OwnerNavigationService.CloseWindow();
            form.ShowDialog();
        }

        private bool CanExecuteLogOut(object obj)
        {
            return true;
        }
    }
}
