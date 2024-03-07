using LiveCharts;
using LiveCharts.Wpf;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using System;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    internal class HomePageViewModel : ViewModel
    {
        public string Username { get; private set; }
        private int _emptyAccommodations;
        private int _occupiedAccommodations;
        public int EmptyAccommodations
        {
            get => _emptyAccommodations;
            set
            {
                if (_emptyAccommodations == value) return;
                _emptyAccommodations = value;
                OnPropertyChanged();
            }
        }

        public int OccupiedAccommodations
        {
            get => _occupiedAccommodations;
            set
            {
                if (_occupiedAccommodations == value) return;
                _occupiedAccommodations = value;
                OnPropertyChanged();
            }
        }

        private string[] _timeLabels;
        public string[] TimeLabels
        {
            get { return _timeLabels; }
            set
            {
                _timeLabels = value;
                OnPropertyChanged("TimeLabels");
            }
        }

        public SeriesCollection TodayOccupancySeries { get; set; }
        public SeriesCollection MonthOccupancySeries { get; set; }
        public Accommodation PopularAccommodation { get; set; }
        public string AccommodationURI { get; set; }


        public User LoggedInUser { get; private set; }
        private AccommodationStatsService _accommodationStatsService;
        private ImageService _imageService;
        public HomePageViewModel()
        {
            LoggedInUser = App.LoggedUser;
            Username = LoggedInUser.Username;
            _accommodationStatsService = new(LoggedInUser.Id);
            _imageService = new();
            var occupationData = _accommodationStatsService.GetCurrentOccupation();
            EmptyAccommodations = occupationData.Item1;
            OccupiedAccommodations = occupationData.Item2;
            GetCurrentOccupation();
            GetMonthOccupation();
            GetMostPopularAccommodation();


        }

        private void GetMostPopularAccommodation()
        {
            PopularAccommodation = _accommodationStatsService.GetMostPopularAccommodation();
            var photo = _imageService.GetAccommodationCover(PopularAccommodation.Id);
            AccommodationURI = "/Resources/Images/UnknownPhoto.png";
            if(photo != null) AccommodationURI = photo.Path;

        }

        private void GetMonthOccupation()
        {

            var today = DateTime.Today;
            DateTime startDate = new DateTime(today.Year, today.Month, 1);
            var chartData = new ChartValues<int>();
            chartData.AddRange(_accommodationStatsService.GetOccupationInDays(startDate, 30));

            MonthOccupancySeries = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Zauzetost",
                    Values = chartData
                }
            };

            
            TimeLabels = new string[chartData.Count];
            for (int i = 0; i < chartData.Count; i++)
            {
                TimeLabels[i] = startDate.AddDays(i).ToString("MMM dd");
            }
        }

        private void GetCurrentOccupation()
        {
            TodayOccupancySeries = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Prazno",
                    Values = new ChartValues<int> { EmptyAccommodations },
                    DataLabels = true,
                    LabelPoint = point => $"{point.Y} ({point.Participation:P})"
                },
                new PieSeries
                {
                    Title = "Zauzeto",
                    Values = new ChartValues<int> { OccupiedAccommodations },
                    DataLabels = true,
                    LabelPoint = point => $"{point.Y} ({point.Participation:P})"
                }
            };
        }
    }
}
