using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSTeam.TravelAgency.Domain.RepositoryInterfaces
{
    public interface IRepository<T>
    {
        List<T> GetAll();
        T? GetById(int id);
        void Save(T entity);
        void Update(T entity);
        void Delete(int id);
        int NextId();

    }
}
