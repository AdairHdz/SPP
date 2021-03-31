using DataPersistenceLayer.Entities;
using System.Collections.Generic;

namespace DataPersistenceLayer.Repositories
{
    public interface IStateRepository : IRepository<State>
    {
        List<State> GetStatesWithCities();
    }
}
