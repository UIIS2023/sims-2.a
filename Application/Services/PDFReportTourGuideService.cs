using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text.pdf.draw;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class PDFReportTourGuideService
    {
        private readonly AppointmentService _appointmentService;
        private readonly TourService _tourService;
        private readonly LocationService _locationService;
        public PDFReportTourGuideService()
        {
            _appointmentService = new AppointmentService();
            _tourService = new TourService();
            _locationService = new LocationService();
        }

        public string GeneratePDFReport(DateTime start, DateTime end)
        {
            var toursForReport = GetToursForReport(start, end);

            if (toursForReport.Count == 0)
            {
                return string.Empty;
            }

            Document document = new Document();
            string fileName = $"TourGuideReport_{DateTime.Now.ToString("dd.MM.yyyy mm:hh").Replace(" ", string.Empty).Replace(":", string.Empty).Replace(".", string.Empty)}.pdf";
            
            FileStream fileStream = new FileStream("../../../Resources/PDFReports/TourGuide/" + fileName,
                FileMode.Create);

            PdfWriter writer = PdfWriter.GetInstance(document, fileStream);

            document.Open();

            Paragraph dateParagraph = new Paragraph(DateTime.Now.ToString("dd.MM.yyyy."));
            dateParagraph.Alignment = Element.ALIGN_LEFT;
            document.Add(dateParagraph);

            Paragraph title = new Paragraph("Report on scheduled tours in the period\nfrom " + start.ToString("dd.MM.yyyy") 
                                              + " until " + end.ToString("dd.MM.yyyy."));
            title.Alignment = Element.ALIGN_CENTER;
            title.Font.Size = 20;
            document.Add(title);

            document.Add(new Paragraph(" "));

            Paragraph userInfoHeader = new Paragraph("Tour guide info");
            userInfoHeader.Font.SetStyle(Font.BOLD);
            document.Add(userInfoHeader);
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph($"Username: {App.LoggedUser.Username}"));
            document.Add(new Paragraph(" "));

            LineSeparator line = new LineSeparator();
            document.Add(line);
            document.Add(new Paragraph(" "));

            
            string logoPath = "../../../Resources/Images/SOSlogo.png";
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
            logo.Alignment = Element.ALIGN_RIGHT;
            logo.ScaleAbsolute(100, 100);

            float logoX = document.PageSize.Width - document.RightMargin - logo.ScaledWidth;
            float logoY = document.PageSize.Height - document.TopMargin - logo.ScaledHeight - 65;

            logo.SetAbsolutePosition(logoX, logoY);
            writer.DirectContent.AddImage(logo);

            //Tabela
            PdfPTable table = new PdfPTable(6);

            table.WidthPercentage = 100;

            table.AddCell(new PdfPCell(new Phrase("Name")) { HorizontalAlignment = Element.ALIGN_CENTER});
            table.AddCell(new PdfPCell(new Phrase("Date start")) { HorizontalAlignment = Element.ALIGN_CENTER});
            table.AddCell(new PdfPCell(new Phrase("Location")) { HorizontalAlignment = Element.ALIGN_CENTER});
            table.AddCell(new PdfPCell(new Phrase("Language")) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Duration")) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Max num. of guests")) { HorizontalAlignment = Element.ALIGN_CENTER });

            

            foreach (var tour in toursForReport)
            {
                table.AddCell(new PdfPCell(new Phrase(tour.Name)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase(tour.Start.ToString("dd.MM.yyyy."))) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase(tour.Location)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase(tour.Language)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase(tour.Duration)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase(tour.MaxNumOfGuests.ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
            }

            table.DefaultCell.Padding = 5;

            table.DefaultCell.NoWrap = false;

            document.Add(table);

            document.Close();

            fileStream.Close();

            return fileName;
        }


        private List<TourForPDFReport> GetToursForReport(DateTime start, DateTime end)
        {
            var tours = new List<TourForPDFReport>();
            var appointmentsInDateRange = GetAppointmentsInRange(start, end);

            foreach (var appointment in appointmentsInDateRange)
            {
                foreach (var tour in _tourService.GetAll())
                {
                    if (tour.Id == appointment.TourId)
                    {
                        var tourForPdfReport = new TourForPDFReport
                        {
                            Start = appointment.Start,
                            Language = tour.Language,
                            Duration = tour.Duration + "h",
                            Location = GetLocation(tour),
                            MaxNumOfGuests = tour.MaxNumOfGuests,
                            Name = tour.Name
                        };
                        tours.Add(tourForPdfReport);
                    }
                }
            }

            return tours;

        }

        private List<Appointment> GetAppointmentsInRange(DateTime start, DateTime end)
        {
            var appointmentsDownRange = _appointmentService.GetAllByUserId(App.LoggedUser.Id)
                .FindAll(a => a.Start >= start);

            var appointmentsInRange = appointmentsDownRange.FindAll(a => a.Start <= end);


            return appointmentsInRange;
        }

        private string GetLocation(Tour tour)
        {
            var location = _locationService.GetById(tour.LocationId);
            return (location.City + ", " + location.Country);
        }

    }
}
