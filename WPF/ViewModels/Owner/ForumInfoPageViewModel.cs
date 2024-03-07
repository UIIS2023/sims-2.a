using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Commands;
using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.Owner
{
    public class ForumInfoPageViewModel : ViewModel
    {
        public string OpenerUsername { get; set; }
        public string Location { get; set; }
        private string _newComment;
        public string NewComment
        {
            get { return _newComment; }
            set
            {
                if (_newComment != value)
                {
                    _newComment = value;
                    OnPropertyChanged("NewComment");
                }
            }
        }
        public Forum ForumInfo { get; set; }
        public RelayCommand AddComment { get; private set; }
        public RelayCommand FlagComment { get; private set; }

        private ObservableCollection<ForumCommentViewModel> _comments;
        public ObservableCollection<ForumCommentViewModel> Comments
        {
            get { return _comments; }
            set
            {
                _comments = value;
                OnPropertyChanged("Comments");
            }
        }

        private UserService _userService;
        private LocationService _locationService;
        private ForumCommentService _forumCommentService;
        private ForumCommentReportService _forumCommentReportService;
        public ForumInfoPageViewModel(Forum forum)
        {
            _userService = new();
            _locationService = new();
            _forumCommentReportService = new();
            _forumCommentService = new();

            ForumInfo = forum;
            OpenerUsername = _userService.GetById(forum.UserId).Username;
            var location = _locationService.GetById(forum.LocationId);
            Location = _locationService.GetFullName(location);
            NewComment = "";


            FillObservableCollection();
            AddComment = new RelayCommand(Execute_AddComment, CanExecuteAddComment);
            FlagComment = new RelayCommand(Execute_FlagComment, CanExecuteFlagComment);
        }

        private void Execute_FlagComment(object obj)
        {
            if (obj is ForumCommentViewModel commentViewModel)
            {
                if (commentViewModel.IsReported) 
                {
                    commentViewModel.IsReported = !commentViewModel.IsReported;
                    commentViewModel.ReportNum--;
                    _forumCommentReportService.Delete(App.LoggedUser.Id, commentViewModel.Comment.Id);
                }
                else 
                {
                    commentViewModel.IsReported = !commentViewModel.IsReported;
                    commentViewModel.ReportNum++;
                    _forumCommentReportService.Save(new ForumCommentReport(0, App.LoggedUser.Id, commentViewModel.Comment.Id));
                }
                
            }
        }

        private bool CanExecuteFlagComment(object obj)
        {
            if (obj is ForumCommentViewModel commentViewModel)
            {
                return (!commentViewModel.Comment.WasOnLocation) && commentViewModel.Comment.UserType != Roles.VLASNIK;
            }
            return false;
        }

        private void Execute_AddComment(object obj)
        {
            //TODO: DOdaj cuvanje
            var forumComment = new ForumComment(
                0,
                App.LoggedUser.Id,
                ForumInfo.Id,
                NewComment,
                false,
                Roles.VLASNIK
                );
            _forumCommentService.Save(forumComment);
            Comments.Add(new ForumCommentViewModel(
                    _userService.GetById(forumComment.UserId).Username,
                    forumComment,
                    0,
                    false
                    ));
            NewComment = "";
        }

        private bool CanExecuteAddComment(object obj)
        {
            return NewComment.Length > 1; 
        }

        private void FillObservableCollection()
        {
            Comments = new();
            foreach(var comment in _forumCommentService.GetAllForForum(ForumInfo.Id)) 
            {
                Comments.Add(new ForumCommentViewModel(
                    _userService.GetById(comment.UserId).Username,
                    comment,
                    _forumCommentReportService.GetReportsNumber(comment.Id),
                    _forumCommentReportService.IsReported(App.LoggedUser.Id,comment.Id)
                    ));
            }
        }

    }

    public class ForumCommentViewModel : ViewModel
    {
        private string _username;
        private ForumComment _comment;
        private int _reportNum;
        private bool _isReported;

        public string Username
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        public ForumComment Comment
        {
            get { return _comment; }
            set
            {
                if (_comment != value)
                {
                    _comment = value;
                    OnPropertyChanged(nameof(Comment));
                }
            }
        }

        public int ReportNum
        {
            get { return _reportNum; }
            set
            {
                if (_reportNum != value)
                {
                    _reportNum = value;
                    OnPropertyChanged(nameof(ReportNum));
                }
            }
        }

        public bool IsReported
        {
            get { return _isReported; }
            set
            {
                if (_isReported != value)
                {
                    _isReported = value;
                    OnPropertyChanged(nameof(IsReported));
                }
            }
        }

        public ForumCommentViewModel(string username, ForumComment comment, int reportNum, bool isReported)
        {
            _username = username;
            _comment = comment;
            _reportNum = reportNum;
            _isReported = isReported;
        }
    }

}
