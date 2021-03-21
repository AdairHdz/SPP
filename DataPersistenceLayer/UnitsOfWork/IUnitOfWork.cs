using DataPersistenceLayer.Entities;
using DataPersistenceLayer.Repositories;
using System;

namespace DataPersistenceLayer.UnitsOfWork
{
    public interface IUnitOfWork : IDisposable
    {
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
        IRepository<State> States { get; }
        IRepository<Teacher> Teachers { get; }
        IUserRepository Users { get; }
        int Complete();
    }
}
