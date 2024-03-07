using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class ChangedReservationRequestService
    {
        private readonly IChangedResRequestRepositroy changedResRequestRepositroy = Injector.CreateInstance<IChangedResRequestRepositroy>();

        public ChangedReservationRequestService() { }

        public void Delete(int id)
        {
            changedResRequestRepositroy.Delete(id);
        }

        public List<ChangedReservationRequest> GetAll()
        {
            return changedResRequestRepositroy.GetAll();
        }

        public ChangedReservationRequest GetById(int id)
        {
            return changedResRequestRepositroy.GetById(id);
        }

        public void Save(ChangedReservationRequest changedReservationRequest)
        {
            changedResRequestRepositroy.Save(changedReservationRequest);
        }

        public void Update(ChangedReservationRequest changedReservationRequest)
        {
            changedResRequestRepositroy.Update(changedReservationRequest);
        }
    }
}
