using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf.hyphenation;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class CreateSuperGuideService
    {
        private readonly AppointmentService _appointmentService;
        private readonly TourService _tourService;
        private readonly TourReviewService _tourReviewService;
        private readonly SuperGuideService _superGuideService;

        public CreateSuperGuideService()
        {
            _appointmentService = new AppointmentService();
            _tourService = new TourService();
            _tourReviewService = new TourReviewService();
            _superGuideService = new SuperGuideService();
        }

        public void CreateSuperTourGuide()
        {
            var superGuideLanguages = FilterAbgGradePerLanguage();

            

            foreach (var language in superGuideLanguages.Keys)
            {
                //Ako vec ima titulu za ovaj jezik ne cuvaj ga
                if (IsSuperGuideAlreadyExists(language))
                {
                    return;
                }
                
                var superTourGuide = new SuperGuide();
                superTourGuide.UserId = App.LoggedUser.Id;
                superTourGuide.Language = language;
                superTourGuide.Year = DateTime.Now.Year;
                _superGuideService.Save(superTourGuide);
            }

        }


        private bool IsSuperGuideAlreadyExists(string language)
        {
            foreach (var superGuide in _superGuideService.GetAllByUserId(App.LoggedUser.Id))
            {
                if(superGuide.Language == language) return true;
            }

            return false;
        }



        private List<string> FindAllLanguagesInLastYear()
        {
            var languages = new List<string>();

            var appointmentsInLastYear = GetAllAppointmentsInLastYear();

            foreach (var appointment in appointmentsInLastYear)
            {
                languages.Add(_tourService.GetById(appointment.TourId).Language);
            }

            var languagesDistinct = new List<string>(languages.Distinct());

            return languagesDistinct;
        }

        private List<Appointment> GetAllAppointmentsInLastYear()
        {

            var appointmentsInLastYear = _appointmentService.GetAllFinishedByUserId(App.LoggedUser.Id).FindAll(a =>
                a.Start.Year == DateTime.Now.AddYears(-1).Year);

            return appointmentsInLastYear;
        }

        private Dictionary<string, double> FilterAbgGradePerLanguage()
        {
            var avgGradePerLanguage = GetAvgGradePerLanguage();
            return avgGradePerLanguage.Where(kv => kv.Value >= 4.0).ToDictionary(kv => kv.Key, kv => kv.Value);
        }


        private Dictionary<string, double> GetAvgGradePerLanguage()
        {

            var languagesInLastYear = FindAllLanguagesInLastYear();

            var avgGradesPerLanguage = new Dictionary<string, double>();

            foreach (var language in languagesInLastYear)
            {
                 var avgPerLanguage = GetAvgGradeForLanguage(language);
                 avgGradesPerLanguage.Add(language, avgPerLanguage);
            }


            return avgGradesPerLanguage;
        }

        private double GetAvgGradeForLanguage(string language)
        {
            var appointmentsInLastYearByLanguage = GetAllAppointmentsByLanguage(language);
            double sumOfAvgGradesByAppointments = 0;

            int numOfAppointmentsInLastYearByLanguage = appointmentsInLastYearByLanguage.Count;

            //Ako nije bilo minimalno 5(20) appointmenta na ovom jeziku on i nije konkurent
            if (numOfAppointmentsInLastYearByLanguage < 5)
            {
                return -1;
            }

            foreach (var appointment in appointmentsInLastYearByLanguage)
            {
                if (language == _tourService.GetById(appointment.TourId).Language)
                {
                    // Prosek ocene za jedan termin
                    double avgGrade = GetAppointmentAvgGrade(appointment);
                    sumOfAvgGradesByAppointments += avgGrade;
                }
            }

            return sumOfAvgGradesByAppointments / numOfAppointmentsInLastYearByLanguage;

        }

        private List<Appointment> GetAllAppointmentsByLanguage(string language)
        {
            var appointments = new List<Appointment>();
            var appointmentsInLastYear = GetAllAppointmentsInLastYear();

            foreach (var appointment in appointmentsInLastYear)
            {
                var tour = _tourService.GetById(appointment.TourId);

                if (tour.Language == language)
                {
                    appointments.Add(appointment);
                }
            }

            return appointments;
        }

        private double GetAppointmentAvgGrade(Appointment appointment)
        {
            double appointmentGradeAvg = 0;
            
            int appointmentCnt = 0;
            foreach (var review in _tourReviewService.GetAllByAppointmentId(appointment.Id))
            {
                int sumOfGrades = 0;
                sumOfGrades += review.GuideLanguage;
                sumOfGrades += review.GuideKnowledge;
                sumOfGrades += review.InterestRating;

                appointmentGradeAvg += sumOfGrades / 3.0;
                appointmentCnt++;
            }

            return appointmentGradeAvg/appointmentCnt;
        }

    }
}
