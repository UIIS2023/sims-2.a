using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class RequestViewModel : ViewModel
    {
        public int UserId { get; set; }

        private string _city;
        
        public String City
        {
            get { return _city; }
            set
            {
                if (value != _city)
                {
                    _city = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _country;

        public String Country
        {
            get { return _country; }
            set
            {
                if (value != _country)
                {
                    _country = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _locationFullName;

        public String LocationFullName
        {
            get { return _locationFullName; }
            set
            {
                if (value != _locationFullName)
                {
                    _locationFullName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _description;

        public String Description
        {
            get { return _description; }
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _language;

        public String Language
        {
            get { return _language; }
            set
            {
                if (value != _language)
                {
                    _language = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _maxNumOfGuests;

        public int MaxNumOfGuests
        {
            get { return _maxNumOfGuests; }
            set
            {
                if (value != _maxNumOfGuests)
                {
                    _maxNumOfGuests = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _maintenanceStartDate;

        public String MaintenanceStartDate
        {
            get { return _maintenanceStartDate; }
            set
            {
                if (value != _maintenanceStartDate)
                {
                    _maintenanceStartDate = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _maintenanceEndDate;

        public String MaintenanceEndDate
        {
            get { return _maintenanceEndDate; }
            set
            {
                if (value != _maintenanceEndDate)
                {
                    _maintenanceEndDate = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Status { get; set; }

        private string _startEndDateRange;
        public String StartEndDateRange
        {
            get { return _startEndDateRange; }
            set
            {
                if (value != _startEndDateRange)
                {
                    _startEndDateRange = value;
                    OnPropertyChanged();
                }
            }
        }
        public RequestViewModel()
        {

        }
        public RequestViewModel(int userId, string city, string country, string description, string language, int maxNumOfGuests, DateOnly maintenanceStartDate, DateOnly maintenanceEndDate, StatusType status)
        {
            UserId = userId;
            City = city;
            Country = country;
            LocationFullName = city + ", " + country;
            Description = description;
            Language = language;
            MaintenanceStartDate = maintenanceStartDate.ToString();
            MaintenanceEndDate = maintenanceEndDate.ToString();
            MaxNumOfGuests= maxNumOfGuests;
            StartEndDateRange = maintenanceStartDate.ToString() + " - " + maintenanceEndDate.ToString();
            if (status.ToString().Equals("ON_HOLD"))
            {
                Status = "na cekanju";
            }
            else if (status.ToString().Equals("INVALID"))
            {
                Status = "nevazeci";
            }
            else
                Status = "prihvacen";
        }

    }
}
