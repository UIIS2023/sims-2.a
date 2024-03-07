using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Guest2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using LibVLCSharp.Shared;
using System.Windows.Controls;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest2
{
    public class HelpWindowViewModel : ViewModel
    {
        public event EventHandler CloseRequested;

        private MediaElement _mediaElement;

        private RelayCommand _backCommand;
        public RelayCommand BackCommand
        {
            get { return _backCommand; }
            set
            {
                _backCommand = value;
            }
        }

        private string _videoPath;
        public string VideoPath
        {
            get { return _videoPath; }
            set
            {
                if (_videoPath != value)
                {
                    _videoPath = value;
                    OnPropertyChanged(nameof(VideoPath));
                }
            }
        }

        private RelayCommand _playCommand;
        public RelayCommand PlayCommand
        {
            get { return _playCommand; }
            set
            {
                _playCommand = value;
            }
        }

        private RelayCommand _pauseCommand;
        public RelayCommand PauseCommand
        {
            get { return _pauseCommand; }
            set
            {
                _pauseCommand = value;
            }
        }

        private RelayCommand _restartCommand;
        public RelayCommand RestartCommand
        {
            get { return _restartCommand; }
            set
            {
                _restartCommand = value;
            }
        }

        private RelayCommand _rewindCommand;
        public RelayCommand RewindCommand
        {
            get { return _rewindCommand; }
            set
            {
                _rewindCommand = value;
            }
        }
        public HelpWindowViewModel(MediaElement mediaElement)
        {
            _mediaElement = mediaElement;
            VideoPath = "D:\\HCI_tutorijal\\guest2_tutorial.mkv";
            BackCommand = new RelayCommand(Execute_BackCommand, CanExecuteMethod);
            PlayCommand = new RelayCommand(Execute_PlayCommand, CanExecuteMethod);
            PauseCommand = new RelayCommand(Execute_PauseCommand, CanExecuteMethod);
            RestartCommand = new RelayCommand(Execute_RestartCommand, CanExecuteMethod);
            RewindCommand = new RelayCommand(Execute_RewindCommand, CanExecuteMethod);
        }

        private void Execute_RewindCommand(object obj)
        {
            _mediaElement.Position -= TimeSpan.FromSeconds(5);
        }

        private void Execute_RestartCommand(object obj)
        {
            _mediaElement.Position = TimeSpan.Zero;
        }

        private void Execute_PauseCommand(object obj)
        {
            _mediaElement.Pause();
        }

        private void Execute_PlayCommand(object obj)
        {
            _mediaElement.Play();
        }

        private void Execute_BackCommand(object obj)
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }
    }
}
