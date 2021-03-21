using DataPersistenceLayer.Entities;

namespace DataPersistenceLayer.Repositories
{
    public interface IResponsibleProjectRepository : IRepository<ResponsibleProject>
    {
        bool ResponsibleProjectIsAssigned(int idResponsibleProject);

    }
}
