﻿using DataPersistenceLayer.Entities;
using DataPersistenceLayer.Repositories;

namespace DataPersistenceLayer.UnitsOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IRepository<Account> Accounts { get; private set; }
        public IRepository<Assignment> Assignments { get; private set; }
        public IRepository<City> Cities { get; private set; }
        public ICoordinatorRepository Coordinators { get; private set; }
        public ILinkedOrganizationRepository LinkedOrganizations { get; private set; }
        public IPracticionerRepository Practicioners { get; private set; }
        public IProjectRepository Projects { get; private set; }
        public IRepository<PartialReport> PartialReports { get; private set; }
        public IMonthlyReportRepository MonthlyReports { get; private set; }
        public IRequestProjectRepository RequestProjects { get; private set; }
        public IResponsibleProjectRepository ResponsibleProjects { get; private set; }
        public IRepository<Sector> Sectors { get; private set; }
        public IStateRepository States { get; private set; }
        public ITeacherRepository Teachers { get; private set; }
        public IPhoneRepository Phones { get; private set; }
        public IUserRepository Users { get; private set; }
        public IRepository<SchedulingActivity> SchedulingActivities { get; private set; }
        public IGroupRepository Groups { get; private set; }
        public IActivityRepository Activities { get; private set; }
        public IRepository<ActivityPracticioner> ActivityPracticioners { get; private set; }
        public IRepository<Document> Documents { get; private set; }
        public IRepository<ActivityMade> ActivityMades { get; private set; }
        public IRepository<OfficeOfAcceptance> OfficesOfAcceptance { get; private set; }
        public IRepository<AdvanceQuestion> AdvanceQuestions { get; private set; }
        private readonly ProfessionalPracticesContext _context;

        public UnitOfWork(ProfessionalPracticesContext context)
        {
            _context = context;
            Accounts = new Repository<Account>(_context);
            Assignments = new Repository<Assignment>(_context);
            Coordinators = new CoordinatorRepository(_context);
            LinkedOrganizations = new LinkedOrganizationRepository(_context);
            Practicioners = new PracticionerRepository(_context);
            Projects = new ProjectRepository(_context);
            PartialReports = new Repository<PartialReport>(_context);
            MonthlyReports = new MonthlyReportRepository(_context);
            RequestProjects = new RequestProjectRepository(_context);
            ResponsibleProjects = new ResponsibleProjectRepository(_context);
            Teachers = new TeacherRepository(_context);
            Users = new UserRepository(_context);
            States = new StateRepository(_context);
            Sectors = new Repository<Sector>(_context);
            Phones = new PhoneRepository(_context);
            SchedulingActivities = new Repository<SchedulingActivity>(_context);
            Groups = new GroupRepository(_context);
            Activities = new ActivityRepository(_context);
            ActivityPracticioners = new Repository<ActivityPracticioner>(_context);
            Documents = new Repository<Document>(_context);
            ActivityMades = new Repository<ActivityMade>(_context);
            AdvanceQuestions = new Repository<AdvanceQuestion>(_context);
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
