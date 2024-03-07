using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Repositories
{
    public class SuperGuideRepository : ISuperGuideRepository
    {
        private const string FilePath = "../../../Resources/Data/superGuide.csv";

        private readonly Serializer<SuperGuide> _serializer;

        private List<SuperGuide> _superGuides;

        public SuperGuideRepository()
        {
            _serializer = new Serializer<SuperGuide>();
            _superGuides = _serializer.FromCSV(FilePath);
        }

        public void Delete(int id)
        {
            _superGuides = _serializer.FromCSV(FilePath);
            SuperGuide founded = _superGuides.Find(sg => sg.Id == id) ?? throw new ArgumentException();
            _superGuides.Remove(founded);
            _serializer.ToCSV(FilePath, _superGuides);
        }

        public List<SuperGuide> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public SuperGuide? GetById(int id)
        {
            _superGuides = _serializer.FromCSV(FilePath);
            return _superGuides.Find(sg => sg.Id == id);
        }

        public int NextId()
        {
            _superGuides = _serializer.FromCSV(FilePath);
            if (_superGuides.Count < 1)
            {
                return 1;
            }
            return _superGuides.Max(sg => sg.Id) + 1;
        }

        public void Save(SuperGuide entity)
        {
            entity.Id = NextId();
            _superGuides = _serializer.FromCSV(FilePath);
            _superGuides.Add(entity);
            _serializer.ToCSV(FilePath, _superGuides);
        }

        public void Update(SuperGuide entity)
        {
            _superGuides = _serializer.FromCSV(FilePath);
            SuperGuide current = _superGuides.Find(sg => sg.Id == entity.Id) ?? throw new ArgumentException();
            int index = _superGuides.IndexOf(current);
            _superGuides.Remove(entity);
            _superGuides.Insert(index, entity);
            _serializer.ToCSV(FilePath, _superGuides);
        }
    }
}
