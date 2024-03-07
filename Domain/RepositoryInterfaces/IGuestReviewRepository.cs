using SOSTeam.TravelAgency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Domain.RepositoryInterfaces
{
    internal interface IGuestReviewRepository : IRepository<GuestReview>
    {
        public bool ReviewExists(int ownerId, int guestId,int accommodationId);
        public GuestReview SaveAndReturn(GuestReview guestReview);
    }
}
