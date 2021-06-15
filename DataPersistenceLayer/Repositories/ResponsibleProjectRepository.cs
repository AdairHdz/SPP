using DataPersistenceLayer.Entities;
using System.Data.Entity;
using System.Linq;

namespace DataPersistenceLayer.Repositories
{
    public class ResponsibleProjectRepository : Repository<ResponsibleProject>, IResponsibleProjectRepository
    {
        public ProfessionalPracticesContext ProfessionalPracticesContext
        {
            get
            {
                return _context as ProfessionalPracticesContext;
            }
        }

        public ResponsibleProjectRepository(DbContext context) : base(context) { }
        public bool ResponsibleProjectIsAssigned(int idResponsibleProject)
        {
            Project proyecIsAssigned = _context.Set<Project>().FirstOrDefault(Project => Project.ResponsibleProject.IdResponsibleProject == idResponsibleProject && (Project.Status == ProjectStatus.FILLED || Project.Status == ProjectStatus.ACTIVE));
            if(!object.ReferenceEquals(null, proyecIsAssigned))
            {
                return true;
            }
            return false;
        }
    }
}
