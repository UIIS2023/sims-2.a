using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Repositories.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Repositories
{
    public class TourReviewRepository : ITourReviewRepository
    {

        private const string FilePath = "../../../Resources/Data/tourReviews.csv";

        private readonly Serializer<TourReview> _serializer;

        private List<TourReview> _reviews;

        public TourReviewRepository()
        {
            _serializer = new Serializer<TourReview>();
            _reviews = _serializer.FromCSV(FilePath);
        }

        public void Delete(int id)
        {
            _reviews = _serializer.FromCSV(FilePath);
            TourReview founded = _reviews.Find(l => l.Id == id) ?? throw new ArgumentException();     //da li da dodam upitnik da ukloni warning
            _reviews.Remove(founded);
            _serializer.ToCSV(FilePath, _reviews);
        }

        public List<TourReview> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public List<TourReview> GetAllByAppointmentId(int id)
        {
            _reviews = _serializer.FromCSV(FilePath);
            return _reviews.FindAll(r => r.AppointmentId == id);
        }

        public TourReview GetById(int id)
        {
            _reviews = _serializer.FromCSV(FilePath);
            return _reviews.Find(l => l.Id == id) ?? throw new ArgumentException();
        }

        public int NextId()
        {
            _reviews = _serializer.FromCSV(FilePath);
            if (_reviews.Count < 1)
            {
                return 1;
            }
            return _reviews.Max(l => l.Id) + 1;
        }

        public void Save(TourReview review)
        {
            review.Id = NextId();
            _reviews = _serializer.FromCSV(FilePath);
            _reviews.Add(review);
            _serializer.ToCSV(FilePath, _reviews);
        }

        public void Update(TourReview review)
        {
            _reviews = _serializer.FromCSV(FilePath);
            TourReview current = _reviews.Find(l => l.Id == review.Id) ?? throw new ArgumentException();
            int index = _reviews.IndexOf(current);
            _reviews.Remove(current);
            _reviews.Insert(index, review);
            _serializer.ToCSV(FilePath, _reviews);
        }
    }
}
