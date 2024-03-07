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
    public class ForumCommentRepository : IForumCommentRepository
    {
        private const string FilePath = "../../../Resources/Data/forumComments.csv";
        private readonly Serializer<ForumComment> _serializer;
        private List<ForumComment> _comments;

        public ForumCommentRepository()
        {
            _serializer = new Serializer<ForumComment>();
            _comments = new List<ForumComment>();
        }

        public void Update(ForumComment comment)
        {
            _comments = _serializer.FromCSV(FilePath);
            ForumComment current = _comments.Find(d => d.Id == comment.Id) ?? throw new ArgumentException();
            int index = _comments.IndexOf(current);
            _comments.Remove(current);
            _comments.Insert(index, comment);
            _serializer.ToCSV(FilePath, _comments);
        }

        public ForumComment GetById(int id)
        {
            _comments = _serializer.FromCSV(FilePath);
            return _comments.Find(a => a.Id == id) ?? throw new ArgumentException();
        }

        public List<ForumComment> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(ForumComment comment)
        {
            comment.Id = NextId();
            _comments = _serializer.FromCSV(FilePath);
            _comments.Add(comment);
            _serializer.ToCSV(FilePath, _comments);
        }

        public void Delete(int id)
        {
            _comments = _serializer.FromCSV(FilePath);
            ForumComment found = _comments.Find(t => t.Id == id) ?? throw new ArgumentException();
            _comments.Remove(found);
            _serializer.ToCSV(FilePath, _comments);
        }

        public int NextId()
        {
            _comments = _serializer.FromCSV(FilePath);
            if (_comments.Count < 1)
            {
                return 1;
            }
            return _comments.Max(l => l.Id) + 1;
        }
    }
}
