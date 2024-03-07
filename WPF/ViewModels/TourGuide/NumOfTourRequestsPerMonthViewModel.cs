using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class NumOfTourRequestsPerMonthViewModel : ViewModel
    {
        private string _month;

        public string Month
        {
            get => _month;
            set
            {
                if (_month != value)
                {
                    _month = value;
                    OnPropertyChanged("Month");
                }
            }
        }

        private int _numOfRequestsPerMonth;

        public int NumOfRequestsPerMonth
        {
            get => _numOfRequestsPerMonth;
            set
            {
                if (_numOfRequestsPerMonth != value)
                {
                    _numOfRequestsPerMonth = value;
                    OnPropertyChanged("NumOfRequestsPerMonth");
                }
            }
        }

        public NumOfTourRequestsPerMonthViewModel(string month, int numOfRequests)
        {
            _month = month;
            _numOfRequestsPerMonth = numOfRequests;
        }

        public string MonthConverter()
        {
            if (Month == "January")
            {
                return "1";
            }
            else if(Month == "February")
            {
                return "2";
            }
            else if(Month == "Mart")
            {
                return "3";
            }
            else if(Month == "April")
            {
                return "4";
            }
            else if(Month == "May")
            {
                return "5";
            }
            else if(Month == "Jun")
            {
                return "6";
            }
            else if(Month == "July")
            {
                return "7";
            }
            else if(Month == "August")
            {
                return "8";
            }
            else if(Month == "September")
            {
                return "9";
            }
            else if(Month == "October")
            {
                return "10";
            }
            else if(Month == "November")
            {
                return "11";
            }
            else
            {
                return "12";
            }
            
        }
    }
}
