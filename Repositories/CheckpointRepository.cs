using System;
using System.Collections.Generic;
using System.Linq;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;
using SOSTeam.TravelAgency.Repositories.Serializer;

namespace SOSTeam.TravelAgency.Repositories
{
    public class CheckpointRepository : ICheckpointRepository
    {
        private const string FilePath = "../../../Resources/Data/checkpoints.csv";

        private readonly Serializer<Checkpoint> _serializer;

        private List<Checkpoint> _checkpoints;

        public CheckpointRepository()
        {
            _serializer = new Serializer<Checkpoint>();
            _checkpoints = _serializer.FromCSV(FilePath);
        }

        public void Delete(int id)
        {
            _checkpoints = _serializer.FromCSV(FilePath);
            Checkpoint founded = _checkpoints.Find(c => c.Id == id) ?? throw new ArgumentException();
            _checkpoints.Remove(founded);
            _serializer.ToCSV(FilePath, _checkpoints);
        }

        public List<Checkpoint> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public List<Checkpoint> GetAllByTourId(int id)
        {
            _checkpoints = _serializer.FromCSV(FilePath);
            return _checkpoints.FindAll(c => c.TourId == id);
        }

        public Checkpoint? GetById(int id)
        {
            _checkpoints = _serializer.FromCSV(FilePath);
            return _checkpoints.Find(c => c.Id == id);
        }

        public int NextId()
        {
            _checkpoints = _serializer.FromCSV(FilePath);
            if (_checkpoints.Count < 1)
            {
                return 1;
            }
            return _checkpoints.Max(c => c.Id) + 1;
        }

        public void Save(Checkpoint checkpoint)
        {
            checkpoint.Id = NextId();
            _checkpoints = _serializer.FromCSV(FilePath);
            _checkpoints.Add(checkpoint);
            _serializer.ToCSV(FilePath, _checkpoints);
        }

        public void SaveAll(List<Checkpoint> checkpoints)
        {
            foreach (Checkpoint checkpoint in checkpoints)
            {
                Save(checkpoint);
            }
        }

        public void Update(Checkpoint checkpoint)
        {
            _checkpoints = _serializer.FromCSV(FilePath);
            Checkpoint current = _checkpoints.Find(c => c.Id == checkpoint.Id) ?? throw new ArgumentException();
            int index = _checkpoints.IndexOf(current);
            _checkpoints.Remove(current);
            _checkpoints.Insert(index, checkpoint);
            _serializer.ToCSV(FilePath, _checkpoints);
        }
    }
}
