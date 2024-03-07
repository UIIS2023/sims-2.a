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
    public class ForumCommentReportRepository : IForumCommentReportRepository
    {
        private const string FilePath = "../../../Resources/Data/forumCommentReport.csv";
        private readonly Serializer<ForumCommentReport> _serializer;
        private List<ForumCommentReport> _forumCommentReports;

        public ForumCommentReportRepository()
        {
            _serializer = new Serializer<ForumCommentReport>();
            _forumCommentReports = new List<ForumCommentReport>();
        }

        public List<ForumCommentReport> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(ForumCommentReport forumCommentReport)
        {
            forumCommentReport.Id = NextId();
            _forumCommentReports = _serializer.FromCSV(FilePath);
            _forumCommentReports.Add(forumCommentReport);
            _serializer.ToCSV(FilePath, _forumCommentReports);
        }

        public void Delete(int id)
        {
            _forumCommentReports = _serializer.FromCSV(FilePath);
            ForumCommentReport found = _forumCommentReports.Find(t => t.Id == id) ?? throw new ArgumentException();
            _forumCommentReports.Remove(found);
            _serializer.ToCSV(FilePath, _forumCommentReports);
        }

        public void Update(ForumCommentReport forumCommentReport)
        {
            _forumCommentReports = _serializer.FromCSV(FilePath);
            ForumCommentReport current = _forumCommentReports.Find(t => t.Id == forumCommentReport.Id) ?? throw new ArgumentException();
            int index = _forumCommentReports.IndexOf(current);
            _forumCommentReports.Remove(current);
            _forumCommentReports.Insert(index, forumCommentReport);
            _serializer.ToCSV(FilePath, _forumCommentReports);
        }

        public int NextId()
        {
            _forumCommentReports = _serializer.FromCSV(FilePath);
            if (_forumCommentReports.Count < 1)
            {
                return 1;
            }
            return _forumCommentReports.Max(l => l.Id) + 1;
        }

        public ForumCommentReport GetById(int id)
        {
            _forumCommentReports = _serializer.FromCSV(FilePath);
            return _forumCommentReports.Find(l => l.Id == id) ?? throw new ArgumentException();
        }
    }

}
