using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.Domain.RepositoryInterfaces
{
    public interface ICheckpointActivityRepository : IRepository<CheckpointActivity>
    {
        void SaveAll(List<CheckpointActivity> activities);
        List<CheckpointActivity> GetAllByAppointmentId(int id);
    }
}
