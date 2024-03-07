using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class QuitGuideJobService
    {
        private readonly AppointmentService _appointmentService;
        private readonly VoucherService _voucherService;
        private readonly UserService _userService;
        private readonly ReservationService _reservationService;

        public QuitGuideJobService()
        {
            _appointmentService = new AppointmentService();
            _voucherService = new VoucherService();
            _userService = new UserService();
            _reservationService = new ReservationService();
        }


        public void QuitJob()
        {
            var quitJobUser = _userService.GetById(App.LoggedUser.Id);
            quitJobUser.IsJobQuit = true;
            _userService.Update(quitJobUser);

            GiveVouchers();

            DeleteFutureAppointments();
        }

        private List<Appointment> GetFutureAppointments()
        {
            return _appointmentService.GetAllByUserId(App.LoggedUser.Id).FindAll(a => a.Start >= DateTime.Now);
        }


        private void GiveVouchers()
        {
            var futureAppointment = GetFutureAppointments();

            //Prolazim kroz sve appointmente u buducnosti koji ce biti otkazani
            foreach (var appointment in futureAppointment)
            {
                //Prodjem kroz sve rezervacije za taj appointment
                foreach (var reservation in _reservationService.GetAllByAppointmentId(appointment.Id))
                {
                    //Proverim da li korisnik koji je rezervasao appointment vec ima vaucer, kod vodica koji daje otkaz
                    if (IsAlreadyHadVoucher(reservation))
                    {
                        foreach(var voucher in _voucherService.GetAllByGuideId(App.LoggedUser.Id))
                        {
                            var dateNow = DateTime.Now;
                            var expiryDate = dateNow.AddYears(2);
                            voucher.GuideId = -1;
                            voucher.ExpiryDate = DateOnly.FromDateTime(expiryDate);
                            _voucherService.Update(voucher);
                        }
                    }
                    else
                    {
                        _voucherService.GiveVoucher(reservation);
                    }
                }
            }
        }


        private bool IsAlreadyHadVoucher(Reservation reservation)
        {
            foreach (var voucher in _voucherService.GetAllByGuideId(App.LoggedUser.Id))
            {
                if (voucher.UserId == reservation.UserId)
                {
                    return true;
                }
            }
            return false;
        }

        private void DeleteFutureAppointments()
        {
            var futureAppointment = GetFutureAppointments();
            foreach (var appointment in futureAppointment)
            {
                _appointmentService.Delete(appointment.Id);
            }
        }

    }
}
