using SOSTeam.TravelAgency.Application.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class StatByTourViewModel : ViewModel
    {
        private ObservableCollection<AttendanceAgeRangeViewModel> _statByAgeRange;

        public ObservableCollection<AttendanceAgeRangeViewModel> StatByAgeRange
        {
            get => _statByAgeRange;
            set
            {
                if (_statByAgeRange != value)
                {
                    _statByAgeRange = value;
                    OnPropertyChanged("StatByAgeRange");
                }
            }
        }

        private SeriesCollection _attendanceVoucherPie;

        public SeriesCollection AttendanceVoucherPie
        {
            get => _attendanceVoucherPie;
            set
            {
                if (_attendanceVoucherPie != value)
                {
                    _attendanceVoucherPie = value;
                    OnPropertyChanged("AttendanceVoucherPie");
                }
            }
        }

        private double _numOfGuestsWithVoucher;
        private double _numOfGuestsWithoutVoucher;


        public string TourName { get; set; }
        public DateTime Date { get; set; }

        private readonly TourStatsService _tourStatsService;

        public StatByTourViewModel(TourCardViewModel selectedTourCard)
        {
            _statByAgeRange = new ObservableCollection<AttendanceAgeRangeViewModel>();
            _attendanceVoucherPie = new SeriesCollection();
            TourName = selectedTourCard.Name;
            Date = selectedTourCard.Start;

            _tourStatsService = new TourStatsService();

            _numOfGuestsWithVoucher = _tourStatsService
                .GetPercentsOfGuestAttendancesVoucher(selectedTourCard.AppointmentId).Item1;
            _numOfGuestsWithoutVoucher =
                _tourStatsService.GetPercentsOfGuestAttendancesVoucher(selectedTourCard.AppointmentId).Item2;

            CreateStatByAgeRange(selectedTourCard);
            CreateAttendanceVoucherPie();
        }

        private void CreateAttendanceVoucherPie()
        {
            AttendanceVoucherPie.Add(new PieSeries
            {
                Title = "With voucher",
                Values = new ChartValues<double> { _numOfGuestsWithVoucher },
                DataLabels = true,
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#87ED41")),
                LabelPoint = point => $"{point.Y} ({point.Participation:P0})"
            });
            AttendanceVoucherPie.Add(new PieSeries
            {
                Title = "Without voucher",
                Values = new ChartValues<double> { _numOfGuestsWithoutVoucher },
                DataLabels = true,
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ED5153")),
                LabelPoint = point => $"{point.Y} ({point.Participation:P0})"
            });
        }

        private void CreateStatByAgeRange(TourCardViewModel selectedTourCard)
        {
            var numOfGuestsByAgeGroup = _tourStatsService.CountNumOfGuestsByAgeGroup(selectedTourCard.AppointmentId);

            var attendanceFirstRange = new AttendanceAgeRangeViewModel
            {
                AgeGroup = "0-18",
                NumOfGuests = numOfGuestsByAgeGroup.Item1
            };
            var attendanceSecondRange = new AttendanceAgeRangeViewModel
            {
                AgeGroup = "18-50",
                NumOfGuests = numOfGuestsByAgeGroup.Item2
            };
            var attendanceThirdRange = new AttendanceAgeRangeViewModel
            {
                AgeGroup = ">50",
                NumOfGuests = numOfGuestsByAgeGroup.Item3
            };

            StatByAgeRange.Add(attendanceFirstRange);
            StatByAgeRange.Add(attendanceSecondRange);
            StatByAgeRange.Add(attendanceThirdRange);
        }

    }
}
