using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Repositories.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class Forum : ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsOpen { get; set; }
        public string Useful { get; set; }


        public Forum()
        {

        }

        public Forum(int userId, int locationId, string title, string description, bool isOpen)
        {
            UserId = userId;
            LocationId = locationId;
            LocationService locationService = new LocationService();
            LocationName = locationService.GetById(locationId).City + ", " + locationService.GetById(locationId).Country;
            Title = title;
            Description = description;
            IsOpen = isOpen;
            Useful = "";
        }
        public Forum(int id, int userId, int locationId, string title, string description, bool isOpen)
        {
            Id = id;
            UserId = userId;
            LocationId = locationId;
            LocationService locationService = new LocationService();
            LocationName = locationService.GetById(locationId).City + ", " + locationService.GetById(locationId).Country;
            Title = title;
            Description = description;
            IsOpen = isOpen;
            Useful = "";
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), UserId.ToString(), LocationId.ToString(), Title, Description, IsOpen.ToString(), Useful };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            UserId = Convert.ToInt32(values[i++]);
            LocationId = Convert.ToInt32(values[i++]);
            LocationService locationService = new LocationService();
            LocationName = locationService.GetById(LocationId).City + ", " + locationService.GetById(LocationId).Country;
            Title = values[i++];
            Description = values[i++];
            if (values[i++].Equals("False"))
                IsOpen = false;
            else 
                IsOpen = true;
            Useful = values[i++];
        }
    }
}
