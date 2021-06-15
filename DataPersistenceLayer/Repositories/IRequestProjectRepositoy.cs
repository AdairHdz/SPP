using DataPersistenceLayer.Entities;
using System;
using System.Collections.Generic;

namespace DataPersistenceLayer.Repositories
{
    public interface IRequestProjectRepository : IRepository<RequestProject>
    {
        int GetPracticionerRequest(string enrollment);
        List<RequestProject> GetListPracticionerRequest(string enrollment);

    }
}
