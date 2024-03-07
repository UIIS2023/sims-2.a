using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.DTO;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Guest1
{
    public class OneForumViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public User LoggedInUser { get; set; }
        public NavigationService NavigationService { get; set; }
        public Forum SelectedForum { get; set; }
        public Location ForumLocation { get; set; }
        public string LocationName { get; set; }
        public string Description { get; set; }
        public string Usebility { get; set; }
        public string UsebilityInformation { get; set; }
        private string newComment { get; set; }
        public string NewComment
        {
            get { return newComment; }
            set
            {
                newComment = value;
                OnPropertyChaged("NewComment");
            }
        }
        private bool isPopupOpen { get; set; }
        public bool IsPopupOpen
        {
            get { return isPopupOpen; }
            set
            {
                isPopupOpen = value;
                OnPropertyChaged("IsPopupOpen");
            }
        }
        private bool closeButtonVisibility { get; set; }
        public bool CloseButtonVisibility
        {
            get { return closeButtonVisibility; }
            set
            {
                closeButtonVisibility = value;
                OnPropertyChaged("CloseButtonVisibility");
            }
        }
        private bool otherCommandsEnabled { get; set; }
        public bool OtherCommandsEnabled
        {
            get { return otherCommandsEnabled; }
            set
            {
                otherCommandsEnabled = value;
                OnPropertyChaged("OtherCommandsEnabled");
            }
        }
        public int SignificantOwnerComments { get; set; }
        public int SignificantGuestComments { get; set; }
        public ObservableCollection<ForumComment> AllComments { get; set; }
        public ObservableCollection<ForumComment> SignificantComments { get; set; }
        public List<User> SignificantUsers { get; set; }
        public RelayCommand UsebilityInfoCommand { get; set; }
        public RelayCommand SendCommentCommand { get; set; }
        public RelayCommand CloseForumCommand { get; set; }
        public RelayCommand GoBackCommand { get; set; }

        public OneForumViewModel(User user, NavigationService service, Forum forum)
        {
            LoggedInUser = user;
            NavigationService = service;
            SelectedForum = forum;
            LocationName = forum.LocationName;
            Usebility = forum.Useful;
            Description = forum.Description;
            IsPopupOpen = false;
            OtherCommandsEnabled = true;
            SignificantOwnerComments = 0;
            SignificantGuestComments = 0;
            NewComment = string.Empty;

            AllComments = new ObservableCollection<ForumComment>();
            SignificantComments = new ObservableCollection<ForumComment>();

            LocationService locationService = new LocationService();
            ForumLocation = locationService.GetById(forum.LocationId);
            UserService userService = new UserService();
            SignificantUsers = userService.GetAllSignificantUsers(ForumLocation);

            FillPopup();
            DefineCloseOption();
            GetAllForumComments();
            GetSignificantComments();

            UsebilityInfoCommand = new RelayCommand(Execute_ShowForumInfo);
            SendCommentCommand = new RelayCommand(Execute_SendComment);
            CloseForumCommand = new RelayCommand(Execute_CloseForum);
            GoBackCommand = new RelayCommand(Execute_GoBack);
        }

        private void GetAllForumComments()
        {
            ForumCommentService forumCommentService = new ForumCommentService();
            foreach(var item in forumCommentService.GetAll())
            {
                if(item.ForumId == SelectedForum.Id)
                {
                    AllComments.Add(item);
                }
            }
        }

        private void GetSignificantComments()
        {
            ForumCommentService forumCommentService = new ForumCommentService();
            foreach (var comment in forumCommentService.GetAll())
            {
                foreach (var user in SignificantUsers)
                {
                    if (comment.UserId == user.Id && comment.ForumId == SelectedForum.Id)
                    {
                        SignificantComments.Add(comment);
                    }
                    if (user.Role == Roles.VLASNIK) SignificantOwnerComments++;
                    else if(user.Role == Roles.GOST1) SignificantGuestComments++;
                }
            }
        }

        private void FillPopup()
        {
            UsebilityInformation = "Bilo koji gost može ostavljati komentare na otvoren forum, ali ako je gost nekada\n"
                                 + "bio na zadatoj lokaciji (postoji rezervacija ili tura na kojoj je potvrđeno njegovo prisustvo), njegov komentar\n"
                                 + "će biti označen kao značajan (drugi tab). Forumi koji dostignu 20 komentara od strane\n"
                                 + "gostiju koji su već posjetili tu lokaciju (i 10 od vlasnika koji posjeduju smještaje na toj lokaciji)\n"
                                 + "će biti označeni kao veoma korisni.";
        }

        private void Execute_GoBack(object sender)
        {
            NavigationService.GoBack();
        }

        private void DefineCloseOption()
        {
            if(LoggedInUser.Id != SelectedForum.UserId || !SelectedForum.IsOpen)
            {
                CloseButtonVisibility = false;
            }
            else
            {
                CloseButtonVisibility = true;
            }
        }

        protected void OnPropertyChaged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Execute_CloseForum(object sender)
        {
            if(SelectedForum.IsOpen)
            {
                CloseButtonVisibility = false;
                SelectedForum.IsOpen = false;
                ForumService forumService = new ForumService();
                forumService.Update(SelectedForum);
                MessageBox.Show("Uspješno ste zatvorili forum. Trebate imati u vidu da ovim isti niste izbrisali, i da će forum ostati vidljiv zauvijek.", "Potvrda", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Execute_SendComment(object sender)
        {
            if(Validate())
            {
                if (SelectedForum.IsOpen)
                {
                    bool flag = false;
                    foreach (var user in SignificantUsers)
                    {
                        if (LoggedInUser.Id == user.Id)
                        {
                            flag = true;
                            break;
                        }
                    }

                    if (!flag) SaveComment();
                    else SaveSignificantComment();

                }
                else
                {
                    MessageBox.Show("Ostavljanje komentara nije moguće jer je ovaj forum zatvoren.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool Validate()
        {
            if (NewComment == null || NewComment.Equals(string.Empty))
            {
                MessageBox.Show("Niste unijeli komentar!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
                return true;
        }

        private void SaveComment()
        {
            ForumCommentService forumCommentService = new ForumCommentService();
            ForumComment comment = new ForumComment(LoggedInUser.Id, SelectedForum.Id, NewComment, false, LoggedInUser.Role);
            AllComments.Add(comment);
            forumCommentService.Save(comment);
            MessageBox.Show("Komentar je uspješno poslat.", "Potvrda", MessageBoxButton.OK, MessageBoxImage.Information);
            NewComment = null;
            UpdateForumUsebility(LoggedInUser);
        }

        private void SaveSignificantComment()
        {
            ForumCommentService forumCommentService = new ForumCommentService();
            ForumComment comment = new ForumComment(LoggedInUser.Id, SelectedForum.Id, NewComment, true, LoggedInUser.Role);
            SignificantComments.Add(comment);
            AllComments.Add(comment);
            forumCommentService.Save(comment);
            MessageBox.Show("Komentar je uspješno poslat i označen je kao značajan.", "Potvrda", MessageBoxButton.OK, MessageBoxImage.Information);
            NewComment = null;
            UpdateForumUsebility(LoggedInUser);
        }

        private void UpdateForumUsebility(User user)
        {
            if (user.Role == Roles.VLASNIK) SignificantOwnerComments++;
            else if (user.Role == Roles.GOST1) SignificantGuestComments++;
            
            if(SignificantOwnerComments >= 10 && SignificantGuestComments >= 20)
            {
                SelectedForum.Useful = "Veoma koristan";
                ForumService forumService = new ForumService();
                forumService.Update(SelectedForum);
            }
        }


        private void Execute_ShowForumInfo(object sender)
        {
            if(!IsPopupOpen)
            {
                IsPopupOpen = true;
                OtherCommandsEnabled = false;
            }
            else
            {
                IsPopupOpen = false;
                OtherCommandsEnabled = true;
            }
        }
    }
}
