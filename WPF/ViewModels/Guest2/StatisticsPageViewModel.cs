using LiveCharts;
using LiveCharts.Wpf;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class StatisticsPageViewModel : ViewModel
    {
        public OrdinaryToursPageViewModel OrdinaryToursPageViewModel { get; set; }
        public User LoggedInUser { get; set; }
        public float AcceptedRequestsPercentage { get; set; }
        public float InvalidRequestsPercentage { get; set; }
        public float AverageTouristNum { get; set; }

        private float _acceptedRequestsPercentageByYear;
        public float AcceptedRequestsPercentageByYear
        {
            get { return _acceptedRequestsPercentageByYear; }
            set
            {
                _acceptedRequestsPercentageByYear = value;
                OnPropertyChanged(nameof(AcceptedRequestsPercentageByYear));
            }
        }

        private float _invalidRequestsPercentageByYear;
        public float InvalidRequestsPercentageByYear
        {
            get { return _invalidRequestsPercentageByYear; }
            set
            {
                _invalidRequestsPercentageByYear = value;
                OnPropertyChanged(nameof(InvalidRequestsPercentageByYear));
            }
        }

        private float _averageTouristNumByYear;
        public float AverageTouristNumByYear
        {
            get { return _averageTouristNumByYear; }
            set
            {
                _averageTouristNumByYear = value;
                OnPropertyChanged(nameof(AverageTouristNumByYear));
            }
        }
        public ObservableCollection<int> AvailableYears { get; set; }

        private int _selectedYear;

        public int SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                (AverageTouristNumByYear, AcceptedRequestsPercentageByYear, InvalidRequestsPercentageByYear) = _tourRequestStatsService.GenerateStatistics(LoggedInUser, SelectedYear);
                GeneratePieData(SelectedYear);
                OnPropertyChanged(nameof(SelectedYear));
            }
        }
        public Dictionary<string, int> LanguageCounts { get; set; }

        private SeriesCollection _tourRequestsByLanguage;
        public SeriesCollection TourRequestsByLanguage
        {
            get { return _tourRequestsByLanguage; }
            set
            {
                _tourRequestsByLanguage = value;
                OnPropertyChanged(nameof(TourRequestsByLanguage));
            }
        }
        public Dictionary<string, int> LocationCounts { get; set; }

        private SeriesCollection _tourRequestsByLocation;
        public SeriesCollection TourRequestsByLocation
        {
            get { return _tourRequestsByLocation; }
            set
            {
                _tourRequestsByLocation = value;
                OnPropertyChanged(nameof(TourRequestsByLocation));
            }
        }

        private SeriesCollection _requestsPercentage;
        public SeriesCollection RequestsPercentage
        {
            get { return _requestsPercentage; }
            set
            {
                _requestsPercentage = value;
                OnPropertyChanged(nameof(RequestsPercentage));
            }
        }

        private SeriesCollection _requestsPercentagePerYear;
        public SeriesCollection RequestsPercentagePerYear
        {
            get { return _requestsPercentagePerYear; }
            set
            {
                _requestsPercentagePerYear = value;
                OnPropertyChanged(nameof(RequestsPercentagePerYear));
            }
        }

        private RelayCommand _createCommand;

        public RelayCommand CreateCommand
        {
            get { return _createCommand; }
            set
            {
                _createCommand = value;
            }
        }

        private readonly TourRequestStatsService _tourRequestStatsService;

        public StatisticsPageViewModel(User loggedInUser,OrdinaryToursPageViewModel ordinaryToursPageViewModel)
        {
            OrdinaryToursPageViewModel = ordinaryToursPageViewModel;
            LoggedInUser = loggedInUser;
            RequestsPercentagePerYear = new SeriesCollection();
            RequestsPercentage = new SeriesCollection();
            TourRequestsByLanguage = new SeriesCollection();
            TourRequestsByLocation = new SeriesCollection();
            _tourRequestStatsService = new TourRequestStatsService();
            AvailableYears = new ObservableCollection<int>();
            SelectedYear = 2024;
            AvailableYears = _tourRequestStatsService.GetAvailableYears();
            (AverageTouristNum, AcceptedRequestsPercentage, InvalidRequestsPercentage) = _tourRequestStatsService.GenerateStatistics(LoggedInUser);
            (AverageTouristNumByYear, AcceptedRequestsPercentageByYear, InvalidRequestsPercentageByYear) = _tourRequestStatsService.GenerateStatistics(LoggedInUser,SelectedYear);
            LocationCounts = _tourRequestStatsService.GenerateTourRequestsByLocation(LoggedInUser);
            LanguageCounts = _tourRequestStatsService.GenerateTourRequestsByLanguage(LoggedInUser);
            TourRequestsByLanguage = GenerateChartData(LanguageCounts);
            TourRequestsByLocation = GenerateChartData(LocationCounts);
            CreateCommand = new RelayCommand(Execute_CreateCommand, CanExecuteMethod);
            GeneratePieData();
            GeneratePieData(SelectedYear);
        }

        private SeriesCollection GenerateChartData(Dictionary<string, int> dataCounts)
        {
            SeriesCollection series = new SeriesCollection();

            var labels = dataCounts.Keys.ToArray();
            var values = dataCounts.Values.ToArray();

            var columnSeries = new ColumnSeries
            {
                Values = new ChartValues<int>(values),
                DataLabels = true
            };

            Axis axis = new Axis
            {
                Labels = labels
            };

            series.Add(columnSeries);

            return series;
        }       

        private void GeneratePieData(int selectedYear = 0)
        {
            if (selectedYear == 0)
            {
                RequestsPercentage = new SeriesCollection
                {
                    new PieSeries
                    {
                        Title = "Prihvaceni zahtevi",
                        Values = new ChartValues<float> { AcceptedRequestsPercentage },
                        DataLabels = true
                    },
                    new PieSeries
                    {
                        Title = "Odbijeni zahtevi",
                        Values = new ChartValues<float> { InvalidRequestsPercentage },
                        DataLabels = true
                    }
                };
            }
            else
            {
                RequestsPercentagePerYear = new SeriesCollection
                {
                    new PieSeries
                    {
                        Title = "Prihvaceni zahtevi",
                        Values = new ChartValues<float> { AcceptedRequestsPercentageByYear },
                        DataLabels = true
                    },
                    new PieSeries
                    {
                        Title = "Odbijeni zahtevi",
                        Values = new ChartValues<float> { InvalidRequestsPercentageByYear },
                        DataLabels = true
                    }
                };
            }
        }
        private void Execute_CreateCommand(object obj)
        {
            CreateTourRequestWindow window = new CreateTourRequestWindow(LoggedInUser,OrdinaryToursPageViewModel);
            window.ShowDialog();
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }
    }  
}
