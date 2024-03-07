using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.Domain.RepositoryInterfaces
{
    public interface IVoucherRepository : IRepository<Voucher>
    {
        void SaveAll(List<Voucher> vouchers);
        void UsedUpdate(int id);
    }
}
