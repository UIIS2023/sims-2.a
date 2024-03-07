using Microsoft.Win32;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class ReviewPageViewModel : ViewModel
    {
        public int AppointmentId { get; set; }
        public int ReservationId { get; set; }
        public User LoggedInUser { get; set; }

        public List<Domain.Models.Image> Images { get; set; }

        private TourReviewService _tourReviewService;
        private ImageService _imageService;
        private ReservationService _reservationService;
        private string _imageUrl;
        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                _imageUrl = value;
                OnPropertyChanged(nameof(ImageUrl));
            }
        }

        private RelayCommand _backCommand;
        public RelayCommand BackCommand
        {
            get { return _backCommand; }
            set
            {
                _backCommand = value;
            }
        }

        private RelayCommand _addImageCommand;
        public RelayCommand AddImageCommand
        {
            get { return _addImageCommand; }
            set
            {
                _addImageCommand = value;
            }
        }

        private RelayCommand _deleteImageCommand;
        public RelayCommand DeleteImageCommand
        {
            get { return _deleteImageCommand; }
            set
            {
                _deleteImageCommand = value;
            }
        }

        private RelayCommand _createReviewCommand;

        public RelayCommand CreateReviewCommand
        {
            get { return _createReviewCommand; }
            set
            {
                _createReviewCommand = value;
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

        private int _guideKnowledge;
        public int GuideKnowledge
        {
            get { return _guideKnowledge; }
            set
            {
                _guideKnowledge = value;
                OnPropertyChanged(nameof(GuideKnowledge));
            }
        }

       
        private int _guideLangguage;
        public int GuideLanguage
        {
            get { return _guideLangguage; }
            set
            {
                _guideLangguage = value;
                OnPropertyChanged();
            }
        }

       
        private int _interestRating;
        public int InterestRating
        {
            get { return _interestRating; }
            set
            {
                _interestRating = value;
                OnPropertyChanged();
            }
        }

        private string _comment;
        public string Comment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                OnPropertyChanged();
            }
        }


        public ReviewPageViewModel(User loggedInUser,int appointmnetId,int reservationId)
        {
            AppointmentId= appointmnetId;
            LoggedInUser = loggedInUser;
            ReservationId= reservationId;
            Images = new List<Domain.Models.Image>();
            _tourReviewService = new TourReviewService();
            _imageService = new ImageService();
            _reservationService = new ReservationService();
            BackCommand = new RelayCommand(Execute_BackCommand,CanExecuteMethod);
            CreateReviewCommand = new RelayCommand(Execute_CreateReviewCommand, CanExecuteMethod);
            AddImageCommand = new RelayCommand(Execute_AddImageCommand, CanExecuteMethod);
            DeleteImageCommand = new RelayCommand(Execute_DeleteImageCommand,CanExecuteMethod);
            HelpCommand = new RelayCommand(Execute_HelpCommand,CanExecuteMethod);
        }
        private void Execute_HelpCommand(object obj)
        {
            HelpWindow window = new HelpWindow();
            window.Show();
        }
        private void Execute_DeleteImageCommand(object obj)
        {
            if(Images.Count > 1)
            {
                Images.RemoveAt(Images.Count - 1);
                ImageUrl = Images[Images.Count - 1].Path;
            }
            else if(Images.Count == 1)
            {
                Images.RemoveAt(Images.Count - 1);
                ImageUrl = string.Empty;
            }
        }

        private void Execute_AddImageCommand(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                ImageUrl = openFileDialog.FileName;
                string substring = @"\Resources\Images\Tours";
                int index = ImageUrl.LastIndexOf(substring);
                string trimmedUrl = ImageUrl.Substring(index).Replace("\\", "/"); ;
                ImageUrl = trimmedUrl;
            }
            Domain.Models.Image image = new Domain.Models.Image(ImageUrl,false, AppointmentId, ImageType.GUEST2);
            Images.Add(image);
        }

        private void Execute_CreateReviewCommand(object obj)
        {
            if(GuideKnowledge == 0)
            {
                MessageBox.Show("Niste ocenili znanje vodica!", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if(GuideLanguage == 0)
            {
                MessageBox.Show("Niste ocenili jezik vodica!", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (InterestRating == 0)
            {
                MessageBox.Show("Niste zanimljivost ture!", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (Comment == null || Comment == string.Empty)
            {
                MessageBox.Show("Niste ostavili komentar!", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBoxResult result = ConfirmReview();
                if (result == MessageBoxResult.Yes)
                {
                    CreateReview();
                }
            }
        }

        private void CreateReview()
        {
            if (Images.Count > 0)
            {
                _imageService.SaveAll(Images);
            }
            _tourReviewService.CreateTourReview(LoggedInUser.Id, AppointmentId, GuideKnowledge, GuideLanguage, InterestRating, Comment, false);
            _reservationService.Reviewed(ReservationId);
            var currentApp = System.Windows.Application.Current;

            MessageBox.Show("Uspesno ste izvrsili ocenjivanje ture i vodica", "Ocenjivanje", MessageBoxButton.OK, MessageBoxImage.Information);

            foreach (Window window in currentApp.Windows)
            {
                if (window is NotificationsWindow)
                {
                    window.Close();
                }
            }

            UpdateNotifications();
        }

        private void UpdateNotifications()
        {
            var currentApp = System.Windows.Application.Current;
            foreach (Window window in currentApp.Windows)
            {
                if (window is ToursOverviewWindow)
                {
                    MainViewModel viewModel = window.DataContext as MainViewModel;
                    viewModel.CheckNotifications();
                }
            }
        }

        private MessageBoxResult ConfirmReview()
        {
            string sMessageBoxText = $"Da li ste sigurni da želite da ocenite turu i vodica";
            string sCaption = "Porvrda ocene";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }
        private void Execute_BackCommand(object obj)
        {
            var currentApp = System.Windows.Application.Current;

            foreach (Window window in currentApp.Windows)
            {
                if (window is NotificationsWindow)
                {
                    NotificationsWindow notificationWindow = new NotificationsWindow(LoggedInUser);
                    notificationWindow.Show();
                    window.Close();
                }
            }
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

      

    }
}
