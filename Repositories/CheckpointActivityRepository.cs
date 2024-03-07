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
    public class CheckpointActivityRepository : ICheckpointActivityRepository
    {
        private const string FilePath = "../../../Resources/Data/checkpointActivities.csv";

        private readonly Serializer<CheckpointActivity> _serializer;

        private List<CheckpointActivity> _checkpointActivities;

        public CheckpointActivityRepository()
        {
            _serializer = new Serializer<CheckpointActivity>();
            _checkpointActivities = _serializer.FromCSV(FilePath);
        }

        public void Delete(int id)
        {
            _checkpointActivities = _serializer.FromCSV(FilePath);
            CheckpointActivity founded = _checkpointActivities.Find(c => c.Id == id) ?? throw new ArgumentException();
            _checkpointActivities.Remove(founded);
            _serializer.ToCSV(FilePath, _checkpointActivities);
        }

        public List<CheckpointActivity> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public List<CheckpointActivity> GetAllByAppointmentId(int id)
        {
            _checkpointActivities = _serializer.FromCSV(FilePath);
            return _checkpointActivities.FindAll(a => a.AppointmentId == id);
        }

        public CheckpointActivity GetById(int id)
        {
            _checkpointActivities = _serializer.FromCSV(FilePath);
            return _checkpointActivities.Find(a => a.Id == id) ?? throw new ArgumentException();
        }

        public int NextId()
        {
            _checkpointActivities = _serializer.FromCSV(FilePath);
            if (_checkpointActivities.Count < 1)
            {
                return 1;
            }
            return _checkpointActivities.Max(d => d.Id) + 1;
        }

        public void Save(CheckpointActivity activity)
        {
            activity.Id = NextId();
            _checkpointActivities = _serializer.FromCSV(FilePath);
            _checkpointActivities.Add(activity);
            _serializer.ToCSV(FilePath, _checkpointActivities);
        }

        public void SaveAll(List<CheckpointActivity> activities)
        {
            foreach (CheckpointActivity activity in activities)
            {
                Save(activity);
            }
        }

        public void Update(CheckpointActivity activity)
        {
            _checkpointActivities = _serializer.FromCSV(FilePath);
            CheckpointActivity current = _checkpointActivities.Find(c => c.Id == activity.Id) ?? throw new ArgumentException();
            int index = _checkpointActivities.IndexOf(current);
            _checkpointActivities.Remove(current);
            _checkpointActivities.Insert(index, activity);
            _serializer.ToCSV(FilePath, _checkpointActivities);
        }
    }
}
