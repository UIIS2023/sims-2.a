using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public enum ImageType { NO_TYPE = 0, ACCOMMODATION = 1, TOUR = 2, RESERVATION = 3, GUEST2 = 4 }
    public class Image : ISerializable
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public bool Cover { get; set; }
        public int EntityId { get; set; }
        public ImageType Type { get; set; }

        public Image()
        {
            Id = -1;
            Path = string.Empty;
            Cover = false;
            EntityId = -1;
            Type = ImageType.NO_TYPE;
        }

        public Image(string path, bool cover, int entityId,ImageType type)
        {
            Path = path;
            Cover = cover;
            EntityId = entityId;
            Type = type;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Path = values[1];
            Cover = bool.Parse(values[2]);
            EntityId = int.Parse(values[3]);
            Type = values[4] switch
            {
                "ACCOMMODATION" => ImageType.ACCOMMODATION,
                "TOUR" => ImageType.TOUR,
                "RESERVATION" => ImageType.RESERVATION,
                "GUEST2" => ImageType.GUEST2,
                _ => ImageType.NO_TYPE,
            };
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Path,
                Cover.ToString(),
                EntityId.ToString(),
                Type.ToString()
            };
            return csvValues;
        }
    }
}
