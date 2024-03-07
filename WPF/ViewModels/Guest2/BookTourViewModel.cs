using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text.pdf.draw;
using Image = iTextSharp.text.Image;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class BookTourViewModel : ViewModel
    {
        public event EventHandler CloseRequested;
        public User LoggedInUser { get; set; }
        public Tour Tour { get; set; }
        private int _availableSlots;

        private VouchersViewModel _selectedVoucher;

        public VouchersViewModel SelectedVoucher
        {
            get { return _selectedVoucher; }
            set
            {
                _selectedVoucher = value;
                OnPropertyChanged("SelectedVoucher");
            }
        }

        private AppoitmentOverviewViewModel _selected;

        public AppoitmentOverviewViewModel Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                OnPropertyChanged("Selected");
            }
        }

        public static ObservableCollection<Appointment> Appointments;
        private readonly AppointmentService _appointmentService;
        private readonly ReservationService _reservationService;
        private readonly TourService _tourService;
        private readonly VoucherService _voucherService;
        private readonly LocationService _locationService;

        public static ObservableCollection<AppoitmentOverviewViewModel> AppoitmentOverviewViewModels { get; set; }
        public static ObservableCollection<VouchersViewModel> Vouchers { get; set; }

        private RelayCommand _backCommand;
        public RelayCommand BackCommand
        {
            get { return _backCommand; }
            set
            {
                _backCommand = value;
            }
        }

        private RelayCommand _reserveCommand;

        public RelayCommand ReserveCommand
        {
            get { return _reserveCommand; }
            set
            {
                _reserveCommand = value;
            }
        }

        private RelayCommand _helpCommand;

        public RelayCommand HelpCommand
        {
            get { return _helpCommand; }
            set
            {
                _helpCommand = value;
            }
        }
        public int AvailableSlots
        {
            get { return _availableSlots; }
            set
            {
                if (value != _availableSlots)
                {
                    _availableSlots = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _touristNum;
        public string TouristNum
        {
            get { return _touristNum; }
            set
            {
                _touristNum = value;
                ValidateTouristNum();
                OnPropertyChanged(nameof(TouristNum));
            }
        }

        private string _averageAge;
        public string AverageAge
        {
            get { return _averageAge; }
            set
            {
                _averageAge = value;
                ValidateAverageAge();
                OnPropertyChanged(nameof(AverageAge));
            }
        }

        private Regex _ageRegex = new Regex(@"^(?!0*(\.0+)?$)\d+(\.\d+)?$");
        private Regex _touristNumRegex = new Regex(@"^[1-9][0-9]*$");

        private bool _isAverageAgeValid;
        public bool IsAverageAgeValid
        {
            get => _isAverageAgeValid;
            set
            {
                _isAverageAgeValid = value;
                OnPropertyChanged(nameof(IsAverageAgeValid));
            }
        }

        private bool _isTouristNumValid;
        public bool IsTouristNumValid
        {
            get => _isTouristNumValid;
            set
            {
                _isTouristNumValid = value;
                OnPropertyChanged(nameof(IsTouristNumValid));
            }
        }
        private void ValidateAverageAge()
        {
            if (!string.IsNullOrEmpty(AverageAge))
            {
                IsAverageAgeValid = _ageRegex.IsMatch(AverageAge);
            }
        }

        private void ValidateTouristNum()
        {
            if (!string.IsNullOrEmpty(TouristNum))
            {
                IsTouristNumValid = _touristNumRegex.IsMatch(TouristNum);
            }
        }
        public BookTourViewModel(int id, User loggedInUser)
        {
            LoggedInUser = loggedInUser;
            _appointmentService = new AppointmentService();
            _reservationService = new ReservationService();
            _tourService = new TourService();
            _voucherService = new VoucherService();
            _locationService = new LocationService();
            Appointments = new ObservableCollection<Appointment>(_appointmentService.GetAll());
            AppoitmentOverviewViewModels = new ObservableCollection<AppoitmentOverviewViewModel>();
            Vouchers = new ObservableCollection<VouchersViewModel>();
            Tour = _tourService.FindTourById(id);
            BackCommand = new RelayCommand(Execute_CancelCommand, CanExecuteMethod);
            HelpCommand = new RelayCommand(Execute_HelpCommand, CanExecuteMethod);
            ReserveCommand = new RelayCommand(Execute_ReserveCommand, CanExecuteMethod);
            FillAppoitmentViewModelList();
            FillVouchersForShowingList();
        }

        private void Execute_HelpCommand(object obj)
        {
            HelpWindow window = new HelpWindow();
            window.Show();
        }
        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void Execute_CancelCommand(object sender)
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }
        private void Execute_ReserveCommand(object sender)
        {

            if (IsDataCorrect())
            {
                MessageBoxResult result = ConfirmReservation();
                ReservationConfirmedEvent(result);
            }
        }

        private bool IsDataCorrect()
        {
            bool isCorrect = true;
            if (Selected == null)
            {
                MessageBox.Show("Niste odabrali termin", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
                isCorrect = false;
            }
            else if (!IsTouristNumValid || _touristNum == "" || _touristNum == null)
            {
                MessageBox.Show("Broj turista nije unet u dobrom formatu", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
                isCorrect = false;
            }
            else if (!IsAverageAgeValid || _averageAge == "" || _averageAge == null)
            {
                MessageBox.Show("Prosecan broj godina nije unet u dobrom formatu", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
                isCorrect = false;
            }
            else if (int.Parse(_touristNum) > _availableSlots)
            {
                MessageBox.Show("Ne moze se rezervisati tura, nema dovoljno slobodnih mesta", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
                isCorrect = false;
            }
            return isCorrect;
        }
        private void ReservationConfirmedEvent(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (_selectedVoucher != null)
                {
                    _voucherService.UsedUpdate(_selectedVoucher.VoucherId);
                    Reservation newReservation = _reservationService.CreateReservation(_selected, LoggedInUser, int.Parse(_touristNum), float.Parse(_averageAge), _selectedVoucher.VoucherId);
                    GeneratePDFReport(newReservation);
                    MessageBox.Show("Rezervacija je uspesno kreirana, izvestaj o rezervaciji mozete pogledati klikom na dugme 'pogledaj izvestaj' u prozoru gde se nalaze vase rezervacije", "Izvestaj", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Reservation newReservation = _reservationService.CreateReservation(_selected, LoggedInUser, int.Parse(_touristNum), float.Parse(_averageAge, CultureInfo.InvariantCulture));
                    GeneratePDFReport(newReservation);
                    MessageBox.Show("Rezervacija je uspesno kreirana, izvestaj o rezervaciji mozete pogledati klikom na dugme 'pogledaj izvestaj' u prozoru gde se nalaze vase rezervacije", "Izvestaj", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                CloseRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        private MessageBoxResult ConfirmReservation()
        {
            string sMessageBoxText = $"Da li ste sigurni da želite da rezervisete turu";
            string sCaption = "Porvrda rezervacije";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }

        public void FillAppoitmentViewModelList()
        {
            foreach(Appointment appointment in Appointments)
            {
                if(appointment.TourId == Tour.Id && appointment.Start > DateTime.Now)
                {
                    AvailableSlots = Tour.MaxNumOfGuests - appointment.Occupancy;
                    AppoitmentOverviewViewModel viewModel = new AppoitmentOverviewViewModel(appointment.Start, AvailableSlots, Tour.Id);
                    AppoitmentOverviewViewModels.Add(viewModel);
                }
            }
        }

        private void FillVouchersForShowingList()
        {
            foreach (Voucher voucher in _voucherService.GetAll())
            {
                if (voucher.ExpiryDate > DateOnly.FromDateTime(DateTime.Today) && voucher.Used == false && voucher.UserId == LoggedInUser.Id && voucher.GuideId == -1)
                {
                    Vouchers.Add(new VouchersViewModel(voucher.Id, voucher.ExpiryDate));
                }
                else if(voucher.ExpiryDate > DateOnly.FromDateTime(DateTime.Today) && voucher.Used == false && voucher.UserId == LoggedInUser.Id && voucher.GuideId == _appointmentService.GetTourGuideId(Tour.Id))
                {
                    Vouchers.Add(new VouchersViewModel(voucher.Id, voucher.ExpiryDate));
                }
            }
        }

        private void GeneratePDFReport(Reservation reservationForReport)
        {
            Location reservedTourLocation = _locationService.GetById(Tour.LocationId);
            Document document = new Document();

            string fileName = $"report_{reservationForReport.Id}.pdf";
            FileStream fileStream = new FileStream($@"D:\Desktop1\Drugi deo projekta\turisticka-agencija\TravelAgency\Report\Guest2-reports\{fileName}", FileMode.Create);

            PdfWriter writer = PdfWriter.GetInstance(document, fileStream);

            document.Open();

            Paragraph dateParagraph = new Paragraph(DateTime.Now.ToString("dd.MM.yyyy"));
            dateParagraph.Alignment = Element.ALIGN_LEFT;
            document.Add(dateParagraph);

            Paragraph title = new Paragraph("Izvestaj o rezervaciji!");
            title.Alignment = Element.ALIGN_CENTER;
            title.Font.Size = 20; 
            document.Add(title);

            document.Add(new Paragraph(" "));

            Paragraph userInfoHeader = new Paragraph("Informacije o gostu");
            userInfoHeader.Font.SetStyle(Font.BOLD); 
            document.Add(userInfoHeader);
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph($"Korisnicko ime gosta: {LoggedInUser.Username}"));
            document.Add(new Paragraph(" "));

            LineSeparator line = new LineSeparator();
            document.Add(line);
            document.Add(new Paragraph(" "));

            Paragraph reservationInfoHeader = new Paragraph("Informacije o rezervisanoj turi");
            reservationInfoHeader.Font.SetStyle(Font.BOLD); 
            document.Add(reservationInfoHeader);


            document.Add(new Paragraph(" "));
            document.Add(new Paragraph($"Naziv ture: {Tour.Name}"));
            document.Add(new Paragraph($"Lokacija: {reservedTourLocation.City}, {reservedTourLocation.Country}"));
            document.Add(new Paragraph($"Jezik: {Tour.Language}"));
            document.Add(new Paragraph($"Broj ljudi: {reservationForReport.TouristNum}"));
            document.Add(new Paragraph($"Datum polaska: {_appointmentService.GetById(reservationForReport.AppointmentId).Start}"));
            document.Add(new Paragraph($"Trajanje: {Tour.Duration}h"));

            string logoPath = "D:\\Desktop1\\Drugi deo projekta\\turisticka-agencija\\TravelAgency\\Resources\\Images\\SOSlogo.png";
            Image logo = Image.GetInstance(logoPath);
            logo.Alignment = Element.ALIGN_RIGHT;
            logo.ScaleAbsolute(100, 100); 

            float logoX = document.PageSize.Width - document.RightMargin - logo.ScaledWidth;
            float logoY = document.PageSize.Height - document.TopMargin - logo.ScaledHeight;

            logo.SetAbsolutePosition(logoX, logoY);
            writer.DirectContent.AddImage(logo);

            document.Close();

            fileStream.Close();

        }
    }
}
