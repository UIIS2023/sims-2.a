using SOSTeam.TravelAgency.Domain.Models;
using System.Collections.Generic;

namespace SOSTeam.TravelAgency.Domain.RepositoryInterfaces
{
    public interface IGuestAttendanceRepository : IRepository<GuestAttendance>
    {
        List<GuestAttendance> GetAllByActivityId(int id);
        List<GuestAttendance> GetByUserId(int id);
        void SaveAll(List<GuestAttendance> attendances);
    }
}
