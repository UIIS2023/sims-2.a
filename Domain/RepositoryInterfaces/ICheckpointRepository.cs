using System.Collections.Generic;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.Domain.RepositoryInterfaces
{
    public interface ICheckpointRepository : IRepository<Checkpoint>
    {
        void SaveAll(List<Checkpoint> checkpoints);
        List<Checkpoint> GetAllByTourId(int id);
    }
}
