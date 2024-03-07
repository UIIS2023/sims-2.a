using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class VoucherService
    {
        private readonly IVoucherRepository _voucherRepository = Injector.CreateInstance<IVoucherRepository>();
        public VoucherService() { }

        public void Delete(int id)
        {
            _voucherRepository.Delete(id);
        }

        public List<Voucher> GetAll()
        {
            return _voucherRepository.GetAll();
        }

        public Voucher GetById(int id)
        {
            return _voucherRepository.GetById(id);
        }

        public void Save(Voucher voucher)
        {
            _voucherRepository.Save(voucher);
        }

        public void SaveAll(List<Voucher> vouchers)
        {
            _voucherRepository.SaveAll(vouchers);
        }

        public void GiveVouchers(List<Reservation> reservations)
        {
            List<Voucher> vouchers = new List<Voucher>();
            foreach (var reservation in reservations)
            {
                var voucher = new Voucher
                {
                    UserId = reservation.UserId,
                    ExpiryDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(6)),
                    Used = false,
                    GuideId = App.LoggedUser.Id
                };
                vouchers.Add(voucher);
            }

            if (vouchers.Count > 0)
            {
                SaveAll(vouchers);
            }
        }

        public void GiveVoucher(Reservation reservation)
        {
            var voucher = new Voucher
            {
                UserId = reservation.UserId,
                ExpiryDate = DateOnly.FromDateTime(DateTime.Now.AddYears(2)),
                Used = false,
                GuideId = -1
            };

            Save(voucher);
        }

        public void Update(Voucher voucher)
        {
            _voucherRepository.Update(voucher);
        }

        public void UsedUpdate(int id)
        {
            _voucherRepository.UsedUpdate(id);
        }

        public List<Voucher> GetAllByGuideId(int id)
        {
            return _voucherRepository.GetAll().FindAll(v => v.GuideId == id);
        }
    }
}
