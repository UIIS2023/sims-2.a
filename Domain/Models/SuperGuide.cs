using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class SuperGuide : ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Language { get; set; }

        public int Year { get; set; }

        public SuperGuide()
        {
            Id = -1;
            UserId = -1;
            Language = string.Empty;
            Year = -1;
        }

        public SuperGuide(int id, int userId, string language, int year)
        {
            Id = id;
            UserId = userId;
            Language = language;
            Year = year;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            UserId = int.Parse(values[1]);
            Language = values[2];
            Year = int.Parse(values[3]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                UserId.ToString(),
                Language,
                Year.ToString()
            };

            return csvValues;
        }

    }
}
