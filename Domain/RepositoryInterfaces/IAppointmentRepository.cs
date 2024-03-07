using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.Domain.RepositoryInterfaces
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        List<Appointment> GetAllByUserId(int userId);
        void SaveAll(List<Appointment> appointments);

        void UpdateAll(List<Appointment> appointments);
    }
}
