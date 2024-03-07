using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Repositories
{
    public class AccommodationReservationRepository : IAccReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationReservation.csv";
        private readonly Serializer<AccommodationReservation> _serializer;
        private List<AccommodationReservation> _accommodationReservations;
        private List<AccommodationReservation> _reservations;

        public AccommodationReservationRepository() 
        { 
            _serializer = new Serializer<AccommodationReservation>();
            _accommodationReservations = new List<AccommodationReservation>();
        }

        public void DeleteFromFinishedCSV(AccommodationReservation reservation)
        {
            const string filePath = "../../../Resources/Data/finishedReservations.csv";
            _accommodationReservations = _serializer.FromCSV(filePath);
            AccommodationReservation found = _accommodationReservations.Find(t => t.Id == reservation.Id) ?? throw new ArgumentException();
            _accommodationReservations.Remove(found);
            _serializer.ToCSV(filePath, _accommodationReservations);
        }

        public void SaveFinishedReservation(AccommodationReservation reservation)
        {
            const string filePath = "../../../Resources/Data/finishedReservations.csv";
            _reservations = _serializer.FromCSV(filePath);
            _reservations.Add(reservation);
            _serializer.ToCSV(filePath, _reservations);
        }

        public void SaveCanceledReservation(AccommodationReservation reservation)
        {
            const string filePath = "../../../Resources/Data/canceledReservations.csv";
            _reservations = _serializer.FromCSV(filePath);
            _reservations.Add(reservation);
            _serializer.ToCSV(filePath, _reservations);
        }

        public List<AccommodationReservation> LoadFinishedReservations()
        {
            const string filePath = "../../../Resources/Data/finishedReservations.csv";
            return _serializer.FromCSV(filePath);
        }

        public List<AccommodationReservation> LoadCanceledReservations()
        {
            const string filePath = "../../../Resources/Data/canceledReservations.csv";
            return _serializer.FromCSV(filePath);
        }

        public void Update(AccommodationReservation accommodationReservation)
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            AccommodationReservation current = _accommodationReservations.Find(d => d.Id == accommodationReservation.Id) ?? throw new ArgumentException();
            int index = _accommodationReservations.IndexOf(current);
            _accommodationReservations.Remove(current);
            _accommodationReservations.Insert(index, accommodationReservation);
            _serializer.ToCSV(FilePath, _accommodationReservations);
        }

        public void SaveOld(AccommodationReservation reservation)
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            if(_accommodationReservations.Count > reservation.Id)
            {
                _accommodationReservations.Insert(reservation.Id, reservation);
            }
            else
            {
                _accommodationReservations.Add(reservation);
            }
            _serializer.ToCSV(FilePath, _accommodationReservations);
        }

        public void UpdateFinishedReservationsCSV(AccommodationReservation accommodationReservation)
        {
            const string FilePath = "../../../Resources/Data/finishedReservations.csv";
            _accommodationReservations = _serializer.FromCSV(FilePath);
            AccommodationReservation current = _accommodationReservations.Find(d => d.Id == accommodationReservation.Id) ?? throw new ArgumentException();
            int index = _accommodationReservations.IndexOf(current);
            _accommodationReservations.Remove(current);
            _accommodationReservations.Insert(index, accommodationReservation);
            _serializer.ToCSV(FilePath, _accommodationReservations);
        }

        public void UpdateToDefinitlyForget(AccommodationReservation accommodationReservation)
        {
            const string filePath = "../../../Resources/Data/shortTimeDeletedReservations.csv";
            _accommodationReservations = _serializer.FromCSV(filePath);
            AccommodationReservation current = _accommodationReservations.Find(d => d.Id == accommodationReservation.Id) ?? throw new ArgumentException();
            int index = _accommodationReservations.IndexOf(current);
            _accommodationReservations.Remove(current);
            _accommodationReservations.Insert(index, accommodationReservation);
            _serializer.ToCSV(filePath, _accommodationReservations);
        }

        public AccommodationReservation GetById(int id)
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            return _accommodationReservations.Find(a => a.Id == id) ?? throw new ArgumentException();
        }

        public void SaveToOtherCSV(AccommodationReservation reservation)
        {
            const string filePath = "../../../Resources/Data/shortTimeDeletedReservations.csv";
            _reservations = _serializer.FromCSV(filePath);
            _reservations.Add(reservation);
            _serializer.ToCSV(filePath, _reservations);
        }

        public void DeleteFromOtherCSV(AccommodationReservation reservation)
        {
            const string filePath = "../../../Resources/Data/shortTimeDeletedReservations.csv";
            _accommodationReservations = _serializer.FromCSV(filePath);
            AccommodationReservation found = _accommodationReservations.Find(t => t.Id == reservation.Id && !t.DefinitlyChanged) ?? throw new ArgumentException();
            _accommodationReservations.Remove(found);
            _serializer.ToCSV(filePath, _accommodationReservations);
        }

        public List<AccommodationReservation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public List<AccommodationReservation> GetProcessedReservations()
        {
            const string filePath = "../../../Resources/Data/shortTimeDeletedReservations.csv";
            return _serializer.FromCSV(filePath);
        }

        public void Save(AccommodationReservation accommodationReservation)
        {
            accommodationReservation.Id = NextId();
            _accommodationReservations = _serializer.FromCSV(FilePath);
            _accommodationReservations.Add(accommodationReservation);
            _serializer.ToCSV(FilePath, _accommodationReservations);
        }

        public void SaveChangeAcceptedReservation(AccommodationReservation accReservation)
        {
            accReservation.Id = NextId()/* + 50*/;
            _accommodationReservations = _serializer.FromCSV(FilePath);
            _accommodationReservations.Add(accReservation);
            _serializer.ToCSV(FilePath, _accommodationReservations);
        }

        public void Delete(int id)
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            AccommodationReservation found = _accommodationReservations.Find(t => t.Id == id) ?? throw new ArgumentException();
            _accommodationReservations.Remove(found);
            _serializer.ToCSV(FilePath, _accommodationReservations);
        }

        public int NextId()
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            if (_accommodationReservations.Count < 1)
            {
                return 1;
            }
            return _accommodationReservations.Max(l => l.Id) + 1;
        }
    }
}
