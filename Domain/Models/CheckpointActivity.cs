using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public enum CheckpointStatus { UNKNOWN = 0, NOT_STARTED = 1, ACTIVE = 2, FINISHED = 3 }
    public class CheckpointActivity : ISerializable
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int CheckpointId { get; set; }
        public CheckpointStatus Status { get; set; }

        public CheckpointActivity()
        {
            Id = -1;
            AppointmentId = -1;
            CheckpointId = -1;
            Status = CheckpointStatus.NOT_STARTED;
        }

        public CheckpointActivity(int id, int appointmentId, int checkpointId, CheckpointStatus status)
        {
            Id = id;
            AppointmentId = appointmentId;
            CheckpointId = checkpointId;
            Status = status;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            AppointmentId = int.Parse(values[1]);
            CheckpointId = int.Parse(values[2]);
            Status = values[3] switch
            {
                "NOT_STARTED" => CheckpointStatus.NOT_STARTED,
                "ACTIVE" => CheckpointStatus.ACTIVE,
                "FINISHED" => CheckpointStatus.FINISHED,
                _ => CheckpointStatus.UNKNOWN,
            };
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AppointmentId.ToString(),
                CheckpointId.ToString(),
                Status.ToString(),
            };
            return csvValues;
        }
    }
}
