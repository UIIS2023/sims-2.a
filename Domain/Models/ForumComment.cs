using SOSTeam.TravelAgency.Application.Services;
using SOSTeam.TravelAgency.Repositories.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SOSTeam.TravelAgency.Domain.Models
{
    public class ForumComment : ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ForumId { get; set; }
        public string Comment { get; set; }
        public bool WasOnLocation { get; set; }
        public Roles UserType { get; set; }
        public string CommentShape { get; set; }

        public ForumComment()
        {
            Id = -1;
            UserId = -1;
            ForumId = -1;
            Comment = "";
            WasOnLocation = false;
            UserType = Roles.GOST1;
        }
        public ForumComment(int id, int userId, int forumId, string comment, bool wasOnLocation, Roles userType)
        {
            Id = id;
            UserId = userId;
            ForumId = forumId;
            Comment = comment;
            WasOnLocation = wasOnLocation;
            UserType = userType;
        }

        public ForumComment(int userId, int forumId, string comment, bool wasOnLocation, Roles userType)
        {
            UserId = userId;
            ForumId = forumId;
            Comment = comment;
            WasOnLocation = wasOnLocation;
            UserType = userType;
            UserService userService = new UserService();
            User user = userService.GetById(userId);
            CommentShape = user.Username + ": " + comment;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), UserId.ToString(), ForumId.ToString(), Comment, WasOnLocation.ToString(), UserType.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            UserId = Convert.ToInt32(values[i++]);
            ForumId = Convert.ToInt32(values[i++]);
            Comment = values[i++];
            if (values[i++].Equals("False"))
                WasOnLocation = false;
            else
                WasOnLocation = true;

            UserType = FindUserType(values[i++]);
            UserService userService = new UserService();
            User user = userService.GetById(UserId);
            CommentShape = user.Username + ": " + Comment;
        }

        private Roles FindUserType(string value)
        {
            if (value.Equals("VLASNIK"))
                return Roles.VLASNIK;
            else if (value.Equals("VODIC"))
                return Roles.VODIC;
            else if (value.Equals("GOST1"))
                return Roles.GOST1;
            else
                return Roles.GOST2;
        }
    }
}
