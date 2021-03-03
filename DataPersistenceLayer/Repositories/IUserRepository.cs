using DataPersistenceLayer.Entities;

namespace DataPersistenceLayer.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        bool UserIsAlreadyRegistered(User user);
    }
}
