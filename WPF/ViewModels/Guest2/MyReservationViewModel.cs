using iTextSharp.text;
using iTextSharp.text.pdf;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.WPF.Views.Guest2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class MyReservationViewModel : ViewModel
    {
        public int ReservationId { get; set; }
        public string Name { get; set; }
        public string LocationFullName { get; set; }
        public string Language { get; set; }
        public string Start { get; set; }
        public int TouristNum { get; set; }
        public string ImagePath { get; set; }

        private RelayCommand _reportCommand;

        public RelayCommand ReportCommand
        {
            get { return _reportCommand; }
            set
            {
                _reportCommand = value;
            }
        }
        public MyReservationViewModel(int reservationId,string name, string locationFullName,string language, DateTime start, int touristNum, string imagePath) 
        {
            Name = name;
            LocationFullName = locationFullName;
            Language = language;
            Start = start.ToString();
            TouristNum = touristNum;
            ReportCommand = new RelayCommand(Execute_ReportCommand,CanExecuteMethod);
            ImagePath= imagePath;
            ReservationId= reservationId;
        }

        private void Execute_ReportCommand(object obj)
        {
            string filePath = $@"D:\Desktop1\Drugi deo projekta\turisticka-agencija\TravelAgency\Report\Guest2-reports\report_{ReservationId}.pdf";

            ShowPDFReport(filePath);
        }
        private void ShowPDFReport(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    //Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                    PDFReportWindow window = new PDFReportWindow(filePath);
                    window.ShowDialog();
                }
                else
                {
                    MessageBox.Show("PDF file not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening PDF report: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }
    }
}
