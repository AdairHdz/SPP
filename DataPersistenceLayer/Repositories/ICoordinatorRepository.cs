using DataPersistenceLayer.Entities;

namespace DataPersistenceLayer.Repositories
{
    public interface ICoordinatorRepository : IRepository<Coordinator>
    {
        bool CoordinatorIsAlreadyRegistered(Coordinator coordinator);
    }
}
