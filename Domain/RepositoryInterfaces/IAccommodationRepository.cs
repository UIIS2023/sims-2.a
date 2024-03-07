using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.Domain.RepositoryInterfaces
{
    public interface IAccommodationRepository : IRepository<Accommodation>
    {
        List<Accommodation> GetAllByUserId(int id);
        Accommodation SaveAndReturn(Accommodation accommodation);
    }
}
