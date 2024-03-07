using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class Reservation : ISerializable
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int TouristNum { get; set; }
        public float AverageAge { get; set; }
        public int UserId { get; set; }
        public bool Presence { get; set; }
        public bool Reviewed { get; set; }
        public int VoucherId { get; set; } 
        public Reservation() { }

        public Reservation(int touristNum,float averageAge, int userId, int appointmentId, int voucherId = -1)
        {
            TouristNum = touristNum;
            AverageAge = averageAge;
            UserId = userId;
            AppointmentId = appointmentId;
            Presence = false;
            Reviewed = false;
            VoucherId = voucherId;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            UserId = int.Parse(values[1]);
            AppointmentId = int.Parse(values[2]);
            TouristNum = int.Parse(values[3]);
            AverageAge = float.Parse(values[4]);
            Presence= bool.Parse(values[5]);
            Reviewed= bool.Parse(values[6]);
            VoucherId = int.Parse(values[7]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                UserId.ToString(),
                AppointmentId.ToString(),
                TouristNum.ToString(),
                AverageAge.ToString(),
                Presence.ToString(),
                Reviewed.ToString(),
                VoucherId.ToString()
            };

            return csvValues;
        }
    }
}
