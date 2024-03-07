using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class TourRequestCardViewModel : ViewModel
    {
        public int Id { get; set; }

        private string _location;

        public string Location
        {
            get => _location;
            set
            {
                if (_location != value)
                {
                    _location = value;
                    OnPropertyChanged("Location");
                }
            }
        }

        private int _numOfGuests;

        public int NumOfGuests
        {
            get => _numOfGuests;
            set
            {
                if (_numOfGuests != value)
                {
                    _numOfGuests = value;
                    OnPropertyChanged("NumOfGuests");
                }
            }
        }

        private string _language;

        public string Language
        {
            get => _language;
            set
            {
                if (_language != value)
                {
                    _language = value;
                    OnPropertyChanged("Language");
                }
            }
        }

        private DateTime _minDate;

        public DateTime MinDate
        {
            get => _minDate;
            set
            {
                if (_minDate != value)
                {
                    _minDate = value;
                    OnPropertyChanged("MinDate");
                }
            }
        }
        
        private DateTime _maxDate;

        public DateTime MaxDate
        {
            get => _maxDate;
            set
            {
                if (_maxDate != value)
                {
                    _maxDate = value;
                    OnPropertyChanged("MaxDate");
                }
            }
        }


        public TourRequestCardViewModel()
        {
            _location = string.Empty;
            _language = string.Empty;
            _minDate = DateTime.MinValue;
            _maxDate = DateTime.MaxValue;
            _numOfGuests = 0;
        }
    }
}
