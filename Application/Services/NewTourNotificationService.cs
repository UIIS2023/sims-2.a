using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class NewTourNotificationService
    {
        private readonly INewTourNotificationRepository _newTourNotificationRepository = Injector.CreateInstance<INewTourNotificationRepository>();
        private readonly UserService _userService;

        public NewTourNotificationService()
        {
            _userService = new UserService();
        }

        public void Delete(int id)
        {
            _newTourNotificationRepository.Delete(id);
        }

        public List<NewTourNotification> GetAll()
        {
            return _newTourNotificationRepository.GetAll();
        }

        public NewTourNotification GetById(int id)
        {
            return _newTourNotificationRepository.GetById(id);
        }

        public void Save(NewTourNotification notification)
        {
            _newTourNotificationRepository.Save(notification);
        }

        public void SaveAll(List<NewTourNotification> notifications)
        {
            _newTourNotificationRepository.SaveAll(notifications);
        }

        public void Update(NewTourNotification notification)
        {
            _newTourNotificationRepository.Update(notification);
        }

        public List<NewTourNotification> GetAllByGuestId(int guestId)
        {
            return _newTourNotificationRepository.GetAllByGuestId(guestId);
        }

        public void CreateNotificationForAllUsers(int appointmentId)
        {
            List<NewTourNotification> notifications = new List<NewTourNotification>();
            foreach (var user in _userService.GetAll().FindAll(u => u.Role == Roles.GOST2))
            {
                NewTourNotification notification = new NewTourNotification
                {
                    AppointmentId = appointmentId,
                    GuideId = App.LoggedUser.Id,
                    RequestId = -1,
                    GuestId = user.Id,
                    IsRead = false,
                    Type = NotificationType.STATS_MADE
                };
                notifications.Add(notification);
            }
            SaveAll(notifications);
        }

        public void CreateNotificationForUser(int appointmentId, int userId, int tourRequestId)
        {
            NewTourNotification notification = new NewTourNotification
            {
                AppointmentId = appointmentId,
                GuideId = App.LoggedUser.Id,
                RequestId = tourRequestId,
                GuestId = userId,
                IsRead = false,
                Type = NotificationType.REQUESTED
            };
            Save(notification);
        }

        public List<NewTourNotification> GetAllByGuideId(int guideId)
        {
            return _newTourNotificationRepository.GetAll().FindAll(nt => nt.GuideId == guideId);
        }

    }
}
