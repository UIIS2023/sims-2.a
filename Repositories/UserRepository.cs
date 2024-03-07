using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private const string FilePath = "../../../Resources/Data/users.csv";

        private readonly Serializer<User> _serializer;

        private List<User> _users;

        public UserRepository()
        {
            _serializer = new Serializer<User>();
            _users = _serializer.FromCSV(FilePath);
        }

        public User GetByUsername(string username)
        {
            User user = _users.Find(l => l.Username == username) ?? throw new ArgumentException();
            return user;
        }

        public User GetById(int id)
        {
            User user = _users.Find(l => l.Id == id) ?? throw new ArgumentException();
            return user;
        }

        public List<User> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(User user)
        {
            user.Id = NextId();
            _users = _serializer.FromCSV(FilePath);
            _users.Add(user);
            _serializer.ToCSV(FilePath, _users);
        }

        public void Update(User user)
        {
            _users = _serializer.FromCSV(FilePath);
            User current = _users.Find(t => t.Id == user.Id) ?? throw new ArgumentException();
            int index = _users.IndexOf(current);
            _users.Remove(current);
            _users.Insert(index, user);
            _serializer.ToCSV(FilePath, _users);
        }

        public void Delete(int id)
        {
            _users = _serializer.FromCSV(FilePath);
            User founded = _users.Find(t => t.Id == id) ?? throw new ArgumentException();
            _users.Remove(founded);
            _serializer.ToCSV(FilePath, _users);
        }

        public int NextId()
        {
            _users = _serializer.FromCSV(FilePath);
            if (_users.Count < 1)
            {
                return 1;
            }
            return _users.Max(t => t.Id) + 1;
        }
    }
}
