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
    public class ForumRepository : IForumRepository
    {
        private const string FilePath = "../../../Resources/Data/forum.csv";
        private readonly Serializer<Forum> _serializer;
        private List<Forum> _forums;

        public ForumRepository()
        {
            _serializer = new Serializer<Forum>();
            _forums = new List<Forum>();
        }

        public void Update(Forum forum)
        {
            _forums = _serializer.FromCSV(FilePath);
            Forum current = _forums.Find(d => d.Id == forum.Id) ?? throw new ArgumentException();
            int index = _forums.IndexOf(current);
            _forums.Remove(current);
            _forums.Insert(index, forum);
            _serializer.ToCSV(FilePath, _forums);
        }

        public Forum GetById(int id)
        {
            _forums = _serializer.FromCSV(FilePath);
            return _forums.Find(a => a.Id == id) ?? throw new ArgumentException();
        }

        public List<Forum> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(Forum forum)
        {
            forum.Id = NextId();
            _forums = _serializer.FromCSV(FilePath);
            _forums.Add(forum);
            _serializer.ToCSV(FilePath, _forums);
        }

        public void Delete(int id)
        {
            _forums = _serializer.FromCSV(FilePath);
            Forum found = _forums.Find(t => t.Id == id) ?? throw new ArgumentException();
            _forums.Remove(found);
            _serializer.ToCSV(FilePath, _forums);
        }

        public int NextId()
        {
            _forums = _serializer.FromCSV(FilePath);
            if (_forums.Count < 1)
            {
                return 1;
            }
            return _forums.Max(l => l.Id) + 1;
        }
    }
}
