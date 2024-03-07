using SOSTeam.TravelAgency.Application.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class GuestAttendanceCardCreatorViewModel
    {
        private readonly GuestAttendanceService _guestAttendanceService;
        private readonly UserService _userService;
        private readonly CheckpointActivityService _checkpointActivityService;
        private readonly CheckpointService _checkpointService;

        public GuestAttendanceCardCreatorViewModel()
        {
            _guestAttendanceService = new GuestAttendanceService();
            _userService = new UserService();
            _checkpointActivityService = new CheckpointActivityService();
            _checkpointService = new CheckpointService();
        }
        public ObservableCollection<GuestAttendanceCardViewModel> CreateUserCards(CheckpointActivityCardViewModel selectedCheckpointActivityActivity)
        {
            var guestAttendanceCards = new ObservableCollection<GuestAttendanceCardViewModel>();
            foreach (var guestAttendance in _guestAttendanceService.GetAllByActivityId(selectedCheckpointActivityActivity.ActivityId))
            {
                var guestAttendanceCard = new GuestAttendanceCardViewModel
                {
                    UserName = _userService.GetById(guestAttendance.UserId).Username
                };
                guestAttendanceCard.SetStatusImageAndBackground(guestAttendance);
                guestAttendanceCards.Add(guestAttendanceCard);
            }
            return guestAttendanceCards;
        }

        public ObservableCollection<GuestAttendanceCardViewModel> CreateCheckpointCards(GuestReviewCardViewModel selectedReview)
        {
            var checkpointAttendanceCards = new ObservableCollection<GuestAttendanceCardViewModel>();
            foreach (var checkpointActivity in _checkpointActivityService.GetAllByAppointmentId(selectedReview.AppointmentId))
            {
                foreach (var guestAttendance in _guestAttendanceService.GetByUserId(selectedReview.UserId))
                {
                    if (checkpointActivity.Id == guestAttendance.CheckpointActivityId)
                    {
                        var guestAttendanceCard = new GuestAttendanceCardViewModel
                        {
                            CheckpointName = _checkpointService.GetById(checkpointActivity.CheckpointId).Name
                        };
                        guestAttendanceCard.SetStatusImageAndBackground(guestAttendance);
                        checkpointAttendanceCards.Add(guestAttendanceCard);
                    }
                }
            }
            return checkpointAttendanceCards;
        }
    }
}
