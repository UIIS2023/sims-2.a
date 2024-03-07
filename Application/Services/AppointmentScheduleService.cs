using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class AppointmentScheduleService
    {
        private readonly AppointmentService _appointmentService;
        private readonly TourRequestService _tourRequestService;
        private readonly TourService _tourService;

        public AppointmentScheduleService(int tourRequestId)
        {
            _appointmentService = new AppointmentService();
            _tourRequestService = new TourRequestService();
            _tourService = new TourService();
        }

        public List<DateRange> CreateTourGuideSchedule(int tourRequestId, int tourDurationInHours, int userId)
        {
            var availableDates = new List<DateRange>();

            var requiredDateRange = CreateRequireDateRange(tourRequestId);

            var appointmentsInDateRange =
                _appointmentService.GetScheduledAppointments(requiredDateRange.Start, requiredDateRange.End, userId);

            var busyDateRanges = GetBusyDateRanges(appointmentsInDateRange);        //ovi ce svakako biti zabranjeni


            //Slobodan je sve vreme
            if(busyDateRanges.Count == 0)
            {
                availableDates.Add(requiredDateRange);
                return availableDates;
            }

            var availableDateRanges = GetAvailableDateRanges(busyDateRanges, requiredDateRange);

            var finalAvailableDateRange = GetAvailableDateRanges(busyDateRanges, requiredDateRange);

            foreach (var availableDateRange in availableDateRanges)
            {
                var freeTime = availableDateRange.End - availableDateRange.Start;
                if (TimeSpan.FromHours(tourDurationInHours) > freeTime)
                {
                    var rangeForRemove = finalAvailableDateRange.Find(r => r.Start == availableDateRange.Start && r.End == availableDateRange.End);
                    finalAvailableDateRange.Remove(rangeForRemove);

                }
            }

            availableDates = finalAvailableDateRange;

            return availableDates;
        }

        private DateRange CreateRequireDateRange(int tourRequestId)
        {
            var tourRequest = _tourRequestService.GetById(tourRequestId);

            var requiredStartDate = new DateTime(tourRequest.MaintenanceStartDate.Year,
                tourRequest.MaintenanceStartDate.Month, tourRequest.MaintenanceStartDate.Day);

            var requiredEndDate = new DateTime(tourRequest.MaintenanceEndDate.Year,
                tourRequest.MaintenanceEndDate.Month, tourRequest.MaintenanceEndDate.Day);

            var requiredDateRange = new DateRange(requiredStartDate, requiredEndDate);


            return requiredDateRange;
        }

        private List<DateRange> GetBusyDateRanges(List<Appointment> appointments)
        {
            var busyDateRanges = new List<DateRange>();

            foreach (var appointment in appointments)
            {
                int appointmentDuration = _tourService.GetById(appointment.TourId).Duration;
                var endDate = appointment.Start.AddHours(appointmentDuration);
                var dateRange = new DateRange(appointment.Start, endDate);
                busyDateRanges.Add(dateRange);
            }

            return busyDateRanges;
        }

        private List<DateRange> GetAvailableDateRanges(List<DateRange> busyDateRanges, DateRange requiredDateRange)
        {
            var availableDateRanges = new List<DateRange>();

            //Granice koje je zadao korisni
            var minDate = requiredDateRange.Start;
            var maxDate = requiredDateRange.End;

            for (int i = 0; i < busyDateRanges.Count; i++)
            {
                //Ako je prvi prolazak ide od Start --> Start
                if (i == 0)
                {
                    //Od zadatog do prvog zauzetog.
                    var currentStartDate = busyDateRanges[i].Start;
                    var currentEndDate = busyDateRanges[i].End;
                    availableDateRanges.Add(new DateRange(requiredDateRange.Start, currentStartDate));
                    //I od kraja prvog zauzetog do narednog koji pocinje ako postoji
                    if ((i + 1 < busyDateRanges.Count))
                    {
                        availableDateRanges.Add(new DateRange(currentEndDate, busyDateRanges[i + 1].Start));
                    }
                    //U suprotnom nema vise od tog jednog zauzetog i od njegovog kraja su svi slobodni
                    else
                    {
                        var freeDateRange = new DateRange(currentEndDate, maxDate);
                        availableDateRanges.Add(freeDateRange);
                    }
                }
                //Ako smo sada na drugom zauzetom npr.
                //Slobodni smo nakon njegovog zavrsetka do pocetka narednog ukoliko naredni termin postoji
                else if(i > 0)
                {
                    var currentEndDate = busyDateRanges[i].End;
                    if ((i + 1 < busyDateRanges.Count))
                    {
                        var nextStartDate = busyDateRanges[i + 1].Start;
                        var freeDateRange = new DateRange(currentEndDate, nextStartDate);
                        availableDateRanges.Add(freeDateRange);
                    }
                    //U suprotno nakon njegovog kraja nema vise termina i svi do kraja su slobodni
                    else
                    {
                        var freeDateRange = new DateRange(currentEndDate, maxDate);
                        availableDateRanges.Add(freeDateRange);
                    }
                }
            }

            return availableDateRanges;
        }

    }
}
