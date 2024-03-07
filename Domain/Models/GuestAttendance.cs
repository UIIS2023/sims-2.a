using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Repositories.Serializer;

public enum GuestPresence { UNKNOWN = 0, YES = 1, NO = 2 };

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class GuestAttendance : ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CheckpointActivityId { get; set; }
        public GuestPresence Presence { get; set; }
        public string Message { get; set; }
        public int ReservationId { get; set; }

        public GuestAttendance()
        {
            Id = -1;
            UserId = -1;
            CheckpointActivityId = -1;
            Presence = GuestPresence.UNKNOWN;
            Message = string.Empty;
            ReservationId = -1;
        }

        public GuestAttendance(int id, int userId, int checkpointActivityId, GuestPresence presence, string message, int reservationId)
        {
            Id = id;
            UserId = userId;
            CheckpointActivityId = checkpointActivityId;
            Presence = presence;
            Message = message;
            ReservationId = reservationId;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            UserId = int.Parse(values[1]);
            CheckpointActivityId = int.Parse(values[2]);
            if (values[3].Equals("YES")) 
            {
                Presence = GuestPresence.YES;
            } 
            else if (values[3].Equals("NO"))
            {
                Presence = GuestPresence.NO;
            }
            else
            {
                Presence = GuestPresence.UNKNOWN;
            }
            Message = values[4];
            ReservationId = int.Parse(values[5]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = 
            { 
                Id.ToString(),
                UserId.ToString(),
                CheckpointActivityId.ToString(),
                Presence.ToString(),
                Message,
                ReservationId.ToString()
            };
            return csvValues;
        }
    }
}
