using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class PDFReportTourGuideViewModel : ViewModel
    {
        private readonly PDFReportTourGuideService _pdfReportTourGuideService;

        private DateTime _start;

        public string PDFPath { get; set; }

        public DateTime Start
        {
            get => _start;
            set
            {
                if (_start != value)
                {
                    _start = value;
                    OnPropertyChanged("Start");
                }
            }
        }

        private DateTime _end;

        public DateTime End
        {
            get => _end;
            set
            {
                if (_end != value)
                {
                    _end = value;
                    OnPropertyChanged("End");
                }
            }
        }


        public RelayCommand GenerateReportCommand { get; set; }

        public PDFReportTourGuideViewModel()
        {
            _pdfReportTourGuideService = new PDFReportTourGuideService();
            GenerateReportCommand = new RelayCommand(GenerateReport, CanExecuteMethod);

            _start = DateTime.Now.AddMinutes(1);
            _end = DateTime.Now.AddMinutes(1);
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }


        public void GenerateReport(object sender)
        {
            var result = _pdfReportTourGuideService.GeneratePDFReport(Start, End);
            PDFPath = result;

            if (result == string.Empty)
            {
                App.TourGuideNavigationService.CreateOkMessageBox("There are no tours in the given time range.");
            }
            else
            {
                App.TourGuideNavigationService.AddPreviousPage();
                App.TourGuideNavigationService.SetMainFrame("ShowPDFReport", App.LoggedUser);
            }
        }

    }
}
