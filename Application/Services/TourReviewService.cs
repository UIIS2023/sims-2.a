using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;
using System.Xml.Linq;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class TourReviewService
    {
        private readonly ITourReviewRepository _tourReviewRepository = Injector.CreateInstance<ITourReviewRepository>();

        public TourReviewService() { }

        public void Delete(int id)
        {
            _tourReviewRepository.Delete(id);
        }

        public List<TourReview> GetAll()
        {
            return _tourReviewRepository.GetAll();
        }

        public TourReview GetById(int id)
        {
            return _tourReviewRepository.GetById(id);
        }

        public void Save(TourReview review)
        {
            _tourReviewRepository.Save(review);
        }

        public void Update(TourReview review)
        {
            _tourReviewRepository.Update(review);
        }

        public List<TourReview> GetAllByAppointmentId(int id)
        {
            return _tourReviewRepository.GetAllByAppointmentId(id);
        }

        public void CreateTourReview(int userId, int appointmentId, int guideKnowledge, int guideLanguage, int interestRating, string comment, bool reported)
        {
            TourReview tourReview = new TourReview(userId,appointmentId,guideKnowledge,guideLanguage,interestRating,comment, false);
            _tourReviewRepository.Save(tourReview);
        }
    }
}
