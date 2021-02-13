using DataPersistenceLayer.Entities;
using System.Data.Entity;

namespace DataPersistenceLayer
{
    public class ProfessionalPracticesContext : DbContext
    {
        public ProfessionalPracticesContext() : base("name=ProfessionalPracticesSystem") { }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Coordinator> Coordinators { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<LinkedOrganization> LinkedOrganizations { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Practicing> Practicings { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectStatus> ProjectStatuses { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportPartial> ReportPartials { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestStatus> RequestStatuses { get; set; }
        public DbSet<ResponsibleProject> ResponsibleProjects { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Turn> Turns { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserStatus> UserStatuses { get; set; }



    }
}
