using DataPersistenceLayer.Entities;
using System.Data.Entity;

namespace DataPersistenceLayer.Repositories
{
    public class PracticionerRepository : Repository<Practicioner>, IPracticionerRepository
    {
        public ProfessionalPracticesContext ProfessionalPracticesContext
        {
            get
            {
                return _context as ProfessionalPracticesContext;
            }
        }

        public PracticionerRepository(DbContext context) : base(context) { }

        public bool PracticionerIsAlreadyRegistered(Practicioner practicioner)
        {
            Practicioner practicionerWithSameEnrollment = Get(practicioner.Enrollment);
            bool exist = false;
            if (practicionerWithSameEnrollment != null)
            {
                exist = true;
            }

            return exist;
        }
    }
}
