using DataPersistenceLayer.Entities;
using DataPersistenceLayer.Repositories;
using System;

namespace DataPersistenceLayer.UnitsOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Assignment> Assignments { get; }
        IRepository<City> Cities { get; }
        ICoordinatorRepository Coordinators { get; }
        ILinkedOrganizationRepository LinkedOrganizations { get; }
        IPracticionerRepository Practicioners { get; }
        IRepository<Project> Projects { get; }
        IRepository<PartialReport> PartialReports { get; }
        IRepository<MonthlyReport> MonthlyReports { get; }
        IRepository<RequestProject> RequestProjects { get; }
        IResponsibleProjectRepository ResponsibleProjects { get; }
        IRepository<Sector> Sectors { get; }
        IStateRepository States { get; }
        ITeacherRepository Teachers { get; }
        IUserRepository Users { get; }
        IPhoneRepository Phones { get; }
        int Complete();
    }
}
