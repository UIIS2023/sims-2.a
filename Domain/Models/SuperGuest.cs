using SOSTeam.TravelAgency.Repositories.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class SuperGuest : ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public int BonusPoints { get; set; }
        public int LastYearReservationsNumber { get; set; }
        public bool IsSuper { get; set; }

        public SuperGuest() { }

        public SuperGuest(int id, string username, int points, int reservations, bool super)
        {
            UserId = id;
            Username = username;
            BonusPoints = points;
            LastYearReservationsNumber = reservations;
            IsSuper = super;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), UserId.ToString(), Username, BonusPoints.ToString(), LastYearReservationsNumber.ToString(), IsSuper.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            Username = values[2];
            BonusPoints = Convert.ToInt32(values[3]);
            LastYearReservationsNumber = Convert.ToInt32(values[4]);
            if (values[5].Equals(("False"))) IsSuper = false;
            else IsSuper = true; 
        }
    }
}
