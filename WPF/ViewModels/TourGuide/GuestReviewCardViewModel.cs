using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class GuestReviewCardViewModel : ViewModel
    {
        public int ReviewId { get; set; }
        public int AppointmentId { get; set; }
        public int UserId { get; set; }
        public string GuestName { get; set; }
        public string TourName { get; set; }
        public string AvgGrade { get; set; }
        public double KnowledgeGrade { get; set; }
        public int LanguageGrade { get; set; }
        public int InterestingGrade { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }

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

        private string _reportedImage;
        public string ReportedImage
        {
            get => _reportedImage;
            set
            {
                if (_reportedImage != value)
                {
                    _reportedImage = value;
                    OnPropertyChanged("ReportedImage");
                }
            }
        }

        public GuestReviewCardViewModel()
        {
            GuestName = string.Empty;
            TourName = string.Empty;
            AvgGrade = string.Empty;
            ReportedImage = string.Empty;
            Background = "#F0F8FF";
        }

        public void SetReportedImage()
        {
            ReportedImage = "/Resources/Icons/reported.png";
            Background = "#FFC7C8";
        }

    }
}
