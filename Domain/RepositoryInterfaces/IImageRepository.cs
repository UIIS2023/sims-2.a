using System.Collections.Generic;
using SOSTeam.TravelAgency.Domain.Models;

namespace SOSTeam.TravelAgency.Domain.RepositoryInterfaces
{
    public interface IImageRepository : IRepository<Image>
    {
        void SaveAll(List<Image> images);
        List<Image> GetAllForTours();
        List<Image> GetAllForAccommodations();
    }
}
