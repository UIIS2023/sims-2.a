using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class NotificationFromOwnerService
    {
        private readonly INotificationFromOwnerRepository notificationFromOwnerRepository = Injector.CreateInstance<INotificationFromOwnerRepository>();

        public NotificationFromOwnerService() { }

        public void Delete(int id)
        {
            notificationFromOwnerRepository.Delete(id);
        }

        public List<NotificationFromOwner> GetAll()
        {
            return notificationFromOwnerRepository.GetAll();
        }

        public NotificationFromOwner GetById(int id)
        {
            return notificationFromOwnerRepository.GetById(id);
        }

        public void Save(NotificationFromOwner notificationFromOwner)
        {
            notificationFromOwnerRepository.Save(notificationFromOwner);
        }

        public void Update(NotificationFromOwner notificationFromOwner)
        {
            notificationFromOwnerRepository.Update(notificationFromOwner);
        }

        public int TestInboxCharge(int loggedInUserId)
        {
            int marksNotifications = TestMarkNotifications(loggedInUserId);
            int ownerNotifications = TestOwnerRequestNotifications(loggedInUserId);
            int ratingNotifications = TestRatingsForGuests(loggedInUserId);
            return marksNotifications + ownerNotifications + ratingNotifications;
        }

        private int TestMarkNotifications(int loggedInUserId)
        {
            AccommodationReservationService reservationService = new AccommodationReservationService();
            List<AccommodationReservation> allRes = reservationService.GetAll();
            int counter = 0;
            foreach (var res in allRes)
            {
                int diff = DateTime.Today.DayOfYear - res.LastDay.DayOfYear;
                bool fullCharge = res.UserId == loggedInUserId && !res.ReadMarkNotification && diff <= 5 && diff > 0;
                if (fullCharge)
                {
                    counter++;
                }
            }
            return counter;
        }

        private int TestOwnerRequestNotifications(int loggedInUserId)
        {
            List<NotificationFromOwner> notifications = notificationFromOwnerRepository.GetAll();
            int counter = 0;
            foreach (var item in notifications)
            {
                if (item.GuestId == loggedInUserId)
                {
                    counter++;
                }
            }
            return counter;
        }

        private int TestRatingsForGuests(int loggedInUserId)
        {
            AccommodationReservationService reservationService = new AccommodationReservationService();
            GuestReviewService guestReviewService = new GuestReviewService();
            List<GuestReview> guestReviews = guestReviewService.GetAll();
            int counter = 0;
            foreach(var item in  guestReviews)
            {
                AccommodationReservation reservation = reservationService.GetById(item.ReservationId);
                if (item.GuestId == loggedInUserId && reservation.ReadMarkNotification)
                {
                    counter++;
                }
            }
            return counter;
        }
    }
}
