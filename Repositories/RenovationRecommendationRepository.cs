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
    public class RenovationRecommendationRepository : IRenovationRecommendationRepository
    {
        private const string FilePath = "../../../Resources/Data/renovationRecommendation.csv";
        private readonly Serializer<RenovationRecommendation> _serializer;
        private List<RenovationRecommendation> _recommendations;

        public RenovationRecommendationRepository()
        {
            _serializer = new Serializer<RenovationRecommendation>();
            _recommendations = new List<RenovationRecommendation>();
        }

        public void Update(RenovationRecommendation renovationRecommendation)
        {
            _recommendations = _serializer.FromCSV(FilePath);
            RenovationRecommendation current = _recommendations.Find(d => d.Id == renovationRecommendation.Id) ?? throw new ArgumentException();
            int index = _recommendations.IndexOf(current);
            _recommendations.Remove(current);
            _recommendations.Insert(index, renovationRecommendation);
            _serializer.ToCSV(FilePath, _recommendations);
        }

        public RenovationRecommendation GetById(int id)
        {
            _recommendations = _serializer.FromCSV(FilePath);
            return _recommendations.Find(a => a.Id == id) ?? throw new ArgumentException();
        }

        public List<RenovationRecommendation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(RenovationRecommendation renovationRecommendation)
        {
            renovationRecommendation.Id = NextId();
            _recommendations = _serializer.FromCSV(FilePath);
            _recommendations.Add(renovationRecommendation);
            _serializer.ToCSV(FilePath, _recommendations);
        }

        public void Delete(int id)
        {
            _recommendations = _serializer.FromCSV(FilePath);
            RenovationRecommendation found = _recommendations.Find(t => t.Id == id) ?? throw new ArgumentException();
            _recommendations.Remove(found);
            _serializer.ToCSV(FilePath, _recommendations);
        }

        public int NextId()
        {
            _recommendations = _serializer.FromCSV(FilePath);
            if (_recommendations.Count < 1)
            {
                return 1;
            }
            return _recommendations.Max(l => l.Id) + 1;
        }
    }

}
