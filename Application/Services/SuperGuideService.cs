using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class SuperGuideService
    {
        private readonly ISuperGuideRepository _superGuideRepository;

        public SuperGuideService()
        {
            _superGuideRepository = Injector.CreateInstance<ISuperGuideRepository>();
        }

        public void Delete(int id)
        {
            _superGuideRepository.Delete(id);
        }

        public List<SuperGuide> GetAll()
        {
            return _superGuideRepository.GetAll();
        }

        public SuperGuide GetById(int id)
        {
            return _superGuideRepository.GetById(id);
        }

        public void Save(SuperGuide superGuide)
        {
            _superGuideRepository.Save(superGuide);
        }

        public void Update(SuperGuide superGuide)
        {
            _superGuideRepository.Update(superGuide);
        }

        public List<SuperGuide> GetAllByUserId(int id)
        {
            return _superGuideRepository.GetAll().FindAll(sg => sg.UserId == id);
        }

    }
}
