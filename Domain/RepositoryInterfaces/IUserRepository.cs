using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.Domain.RepositoryInterfaces
{
    internal interface IUserRepository : IRepository<User>
    {
        User GetByUsername(string username);
    }
}
