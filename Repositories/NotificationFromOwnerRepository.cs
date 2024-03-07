using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Repositories.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Repositories
{
    public class NotificationFromOwnerRepository : INotificationFromOwnerRepository
    {
        private const string FilePath = "../../../Resources/Data/notificationsFromOwner.csv";
        private readonly Serializer<NotificationFromOwner> _serializer;
        private List<NotificationFromOwner> _notifications;

        public NotificationFromOwnerRepository()
        {
            _serializer = new Serializer<NotificationFromOwner>();
            _notifications = new List<NotificationFromOwner>();
        }

        public void Update(NotificationFromOwner notificationFromOwner)
        {
            _notifications = _serializer.FromCSV(FilePath);
            NotificationFromOwner current = _notifications.Find(d => d.Id == notificationFromOwner.Id) ?? throw new ArgumentException();
            int index = _notifications.IndexOf(current);
            _notifications.Remove(current);
            _notifications.Insert(index, notificationFromOwner);
            _serializer.ToCSV(FilePath, _notifications);
        }

        public NotificationFromOwner GetById(int id)
        {
            _notifications = _serializer.FromCSV(FilePath);
            return _notifications.Find(a => a.Id == id) ?? throw new ArgumentException();
        }

        public List<NotificationFromOwner> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(NotificationFromOwner notificationFromOwner)
        {
            notificationFromOwner.Id = NextId();
            _notifications = _serializer.FromCSV(FilePath);
            _notifications.Add(notificationFromOwner);
            _serializer.ToCSV(FilePath, _notifications);
        }

        public void Delete(int id)
        {
            _notifications = _serializer.FromCSV(FilePath);
            NotificationFromOwner found = _notifications.Find(t => t.Id == id) ?? throw new ArgumentException();
            _notifications.Remove(found);
            _serializer.ToCSV(FilePath, _notifications);
        }

        public int NextId()
        {
            _notifications = _serializer.FromCSV(FilePath);
            if (_notifications.Count < 1)
            {
                return 1;
            }
            return _notifications.Max(l => l.Id) + 1;
        }
    }
}
