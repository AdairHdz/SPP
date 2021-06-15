using DataPersistenceLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPersistenceLayer.Repositories
{
    public class PhoneRepository : Repository<Phone>, IPhoneRepository
    {

        public ProfessionalPracticesContext ProfessionalPracticesContext
        {
            get
            {
                return _context as ProfessionalPracticesContext;
            }
        }

        public PhoneRepository(DbContext context) : base(context) { }

        public bool PhoneIsAlreadyRegistered(LinkedOrganization linkedOrganization)
        {
            Phone phone1 = linkedOrganization.PhoneNumbers[0];
            Phone phone2 = linkedOrganization.PhoneNumbers[1];

            IQueryable<Phone> retrievedPhones = _context.Set<Phone>().Where(ph => (ph.PhoneNumber.Equals(phone1.PhoneNumber)
                || ph.PhoneNumber.Equals(phone2.PhoneNumber))
                && ph.IdLinkedOrganization != linkedOrganization.IdLinkedOrganization);

            if (retrievedPhones.Count() > 0)
            {
                return true;              
            }
            return false;
        }
    }
}
