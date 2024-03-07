using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public enum CheckpointType { UNKNOWN = 0, START = 1, END = 2, EXTRA = 3 }
    public class Checkpoint : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CheckpointType Type { get; set; }
        public int TourId { get; set; }
        public Checkpoint()
        {
            Id = -1;
            Name = string.Empty;
            Type = CheckpointType.UNKNOWN;
            TourId = -1;
        }

        public Checkpoint(int id, string name, CheckpointType type, int tourId)
        {
            Id = id;
            Name = name;
            Type = type;
            TourId = tourId;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Name = values[1];
            Type = values[2] switch
            {
                "START" => CheckpointType.START,
                "END" => CheckpointType.END,
                "EXTRA" => CheckpointType.EXTRA,
                _ => CheckpointType.UNKNOWN,
            };
            TourId = int.Parse(values[3]);
        }


        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Name,
                Type.ToString(),
                TourId.ToString()
            };
            return csvValues;
        }
    }
}
