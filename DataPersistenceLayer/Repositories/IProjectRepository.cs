using DataPersistenceLayer.Entities;
using System.Collections.Generic;

namespace DataPersistenceLayer.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        IList<Project> GetProjectsAvailableToRequest(string enrollment);
    }
}
