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

        public bool LinkedOrganizationIsAlreadyRegistered(LinkedOrganization linkedOrganization)
        {
            LinkedOrganization retrievedLinkedOrganization = FindFirstOccurence
                (x => x.Name.Equals(linkedOrganization.Name) || x.Email.Equals(linkedOrganization.Email)); ;

            if (retrievedLinkedOrganization != null)
            {
                return true;
            }

            return false;
        }
    }
}