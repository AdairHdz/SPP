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
            Project proyecIsAssigned = _context.Set<Project>().SingleOrDefault(Project => Project.ResponsibleProject.IdResponsibleProject == idResponsibleProject && Project.Status == ProjectStatus.FILLED || Project.Status == ProjectStatus.ACTIVE);
            if(!object.ReferenceEquals(null, proyecIsAssigned))
            {
                return true;
            }
            return false;
        }

        public void SoftDeleteResponsibleProject(ResponsibleProject responsibleProject)
        {
            ResponsibleProject responsibleProjectCurrent = _context.Set<ResponsibleProject>().SingleOrDefault(ResponsibleProject => ResponsibleProject.IdResponsibleProject == responsibleProject.IdResponsibleProject);
            if (!object.ReferenceEquals(null, responsibleProjectCurrent)) {
                responsibleProjectCurrent.ResponsibleProjectStatus = ResponsibleProjectStatus.INACTIVE;
            }
        }

        public void UpdateResponsibleProject(ResponsibleProject responsibleProjectEdit)
        {
            ResponsibleProject responsibleProjectCurrent = _context.Set<ResponsibleProject>().SingleOrDefault(responsible => responsible.IdResponsibleProject == responsibleProjectEdit.IdResponsibleProject);
            if (!object.ReferenceEquals(null, responsibleProjectCurrent))
            {
                responsibleProjectCurrent.Name = responsibleProjectEdit.Name;
                responsibleProjectCurrent.LastName = responsibleProjectEdit.LastName;
                responsibleProjectCurrent.EmailAddress = responsibleProjectEdit.EmailAddress;
                responsibleProjectCurrent.Charge = responsibleProjectEdit.Charge;
            }
        }

        public void ActiveResponsibleProject(ResponsibleProject responsibleProject)
        {
            ResponsibleProject responsibleProjectCurrent = _context.Set<ResponsibleProject>().SingleOrDefault(ResponsibleProject => ResponsibleProject.IdResponsibleProject == responsibleProject.IdResponsibleProject);
            if (!object.ReferenceEquals(null, responsibleProjectCurrent))
            {
                responsibleProjectCurrent.ResponsibleProjectStatus = ResponsibleProjectStatus.ACTIVE;
            }
        }

    }
}
