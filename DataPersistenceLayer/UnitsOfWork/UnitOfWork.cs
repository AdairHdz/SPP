using DataPersistenceLayer.Entities;
using DataPersistenceLayer.Repositories;

namespace DataPersistenceLayer.UnitsOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IRepository<Account> Accounts { get; private set; }
        public IRepository<City> Cities { get; private set; }
        public ICoordinatorRepository Coordinators { get; private set; }
        public IRepository<LinkedOrganization> LinkedOrganizations { get; private set; }
        public IRepository<Practicioner> Practicioners { get; private set; }
        public IRepository<Project> Projects { get; private set; }
        public IRepository<PartialReport> PartialReports { get; private set; }
        public IRepository<MonthlyReport> MonthlyReports { get; private set; }
        public IRepository<RequestProject> RequestProjects { get; private set; }
        public IResponsibleProjectRepository ResponsibleProjects { get; private set; }
        public IRepository<Sector> Sectors { get; private set; }
        public IRepository<State> States { get; private set; }
        public IRepository<Teacher> Teachers { get; private set; }
        public IUserRepository Users { get; private set; }
        private readonly ProfessionalPracticesContext _context;

        public UnitOfWork(ProfessionalPracticesContext context)
        {
            _context = context;
            Accounts = new Repository<Account>(_context);
            Coordinators = new CoordinatorRepository(_context);
            LinkedOrganizations = new Repository<LinkedOrganization>(_context);
            Practicioners = new Repository<Practicioner>(_context);
            Projects = new Repository<Project>(_context);
            PartialReports = new Repository<PartialReport>(_context);
            MonthlyReports = new Repository<MonthlyReport>(_context);
            RequestProjects = new Repository<RequestProject>(_context);
            ResponsibleProjects = new ResponsibleProjectRepository(_context);
            Teachers = new Repository<Teacher>(_context);
            Users = new UserRepository(_context);
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
