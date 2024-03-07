using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Repositories
{
    public class VoucherRepository : IVoucherRepository
    {
        private const string FilePath = "../../../Resources/Data/vouchers.csv";

        private readonly Serializer<Voucher> _serializer;

        private List<Voucher> _vouchers;

        public VoucherRepository()
        {
            _serializer = new Serializer<Voucher>();
            _vouchers = _serializer.FromCSV(FilePath);
        }

        public void Delete(int id)
        {
            _vouchers = _serializer.FromCSV(FilePath);
            Voucher founded = _vouchers.Find(i => i.Id == id) ?? throw new ArgumentException();
            _vouchers.Remove(founded);
            _serializer.ToCSV(FilePath, _vouchers);
        }

        public List<Voucher> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Voucher GetById(int id)
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

        public void Save(Voucher voucher)
        {
            voucher.Id = NextId();
            _vouchers = _serializer.FromCSV(FilePath);
            _vouchers.Add(voucher);
            _serializer.ToCSV(FilePath, _vouchers);
        }

        public void SaveAll(List<Voucher> vouchers)
        {
            foreach (var voucher in vouchers)
            {
                Save(voucher);
            }
        }

        public void Update(Voucher voucher)
        {
            _vouchers = _serializer.FromCSV(FilePath);
            Voucher current = _vouchers.Find(t => t.Id == voucher.Id) ?? throw new ArgumentException();
            int index = _vouchers.IndexOf(current);
            _vouchers.Remove(current);
            _vouchers.Insert(index, voucher);
            _serializer.ToCSV(FilePath, _vouchers);
        }

        public void UsedUpdate(int id)
        {
            _vouchers = _serializer.FromCSV(FilePath);
            foreach (var voucher in _vouchers)
            {
                if (voucher.Id == id)
                {
                    voucher.Used = true;
                    Update(voucher);
                    break;
                }
            }
        }
    }
}
