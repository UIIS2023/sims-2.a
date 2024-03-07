using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class Location : ISerializable
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        public Location()
        {
            Id = -1;
            Country = string.Empty;
            City = string.Empty;
        }

        public Location(string country, string city)
        {
            Country = country;
            City = city;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Country = values[1];
            City = values[2];
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Country,
                City
            };
            return csvValues;
        }
    }
}
