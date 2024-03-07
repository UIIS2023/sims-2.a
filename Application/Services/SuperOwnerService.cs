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

    public class SuperOwnerService
    {
        private readonly IUserRepository _userRepository = Injector.CreateInstance<IUserRepository>();
        private readonly IGuestReviewRepository _guestReviewRepository = Injector.CreateInstance<IGuestReviewRepository>();
        private readonly IGuestAccommodationMarkRepository _guestAccommodationMarkRepository = Injector.CreateInstance<IGuestAccommodationMarkRepository>();
        private readonly IAccommodationRepository _accommodationRepository = Injector.CreateInstance<IAccommodationRepository>();


        public SuperOwnerService() { }

        public List<SuperOwner> GetSuperOwners()
        {
            List<SuperOwner> superOwners = new();

            foreach (var user in _userRepository.GetAll()) 
            {
                if (user.Role == Roles.VLASNIK && IsSuperOwner(user.Id)) 
                {
                    var marks = GetOwnerMarks(user.Id);
                    superOwners.Add(new SuperOwner(user.Id,user.Username, GetAveridgeGrade(user.Id)) );
                }
            }

            
            return superOwners;
        }

        public bool IsSuperOwner(int ownerId) 
        {
            return GetOwnerMarks(ownerId).Count > 5 && GetAveridgeGrade(ownerId) >= 9.5;
        }

        public bool IsSuperOwnerAccommodation(int accommodationId)
        {
            var ownerId = _accommodationRepository.GetById(accommodationId).OwnerId;
            return IsSuperOwner(ownerId);
        }

        public List<Accommodation> GetSuperOwnerAccommodations() 
        {
            List<Accommodation> superAccommodations = new List<Accommodation>();
            foreach (var superOwner in GetSuperOwners()) 
            {
                foreach (var accommodation in _accommodationRepository.GetAllByUserId(superOwner.UserId)) 
                {
                    superAccommodations.Add(accommodation);

                }
            }
            return superAccommodations;
        }

        public List<Accommodation> GetRegularOwnerAccommodations()
        {
            var superOwners = GetSuperOwnerAccommodations().Select(a => a.OwnerId).ToList();
            return _accommodationRepository.GetAll().Where(a => !superOwners.Contains(a.OwnerId)).ToList();
        }
        


        private List<GuestAccommodationMark> GetOwnerMarks(int ownerId)
        {
            List<GuestAccommodationMark> guestAccommodationMarks = new List<GuestAccommodationMark>();
            foreach (var mark in _guestAccommodationMarkRepository.GetAll())
            {
                var accommodation = _accommodationRepository.GetById(mark.AccommodationId);

                if (ownerId == accommodation.OwnerId)
                {
                    guestAccommodationMarks.Add(mark);
                }

            }
            return guestAccommodationMarks;
        }

        private double GetAveridgeGrade(int ownerId)
        {
            List<GuestAccommodationMark> marks = GetOwnerMarks(ownerId);
            if (marks == null || marks.Count < 1) return 0;
            return marks.Average(r => r.OwnerMark + r.CleanMark);
        }


    }
}
