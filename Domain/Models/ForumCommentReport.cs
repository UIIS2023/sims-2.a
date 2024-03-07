using SOSTeam.TravelAgency.Repositories.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class ForumCommentReport : ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CommentId { get; set; }

        public ForumCommentReport(int id, int userId, int commentId)
        {
            Id = id;
            UserId = userId;
            CommentId = commentId;
        }

        public ForumCommentReport()
        {
            Id = -1;
            UserId = -1;
            CommentId = -1;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), UserId.ToString(), CommentId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            UserId = Convert.ToInt32(values[i++]);
            CommentId = Convert.ToInt32(values[i++]);
        }
    }
}
