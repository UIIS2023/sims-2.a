using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Repositories.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Repositories
{
    public class FrequentUserVoucherRepository : IFrequentUserVoucherRepository
    {
        private const string FilePath = "../../../Resources/Data/frequentUserVouchers.csv";

        private readonly Serializer<FrequentUserVoucher> _serializer;

        private List<FrequentUserVoucher> _vouchers;

        public FrequentUserVoucherRepository()
        {
            _serializer = new Serializer<FrequentUserVoucher>();
            _vouchers = _serializer.FromCSV(FilePath);
        }

        public void Delete(int id)
        {
            _vouchers = _serializer.FromCSV(FilePath);
            FrequentUserVoucher founded = _vouchers.Find(i => i.Id == id) ?? throw new ArgumentException();
            _vouchers.Remove(founded);
            _serializer.ToCSV(FilePath, _vouchers);
        }

        public List<FrequentUserVoucher> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public FrequentUserVoucher GetById(int id)
        {
            _vouchers = _serializer.FromCSV(FilePath);
            return _vouchers.Find(i => i.Id == id) ?? throw new ArgumentException();
        }

        public int NextId()
        {
            _vouchers = _serializer.FromCSV(FilePath);
            if (_vouchers.Count < 1)
            {
                return 1;
            }
            return _vouchers.Max(v => v.Id) + 1;
        }

        public void Save(FrequentUserVoucher voucher)
        {
            voucher.Id = NextId();
            _vouchers = _serializer.FromCSV(FilePath);
            _vouchers.Add(voucher);
            _serializer.ToCSV(FilePath, _vouchers);
        }

        public void Update(FrequentUserVoucher voucher)
        {
            _vouchers = _serializer.FromCSV(FilePath);
            FrequentUserVoucher current = _vouchers.Find(t => t.Id == voucher.Id) ?? throw new ArgumentException();
            int index = _vouchers.IndexOf(current);
            _vouchers.Remove(current);
            _vouchers.Insert(index, voucher);
            _serializer.ToCSV(FilePath, _vouchers);
        }
    }
}
