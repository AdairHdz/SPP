using DataPersistenceLayer.Entities;
using System.Data.Entity;

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
        public bool ResponsibleProjectIsAlreadyRegistered(string emailAddress)
        {
            ResponsibleProject responsibleProjectWithSameEmailAddress = FindFirstOccurence(ResponsibleProject => ResponsibleProject.EmailAddress == emailAddress);

            if (responsibleProjectWithSameEmailAddress != null)
            {
                return true;
            }
            return false;
        }
    }
}
