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
using LiveCharts;
using LiveCharts.Wpf;
using System.Reflection;
using System.Globalization;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class PDFReportOwnerService
    {
        private readonly AccommodationStatsService _accommodationStatsService;
        private readonly LocationService _locationService;
        public PDFReportOwnerService()
        {
            _accommodationStatsService = new(App.LoggedUser.Id);
            _locationService = new();
        }

        public string GeneratePDFReport(Accommodation accommodation)
        {
            Document document = new Document();

            string directoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string fileName = $"AccommodationReport_{accommodation.Name.Replace(" ", "_")}_{DateTime.Now.ToString("dd.MM.yyyy mm:hh").Replace(" ", "_").Replace(":", string.Empty).Replace(".", string.Empty)}.pdf";

            string filePath = Path.Combine(directoryPath, fileName);

            // Open the FileStream using the specified file path
            FileStream fileStream = new FileStream(filePath, FileMode.Create);

            PdfWriter writer = PdfWriter.GetInstance(document, fileStream);

            document.Open();

            Paragraph dateParagraph = new Paragraph(DateTime.Now.ToString("dd.MM.yyyy."));
            dateParagraph.Alignment = Element.ALIGN_LEFT;
            document.Add(dateParagraph);

            Paragraph title = new Paragraph("Izveštaj o smeštaju " + accommodation.Name + " \nza godinu " + DateTime.Now.ToString("yyyy") );
            title.Alignment = Element.ALIGN_CENTER;
            title.Font.Size = 20;
            document.Add(title);

            document.Add(new Paragraph(" "));

            Paragraph userInfoHeader = new Paragraph("");
            userInfoHeader.Font.SetStyle(Font.BOLD);
            document.Add(userInfoHeader);
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph($"Korisnik: {App.LoggedUser.Username}"));
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

            document.Add(new Paragraph(" "));
            document.Add(new Paragraph(""));
            document.Add(new Paragraph(" "));

            var table = GetTable(accommodation);

            document.Add(table);

            document.Close();

            fileStream.Close();

            return fileName;
        }
       

        private PdfPTable GetTable(Accommodation accommodation) 
        {
            PdfPTable table = new PdfPTable(5);

            var year = new DateTime(DateTime.Today.Year, 1, 1);

            var regular = _accommodationStatsService.GetOccupationInMonths(year, accommodation.Id);

            var moved = _accommodationStatsService.GetReservationMovesInMonths(year, accommodation.Id);

            var changed = _accommodationStatsService.GetCancelationInMonths(year, accommodation.Id);

            var renovation = _accommodationStatsService.GetReservationMovesInMonths(year, accommodation.Id);

            var temp = new List<string>();
            CultureInfo serbianCulture = new CultureInfo("sr-RS");
            for (int i = 0; i < 12; i++)
            {
                temp.Add(year.AddMonths(i).ToString("MMMM"));
            }

            table.WidthPercentage = 100;

            table.AddCell(new PdfPCell(new Phrase("Mesec")) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Broj rezervacija")) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Broj pomeranja rezervacije")) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Broj otkazivanja rezervacije")) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Broj renoviranja")) { HorizontalAlignment = Element.ALIGN_CENTER });



            for(int i = 0; i < 12; i++)
            {
                table.AddCell(new PdfPCell(new Phrase(temp[i])) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase(regular[i].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase(moved[i].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase(changed[i].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase(renovation[i].ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
            }

            table.DefaultCell.Padding = 5;

            table.DefaultCell.NoWrap = false;

            return table;
        }


        private string GetLocation(Tour tour)
        {
            var location = _locationService.GetById(tour.LocationId);
            return (location.City + ", " + location.Country);
        }

    }
}
