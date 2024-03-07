using SOSTeam.TravelAgency.Domain;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Application.Services
{
    internal class ForumCommentReportService
    {
        private readonly IForumCommentReportRepository _forumCommentReportRepository = Injector.CreateInstance<IForumCommentReportRepository>();

        public ForumCommentReportService()
        {

        }

        public List<ForumCommentReport> GetAll()
        {
            return _forumCommentReportRepository.GetAll();
        }

        public void Save(ForumCommentReport forumCommentReport)
        {
            _forumCommentReportRepository.Save(forumCommentReport);
        }

        public void Delete(int id)
        {
            _forumCommentReportRepository.Delete(id);
        }
        
        public void Delete(int userId, int commentId)
        {
            var report = _forumCommentReportRepository.GetAll().Find(r => r.CommentId == commentId && r.UserId == userId);
            if (report == null) return;
            _forumCommentReportRepository.Delete(report.Id);
        }

        public void Update(ForumCommentReport forumCommentReport)
        {
            _forumCommentReportRepository.Update(forumCommentReport);
        }

        public ForumCommentReport GetById(int id)
        {
            return _forumCommentReportRepository.GetById(id);
        }
        public int GetReportsNumber(int commentId)
        {
            return _forumCommentReportRepository.GetAll().Where(r => r.CommentId == commentId).Count();
        }
        public bool IsReported(int userId, int commentId)
        {
            return _forumCommentReportRepository.GetAll().Exists(r => r.UserId == userId && r.CommentId == commentId);
        }
    }

}
