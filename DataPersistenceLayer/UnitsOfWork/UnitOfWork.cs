using DataPersistenceLayer.Entities;
using DataPersistenceLayer.Repositories;

namespace DataPersistenceLayer.UnitsOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IRepository<Activity> Activities { get; private set; }
        public IRepository<City> Cities { get; private set; }
        public IRepository<Coordinator> Coordinators { get; private set; }
        public IRepository<Gender> Genders { get; private set; }
        public IRepository<LinkedOrganization> LinkedOrganizations { get; private set; }
        public IRepository<Period> Periods { get; private set; }
        public IRepository<Position> Positions { get; private set; }
        public IRepository<Practicing> Practicings { get; private set; }
        public IRepository<Project> Projects { get; private set; }
        public IRepository<ProjectStatus> ProjectStatuses { get; private set; }
        public IRepository<Report> Reports { get; private set; }
        public IRepository<ReportPartial> ReportPartials { get; private set; }
        public IRepository<Request> Requests { get; private set; }
        public IRepository<RequestStatus> RequestStatuses { get; private set; }
        public IRepository<ResponsibleProject> ResponsibleProjects { get; private set; }
        public IRepository<Sector> Sectors { get; private set; }
        public IRepository<State> States { get; private set; }
        public IRepository<Teacher> Teachers { get; private set; }
        public IRepository<Turn> Turns { get; private set; }
        public IRepository<User> Users { get; private set; }
        public IRepository<UserStatus> UserStatuses { get; private set; }

        private readonly ProfessionalPracticesContext _context;

        public UnitOfWork(ProfessionalPracticesContext context)
        {
            _context = context;
            Users = new Repository<User>(_context);
        }

        /// <summary>
        /// Completes the transaction on the database.
        /// </summary>
        /// <returns>The number of entries that were written to the database.</returns>
        public int Complete()
        {
            return _context.SaveChanges();
        }

        /// <summary>
        /// Frees up the resources of the database connection.
        /// </summary>
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
