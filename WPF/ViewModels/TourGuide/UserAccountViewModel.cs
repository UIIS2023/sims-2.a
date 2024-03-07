using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{

    public class UserAccountViewModel
    {
        public string Username { get; set; }
        public string Title { get; set; }

        public ObservableCollection<string> Languages { get; set; }

        public RelayCommand QuitJobCommand { get; set; }
        private readonly QuitGuideJobService _quitGuideJobService;
        private readonly SuperGuideService _superGuideService;

        public UserAccountViewModel()
        {
            Username = App.LoggedUser.Username;
            _quitGuideJobService = new QuitGuideJobService();
            _superGuideService = new SuperGuideService();
            QuitJobCommand = new RelayCommand(QuitJob, CanExecuteMethod);
            Languages = new ObservableCollection<string>();
            SetTitle();
            
        }

        private void SetTitle()
        {
            var isSuperGuide = _superGuideService.GetAll().Any(sg => sg.UserId == App.LoggedUser.Id && sg.Year == DateTime.Now.Year);

            if (isSuperGuide)
            {
                Title = "Super guide";
                foreach (var superGuide in _superGuideService.GetAllByUserId(App.LoggedUser.Id))
                {
                    Languages.Add(superGuide.Language);
                }
            }
            else
            {
                Title = "Guide";
            }

            
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void QuitJob(object sender)
        {
            _quitGuideJobService.QuitJob();
            App.TourGuideNavigationService.LogOut();
        }

    }
}
