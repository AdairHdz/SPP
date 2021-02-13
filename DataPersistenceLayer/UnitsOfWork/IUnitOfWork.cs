using DataPersistenceLayer.Entities;
using DataPersistenceLayer.Repositories;
using System;

namespace DataPersistenceLayer.UnitsOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Activity> Activities { get; }
        IRepository<City> Cities { get; }
        IRepository<Coordinator> Coordinators { get; }
        IRepository<Gender> Genders { get; }
        IRepository<LinkedOrganization> LinkedOrganizations { get; }
        IRepository<Period> Periods { get; }
        IRepository<Position> Positions { get; }
        IRepository<Practicing> Practicings { get; }
        IRepository<Project> Projects { get; }
        IRepository<ProjectStatus> ProjectStatuses { get; }
        IRepository<Report> Reports { get; }
        IRepository<ReportPartial> ReportPartials { get; }
        IRepository<Request> Requests { get; }
        IRepository<RequestStatus> RequestStatuses { get; }
        IRepository<ResponsibleProject> ResponsibleProjects { get; }
        IRepository<Sector> Sectors { get; }
        IRepository<State> States { get; }
        IRepository<Teacher> Teachers { get; }
        IRepository<Turn> Turns { get; }
        IRepository<User> Users { get; }
        IRepository<UserStatus> UserStatuses { get; }
        int Complete();
    }
}
