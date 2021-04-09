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
        IProjectRepository Projects { get; }
        IRepository<PartialReport> PartialReports { get; }
        IRepository<MonthlyReport> MonthlyReports { get; }
        IRequestProjectRepository RequestProjects { get; }
        IResponsibleProjectRepository ResponsibleProjects { get; }
        IRepository<Sector> Sectors { get; }
        IStateRepository States { get; }
        ITeacherRepository Teachers { get; }
        IUserRepository Users { get; }
        IPhoneRepository Phones { get; }
        IRepository<SchedulingActivity> SchedulingActivities { get; }
        IGroupRepository Groups { get; }
        IRepository<Activity> Activities { get; }
        IRepository<ActivityPracticioner> ActivityPracticioners { get; }
        IRepository<Document> Documents { get;}
        int Complete();
    }
}
