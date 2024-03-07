using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSTeam.TravelAgency.Domain;
using SOSTeam.TravelAgency.Domain.Models;
using SOSTeam.TravelAgency.Domain.RepositoryInterfaces;

namespace SOSTeam.TravelAgency.Application.Services
{
    public class CheckpointService
    {
        private readonly ICheckpointRepository _checkpointRepository;

        public CheckpointService()
        {
            _checkpointRepository = Injector.CreateInstance<ICheckpointRepository>();
        }

        public void Delete(int id)
        {
            _checkpointRepository.Delete(id);
        }

        public List<Checkpoint> GetAll()
        {
            return _checkpointRepository.GetAll();
        }

        public List<Checkpoint> GetAllByTourId(int id)
        {
            return _checkpointRepository.GetAllByTourId(id);
        }

        public Checkpoint? GetById(int id)
        {
            return _checkpointRepository.GetById(id);
        }

        public void Save(Checkpoint checkpoint)
        {
            _checkpointRepository.Save(checkpoint);
        }

        public void SaveAll(List<Checkpoint> checkpoints)
        {
            _checkpointRepository.SaveAll(checkpoints);
        }

        public void Update(Checkpoint checkpoint)
        {
            _checkpointRepository.Update(checkpoint);
        }

    }
}
