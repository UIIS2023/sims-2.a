using SOSTeam.TravelAgency.Repositories.Serializer;
using SOSTeam.TravelAgency.WPF.ViewModels.TourGuide;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class ComplexTourRequest : ISerializable
    {
        public int Id { get; set; }
        public StatusType Status { get; set; }
        public int UserId { get; set; }

        public ComplexTourRequest() {
            Status = StatusType.ON_HOLD;
            UserId = -1;
        }

        public ComplexTourRequest(int userId, StatusType status = StatusType.ON_HOLD)
        {
            Status= status;
            UserId= userId;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            UserId= int.Parse(values[1]);
            Status = values[2] switch
            {
                "ON_HOLD" => StatusType.ON_HOLD,
                "INVALID" => StatusType.INVALID,
                "ACCEPTED" => StatusType.ACCEPTED,
            };

        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                UserId.ToString(),
                Status.ToString(),
            };
            return csvValues;
        }
    }
}
