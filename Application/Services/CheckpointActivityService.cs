using System.Collections.Generic;
using SOSTeam.TravelAgency.Domain;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class CheckpointActivityService
    {
        private readonly ICheckpointActivityRepository _checkpointActivityRepository;

        public CheckpointActivityService()
        {
            _checkpointActivityRepository = Injector.CreateInstance<ICheckpointActivityRepository>();
        }

        public void Delete(int id)
        {
            _checkpointActivityRepository.Delete(id);
        }

        public List<CheckpointActivity> GetAll()
        {
            return _checkpointActivityRepository.GetAll();
        }

        public List<CheckpointActivity> GetAllByAppointmentId(int id)
        {
            return _checkpointActivityRepository.GetAllByAppointmentId(id);
        }

        public CheckpointActivity? GetById(int id)
        {
            return _checkpointActivityRepository.GetById(id);
        }

        public void Save(CheckpointActivity activity)
        {
            _checkpointActivityRepository.Save(activity);
        }

        public void SaveAll(List<CheckpointActivity> activities)
        {
            _checkpointActivityRepository.SaveAll(activities);
        }

        public void Update(CheckpointActivity activity)
        {
            _checkpointActivityRepository.Update(activity);
        }

        public CheckpointActivity? FindActiveCheckpoint(int appointmentId)
        {
            return _checkpointActivityRepository.GetAllByAppointmentId(appointmentId).Find(a => a.Status == CheckpointStatus.ACTIVE);
        }

        public void CreateActivities(List<Checkpoint> tourCheckpoints, int startedAppointmentId)
        {
            var checkpointActivities = new List<CheckpointActivity>();
            foreach (var checkpoint in tourCheckpoints)
            {
                var checkpointActivity = new CheckpointActivity
                {
                    AppointmentId = startedAppointmentId,
                    CheckpointId = checkpoint.Id,
                    Status = checkpoint.Type == CheckpointType.START ? CheckpointStatus.ACTIVE : CheckpointStatus.NOT_STARTED
                };
                checkpointActivities.Add(checkpointActivity);
            }
            SaveAll(checkpointActivities);
        }

        public void ActivateCheckpoint(int activityId)
        {
            var checkpointActivity = _checkpointActivityRepository.GetById(activityId);
            if (checkpointActivity == null) { return; }
            checkpointActivity.Status = CheckpointStatus.ACTIVE;
            Update(checkpointActivity);
        }

        public void FinishCheckpoint(int activityId)
        {
            var checkpointActivity = _checkpointActivityRepository.GetById(activityId);
            if (checkpointActivity == null) { return; }
            checkpointActivity.Status = CheckpointStatus.FINISHED;
            Update(checkpointActivity);
        }
    }
}
