using SOSTeam.TravelAgency.Repositories.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class TourReview : ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AppointmentId { get; set; }
        public int GuideKnowledge { get; set; }
        public int GuideLanguage { get; set; }
        public int InterestRating { get; set; }
        public string Comment { get; set; }
        public bool Reported { get; set; }

        public TourReview()
        {
            Id = -1;
            UserId = -1;
            AppointmentId = -1;
            GuideKnowledge = -1;
            GuideLanguage = -1;
            InterestRating = -1;
            Comment = string.Empty;
            Reported = false;
        }

        public TourReview(int userId,int appointmentId, int guideKnowledge, int guideLanguage, int interestRating, string comment, bool reported)
        {
            UserId = userId;
            AppointmentId = appointmentId;
            GuideKnowledge = guideKnowledge;
            GuideLanguage = guideLanguage;
            InterestRating = interestRating;
            Comment = comment;
            Reported = reported;
        }

        public string[] ToCSV()
        {
            string[] csvValues = {
                Id.ToString(),
                UserId.ToString(),
                AppointmentId.ToString(),
                GuideKnowledge.ToString(),
                GuideLanguage.ToString(),
                InterestRating.ToString(),
                Comment,
                Reported.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            UserId = Convert.ToInt32(values[i++]);
            AppointmentId = Convert.ToInt32(values[i++]);
            GuideKnowledge = Convert.ToInt32(values[i++]);
            GuideLanguage = Convert.ToInt32(values[i++]);
            InterestRating = Convert.ToInt32(values[i++]);
            Comment = values[i++];
            Reported = Boolean.Parse(values[i++]);
        }
    }
}
