using PdfSharp.Drawing;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Drawing.Text;
using System.IO;
using Image = iTextSharp.text.Image;
using iTextSharp.text.pdf.draw;
using iTextSharp.text.pdf;
using iTextSharp.text;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using System.Windows;
using Syncfusion.Pdf.Graphics;
using System.ComponentModel;
using SOSTeam.TravelAgency.WPF.Views.Guest1;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class ReportResultsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public string PdfTitle { get; set; }
        public string TextBlockText { get; set; }
        private bool canGeneratePdf { get; set; }
        public bool CanGeneratePdf
        {
            get { return canGeneratePdf; }
            set
            {
                canGeneratePdf = value;
                OnPropertyChaged("CanGeneratePdf");
            }
        }
        private bool canShowPdf { get; set; }
        public bool CanShowPdf
        {
            get { return canShowPdf; }
            set
            {
                canShowPdf = value;
                OnPropertyChaged("CanShowPdf");
            }
        }
        public NavigationService NavigationService { get; set; }
        public List<CancelAndMarkResDTO> _presentedReservations { get; set; }
        public RelayCommand GoBackCommand { get; set; }
        public RelayCommand GetPDFCommand { get; set; }
        public RelayCommand ShowPdfCommand { get; set; }

        public ReportResultsViewModel(List<CancelAndMarkResDTO> list, NavigationService service, int type, DateTime fDate, DateTime lDate)
        {
            _presentedReservations = list;
            NavigationService = service;
            TextBlockText = string.Empty;
            PdfTitle = string.Empty;
            CanGeneratePdf = true;
            CanShowPdf = false;

            FillTextBlock(type, fDate, lDate);
            CreateTitle(type, fDate, lDate);

            GoBackCommand = new RelayCommand(Execute_GoBack);
            GetPDFCommand = new RelayCommand(Execute_GetPDF);
            ShowPdfCommand = new RelayCommand(Execute_ShowPDF);
        }

        protected void OnPropertyChaged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Execute_ShowPDF(object sender)
        {
            string fileName = $"{TextBlockText}.pdf";
            NavigationService.Navigate(new ShowPDFPage($@"C:\\Users\\korisnik\\Desktop\\SIMS\\turisticka-agencija\\{fileName}", NavigationService));
        }

        private void Execute_GetPDF(object sender)
        {
            GeneratePDF();
            CanShowPdf = true;
            CanGeneratePdf = false;
            MessageBox.Show("PDF je uspješno kreiran. Možete ga pogledati klikom na dugme \"Prikaži PDF\".", "Potvrda", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void GeneratePDF()
        {
            Document document = new Document();
            string fileName = $"{TextBlockText}.pdf";
            FileStream fileStream = new FileStream($@"C:\Users\korisnik\Desktop\SIMS\turisticka-agencija\{fileName}", FileMode.Create);
            PdfWriter writer = PdfWriter.GetInstance(document, fileStream);

            document.Open();
            Paragraph dateParagraph = new Paragraph(DateTime.Now.ToString("dd.MM.yyyy"));
            dateParagraph.Alignment = Element.ALIGN_LEFT;
            document.Add(dateParagraph);
            Paragraph title = new Paragraph(PdfTitle);
            title.Alignment = Element.ALIGN_CENTER;
            title.Font.Size = 18;
            title.Font.SetStyle(Font.BOLDITALIC);
            document.Add(title);
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph($"Korisničko ime gosta: {App.LoggedUser.Username}"));
            document.Add(new Paragraph(" "));
            LineSeparator line = new LineSeparator();
            document.Add(line);
            document.Add(new Paragraph(" "));

            foreach (var item in _presentedReservations)
            {
                Paragraph littleTitle = new Paragraph("REZERVACIJA");
                littleTitle.Font.SetStyle(Font.BOLD);
                document.Add(littleTitle);
                document.Add(new Paragraph($"Naziv smještaja: {item.AccommodationName}"));
                document.Add(new Paragraph($"Grad: {item.AccommodationCity}"));
                document.Add(new Paragraph($"Država: {item.AccommodationCountry}"));
                document.Add(new Paragraph($"Tip: {item.TypeString}"));
                document.Add(new Paragraph($"Prvi dan: {item.FirstDayStr}"));
                document.Add(new Paragraph($"Poslednji dan: {item.LastDayStr}"));
                document.Add(new Paragraph(" "));
            }

            string logoPath = "C:\\Users\\korisnik\\Desktop\\SIMS\\turisticka-agencija\\TravelAgency\\Resources\\Images\\SOSlogo.png";
            Image logo = Image.GetInstance(logoPath);
            logo.Alignment = Element.ALIGN_RIGHT;
            logo.ScaleAbsolute(100, 100);

            float logoX = document.PageSize.Width - document.RightMargin - logo.ScaledWidth;
            float logoY = document.PageSize.Height - document.TopMargin - logo.ScaledHeight - 37;

            logo.SetAbsolutePosition(logoX, logoY);
            writer.DirectContent.AddImage(logo);

            document.Close();

            fileStream.Close();
        }

        private void CreateTitle(int type, DateTime fDate, DateTime lDate)
        {
            if (type == 0)
            {
                PdfTitle = "Prikaz zakaznih rezervacija u periodu\nod " + fDate.ToShortDateString() + " do " + lDate.ToShortDateString();
            }
            else
            {
                PdfTitle = "Prikaz otkazanih rezervacija u periodu\nod " + fDate.ToShortDateString() + " do " + lDate.ToShortDateString();
            }
        }

        private void FillTextBlock(int type, DateTime fDate, DateTime lDate)
        {
            if(type == 0)
            {
                TextBlockText = "Prikaz zakaznih rezervacija u periodu od " + fDate.ToShortDateString() + " do " + lDate.ToShortDateString();
            }
            else
            {
                TextBlockText = "Prikaz otkazanih rezervacija u periodu od " + fDate.ToShortDateString() + " do " + lDate.ToShortDateString();
            }
        }

        public void Execute_GoBack(object sender)
        {
            NavigationService.GoBack();
        }
    }
}
