using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class GalleryViewModel : ViewModel
    {

        private Image _currentImage;

        public Image CurrentImage
        {
            get => _currentImage;
            set
            {
                if (_currentImage != value)
                {
                    _currentImage = value;
                    OnPropertyChanged("CurrentImage");
                }
            }
        }

        private string _currentImageName;

        public string CurrentImageName
        {
            get => _currentImageName;
            set
            {
                if (_currentImageName != value)
                {
                    _currentImageName = value;
                    OnPropertyChanged("CurrentImageName");
                }
            }
        }

        public bool IsButtonsEnabled { get; set; }

        private int _currentImageIndex;
        public List<Image> Images { get; set; }

        public RelayCommand NextImageCommand { get; set; }
        public RelayCommand PreviousImageCommand { get; set; }

        public RelayCommand SetCoverCommand { get; set; }

        public RelayCommand DeleteImageCommand { get; set; }

        public GalleryViewModel(List<Image> images)
        {
            Images = images;
            SetCurrentImage();

            NextImageCommand = new RelayCommand(NextImage, CanExecuteMethod);
            PreviousImageCommand = new RelayCommand(PreviousImage, CanExecuteMethod);
            SetCoverCommand = new RelayCommand(SetAsCover, CanExecuteMethod);
            DeleteImageCommand = new RelayCommand(DeleteImage, CanExecuteMethod);
        }

        private void SetCurrentImage()
        {
            if (Images.Count == 0)
            {
                _currentImageIndex = -1;
                CurrentImage = null;
                CurrentImageName = string.Empty;
                IsButtonsEnabled = false;
            }
            else
            {
                _currentImageIndex = 0;
                CurrentImage = Images[_currentImageIndex];
                CurrentImageName = CurrentImage.Path.Split("/").Last();
                IsButtonsEnabled = true;
            }
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void NextImage(object sender)
        {
            if (Images.Count > 0)
            {
                if (_currentImageIndex == Images.Count - 1)
                {
                    _currentImageIndex = 0;
                    CurrentImage = Images[_currentImageIndex];
                    CurrentImageName = CurrentImage.Path.Split("/").Last();
                }
                else
                {
                    _currentImageIndex++;
                    CurrentImage = Images[_currentImageIndex];
                    CurrentImageName = CurrentImage.Path.Split("/").Last();
                }
            }
        }

        private void PreviousImage(object sender)
        {
            if (Images.Count > 0)
            {
                if (_currentImageIndex == 0)
                {
                    _currentImageIndex = Images.Count - 1;
                    CurrentImage = Images[_currentImageIndex];
                    CurrentImageName = CurrentImage.Path.Split("/").Last();
                }
                else
                {
                    _currentImageIndex--;
                    CurrentImage = Images[_currentImageIndex];
                    CurrentImageName = CurrentImage.Path.Split("/").Last();
                }
            }
        }

        private void SetAsCover(object sender)
        {
            if (_currentImageIndex != -1)
            {
                if (!CurrentImage.Cover)
                {
                    ResetCover();
                    CurrentImage.Cover = true;
                }
            }
        }

        private void ResetCover()
        {
            var image = Images.Find(i => i.Cover);
            if (image != null)
            {
                image.Cover = false;
            }
        }

        private void DeleteImage(object sender)
        {
            if (Images.Count > 1)
            {
                if (_currentImageIndex == Images.Count - 1)
                {
                    Images.Remove(CurrentImage);
                    _currentImageIndex--;
                    CurrentImage = Images[_currentImageIndex];
                    CurrentImageName = CurrentImage.Path.Split("/").Last();
                }
                else
                {
                    Images.Remove(CurrentImage);
                    CurrentImage = Images[_currentImageIndex];
                    CurrentImageName = CurrentImage.Path.Split("/").Last();
                }
            }
        }

    }
}
