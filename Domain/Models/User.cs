using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public enum Roles { VLASNIK, VODIC, GOST1, GOST2 }
    public class User : ISerializable
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; }

        public bool IsJobQuit { get; set; }

        public User() { }

        public User(string username, string password, Roles role, bool isJobQuit)
        {
            Username = username;
            Password = password;
            Role = role;
            IsJobQuit = isJobQuit;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username, Password, Role.ToString(), IsJobQuit.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Username = values[1];
            Password = values[2];
            switch (values[3])
            {
                case "VLASNIK":
                    Role = Roles.VLASNIK;
                    break;
                case "VODIC":
                    Role = Roles.VODIC;
                    break;
                case "GOST1":
                    Role = Roles.GOST1;
                    break;
                case "GOST2":
                    Role = Roles.GOST2;
                    break;
            }
            IsJobQuit = bool.Parse(values[4]);
        }
    }
}
