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

        public bool CoordinatorIsAlreadyRegistered(Coordinator coordinator, bool isUpdate)
        {
            User coordinatorUser = coordinator.User;
            int coordinatorsThatMatch = 0;
            try
            {                
                if (isUpdate)
                {
                coordinatorsThatMatch = _context.Set<Coordinator>().Include(s => s.User)
                .Where(c => c.User.IdUser != coordinatorUser.IdUser
                && (c.User.AlternateEmail.Equals(coordinatorUser.AlternateEmail)
                || c.User.Email.Equals(coordinatorUser.Email)
                || c.User.Email.Equals(coordinatorUser.AlternateEmail)
                || c.User.AlternateEmail.Equals(coordinatorUser.Email)
                || c.User.PhoneNumber.Equals(coordinatorUser.PhoneNumber))).Count();
                }
                else
                {
                    coordinatorsThatMatch = _context.Set<Coordinator>()
                    .Where(c => c.StaffNumber.Equals(coordinator.StaffNumber)
                    || c.User.AlternateEmail.Equals(coordinatorUser.AlternateEmail)
                    || c.User.Email.Equals(coordinatorUser.Email)
                    || c.User.Email.Equals(coordinatorUser.AlternateEmail)
                    || c.User.AlternateEmail.Equals(coordinatorUser.Email)
                    || c.User.PhoneNumber.Equals(coordinatorUser.PhoneNumber)).Include(c => c.User).Count();                   
                }

                if (coordinatorsThatMatch != 0)
                {
                    return true;
                }
                return false;
            }
            catch (ArgumentNullException)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }                                  
        }

        public Coordinator GetCoordinatorWithUserAndAccountData(string staffNumber)
        {
            Coordinator retrievedCoordinator = _context.Set<Coordinator>().Include(s => s.User.Account).Where(coordinator => coordinator.StaffNumber == staffNumber).First();
            return retrievedCoordinator;
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
