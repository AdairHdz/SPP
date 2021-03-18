using DataPersistenceLayer.Entities;

namespace DataPersistenceLayer.Repositories
{
    public interface IPracticionerRepository : IRepository<Practicioner>
    {
        bool PracticionerIsAlreadyRegistered(Practicioner practicioner);
        bool PracticionerHasActiveProject(string enrollment);
    }
}
