using DataPersistenceLayer.Entities;
using System.Data.Entity;

namespace DataPersistenceLayer.Repositories
{
    public class CoordinatorRepository : Repository<Coordinator>, ICoordinatorRepository
    {
        public ProfessionalPracticesContext ProfessionalPracticesContext
        {
            get
            {
                return _context as ProfessionalPracticesContext;
            }
        }

        public CoordinatorRepository(DbContext context) : base(context) { }

        public bool CoordinatorIsAlreadyRegistered(Coordinator coordinator)
        {
            Coordinator coordinatorWithSameStaffNumber = Get(coordinator.StaffNumber);            

            if(coordinatorWithSameStaffNumber != null)
            {
                return true;
            }
            
            return false;
        }
    }
}
