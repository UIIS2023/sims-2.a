using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SOSTeam.TravelAgency.Domain.RepositoryInterfaces
{
    public interface IAccReservationRepository : IRepository<AccommodationReservation>
    {
        void SaveToOtherCSV(AccommodationReservation reservation);
        void DeleteFromOtherCSV(AccommodationReservation reservation);
        void DeleteFromFinishedCSV(AccommodationReservation reservation);
        void SaveCanceledReservation(AccommodationReservation reservation);
        void SaveFinishedReservation(AccommodationReservation reservation);
        List<AccommodationReservation> LoadFinishedReservations();
        List<AccommodationReservation> LoadCanceledReservations();
        List<AccommodationReservation> GetProcessedReservations();
        void SaveChangeAcceptedReservation(AccommodationReservation reservation);
        void UpdateFinishedReservationsCSV(AccommodationReservation accommodationReservation);
        void UpdateToDefinitlyForget(AccommodationReservation accommodationReservation);
        void SaveOld(AccommodationReservation reservation);
    }
}
