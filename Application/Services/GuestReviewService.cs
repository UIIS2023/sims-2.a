using SOSTeam.TravelAgency.Domain;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class GuestReviewService 
    {
        private readonly IGuestReviewRepository _guestReviewRepository = Injector.CreateInstance<IGuestReviewRepository>();

        public GuestReviewService() { }

        public void Delete(int id)
        {
            _guestReviewRepository.Delete(id);
        }

        public List<GuestReview> GetAll()
        {
            return _guestReviewRepository.GetAll();
        }

        public List<GuestReview> GetAllReviewed()
        {
            return _guestReviewRepository.GetAll().Where(t => t.IsReviewed).ToList();
        }

        public List<GuestReview> GetAllByUserId(int ownerId)
        {
            return _guestReviewRepository.GetAll().Where(p => p.OwnerId == ownerId).ToList();
        }


        public GuestReview GetById(int id)
        {
            return _guestReviewRepository.GetById(id);
        }


        public void Save(GuestReview guestReview)
        {
            _guestReviewRepository.Save(guestReview);
        }

        public bool ReviewExists(int ownerId, int guestId,int accommodationId)
        {
            return _guestReviewRepository.ReviewExists(ownerId, guestId, accommodationId);
        }

        public bool ReviewByGuestExists(int ownerId, int guestId)
        {
            var guestReviews = _guestReviewRepository.GetAll();
            return guestReviews.Exists(l => l.GuestId == guestId && l.OwnerId == ownerId && l.IsReviewed);
        }

        public bool IsReviewed(int reviewId) 
        {
            return _guestReviewRepository.GetById(reviewId).IsReviewed;
        }


        public void Update(GuestReview guestReview)
        {
            _guestReviewRepository.Update(guestReview);
        }
    }
    
}
