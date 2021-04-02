using DataPersistenceLayer.Entities;

namespace DataPersistenceLayer.Repositories
{
    public interface IPhoneRepository : IRepository<Phone>
    {
        bool PhoneIsAlreadyRegistered(LinkedOrganization linkedOrganization);

    }
}
