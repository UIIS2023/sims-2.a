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
    public class WantedNewDateService
    {
        private readonly IWantedNewDateRepository wantedNewDateRepository = Injector.CreateInstance<IWantedNewDateRepository>();

        public WantedNewDateService() { }

        public void Delete(int id)
        {
            wantedNewDateRepository.Delete(id);
        }

        public List<WantedNewDate> GetAll()
        {
            return wantedNewDateRepository.GetAll();
        }

        public WantedNewDate GetById(int id)
        {
            return wantedNewDateRepository.GetById(id);
        }

        public void Save(WantedNewDate wantedNewDate)
        {
            wantedNewDateRepository.Save(wantedNewDate);
        }

        public void Update(WantedNewDate wantedNewDate)
        {
            wantedNewDateRepository.Update(wantedNewDate);
        }
    }
}
