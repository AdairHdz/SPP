using DataPersistenceLayer.Entities;

namespace DataPersistenceLayer.Repositories
{
    public interface IResponsibleProjectRepository: IRepository<ResponsibleProject>
    {
        bool ResponsibleProjectIsAlreadyRegistered(string emailAddress);
    }
}
