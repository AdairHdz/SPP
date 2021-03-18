using DataPersistenceLayer.Entities;

namespace DataPersistenceLayer.Repositories
{
    public interface IResponsibleProjectRepository : IRepository<ResponsibleProject>
    {
        bool ResponsibleProjectIsAssigned(int idResponsibleProject);

        void SoftDeleteResponsibleProject(ResponsibleProject responsibleProject);

        void UpdateResponsibleProject(ResponsibleProject responsibleProjectEdit);

        void ActiveResponsibleProject(ResponsibleProject responsibleProject);
    }
}
