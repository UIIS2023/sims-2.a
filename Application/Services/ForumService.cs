using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class ForumService
    {
        private readonly IForumRepository forumRepository = Injector.CreateInstance<IForumRepository>();

        public ForumService() { }

        public void Delete(int id)
        {
            forumRepository.Delete(id);
        }

        public List<Forum> GetAll()
        {
            return forumRepository.GetAll();
        }

        public Forum GetById(int id)
        {
            return forumRepository.GetById(id);
        }

        public void Save(Forum forum)
        {
            forumRepository.Save(forum);
        }

        public void Update(Forum forum)
        {
            forumRepository.Update(forum);
        }
    }
}
