using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using SOSTeam.TravelAgency.Domain.Models;
using static System.Net.Mime.MediaTypeNames;
using SOSTeam.TravelAgency.Domain.DTO;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class GuestAccMarkService
    {
        private readonly IGuestAccommodationMarkRepository _guestAccommodationMarkRepository = Injector.CreateInstance<IGuestAccommodationMarkRepository>();

        public GuestAccMarkService() { }

        public void MarkAccommodation(int cleanMark, int ownerMark, string comment, string urls, User user, CancelAndMarkResDTO acc, string renovationMark, string suggest,int renovationNumber)
        {
            MakeAndSaveMark(cleanMark, ownerMark, comment, urls, user, acc, renovationMark, suggest);
            SaveChangesToCSVs(acc);
            SaveImages(urls, acc.ReservationId);
            CreateNotificationToOwner(acc, renovationMark, suggest, renovationNumber);
        }

        private void CreateNotificationToOwner(CancelAndMarkResDTO acc, string renovationMark, string suggest,int renovationNumber)
        {
            AccommodationReservationService reservationService = new AccommodationReservationService();
            AccommodationService accommodationService = new AccommodationService();
            NotificationService notificationService = new NotificationService();
            UserService userService = new UserService();
            Accommodation accommodation = accommodationService.GetById(acc.AccommodationId);
            AccommodationReservation reservation = reservationService.GetById(acc.ReservationId);
            User user = userService.GetById(reservation.UserId);
            string Text = "";
            if (suggest != null && !renovationMark.Equals(""))
                Text = "Gost " + user.Username + " je dao predlog za renoviranje smještaja: " + suggest + " " +
                       "Hitnost renoviranja je ocijenio sa: " + renovationMark;
            else if(suggest != null && renovationMark.Equals(""))
                Text = "Gost " + user.Username + " je dao predlog za renoviranje smještaja: " + suggest + " " +
                       "Nije komentarisao nivo hitnosti za renoviranje.";
            else if(suggest == null && !renovationMark.Equals(""))
                Text = "Gost " + user.Username + " je ocijenio hitnost renoviranja smještaja: " + renovationMark + " " +
                       "Nije ostavljao preporuku za renoviranje.";

            if(!Text.Equals(""))
            {
                Notification notification = new Notification(accommodation.OwnerId, Text, Notification.NotificationType.SUGGESTION, false);
                notificationService.Save(notification);
            }
        }

        private void SaveChangesToCSVs(CancelAndMarkResDTO acc)
        {
            AccommodationReservationService reservationService = new AccommodationReservationService();
            List<AccommodationReservation> finishedReservations = reservationService.LoadFinishedReservations();
            foreach (var item in finishedReservations)
            {
                if (item.Id == acc.ReservationId)
                {
                    item.ReadMarkNotification = true;
                    reservationService.UpdateFinishedReservationsCSV(item);
                    reservationService.Update(item);
                }
            }
        }

        private void MakeAndSaveMark(int cleanMark, int ownerMark, string comment, string urls, User user, CancelAndMarkResDTO acc, string renovationMark, string suggest)
        {
            GuestAccommodationMark accommodationMarks = new GuestAccommodationMark(cleanMark, ownerMark, comment, urls, user.Id, acc.AccommodationId, renovationMark, suggest);
            if (accommodationMarks.UrlGuestImage.Equals(""))
                accommodationMarks.UrlGuestImage = "Nema priloženih slika.";
            _guestAccommodationMarkRepository.Save(accommodationMarks);
        }

        private void SaveImages(string urls, int resId)
        {
            ImageService imageService = new ImageService();
            if (!urls.Equals(""))
            {
                string[] urlArray = urls.Split(',');
                foreach(string url in urlArray) 
                {
                    if (!url.Equals(""))
                    {
                        Domain.Models.Image image = new Domain.Models.Image(url, false, resId, ImageType.RESERVATION);
                        imageService.Save(image);
                    }
                }
            }
        }

        public void Delete(int id)
        {
            _guestAccommodationMarkRepository.Delete(id);
        }

        public List<GuestAccommodationMark> GetAll()
        {
            return _guestAccommodationMarkRepository.GetAll();
        }


        public GuestAccommodationMark GetById(int id)
        {
            return _guestAccommodationMarkRepository.GetById(id);
        }

        public void Save(GuestAccommodationMark guestAccommodationMark)
        {
            _guestAccommodationMarkRepository.Save(guestAccommodationMark);
        }

        public void Update(GuestAccommodationMark guestAccommodationMark)
        {
            _guestAccommodationMarkRepository.Update(guestAccommodationMark);
        }
    }
}
