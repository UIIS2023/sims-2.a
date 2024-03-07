using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.Domain.RepositoryInterfaces
{
    public interface INewTourNotificationRepository : IRepository<NewTourNotification>
    {
        List<NewTourNotification> GetAllByGuestId(int guestId);
        void SaveAll(List<NewTourNotification> notifications);
    }
}
