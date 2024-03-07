using SOSTeam.TravelAgency.Application.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.WPF.ViewModels.TourGuide
{
    public class AvailableYearsCreator
    {
        private readonly AppointmentService _appointmentService;

        public AvailableYearsCreator()
        {
            _appointmentService = new AppointmentService();
        }

        public ObservableCollection<string> GetAvailableYears(User loggedUser)
        {
            ObservableCollection<string> availableYears = new ObservableCollection<string>();
            foreach (var appointment in _appointmentService.GetAllByUserId(loggedUser.Id))
            {
                availableYears.Add(appointment.Start.Year.ToString());
            }

            availableYears = new ObservableCollection<string>(availableYears.Distinct().OrderByDescending(y => Convert.ToInt32(y)));

            return availableYears;
        }
    }
}
