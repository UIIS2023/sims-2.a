using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.WPF.Views;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.Intrinsics.X86;
using System.Windows.Documents;
using System.Windows.Shapes;
using System.Windows.Navigation;
using SOSTeam.TravelAgency.Domain.DTO;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class UserProfilleViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public User LoggedInUser { get; set; }
        public NavigationService NavigationService { get; set; }
        public int ThisYearCounter { get; set; }
        public int Notifications { get; set; }
        public int WizardCounter { get; set; }
        public string StatusesInfo { get; set; }
        public string ReportInfo { get; set; }
        public string FinishedReservationsInfo { get; set; }
        public string FuturedReservationsInfo { get; set; }
        public string UsernameTextBlock { get; set; }
        public string MessageNumberTextBlock { get; set; }
        public string CounterTextBlock { get; set; }
        public string SuperGuestBonusPoints { get; set; }
        public string SuperGuestReservations { get; set; }
        public string SuperGuestConclusion { get; set; }
        public string SuperGuestInformation { get; set; }
        public string ForumInfo { get; set; }
        public string InboxInfo { get; set; }
        public string MenuInfo { get; set; }
        public bool FirstLogging { get; set; }
        
        private bool isPopupOpen;
        public bool IsPopupOpen
        {
            get { return isPopupOpen; }
            set
            {
                isPopupOpen = value;
                OnPropertyChaged("IsPopupOpen");
            }
        }

        private bool futuredDataGridPopup;
        public bool FuturedDataGridPopup
        {
            get { return futuredDataGridPopup; }
            set
            {
                futuredDataGridPopup = value;
                OnPropertyChaged("FuturedDataGridPopup");
            }
        }
        private bool finishedDataGridPopup;
        public bool FinishedDataGridPopup
        {
            get { return finishedDataGridPopup; }
            set
            {
                finishedDataGridPopup = value;
                OnPropertyChaged("FinishedDataGridPopup");
            }
        }

        private bool infoButtons;
        public bool InfoButtons
        {
            get { return infoButtons; }
            set
            {
                infoButtons = value;
                OnPropertyChaged("InfoButtons");
            }
        }

        private bool statusPopup;
        public bool StatusPopup
        {
            get { return statusPopup; }
            set
            {
                statusPopup = value;
                OnPropertyChaged("StatusPopup");
            }
        }
        private bool reportPopup;
        public bool ReportPopup
        {
            get { return reportPopup; }
            set
            {
                reportPopup = value;
                OnPropertyChaged("ReportPopup");
            }
        }

        private bool forumPopup;
        public bool ForumPopup
        {
            get { return forumPopup; }
            set
            {
                forumPopup = value;
                OnPropertyChaged("ForumPopup");
            }
        }
        private bool menuPopup;
        public bool MenuPopup
        {
            get { return menuPopup; }
            set
            {
                menuPopup = value;
                OnPropertyChaged("MenuPopup");
            }
        }
        private bool inboxPopup;
        public bool InboxPopup
        {
            get { return inboxPopup; }
            set
            {
                inboxPopup = value;
                OnPropertyChaged("InboxPopup");
            }
        }

        private bool commandsEnabled;
        public bool CommandsEnabled
        {
            get { return commandsEnabled; }
            set
            {
                commandsEnabled = value;
                OnPropertyChaged("CommandsEnabled");
            }
        }
        public List<CancelAndMarkResDTO> _futuredReservations { get; set; }
        public List<CancelAndMarkResDTO> _finishedReservations { get; set; }
        public List<AccommodationReservation> _accommodationReservations { get; set; }
        public ObservableCollection<LocAccommodationDTO> _locAccommodationViewModels { get; set; }
        public RelayCommand ShowMenuCommand { get; set; }
        public RelayCommand ShowRequestsCommand { get; set; }
        public RelayCommand ShowInboxCommand { get; set; }
        public RelayCommand SignOutCommand { get; set; }
        public RelayCommand CanceledQueryCommand { get; set; }
        public RelayCommand SuperGuestInfoCommand { get; set; }
        public RelayCommand OkCommand { get; set; }
        public RelayCommand ShowForumsCommand { get; set; }


        public UserProfilleViewModel(User user, int notifications, NavigationService service) 
        {
            LoggedInUser = user;
            UsernameTextBlock = user.Username;
            ThisYearCounter = 0;
            NavigationService = service;
            Notifications = notifications;
            InitializeBooleans();
            WizardCounter = 0;

            _futuredReservations = new List<CancelAndMarkResDTO>();
            _finishedReservations = new List<CancelAndMarkResDTO>();

            AccommodationReservationService accResService = new AccommodationReservationService();
            _accommodationReservations = accResService.GetAll();

            AccommodationService AccommodationService = new AccommodationService();
            _locAccommodationViewModels = AccommodationService.CreateAllDTOForms();

            FillPopups();
            ApplyToThisYearCounter(_accommodationReservations);
            ControlInboxButton(Notifications);
            AddFuturedReservations();
            CollectFinishedReservations();
            AddFinishedReservations();
            FillCounterTextBlock();
            IsFirstLogging();

            SuperGuestService superGuestService = new SuperGuestService();
            superGuestService.CreateSuperGuestAccounts();
            FindSuperGuestInformations();

            ShowMenuCommand = new RelayCommand(Execute_ShowMenu);
            ShowRequestsCommand = new RelayCommand(Execute_ShowStatuses);
            ShowInboxCommand = new RelayCommand(Execute_ShowInbox);
            SignOutCommand = new RelayCommand(Execute_SignOut);
            CanceledQueryCommand = new RelayCommand(Execute_ShowCanceledReservations);
            SuperGuestInfoCommand = new RelayCommand(Execute_OpenPopup);
            OkCommand = new RelayCommand(Execute_OK);
            ShowForumsCommand = new RelayCommand(Execute_ShowForums);
        }

        private void InitializeBooleans()
        {
            FirstLogging = false;
            IsPopupOpen = false;
            InfoButtons = true;
            FuturedDataGridPopup = false;
            FinishedDataGridPopup = false;
            CommandsEnabled = true;
            StatusPopup = false;
            ReportPopup = false;
            ForumPopup = false;
            InboxPopup = false;
            MenuPopup = false;
        }

        private void Execute_ShowForums(object sender)
        {
            NavigationService.Navigate(new AllForumsPage(LoggedInUser, NavigationService));
        }

        private void Execute_OK(object sender)
        {
            if (WizardCounter == 1)
            {
                FuturedDataGridPopup = false;
                FinishedDataGridPopup = true;
                WizardCounter++;
            }
            else if (WizardCounter == 2)
            {
                FinishedDataGridPopup = false;
                StatusPopup = true;
                WizardCounter++;
            }
            else if (WizardCounter == 3)
            {
                StatusPopup = false;
                ReportPopup = true;
                WizardCounter++;
            }
            else if (WizardCounter == 4)
            {
                ReportPopup = false;
                ForumPopup = true;
                WizardCounter++;
            }
            else if (WizardCounter == 5)
            {
                ForumPopup = false;
                InboxPopup = true;
                WizardCounter++;
            }
            else if (WizardCounter == 6)
            {
                InboxPopup = false;
                MenuPopup = true;
                WizardCounter++;
            }
            else if (WizardCounter == 7)
            {
                MenuPopup = false;
                MessageBox.Show("                   Uživajte u našoj aplikaciji.\nŽelimo Vam puno dobrih i nezaboravnih iskustava!", "Možete započeti korišćenje", MessageBoxButton.OK, MessageBoxImage.Information);
                CommandsEnabled = true;
                InfoButtons = true;
                WizardCounter = 0;
            }
        }

        private void FillPopups()
        {
            SuperGuestInformation = "Gost može postati super-gost ako u prethodnoj godini ima bar 10 rezervacija.\n"
                                  + "Super-gost titula traje godinu dana i prestaje da važi ako gost\n"
                                  + "ne bude ponovo zadovoljio uslov od 10 rezervacija.\n"
                                  + "Super-gost dobija 5 bonus poena koje može potrošiti u tekućoj godini,\n"
                                  + "nakon čega se bodovi resetuju na 0 (ne mogu se akumulirati).\n"
                                  + "Prilikom svake naredne rezervacije se troši jedan bonus poen što donosi popuste,\n"
                                  + "što znači da će super-gost imati 5 rezervacija sa popustom.";
            FinishedReservationsInfo = "U tabeli \"Vaše dosadašnje rezervacije\" će biti prikazane sve Vaše realizovane rezervacije.";
            FuturedReservationsInfo = "U tabeli \"Vaše predstojeće rezervacije\" će biti prikazane sve Vaše zakazane rezervacije.";
            StatusesInfo = "U odjeljku \"Status Vaših zahtjeva\" ćete imati uvid u statuse Vaših rezervacija\nkoje želite pomjeriti, "
                         + "kao i mogućnost otkazivanja zakaznih rezervacija.";
            ReportInfo = "Klikom na link \"izvještaj\" se otvara stranica za unošenje filtera, na osnovu\n"
                       + "kojih će Vaš izvještaj o svim zakazanim ili otkazanim rezervacijama (zavisno šta\n"
                       + "prethodno izaberete) biti formiran. Takođe ćete imati mogućnost preuzimanja istog u formi PDF fajla.";
            ForumInfo = "Iza dugmeta \"Forumi\" se nalazi lista svih foruma, među kojima su posebno naglašeni\n"
                      + "forumi koje ste Vi otvorili. Takođe, ispod pomenutih listi se nalazi forma za otvaranje novog foruma";
            InboxInfo = "U Vašem inboksu se nalaze sva obavještenja koja su upućena Vama:\n"
                      + "podsjetnici za ocjenjivanje smještaja u kom ste odsjeli, obavještenja o\n"
                      + "vlasnikovim odgovorima na Vaše zahtjeve za pomjeranje rezervacija,\nkao i recenzije koje Vam je vlasnik uputio.";
            MenuInfo = "Klikom na dugme \"Glavni meni\" otvoriće se stranica preko koje možete\n"
                     + "pristupiti pretrazi, prikazu i rezervaciji smještaja.";
        }

        private void Execute_OpenPopup(object sender)
        {
            if (!IsPopupOpen)
            {
                IsPopupOpen = true;
                CommandsEnabled = false;
            }
            else
            {
                IsPopupOpen = false;
                CommandsEnabled = true;
            }
        }

        protected void OnPropertyChaged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void FindSuperGuestInformations()
        {
            SuperGuestService superGuestService = new SuperGuestService();
            List<SuperGuest> superGuests = superGuestService.GetAll();
            foreach(SuperGuest superGuest in superGuests)
            {
                if(superGuest.UserId == LoggedInUser.Id)
                {
                    DisplaySuperGuestStatus(superGuest);
                    break;
                }
            }
        }

        private void DisplaySuperGuestStatus(SuperGuest superGuest)
        {
            SuperGuestBonusPoints = "Super gost : " + superGuest.BonusPoints.ToString();
            if (superGuest.BonusPoints == 1) SuperGuestBonusPoints += " bonus poen";
            else SuperGuestBonusPoints += " bonus poena";
            SuperGuestReservations = "Broj rezervacija u prošloj godini : " + superGuest.LastYearReservationsNumber.ToString();
            if (superGuest.LastYearReservationsNumber < 10) SuperGuestConclusion = "U prošloj godini niste ispunili potrebnu normu.";
            else SuperGuestConclusion = "U prošloj godini ste ostvarili status super-gosta.";
        }

        private void IsFirstLogging()
        {
            if(Notifications == 0 && _futuredReservations.Count == 0 && _finishedReservations.Count == 0)
            {
                FirstLogging = true;
                MessageBox.Show("Sada ćemo Vas upoznati sa osnovnim funkcionalnostima ove aplikacije.", "Dobro došli u SOS-Booking!", MessageBoxButton.OK, MessageBoxImage.Information);
                CommandsEnabled = false;
                InfoButtons = false;
                FuturedDataGridPopup = true;
                WizardCounter++;
            }
        }

        private void Execute_ShowCanceledReservations(object sender)
        {
            NavigationService.Navigate(new RequestsStatusPage(LoggedInUser, Notifications, true, NavigationService));
        }

        private void FillCounterTextBlock()
        {
            CounterTextBlock = "Broj rezervacija u ovoj godini : " + ThisYearCounter.ToString();
        }

        private void AddFuturedReservations()
        {
            foreach(var lavm in _locAccommodationViewModels)
            {
                foreach(var res in _accommodationReservations)
                {
                    if(lavm.AccommodationId == res.AccommodationId && res.UserId == LoggedInUser.Id && DateTime.Today < res.FirstDay)
                    {
                        CancelAndMarkResDTO model = new CancelAndMarkResDTO(lavm.AccommodationName, lavm.LocationCity, lavm.LocationCountry, res.FirstDay, res.LastDay, res.Id, lavm.AccommodationId, "", res.ReservationDuration, lavm.AccommodationType);
                        _futuredReservations.Add(model);
                    } 
                }
            }
        }

        private void ApplyToThisYearCounter(List<AccommodationReservation> _accommodationReservations)
        {
            foreach(var item in _accommodationReservations)
            {
                if (item.UserId == LoggedInUser.Id && DateTime.Today.Year == item.FirstDay.Year)
                {
                    ThisYearCounter++;
                }
            }
        }

        private void ControlInboxButton(int notifications)
        {
            if (notifications > 0)
            {
                MessageNumberTextBlock = "   " + notifications.ToString();
            }
        }

        private void CollectFinishedReservations()
        {
            AccommodationReservationService service = new AccommodationReservationService();
            List<AccommodationReservation> localList = service.LoadFinishedReservations();

            if(localList.Count > 0)
            {
                foreach (var item in localList)
                {
                    service.DeleteFromFinsihedCSV(item);
                }
            }

            foreach (var item in service.GetAll())
            {
                if (DateTime.Today > item.LastDay)
                {
                    service.SaveFinishedReservation(item);
                }
            }
        }

        private void AddFinishedReservations()
        {
            AccommodationReservationService accResService = new AccommodationReservationService();
            foreach(var lavm in _locAccommodationViewModels)
            {
                foreach(var fres in accResService.LoadFinishedReservations())
                {
                    if(lavm.AccommodationId == fres.AccommodationId && fres.UserId == LoggedInUser.Id)
                    {
                        CancelAndMarkResDTO model = new CancelAndMarkResDTO(lavm.AccommodationName, lavm.LocationCity, lavm.LocationCountry, fres.FirstDay, fres.LastDay, fres.Id, lavm.AccommodationId, "", fres.ReservationDuration, lavm.AccommodationType);
                        _finishedReservations.Add(model);
                    }
                }
            }
        }

        private void Execute_ShowInbox(object sender)
        {
            if (Notifications == 0)
            {
                MessageBox.Show("     Vaš inboks je prazan!\nNemate nepročitanih poruka.", "Potvrda", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                NavigationService.Navigate(new GuestInboxPage(LoggedInUser, NavigationService));
            }
        }

        private void Execute_ShowStatuses(object sender)
        {
            NavigationService.Navigate(new RequestsStatusPage(LoggedInUser, Notifications, false, NavigationService));
        }

        private void Execute_ShowMenu(object sender)
        {
            NotificationFromOwnerService service = new NotificationFromOwnerService();
            NavigationService.Navigate(new SearchAccommodationPage(LoggedInUser, NavigationService, service.TestInboxCharge(LoggedInUser.Id)));
        }

        private void Execute_SignOut(object sender)
        {
            SignInForm form = new SignInForm();
            Guest1MainWindow.Instance.Close();
            form.ShowDialog();
        }
    }
}
