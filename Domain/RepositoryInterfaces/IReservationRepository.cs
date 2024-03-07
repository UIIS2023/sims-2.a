using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.Domain.RepositoryInterfaces
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        int GetId(Reservation reservation);
        void SetPresence(int id);
        void Reviewed(int id);
        List<Reservation> GetAllByAppointmentId(int id);
    }
}
