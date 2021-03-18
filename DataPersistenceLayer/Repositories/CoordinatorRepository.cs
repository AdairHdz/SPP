using DataPersistenceLayer.Entities;
using System;
using System.Data.Entity;
using System.Linq;

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

        public Coordinator GetActiveCoordinator()
        {

            IQueryable<Coordinator> registeredCoordinators = _context.Set<Coordinator>().Include(coordinator => coordinator.User);
            try
            {
                Coordinator activeCoordinator = registeredCoordinators.Where(coordinator => coordinator.User.UserStatus == UserStatus.ACTIVE).First();
                return activeCoordinator;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            catch (ArgumentNullException)
            {
                return null;
            }
        }

        public void SetCoordinatorStatusAsInactive(string staffNumber)
        {
            Coordinator retrievedCoordinator = _context.Set<Coordinator>().Where(coordinator => coordinator.StaffNumber.Equals(staffNumber)).Include(c => c.User).First();
            retrievedCoordinator.User.UserStatus = UserStatus.INACTIVE;
        }
    }
}
