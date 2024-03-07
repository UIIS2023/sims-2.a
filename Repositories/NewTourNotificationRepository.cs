using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Repositories
{
    public class NewTourNotificationRepository : INewTourNotificationRepository
    {
        private const string FilePath = "../../../Resources/Data/newTourNotifications.csv";

        private readonly Serializer<NewTourNotification> _serializer;

        private List<NewTourNotification> _newTourNotifications;

        public NewTourNotificationRepository()
        {
            _serializer = new Serializer<NewTourNotification>();
            _newTourNotifications = _serializer.FromCSV(FilePath);
        }
        public void Delete(int id)
        {
            _newTourNotifications = _serializer.FromCSV(FilePath);
            var founded = _newTourNotifications.Find(n=> n.Id == id);
            _newTourNotifications.Remove(founded);
            _serializer.ToCSV(FilePath, _newTourNotifications);
        }

        public List<NewTourNotification> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public List<NewTourNotification> GetAllByGuestId(int guestId)
        {
            _newTourNotifications = _serializer.FromCSV(FilePath);
            return _newTourNotifications.FindAll(r => r.GuestId == guestId);
        }

        public NewTourNotification? GetById(int id)
        {
            _newTourNotifications = _serializer.FromCSV(FilePath);
            return _newTourNotifications.FirstOrDefault(r => r.Id == id);
        }

        public int NextId()
        {
            _newTourNotifications = _serializer.FromCSV(FilePath);
            if (_newTourNotifications.Count < 1)
            {
                return 1;
            }
            return _newTourNotifications.Max(t => t.Id) + 1;
        }

        public void Save(NewTourNotification entity)
        {
            entity.Id = NextId();
            _newTourNotifications = _serializer.FromCSV(FilePath);
            _newTourNotifications.Add(entity);
            _serializer.ToCSV(FilePath, _newTourNotifications);
        }

        public void SaveAll(List<NewTourNotification> notifications)
        {
            foreach (var notification in notifications)
            {
                Save(notification);
            }
        }

        public void Update(NewTourNotification entity)
        {
            _newTourNotifications = _serializer.FromCSV(FilePath);
            var current = _newTourNotifications.Find(t => t.Id == entity.Id) ?? throw new ArgumentException();
            int index = _newTourNotifications.IndexOf(current);
            _newTourNotifications.Remove(current);
            _newTourNotifications.Insert(index, entity);
            _serializer.ToCSV(FilePath, _newTourNotifications);
        }
    }
}
