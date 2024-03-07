using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class ForumCommentService
    {
        private readonly IForumCommentRepository forumCommentRepository = Injector.CreateInstance<IForumCommentRepository>();

        public ForumCommentService() { }

        public void Delete(int id)
        {
            forumCommentRepository.Delete(id);
        }

        public List<ForumComment> GetAll()
        {
            return forumCommentRepository.GetAll();
        }
        
        public List<ForumComment> GetAllForForum(int forumId)
        {
            return forumCommentRepository.GetAll().Where(f => f.ForumId == forumId).ToList();
        }

        public ForumComment GetById(int id)
        {
            return forumCommentRepository.GetById(id);
        }

        public void Save(ForumComment comment)
        {
            forumCommentRepository.Save(comment);
        }

        public void Update(ForumComment comment)
        {
            forumCommentRepository.Update(comment);
        }
    }
}
