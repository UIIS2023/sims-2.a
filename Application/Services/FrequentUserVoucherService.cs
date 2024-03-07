using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class FrequentUserVoucherService
    {
        private readonly IFrequentUserVoucherRepository _voucherRepository = Injector.CreateInstance<IFrequentUserVoucherRepository>();
        public FrequentUserVoucherService() { }

        public void Delete(int id)
        {
            _voucherRepository.Delete(id);
        }

        public List<FrequentUserVoucher> GetAll()
        {
            return _voucherRepository.GetAll();
        }

        public FrequentUserVoucher GetById(int id)
        {
            return _voucherRepository.GetById(id);
        }

        public void Save(FrequentUserVoucher voucher)
        {
            _voucherRepository.Save(voucher);
        }

        public void Update(FrequentUserVoucher voucher)
        {
            _voucherRepository.Update(voucher);
        }
    }
}
