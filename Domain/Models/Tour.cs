using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class Tour : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int MaxNumOfGuests { get; set; }
        public int Duration { get; set; }

        public Tour()
        {
            Id = -1;
            Name = string.Empty;
            LocationId = -1;
            Description = string.Empty;
            Language = string.Empty;
            MaxNumOfGuests = 0;
            Duration = 0;
        }
        public Tour(int id, string name, int locationId, string description, string language, int maxNumOfGuests, int duration)
        {
            Id = id;
            Name = name;
            LocationId = locationId;
            Description = description;
            Language = language;
            MaxNumOfGuests = maxNumOfGuests;
            Duration = duration;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Name = values[1];
            LocationId = int.Parse(values[2]);
            Description = values[3];
            Language = values[4];
            MaxNumOfGuests = int.Parse(values[5]);
            Duration = int.Parse(values[6]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Name,
                LocationId.ToString(),
                Description,
                Language,
                MaxNumOfGuests.ToString(),
                Duration.ToString()
            };
            return csvValues;
        }
    }
}
