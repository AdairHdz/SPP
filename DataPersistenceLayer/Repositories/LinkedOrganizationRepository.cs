using DataPersistenceLayer.Entities;
using System.Data.Entity;

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
    }
}