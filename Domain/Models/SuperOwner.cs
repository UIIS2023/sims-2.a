using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class SuperOwner
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public double AveridgeGrade { get; set; }

        public SuperOwner(int userId, string username, double averidgeGrade)
        {
            UserId = userId;
            Username = username;
            AveridgeGrade = averidgeGrade;
        }
    }
}
