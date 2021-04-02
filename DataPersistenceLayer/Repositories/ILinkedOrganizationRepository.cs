using DataPersistenceLayer.Entities;

namespace DataPersistenceLayer.Repositories
{
    public interface ILinkedOrganizationRepository : IRepository<LinkedOrganization>
    {
        LinkedOrganization GetLinkedOrganization(int id);
        bool ThereIsAnotherLinkedOrganizationWithSameData(LinkedOrganization linkedOrganization);
    }
}
