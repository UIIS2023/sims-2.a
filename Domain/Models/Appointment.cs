using System;
using System.Globalization;
using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class Appointment : ISerializable
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public int Occupancy { get; set; }
        public bool Started { get; set; }
        public bool Finished { get; set; }
        public int TourId { get; set; }
        public int UserId { get; set; }

        public Appointment()
        {
            Id = -1;
            Start = DateTime.MinValue;
            Occupancy = 0;
            Started = false;
            Finished = false;
            TourId = -1;
            UserId = -1;
        }

        public Appointment(int id, DateTime start, int occupancy, bool started, bool finished, int tourId, int userId)
        {
            Id = id;
            Start = start;
            Occupancy = occupancy;
            Started = started;
            Finished = finished;
            TourId = tourId;
            UserId = userId;
        }

        public bool IsExpired(int durationInHours)
        {
            if (!Started)
            {
                return DateTime.Now > Start;
            }
            
            var duration = TimeSpan.FromHours(durationInHours);
            var end = Start + duration;

            return DateTime.Now > end;
        }

        public bool IsActive()
        {
            return Started && !Finished;
        }

        public bool IsFinished()
        {
            return Started && Finished;
        }

        public bool IsNotStarted()
        {
            return !Started && !Finished;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Start = DateTime.ParseExact(values[1], "dd.MM.yyyy. HH:mm", CultureInfo.InvariantCulture);
            Occupancy = int.Parse(values[2]);
            Started = bool.Parse(values[3]);
            Finished = bool.Parse(values[4]);
            TourId = int.Parse(values[5]);
            UserId = int.Parse(values[6]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Start.ToString("dd.MM.yyyy. HH:mm"),
                Occupancy.ToString(),
                Started.ToString(),
                Finished.ToString(),
                TourId.ToString(),
                UserId.ToString()
            };
            return csvValues;
        }
    }
}
