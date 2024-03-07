using System;
using System.Windows.Threading;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class TourCardViewModel : ViewModel
    {
        public int TourId { get; set; }
        public int LocationId { get; set; }
        public int AppointmentId { get; set; }
        public string CoverImagePath { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime Start { get; set; }
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
        public string CancelImage { get; set; }

        private bool _canCancel;
        public bool CanCancel 
        { 
            get => _canCancel;
            set
            {
                if (_canCancel != value)
                {
                    _canCancel = value;
                    OnPropertyChanged("CanCancel");
                }
            }
        }

        private bool _canStart;

        public bool CanStart
        {
            get => _canStart;
            set
            {
                if (_canStart != value)
                {
                    _canStart = value;
                    OnPropertyChanged("CanStart");
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

        public TourCardViewModel()
        {
            TourId = -1;
            LocationId = -1;
            AppointmentId = -1;
            CoverImagePath = string.Empty;
            Name = string.Empty;
            Location = string.Empty;
            Start = DateTime.MinValue;
            _status = string.Empty;
            CancelImage = string.Empty;
            _canCancel = false;
            _canStart = false;
            _background = string.Empty;
        }

        public TourCardViewModel(int tourId, int locationId, int appointmentId, string coverImagePath, string name, string location, DateTime start, string status, string cancelImage, bool canCancel, bool canStart, string background)
        {
            TourId = tourId;
            LocationId = locationId;
            AppointmentId = appointmentId;
            CoverImagePath = coverImagePath;
            Name = name;
            Location = location;
            Start = start;
            _status = status;
            CancelImage = cancelImage;
            _canCancel = canCancel;
            _canStart = canStart;
            _background = background;

        }

        public void SetCanCancel(Appointment appointment)
        {
            var timeDifference = Start - DateTime.Now;

            if (timeDifference.TotalHours < 48)
            {
                CancelImage = "/Resources/Icons/cancel_light.png";
                CanCancel = false;

            }
            else
            {
                CancelImage = "/Resources/Icons/cancel.png";
                CanCancel = true;
            }
        }

        public void SetCanStart(Appointment appointment, Appointment? activeAppointment)
        {
            if (Status == "Not started" && activeAppointment == null)
            {
                CanStart = true;
            }
            else if (Status == "Expired" || activeAppointment != null)
            {
                CanStart = false;
            }
        }

        public void SetAppointmentStatusAndBackground(Appointment appointment)
        {
            if (appointment.IsActive())
            {
                Status = "Active";
                Background = "#C7FFC2";
            }
            else if (appointment.IsFinished())
            {
                Status = "Finished";
                Background = "#F0F8FF";
            }
            else if (appointment.IsNotStarted())
            {
                Status = "Not started";
                Background = "#F8FFB7";
            }
            else
            {
                Status = "Expired";
                Background = "#FFC7C8";
            }
        }

        public void SetLocation(Location? location)
        {
            if (location == null)
            {
                Location = "UNKNOWN";
            }
            else
            {
                Location = location.City + ", " + location.Country;
                LocationId = location.Id;
            }
            
        }
        public void SetCoverImage(Image? image)
        {
            CoverImagePath = image != null ? image.Path : "/Resources/Images/Tours/unknown_cover.jpg";
        }

    }
}
