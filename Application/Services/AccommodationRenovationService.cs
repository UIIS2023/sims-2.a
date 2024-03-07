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
    public class AccommodationRenovationService
    {
        private readonly IAccommodationRenovationRepository _accommodationRenovationRepository = Injector.CreateInstance<IAccommodationRenovationRepository>();

        public AccommodationRenovationService()
        {

        }

        public void Delete(int id)
        {
            _accommodationRenovationRepository.Delete(id);
        }

        public List<AccommodationRenovation> GetAll()
        {
            return _accommodationRenovationRepository.GetAll();
        }

        public AccommodationRenovation GetById(int id)
        {
            return _accommodationRenovationRepository.GetById(id);
        }


        public void Save(AccommodationRenovation accommodationRenovation)
        {
            _accommodationRenovationRepository.Save(accommodationRenovation);
        }

        public void Update(AccommodationRenovation accommodationRenovation)
        {
            _accommodationRenovationRepository.Update(accommodationRenovation);
        }
    }

}
