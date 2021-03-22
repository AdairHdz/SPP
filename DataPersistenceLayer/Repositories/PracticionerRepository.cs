using DataPersistenceLayer.Entities;
using System.Data.Entity;
using System.Linq;

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
            if (practicionerWithSameEnrollment != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool PracticionerHasActiveProject(string enrollment)
        {
            Assignment assignment = _context.Set<Assignment>().SingleOrDefault(Assignment => Assignment.Enrollment == enrollment
            && (Assignment.Project.Status == ProjectStatus.FILLED || Assignment.Project.Status == ProjectStatus.ACTIVE));
            if (!object.ReferenceEquals(null, assignment))
            {
                return true;
            }
            return false;
        }

    }
}
