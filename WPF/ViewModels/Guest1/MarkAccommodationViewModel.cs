using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml.Linq;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using System.Windows.Data;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Navigation;
using SOSTeam.TravelAgency.Domain.DTO;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class MarkAccommodationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public User LoggedInUser { get; set; }
        public NavigationService NavigationService { get; set; }
        public string AccNameTextBlock { get; set; }
        public string AllUrls { get; set; }
        public List<string> AllUrlsList { get; set; }
        private string enteredGuestComment { get; set; }
        public string EnteredGuestComment
        {
            get { return enteredGuestComment; }
            set
            {
                enteredGuestComment = value;
                OnPropertyChaged("EnteredGuestComment");
            }
        }
        private string enteredGuestSuggest { get; set; }
        public string EnteredGuestSuggest
        {
            get { return enteredGuestSuggest; }
            set
            {
                enteredGuestSuggest = value;
                OnPropertyChaged("EnteredGuestSuggest");
            }
        }
        public CancelAndMarkResDTO Accommodation { get; set; }
         
        public List<RadioButton> CleanMarks { get; set; }
        public List<RadioButton> OwnerMarks { get; set; }
        public List<RadioButton> RenovationMarks { get; set; }

        public ObservableCollection<CancelAndMarkResDTO> ReservationsForMark { get; set; }
        public ObservableCollection<CancelAndMarkResDTO> RatingsFromOwner { get; set; }

        private ObservableCollection<System.Windows.Controls.Image> selectedImages;
        public ObservableCollection<System.Windows.Controls.Image> SelectedImages
        {
            get { return selectedImages; }
            set
            {
                selectedImages = value;
                OnPropertyChaged("SelectedImages");
            }
        }
        public RelayCommand MarkAccCommand { get; set; }
        public RelayCommand AddImagesCommand { get; set; }
        public RelayCommand DeleteSharedImageCommand { get; set; }
        public RelayCommand GoBackCommand { get; set; }

        public MarkAccommodationViewModel(List<RadioButton> cleans, List<RadioButton> owners, List<RadioButton> renovations, User user, CancelAndMarkResDTO acc, NavigationService service, ObservableCollection<CancelAndMarkResDTO> reservationsForMark, ObservableCollection<CancelAndMarkResDTO> ratingsFromOwner) 
        {
            AccNameTextBlock = acc.AccommodationName;
            CleanMarks = cleans;
            OwnerMarks = owners;
            RenovationMarks = renovations;
            RatingsFromOwner = ratingsFromOwner;
            LoggedInUser = user;
            Accommodation = acc;
            NavigationService = service;
            AllUrls = String.Empty;
            ReservationsForMark = reservationsForMark;

            SelectedImages = new ObservableCollection<System.Windows.Controls.Image>();
            AllUrlsList = new List<string>();

            MarkAccCommand = new RelayCommand(ExecuteAccommodationMarking);
            AddImagesCommand = new RelayCommand(Execute_AddImages);
            GoBackCommand = new RelayCommand(Execute_GoBack);
            DeleteSharedImageCommand = new RelayCommand(Execute_DeleteSharedImage);
        }

        protected void OnPropertyChaged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Execute_DeleteSharedImage(object sender)
        {
            System.Windows.Controls.Image? selected = sender as System.Windows.Controls.Image;
            SelectedImages.Remove(selected);
            RemoveURL(selected);
        }

        private void RemoveURL(System.Windows.Controls.Image selected)
        {
            string realPath = selected.Source.ToString();
            string deletingPath = "";
            foreach(var url in AllUrlsList)
            {
                if(realPath.Contains(url))
                {
                    deletingPath = url;
                    break;
                }
            }

            AllUrlsList.Remove(deletingPath);
        }

        private void Execute_GoBack(object sender)
        {
            NavigationService.GoBack();
        }

        private void Execute_AddImages(object sender)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Priložite slike smještaja";
            op.Filter = "Image Files|*.jpg;*.png;*.bmp|All Files|*.*";
            op.Multiselect = true;
            op.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\Images\\Guest1");

            if (op.ShowDialog() == true)
            {
                foreach(string imageUrl in op.FileNames)
                {
                    string csvPath = CreateCSVPath(op, imageUrl);
                    AllUrlsList.Add(csvPath);
                    
                    System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                    image.Source = new BitmapImage(new Uri(imageUrl));
                    SelectedImages.Add(image);
                }
            } 
        }

        private string CreateCSVPath(OpenFileDialog op, string imageUrl)
        {
            string defaultPath = imageUrl.Replace(op.InitialDirectory, "").TrimStart('\\');
            string updatedPath = Path.Combine("/Resources/Images/Guest1", defaultPath).Replace('\\', '/');
            string[] dividedPath = updatedPath.Split('/');
            string csvPath = "/";
            int j = 0;
            foreach (string part in dividedPath)
            {
                j++;
                if (part.Equals("Resources"))
                {
                    csvPath += part + "/";
                }
                if (part.Equals("Images"))
                {
                    csvPath += part + "/";
                }
                if (part.Equals("Guest1"))
                {
                    csvPath += part + "/";
                }
                if (dividedPath.Count() == j)
                {
                    csvPath += part;
                }
            }

            return csvPath;
        }

        public int FindCleanMark(List<RadioButton> list)
        {
            int i = 1;
            foreach(var radio in list)
            {
                if(radio.IsChecked == true)
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        public int FindOwnerMark(List<RadioButton> list)
        {
            int i = 1;
            foreach (var radio in list)
            {
                if (radio.IsChecked == true)
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        public string FindRenovationMark(List<RadioButton> list)
        {
            int i = 1;
            string markDescription = String.Empty;
            foreach (var radio in list)
            {
                if (radio.IsChecked == true)
                {
                    break;
                }
                i++;
            }

            if (i == 1) markDescription += "Nivo 1 - bilo bi dobro renovirati neke sitnice, ali sve funkcioniše kako treba i bez toga.";
            else if (i == 2) markDescription = "Nivo 2 - male zamjerke na smještaj koje kada bi se uklonile bi ga učinile savršenim.";
            else if (i == 3) markDescription = "Nivo 3 - nekoliko stvari koje su baš zasmetale bi trebalo renovirati.";
            else if (i == 4) markDescription = "Nivo 4 - ima dosta loših stvari i renoviranje je stvarno neophodno.";
            else if (i == 5) markDescription = "Nivo 5 - smještaj je u jako lošem stanju i ne vrijedi ga uopšte iznajmljivati ukoliko se ne renovira.";

            return markDescription;
        }

        private int GetMarkNumber(List<RadioButton> list) 
        {
            int i = 1;
            foreach (var radio in list)
            {
                if (radio.IsChecked == true)
                {
                    break;
                }
                i++;
            }
            return Math.Min(i,5);
        }

        private void ExecuteAccommodationMarking(object sender)
        {
            int cleanMark = FindCleanMark(CleanMarks);
            int ownerMark = FindOwnerMark(OwnerMarks);
            string renovationMark = FindRenovationMark(RenovationMarks);
            int renovationNumber = GetMarkNumber(RenovationMarks);
            foreach (var url in AllUrlsList)
            {
                AllUrls += url + ",";
            }
            GuestAccMarkService service = new GuestAccMarkService();
            service.MarkAccommodation(cleanMark, ownerMark, EnteredGuestComment, AllUrls, LoggedInUser, Accommodation, renovationMark, EnteredGuestSuggest,renovationNumber);
            MessageBox.Show("Uspješno ocjenjen smještaj!", "Potvrda", MessageBoxButton.OK, MessageBoxImage.Information);
            ReservationsForMark.Remove(Accommodation);
            ShowGuestRatings();
            NavigationService.Navigate(new GuestInboxPage(LoggedInUser, NavigationService));
        }

        private void ShowGuestRatings()
        {
            GuestReviewService reviewService = new GuestReviewService();
            UserService userService = new UserService();
            AccommodationService accommodationService = new AccommodationService();
            AccommodationReservationService reservationService = new AccommodationReservationService();
            List<GuestReview> guestReviews = reviewService.GetAll();

            if (guestReviews.Count > 0)
            {
                foreach (var item in guestReviews)
                {
                    AccommodationReservation reservation = reservationService.GetById(item.ReservationId);
                    if (item.GuestId == LoggedInUser.Id && item.ReservationId == Accommodation.ReservationId)
                    {
                        User owner = userService.GetById(item.OwnerId);
                        Accommodation accommodation = accommodationService.GetById(item.AccommodationId);
                        string shape = "Vlasnik " + owner.Username + " je ocijenio Vašu čistoću u smještaju " + accommodation.Name +
                                       " u periodu od " + reservation.FirstDay.ToShortDateString() + " do " + reservation.LastDay.ToShortDateString() +
                                       " ocjenom " + item.CleanlinessGrade + ", a poštovanje pravila sa " + item.RespectGrade + ". ";
                        if (item.Comment.Equals("")) shape += "Nije ostavljao dodatni komentar.";
                        else shape += "Ostavio je i dodatni komentar: " + item.Comment;
                        CancelAndMarkResDTO model = new CancelAndMarkResDTO();
                        model.NotificationShape = shape;
                        RatingsFromOwner.Add(model);
                        break;
                    }
                }
            }
        }
    }
}
