using DataPersistenceLayer.Entities;
using System.Data.Entity;

namespace DataPersistenceLayer
{
    public class ProfessionalPracticesContext : DbContext
    {
        public ProfessionalPracticesContext() : base("name=ProfessionalPracticesSystem") { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<ActivityMade> ActivitiesMade { get; set; }
        public DbSet<AdvanceQuestion> AdvanceQuestions { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Coordinator> Coordinators { get; set; }
        public DbSet<Document> Documents { get; set; }        
        public DbSet<Group> Groups { get; set; }
        public DbSet<LinkedOrganization> LinkedOrganizations { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<MonthlyReport> MonthlyReports { get; set; }
        public DbSet<OfficeOfAcceptance> OfficeOfAcceptances { get; set; }
        public DbSet<PartialReport> PartialReports { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Practicioner> Practicioners { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<RequestProject> RequestProjects { get; set; }
        public DbSet<ResponsibleProject> ResponsibleProjects { get; set; }
        public DbSet<SchedulingActivity> SchedulingActivities { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityPracticioner> ActivityPracticioners { get; set; }
    }
}
