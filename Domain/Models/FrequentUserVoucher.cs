using SOSTeam.TravelAgency.Repositories.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class FrequentUserVoucher : ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int VoucherId { get; set; }
        public string Year { get; set; }
        
        public FrequentUserVoucher() 
        {
        
        }
    
        public FrequentUserVoucher(int voucherId,int userId,string year)
        {
            UserId = userId;
            VoucherId = voucherId;
            Year = year;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            VoucherId = int.Parse(values[1]);
            UserId = int.Parse(values[2]);
            Year = values[3];
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                VoucherId.ToString(),
                UserId.ToString(),
                Year,
            };
            return csvValues;
        }
    }
}
