using DataPersistenceLayer.Entities;
using System.Data.Entity;
using System.Linq;

namespace DataPersistenceLayer.Repositories
{
    public class LinkedOrganizationRepository : Repository<LinkedOrganization>, ILinkedOrganizationRepository
    {
        public ProfessionalPracticesContext ProfessionalPracticesContext
        {
            get
            {
                return _context as ProfessionalPracticesContext;
            }
        }

        public LinkedOrganizationRepository(DbContext context) : base(context) { }
        
        public LinkedOrganization GetLinkedOrganization(int id)
        {
            IQueryable<LinkedOrganization> linkedOrganizations = _context.Set<LinkedOrganization>().Include(l => l.State).Include(l => l.City).Include(l => l.Sector);
            LinkedOrganization linkedOrganization = linkedOrganizations.Where(l => l.IdLinkedOrganization == id).First();
            return linkedOrganization;            
        }

        public bool ThereIsAnotherLinkedOrganizationWithSameData(LinkedOrganization linkedOrganization)
        {
            IQueryable<LinkedOrganization> retrievedLinkedOrganizations =
                _context.Set<LinkedOrganization>().Where(linkedOrg =>
                (linkedOrg.Name.Equals(linkedOrganization.Name)
                || linkedOrg.Email.Equals(linkedOrganization.Email))
                && linkedOrg.IdLinkedOrganization != linkedOrganization.IdLinkedOrganization);

            return retrievedLinkedOrganizations.Count() > 0;
        }
    }
}
