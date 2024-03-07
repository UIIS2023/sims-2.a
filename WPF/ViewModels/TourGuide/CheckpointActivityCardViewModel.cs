using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class CheckpointActivityCardViewModel : ViewModel
    {
        public int CheckpointId { get; set; }
        public int ActivityId { get; set; }
        public string Name { get; set; }
        public CheckpointType Type { get; set; }

        public CheckpointStatus StatusEnum { get; set; }

        private string _status;

        public string Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        private bool _canShowAttendance;
        public bool CanShowAttendance
        {
            get => _canShowAttendance;
            set
            {
                if (_canShowAttendance != value)
                {
                    _canShowAttendance = value;
                    OnPropertyChanged("CanShowAttendance");
                }
            }
        }

        private string _background;

        public string Background
        {
            get => _background;
            set
            {
                if (_background != value)
                {
                    _background = value;
                    OnPropertyChanged("Background");
                }
            }
        }

        private string _guestImagePath;

        public string GuestImagePath
        {
            get => _guestImagePath;
            set
            {
                if (_guestImagePath != value)
                {
                    _guestImagePath = value;
                    OnPropertyChanged("GuestImagePath");
                }
            }
        }

        public CheckpointActivityCardViewModel()
        {
            CheckpointId = -1;
            ActivityId = -1;
            Name = string.Empty;
            StatusEnum = CheckpointStatus.UNKNOWN;
            _canShowAttendance = false;
            _guestImagePath = "/Resources/Icons/guests_with_text_light.png";
        }

        public CheckpointActivityCardViewModel(int checkpointId, string name, CheckpointType type, CheckpointStatus statusEnum)
        {
            CheckpointId = checkpointId;
            Name = name;
            Type = type;
            StatusEnum = statusEnum;
        }

        public void SetCanShowAttendance()
        {
            if (StatusEnum != CheckpointStatus.NOT_STARTED)
            {
                CanShowAttendance = true;
                GuestImagePath = "/Resources/Icons/guests_with_text.png";
            }
        }

        public void SetStatusAndBackground()
        {
            if (StatusEnum == CheckpointStatus.NOT_STARTED)
            {
                Status = "Not started";
                Background = "#F8FFB7";
            }
            else if(StatusEnum == CheckpointStatus.ACTIVE)
            {
                Status = "Active";
                Background = "#C7FFC2";
            }
            else
            {
                Status = "Finished";
                Background = "#F0F8FF";
            }
        }
    }
}
