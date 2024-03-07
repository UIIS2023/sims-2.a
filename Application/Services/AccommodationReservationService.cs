using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Repositories;
using System.Windows;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using SOSTeam.TravelAgency.WPF.Views;
using System.Windows.Controls;
using SOSTeam.TravelAgency.WPF.Views.Guest1;
using System.Runtime.InteropServices;
using SOSTeam.TravelAgency.Domain.DTO;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class AccommodationReservationService
    {
        private readonly IAccReservationRepository _accReservationRepository = Injector.CreateInstance<IAccReservationRepository>();

        public AccommodationReservationService() { }

        public struct ReservationsInformations
        {
            public List<ChangedReservationRequest> requests { get; set; }
            public List<WantedNewDate> newDates { get; set; }

            public ReservationsInformations()
            {
                requests = new List<ChangedReservationRequest>();
                newDates = new List<WantedNewDate>();
            }
        }

        public List<AccommodationReservation> LoadCanceledReservations()
        {
           return _accReservationRepository.LoadCanceledReservations();
        }

        public void SaveOld(AccommodationReservation reservation)
        {
            _accReservationRepository.SaveOld(reservation);
        }

        public void UpdateToDefinitlyForget(AccommodationReservation accommodationReservation)
        {
            _accReservationRepository.UpdateToDefinitlyForget(accommodationReservation);
        }

        public void SaveFinishedReservation(AccommodationReservation reservation)
        {
            _accReservationRepository.SaveFinishedReservation(reservation);
        }

        public List<AccommodationReservation> LoadFinishedReservations()
        {
            return _accReservationRepository.LoadFinishedReservations();
        }

        public void Delete(int id)
        {
            _accReservationRepository.Delete(id);
        }

        public void DeleteFromFinsihedCSV(AccommodationReservation reservation)
        {
            _accReservationRepository.DeleteFromFinishedCSV(reservation);
        }

        public List<AccommodationReservation> GetAll()
        {
            return _accReservationRepository.GetAll();
        }

        public AccommodationReservation GetById(int id)
        {
            return _accReservationRepository.GetById(id);
        }

        public void SaveToOtherCSV(AccommodationReservation reservation)
        {
            _accReservationRepository.SaveToOtherCSV(reservation);
        }

        public void DeleteFromOtherCSV(AccommodationReservation reservation)
        {
            _accReservationRepository.DeleteFromOtherCSV(reservation);
        }

        public List<AccommodationReservation> GetProcessedReservations()
        {
            return _accReservationRepository.GetProcessedReservations();
        }

        public void Save(AccommodationReservation accommodationReservation)
        {
            _accReservationRepository.Save(accommodationReservation);
        }

        public void SaveChangeAcceptedReservation(AccommodationReservation reservation)
        {
            _accReservationRepository.SaveChangeAcceptedReservation(reservation);
        }

        public void Update(AccommodationReservation accommodationReservation)
        {
            _accReservationRepository.Update(accommodationReservation);
        }

        public void UpdateFinishedReservationsCSV(AccommodationReservation accommodationReservation)
        {
            _accReservationRepository.UpdateFinishedReservationsCSV(accommodationReservation);
        }

        public void AddReservation(DateTime start, DateTime end, int guests, int days, int accId, User LoggedInUser)
        {
            AccommodationReservation reservation = new AccommodationReservation(start, end, days, guests, accId, LoggedInUser.Id);
            _accReservationRepository.Save(reservation);
        }

        public void ProcessReservation(AccReservationDTO forwardedItem, User LoggedInUser, ChangedReservationRequest selectedReservation)
        {
            SaveWantedDates(forwardedItem, LoggedInUser, selectedReservation);
            SaveAdaptedChangedRequest(forwardedItem, selectedReservation);
        }

        private void SaveAdaptedChangedRequest(AccReservationDTO forwardedItem, ChangedReservationRequest selectedReservation)
        {
            selectedReservation.NewFirstDay = forwardedItem.ReservationFirstDay;
            selectedReservation.NewLastDay = forwardedItem.ReservationLastDay;

            selectedReservation.NewReservationString = forwardedItem.ReservationFirstDay.ToShortDateString() +
                                                       " - " + forwardedItem.ReservationLastDay.ToShortDateString();

            selectedReservation.status = ChangedReservationRequest.Status.ON_HOLD;
            selectedReservation.StatusString = "NA ČEKANjU";
            selectedReservation.ownerComment = "Komentar nije dostupan";
            ChangedReservationRequestService changedReservationRequestService = new ChangedReservationRequestService();
            changedReservationRequestService.Save(selectedReservation);
        }  

        private void SaveWantedDates(AccReservationDTO forwardedItem, User LoggedInUser, ChangedReservationRequest selectedReservation)
        {
            WantedNewDate wanted = new WantedNewDate(forwardedItem.AccommodationId, forwardedItem.AccommodationName,
                                                      forwardedItem.AccommodationMinDaysStay, forwardedItem.ReservationFirstDay,
                                                      forwardedItem.ReservationLastDay, forwardedItem.ReservationDuration,
                                                      forwardedItem.AccommodationMaxGuests, forwardedItem.CurrentGuestNumber,
                                                      LoggedInUser.Id, selectedReservation.reservationId);
            WantedNewDateService wantedNewDateService = new WantedNewDateService();
            wantedNewDateService.Save(wanted);
        }

        public ReservationsInformations SendRequestToOwner(int ownerId)
        {
            AccommodationService accommodationService = new AccommodationService();
            ChangedReservationRequestService changedReservationRequestService = new ChangedReservationRequestService();
            WantedNewDateService wantedNewDateService = new WantedNewDateService();
            List<ChangedReservationRequest> processedReservations = changedReservationRequestService.GetAll();
            List<WantedNewDate> wantedDates = wantedNewDateService.GetAll();

            ReservationsInformations reservationsInformations = new ReservationsInformations();
            foreach(var item in processedReservations)
            {
                Accommodation accommodation = accommodationService.GetById(item.AccommodationId);
                foreach(var item2 in wantedDates)
                {
                    if(IsValidToBeRequest(ownerId, accommodation, item, item2))
                    {
                        reservationsInformations.requests.Add(item);
                        reservationsInformations.newDates.Add(item2);
                    }
                }
            }

            return reservationsInformations;
        }

        private bool IsValidToBeRequest(int ownerId, Accommodation accommodation, ChangedReservationRequest item, WantedNewDate item2)
        {
            return accommodation.OwnerId == ownerId && accommodation.Id == item2.AccommodationId && item.UserId == item2.UserId && item.reservationId == item2.OldReservationId && item.status == ChangedReservationRequest.Status.ON_HOLD;
        }

        public void AcceptReservationChanges(WantedNewDate newReservation, ChangedReservationRequest oldReservation, int ownerId)
        {
            AccommodationReservation reservation = SaveChangeAcceptedReservation(newReservation, oldReservation);
            CreateAcceptedReportItem(newReservation, oldReservation);
            SaveAcceptedNotificationToGuest(reservation, ownerId);
        }
        
        private void SaveAcceptedNotificationToGuest(AccommodationReservation reservation, int ownerId)
        {
            AccommodationService accommodationService = new AccommodationService();
            NotificationFromOwnerService notificationFromOwnerService = new NotificationFromOwnerService();
            Accommodation accommodation = accommodationService.GetById(reservation.AccommodationId);
            NotificationFromOwner newNotification = new NotificationFromOwner(reservation.Id, accommodation, ownerId, reservation.UserId);
            newNotification.Answer = "Odobreno";
            notificationFromOwnerService.Save(newNotification);
        }

        private void CreateAcceptedReportItem(WantedNewDate newReservation, ChangedReservationRequest oldReservation)
        {
            ChangedReservationRequestService changedReservationRequestService = new ChangedReservationRequestService();
            WantedNewDateService wantedNewDateService = new WantedNewDateService();
            changedReservationRequestService.Delete(oldReservation.Id);
            ChangedReservationRequest processedReservation = new ChangedReservationRequest(oldReservation.reservationId, oldReservation.AccommodationId,
                                                                                            oldReservation.AccommodationName, oldReservation.City, oldReservation.Country,
                                                                                            oldReservation.OldFirstDay, oldReservation.OldLastDay,
                                                                                            oldReservation.GuestNumber, oldReservation.UserId);
            processedReservation.NewFirstDay = newReservation.ReservationFirstDay;
            processedReservation.NewLastDay = newReservation.ReservationLastDay;
            processedReservation.status = ChangedReservationRequest.Status.ACCEPTED;
            processedReservation.StatusString = "PRIHVAĆENO";
            processedReservation.ownerComment = "Uspješno pomjereno.";
            changedReservationRequestService.Save(processedReservation);

            wantedNewDateService.Delete(newReservation.Id);
            DefinitlyForgetReservation(oldReservation);
        }

        private void DefinitlyForgetReservation(ChangedReservationRequest oldReservation)
        {
            List<AccommodationReservation> local = _accReservationRepository.GetProcessedReservations();
            foreach(var item in local)
            {
                if(item.Id == oldReservation.reservationId)
                {
                    item.DefinitlyChanged = true;
                    _accReservationRepository.UpdateToDefinitlyForget(item);
                }
            }
        }

        private AccommodationReservation SaveChangeAcceptedReservation(WantedNewDate newReservation, ChangedReservationRequest oldReservation)
        {
            AccommodationReservation reservation = new AccommodationReservation(newReservation.ReservationFirstDay, newReservation.ReservationLastDay,
                                                                                newReservation.ReservationDuration, newReservation.AccommodationMaxGuests,
                                                                                newReservation.AccommodationId, oldReservation.UserId);
            _accReservationRepository.SaveChangeAcceptedReservation(reservation);
            return reservation;
        }

        private AccommodationReservation DeleteFromShortTimeDeletedCSV(ChangedReservationRequest oldReservation)
        {
            List<AccommodationReservation> helpList = _accReservationRepository.GetProcessedReservations();
            AccommodationReservation reservation = new AccommodationReservation();
            foreach (var item in helpList)
            {
                bool correct = item.FirstDay == oldReservation.OldFirstDay && item.LastDay == oldReservation.OldLastDay && item.Id == oldReservation.reservationId;
                if (correct)
                {
                    reservation = item;
                    _accReservationRepository.DeleteFromOtherCSV(reservation);
                    break;
                }
            }

            _accReservationRepository.Save(reservation);
            return reservation;
        }

        private void CreateDeclinedReportItem(string ownerComment, WantedNewDate newReservation, ChangedReservationRequest oldReservation)
        {
            ChangedReservationRequestService changedReservationRequestService = new ChangedReservationRequestService();
            WantedNewDateService wantedNewDateService = new WantedNewDateService();
            changedReservationRequestService.Delete(oldReservation.Id);
            ChangedReservationRequest processedReservation = new ChangedReservationRequest(oldReservation.reservationId, oldReservation.AccommodationId,
                                                                                            oldReservation.AccommodationName, oldReservation.City, oldReservation.Country,
                                                                                            oldReservation.OldFirstDay, oldReservation.OldLastDay,
                                                                                            oldReservation.GuestNumber, oldReservation.UserId);
            processedReservation.NewFirstDay = newReservation.ReservationFirstDay;
            processedReservation.NewLastDay = newReservation.ReservationLastDay;
            processedReservation.status = ChangedReservationRequest.Status.REFUSED;
            processedReservation.StatusString = "ODBIJENO";
            processedReservation.ownerComment = ownerComment;
            changedReservationRequestService.Save(processedReservation);

            wantedNewDateService.Delete(newReservation.Id);
        }

        public void DeclineReservationChanges(string ownerComment, WantedNewDate newReservation, ChangedReservationRequest oldReservation, int ownerId)
        {
            AccommodationReservation reservation = DeleteFromShortTimeDeletedCSV(oldReservation);
            CreateDeclinedReportItem(ownerComment, newReservation, oldReservation);
            SaveDeclinedNotificationToGuest(reservation, ownerId);
        }

        private void SaveDeclinedNotificationToGuest(AccommodationReservation reservation, int ownerId)
        {
            AccommodationService accommodationService = new AccommodationService();
            NotificationFromOwnerService notificationFromOwnerService = new NotificationFromOwnerService();
            Accommodation accommodation = accommodationService.GetById(reservation.AccommodationId);
            NotificationFromOwner newNotification = new NotificationFromOwner(reservation.Id, accommodation, ownerId, reservation.UserId);
            newNotification.Answer = "Odbijeno";
            notificationFromOwnerService.Save(newNotification);
        }

        public bool CheckDays(LocAccommodationDTO DTO, int DaysDuration)
        {
            int check = DTO.AccommodationMinDaysStay;
            if (DaysDuration >= check) return true;
            else return false;
        }

        public bool CheckDates(DateTime start, DateTime end)
        {
            if (start.Year < end.Year) return true;
            else if (start.Year == end.Year)
            {
                if (start.Month < end.Month) return true;
                else if (start.Month == end.Month)
                {
                    if (start.Day <= end.Day) return true;
                    else return false;
                }
                else return false;
            }
            else return false;
        }

        public void CancelReservation(CancelAndMarkResDTO selectedReservation)
        {
            SaveCanceledReservation(selectedReservation);
            CreateNotificationToOwner(selectedReservation);
        }

        private void CreateNotificationToOwner(CancelAndMarkResDTO selectedReservation)
        {
            AccommodationService accommodationService = new AccommodationService();
            NotificationService notificationService = new NotificationService();
            Accommodation accommodation = accommodationService.GetById(selectedReservation.AccommodationId);
            string Text = "Otkazana je rezervacija u periodu od " + selectedReservation.FirstDay.ToString() + " do " +
                          selectedReservation.LastDay.ToString() + " u smještaju " + selectedReservation.AccommodationName + ".";
            Notification notification = new Notification(accommodation.OwnerId, Text, Notification.NotificationType.NOTYPE, false);
            notificationService.Save(notification);
        }

        private void SaveCanceledReservation(CancelAndMarkResDTO selectedReservation)
        {
            AccommodationReservation accommodationReservation = _accReservationRepository.GetById(selectedReservation.ReservationId);
            _accReservationRepository.Delete(selectedReservation.ReservationId);
            _accReservationRepository.SaveCanceledReservation(accommodationReservation);
        }

        public int FindLastYearReservationsNumber(User user)
        {
            int resNumber = 0;
            foreach (var reservation in _accReservationRepository.GetAll())
            {
                if (reservation.UserId == user.Id && reservation.FirstDay.Year == DateTime.Today.Year - 1)
                {
                    resNumber++;
                }
            }
            resNumber += InvestigateProcessedReservations(user);
            return resNumber;
        }

        private int InvestigateProcessedReservations(User user)
        {
            List<AccommodationReservation> _processedReservations = _accReservationRepository.GetProcessedReservations();
            int resNumber = 0;
            foreach (var reservation in _processedReservations)
            {
                if (user.Id == reservation.UserId && reservation.FirstDay.Year == DateTime.Today.Year - 1 && !reservation.DefinitlyChanged)
                {
                    resNumber++;
                }
            }
            return resNumber;
        }

        public int CalculateUnusedBonusPoints(User user, int points)
        {
            foreach (var reservation in _accReservationRepository.GetAll())
            {
                if (reservation.UserId == user.Id && reservation.FirstDay.Year == DateTime.Today.Year && points > 0)
                {
                    points--;
                }
            }
            return points;
        }
    }
}
