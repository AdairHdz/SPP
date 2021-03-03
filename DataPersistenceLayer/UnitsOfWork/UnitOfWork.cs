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
