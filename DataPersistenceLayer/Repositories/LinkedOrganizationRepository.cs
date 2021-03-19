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

        public bool LinkedOrganizationIsAlreadyRegistered(LinkedOrganization linkedOrganizationToBeRegistered)
        {
            LinkedOrganization retrievedLinkedOrganization = null;

            if (linkedOrganizationToBeRegistered.Name != null && linkedOrganizationToBeRegistered.Email != null)
            {
                retrievedLinkedOrganization = FindFirstOccurence(linkedOrganization => linkedOrganization.Name.Equals(linkedOrganizationToBeRegistered.Name)
                || linkedOrganization.Email.Equals(linkedOrganizationToBeRegistered.Email));
            }

            return retrievedLinkedOrganization != null;
        }
    }
}