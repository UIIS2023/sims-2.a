using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.WPF.Views.Owner;
using System.Collections.ObjectModel;
using System.Linq;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    public class MainWindowViewModel : ViewModel
    {
        private UserService _userService;
        public string Username { get; private set; }
        public User LoggedInUser { get; private set; }
        private NotificationService _notificationService;
        private GuestReviewService _guestReviewService;
        private ForumService _forumService;

        public ObservableCollection<Notification> Notifications { get; set; }
        public Notification SelectedNotification { get; set; }

        public RelayCommand DeleteNotification { get; private set; }
        public RelayCommand MarkAsReadNotification { get; private set; }
        public RelayCommand NavigationButtonCommand { get; private set; }
        public RelayCommand ToggleShowNotifications { get; private set; }
        public RelayCommand NotificationDoubleClick { get; private set; }



        private bool _isUnread;
        public bool IsUnread
        {
            get => _isUnread;
            set
            {
                if (_isUnread != value)
                {
                    _isUnread = value;
                    OnPropertyChanged(nameof(IsUnread));

                }
            }
        }

        private bool _isDropdownOpen;
        public bool IsDropdownOpen
        {
            get => _isDropdownOpen;
            set
            {
                if (_isDropdownOpen != value)
                {
                    _isDropdownOpen = value;
                    IsDropdownClosed = !IsDropdownOpen;
                    OnPropertyChanged(nameof(IsDropdownOpen));

                }
            }
        }
        public bool IsDropdownClosed { get; set; }
        public MainWindowViewModel()
        {
            _userService = new();
            _guestReviewService = new();
            _forumService = new();
            LoggedInUser = App.LoggedUser;
            Username = LoggedInUser.Username;
            IsDropdownOpen = false;
            IsDropdownClosed = !IsDropdownOpen;
            DeleteNotification = new RelayCommand(Execute_DeleteNotification, CanExecuteDeleteNotification);
            MarkAsReadNotification = new RelayCommand(Execute_MarkAsReadNotification, CanExecuteDeleteNotification);
            NavigationButtonCommand = new RelayCommand(Execute_NavigationButtonCommand, CanExecuteMethod);
            ToggleShowNotifications = new RelayCommand(Execute_ToggleShowNotifications, CanExecuteMethod);
            NotificationDoubleClick = new RelayCommand(Execute_NotificationDoubleClick, CanExecuteMethod);


            Notifications = new ObservableCollection<Notification>();

            _notificationService = new();
            _notificationService.Refresh(LoggedInUser.Id);
            GetUserNotifications();
            IsUnread = Notifications.Any(t => !t.Read);

        }

        private void Execute_MarkAsReadNotification(object obj)
        {
            SelectedNotification.Read = true;
            _notificationService.Update(SelectedNotification);
            UpdateNotifications();
        }

        private void Execute_DeleteNotification(object obj)
        {
            _notificationService.Delete(SelectedNotification.Id);
            UpdateNotifications();
        }

        private bool CanExecuteDeleteNotification(object obj)
        {
            return SelectedNotification != null;
        }

        private void GetUserNotifications()
        {
            foreach (Notification notification in _notificationService.GetAll())
            {
                if (notification.UserId == LoggedInUser.Id)
                {
                    Notifications.Add(notification);
                }
            }
        }

        public void SetStartupPage()
        {
            Execute_NavigationButtonCommand("Home");
        }

        internal void SetPage(object root)
        {
            App.OwnerNavigationService.SetPage(root);
        }

        public void Execute_ToggleShowNotifications(object parameter)
        {
            IsDropdownOpen = !IsDropdownOpen;
        }

        public void Execute_NotificationDoubleClick(object parameter)
        {
            if (SelectedNotification == null) return;

            SelectedNotification.Read = true;
            _notificationService.Update(SelectedNotification);

            if (SelectedNotification.Type == Notification.NotificationType.GUESTREVIEW && !_guestReviewService.IsReviewed(SelectedNotification.EntityId))
            {
                SetPage(new GuestReviewPage(SelectedNotification.EntityId));
                Execute_ToggleShowNotifications(null);
            }
            if (SelectedNotification.Type == Notification.NotificationType.FORUM)
            {
                SetPage(new ForumInfoPage(_forumService.GetById(SelectedNotification.EntityId)));
                Execute_ToggleShowNotifications(null);
            }
            if (SelectedNotification.Type == Notification.NotificationType.SUGGESTION)
            {
                SetPage(new SuggestionPage());
                Execute_ToggleShowNotifications(null);
            }
            UpdateNotifications();
        }

        public void Execute_NavigationButtonCommand(object parameter)
        {
            App.OwnerNavigationService.NavigateMainWindow(parameter);
        }

        public void CloseWindow()
        {
            App.OwnerNavigationService.CloseWindow();
        }

        private bool CanExecuteMethod(object parameter)
        {
            return true;
        }

        private void FillObservableCollection()
        {
            foreach (Notification notification in _notificationService.GetAll())
            {
                if (notification.UserId == LoggedInUser.Id)
                {
                    Notifications.Add(notification);
                }
            }
        }

        private void UpdateNotifications()
        {
            Notifications.Clear();
            FillObservableCollection();
            IsUnread = Notifications.Any(t => !t.Read);
        }

    }
}
