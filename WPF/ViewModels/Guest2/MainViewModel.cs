using Microsoft.Win32;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views;
using SOSTeam.TravelAgency.WPF.Views.Guest2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class MainViewModel : ViewModel
    {
        public NavigationService NavigationService { get; set; }

        public event EventHandler CloseRequested;
        public static User LoggedInUser { get; set; }
        public static ObservableCollection<Tour> Tours { get; set; }
        public static ObservableCollection<Tour> SuperGuideTours { get; set; }
        public static ObservableCollection<Location> Locations { get; set; }
        public List<GuestAttendance> UserAttendances { get; set; }

        public static ObservableCollection<TourViewModel> TourViewModels { get; set; }
        public static ObservableCollection<TourViewModel> SerbiaTourViewModels { get; set; }
        public static ObservableCollection<TourViewModel> SummerTourViewModels { get; set; }

        public static ArrowCommandsViewModel ArrowCommands { get; set; }

        private string _iconPath;

        public string IconPath
        {
            get { return _iconPath; }
            set
            {
                _iconPath = value;
                OnPropertyChanged(nameof(IconPath));
            }
        }


        private TourService _tourService;
        private LocationService _locationService;
        private GuestAttendanceService _guestAttendanceService;
        private ReservationService _reservationService;
        private AppointmentService _appointmentService;
        private ImageService _imageService;
        private TourRequestService _tourRequestService;
        private VoucherService _voucherService;
        private FrequentUserVoucherService _frequentUserVoucherService;
        private SuperGuideService _superGuideService;

        private RelayCommand _navigationCommand;
        public RelayCommand NavigationCommand
        {
            get { return _navigationCommand; }
            set
            {
                _navigationCommand = value;
            }
        }

        private RelayCommand _vouchersCommand;

        public RelayCommand VouchersCommand
        {
            get { return _vouchersCommand; }
            set
            {
                _vouchersCommand = value;
            }
        }

        private RelayCommand _notificationsCommand;

        public RelayCommand NotificationsCommand
        {
            get { return _notificationsCommand; }
            set
            {
                _notificationsCommand = value;
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

        private RelayCommand _logOutCommand;

        public RelayCommand LogOutCommand
        {
            get { return _logOutCommand; }
            set
            {
                _logOutCommand = value;
            }
        }
        public MainViewModel(User loggedInUser, NavigationService navigationService)
        {
            NavigationService = navigationService;
            InitializeServices();
            GetUsableLists();
            SuperGuideTours = new ObservableCollection<Tour>();
            AddSuperGuidesTours();
            LoggedInUser = loggedInUser;
            TourViewModels = new ObservableCollection<TourViewModel>();
            SummerTourViewModels = new ObservableCollection<TourViewModel>();
            SerbiaTourViewModels = new ObservableCollection<TourViewModel>();
            CreateCommands();
            CheckNotifications();
            FillTourViewModelList();
            FillSerbiaTourViewModelList();
            FillSummerTourViewModelList();
            ArrowCommands = new ArrowCommandsViewModel(TourViewModels,SerbiaTourViewModels,SummerTourViewModels);
        }

        public void GiveVoucherToFrequentUser()
        {
            List<Reservation> usersReservations = _reservationService.GetFulfilledReservations(LoggedInUser);
            int count = CountThisYearUserReservations(usersReservations);

            if(_frequentUserVoucherService.GetAll().Count() == 0)
            {
                if(count >= 5)
                {
                    CreateNewVoucher();
                }
            }
            else
            {
                bool voucherExists = CheckIfVoucherAlreadyExists();
                if (!voucherExists && count >= 5)
                {
                    CreateNewVoucher();
                }
            }

        }

        private bool CheckIfVoucherAlreadyExists()
        {
            bool voucherExists = false;
            foreach (var frequentUserVoucher in _frequentUserVoucherService.GetAll())
            {
                if (frequentUserVoucher.UserId == LoggedInUser.Id && frequentUserVoucher.Year == DateTime.Now.Year.ToString())
                {
                    voucherExists = true;
                    break;
                }
            }

            return voucherExists;
        }

        private void CreateNewVoucher()
        {
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            DateTime expiryDateTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day).AddMonths(6);
            DateOnly expiryDate = DateOnly.FromDateTime(expiryDateTime);
            Voucher newVoucher = new Voucher(false, expiryDate, LoggedInUser.Id, -1);
            _voucherService.Save(newVoucher);
            FrequentUserVoucher newFrequentUserVoucher = new FrequentUserVoucher(newVoucher.Id, LoggedInUser.Id, DateTime.Now.Year.ToString());
            _frequentUserVoucherService.Save(newFrequentUserVoucher);
            MessageBox.Show("Osvojili ste vaucer! U toku godine ste posetili najmanje 5 tura. Vaucer mozete pogledati u prozoru sa Vasim vaucerima.", "Novi vaucer", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private int CountThisYearUserReservations(List<Reservation> usersReservations)
        {
            int count = 0;

            foreach (var reservation in usersReservations)
            {
                if (_appointmentService.GetById(reservation.AppointmentId).Start.Year == DateTime.Now.Year) count++;
            }

            return count;
        }

        public void CheckNotifications()
        {
            bool isFinished = IsFinishedToursExist();

            if (!isFinished)
            {
                IconPath = "/Resources/Icons/notification.png";
            }
            else
            {
                IconPath = "/Resources/Icons/new_notifications.png";
            }
        }

        private bool IsFinishedToursExist()
        {
            bool isFinished = false;
            foreach (Reservation reservation in _reservationService.GetAll())
            {
                if (reservation.Presence && _appointmentService.GetById(reservation.AppointmentId).Finished && reservation.Reviewed == false)
                {
                    isFinished = true;
                    break;
                }
            }

            return isFinished;
        }

        private void FindImage(Tour t,Location l, ObservableCollection<TourViewModel> tourViewModels)
        {
            foreach(Image i in _imageService.GetAll())
            {
                if(i.Type.Equals(ImageType.TOUR) && i.EntityId == t.Id && i.Cover)
                {
                    TourViewModel tourDTO = new TourViewModel(t.Id, t.Name, t.Language, t.Duration, t.MaxNumOfGuests, l.City, l.Country, LoggedInUser,i.Path);
                    tourViewModels.Add(tourDTO);
                    break;
                }
            }
        }

        private void AddSuperGuidesTours()
        {
            foreach(var superGuide in _superGuideService.GetAll())
            {
                foreach(var appointment in _appointmentService.GetAll())
                {
                    if(superGuide.UserId == appointment.UserId)
                    {
                        SuperGuideTours.Add(_tourService.GetById(appointment.TourId));
                    }
                }
            }

            SuperGuideTours = new ObservableCollection<Tour>(SuperGuideTours.DistinctBy(t => t.Id));
        }

        private void FillSummerTourViewModelList()
        {
            DateTime summerStart = new DateTime(DateTime.Now.Year, 6, 21);
            DateTime summerEnd = new DateTime(DateTime.Now.Year, 9, 23);

            foreach (Tour t in SuperGuideTours)
            {
                foreach (Location l in Locations)
                {
                    if (l.Id == t.LocationId)
                    {
                        FillList(summerStart, summerEnd, t, l);
                    }
                }
            }

            List<int> superGuideToursIds = new List<int>();
            foreach (var t in SuperGuideTours)
            {
                superGuideToursIds.Add(t.Id);
            }

            foreach (Tour t in Tours)
            {
                if (!superGuideToursIds.Contains(t.Id))
                {
                    foreach (Location l in Locations)
                    {
                        if (l.Id == t.LocationId)
                        {
                            FillList(summerStart, summerEnd, t, l);
                        }
                    }
                }
            }
        }

        private void FillList(DateTime summerStart, DateTime summerEnd, Tour t, Location l)
        {
            foreach (Appointment a in _appointmentService.GetAll())
            {
                if (a.TourId == t.Id && a.Start >= summerStart && a.Start <= summerEnd)
                {
                    FindImage(t, l,SummerTourViewModels);
                    break;
                }
            }
        }

        private void FillSerbiaTourViewModelList()
        {
            foreach (Tour t in SuperGuideTours)
            {
                foreach (Location l in Locations)
                {
                    if (l.Id == t.LocationId && l.Country == "Srbija")
                    {
                        FindImage(t, l,SerbiaTourViewModels);
                    }
                }
            }

            List<int> superGuideToursIds = new List<int>();
            foreach (var t in SuperGuideTours)
            {
                superGuideToursIds.Add(t.Id);
            }

            foreach (Tour t in Tours)
            {
                if (!superGuideToursIds.Contains(t.Id))
                {
                    foreach (Location l in Locations)
                    {
                        if (l.Id == t.LocationId && l.Country == "Srbija")
                        {
                            FindImage(t, l, SerbiaTourViewModels);
                        }
                    }
                }
            }
        }

        private void CreateCommands()
        {
            NavigationCommand = new RelayCommand(Execute_NavigationCommand, CanExecuteMethod);
            VouchersCommand = new RelayCommand(Execute_VouchersWindowCommand, CanExecuteMethod);
            NotificationsCommand = new RelayCommand(Execute_NotificationsWindowCommand, CanExecuteMethod);
            LogOutCommand = new RelayCommand(Execute_LogOutCommand, CanExecuteMethod);
            HelpCommand = new RelayCommand(Execute_HelpCommand, CanExecuteMethod);
        }

        private MessageBoxResult ConfirmLogOut()
        {
            string sMessageBoxText = $"Da li ste sigurni da želite da se odjavite?";
            string sCaption = "Porvrda odjave";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Question;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }
        private void Execute_LogOutCommand(object obj)
        {
            MessageBoxResult result = ConfirmLogOut();

            if(result == MessageBoxResult.Yes)
            {
                SignInForm signInForm = new SignInForm();
                signInForm.Show();
                CloseRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        private void Execute_NotificationsWindowCommand(object obj)
        {
            NotificationsWindow window = new NotificationsWindow(LoggedInUser);
            window.ShowDialog();
        }

        private void Execute_VouchersWindowCommand(object obj)
        {
            VouchersWindow window = new VouchersWindow(LoggedInUser);
            window.ShowDialog();
        }

        private void Execute_NavigationCommand(object obj)
        {
            string nextPage = obj.ToString();

            switch (nextPage)
            {
                case "MyTours":
                    NavigationService.Navigate(new MyToursPage(LoggedInUser));
                    break;
                case "Requests":
                    NavigationService.Navigate(new RequestsPage(LoggedInUser));
                    break;
                case "Search":
                    NavigationService.Navigate(new SearchPage(LoggedInUser,TourViewModels, SerbiaTourViewModels, SummerTourViewModels));
                    break;
            }
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
        private void GetUsableLists()
        {
            Locations = new ObservableCollection<Location>(_locationService.GetAll());
            Tours = new ObservableCollection<Tour>(_tourService.GetAll());
            UserAttendances = new List<GuestAttendance>(_guestAttendanceService.GetAll());
        }

        private void InitializeServices()
        {
            _tourService = new TourService();
            _locationService = new LocationService();
            _guestAttendanceService = new GuestAttendanceService();
            _reservationService = new ReservationService();
            _appointmentService= new AppointmentService();
            _imageService = new ImageService();
            _tourRequestService= new TourRequestService();
            _voucherService = new VoucherService();
            _frequentUserVoucherService = new FrequentUserVoucherService();
            _superGuideService = new SuperGuideService();
        }

        public void GetAcceptedRequestMessage()
        {
            foreach(var request in _tourRequestService.GetAll())
            {
                if(request.Status == StatusType.ACCEPTED && !request.IsNotificationViewed && request.UserId == LoggedInUser.Id && request.ComplexTourRequestId == -1)
                {
                    MessageBox.Show("Vas zahtev za turom je prihvacen. Novu turu mozete pronaci u prozoru sa svim turama (mozete pogledati i rezervisati odredjeni termin)","Obavestenje", MessageBoxButton.OK,MessageBoxImage.Information);
                    request.IsNotificationViewed = true;
                    _tourRequestService.Update(request);
                }
            }
        }
        public void GetAttendanceMessage()
        {
            foreach (var attendance in UserAttendances)
            {
                if (attendance.Presence.Equals(GuestPresence.UNKNOWN) && attendance.UserId == LoggedInUser.Id)
                {
                    ShowAttendanceMessage(attendance);
                }
            }
        }

        private void ShowAttendanceMessage(GuestAttendance attendance)
        {
            MessageBoxResult result = MessageBox.Show(attendance.Message, "Prisutnost",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                attendance.Presence = GuestPresence.YES;
                _guestAttendanceService.Update(attendance);
                _reservationService.SetPresence(attendance.ReservationId);
            }
            else
            {
                attendance.Presence = GuestPresence.NO;
                _guestAttendanceService.Update(attendance);                
            }
        }

        private void FillTourViewModelList()
        {
            foreach (Tour t in SuperGuideTours)
            {
                foreach (Location l in Locations)
                {
                    if (l.Id == t.LocationId)
                    {
                        FindImage(t, l,TourViewModels);
                    }
                }
            }

            List<int> superGuideToursIds = new List<int>();
            foreach(var t in SuperGuideTours)
            {
                superGuideToursIds.Add(t.Id);
            }

            foreach(Tour t in Tours)
            {
                if (!superGuideToursIds.Contains(t.Id))
                {
                    foreach (Location l in Locations)
                    {
                        if (l.Id == t.LocationId)
                        {
                            FindImage(t, l, TourViewModels);
                        }
                    }
                }
            }
        }
    }
}
