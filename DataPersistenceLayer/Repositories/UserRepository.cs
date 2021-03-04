using DataPersistenceLayer.Entities;
using System.Data.Entity;

namespace DataPersistenceLayer.Repositories
{
    class UserRepository : Repository<User>, IUserRepository
    {
        public ProfessionalPracticesContext ProfessionalPracticesContext
        {
            get
            {
                return _context as ProfessionalPracticesContext;
            }
        }

        public UserRepository(DbContext context) : base(context) { }

        public bool UserIsAlreadyRegistered(User userToBeRegistered)
        {
            User retrievedUser;

            if (userToBeRegistered.AlternateEmail != null && userToBeRegistered.AlternateEmail.Length > 0)
            {
                retrievedUser = FindFirstOccurence(user => user.Email.Equals(userToBeRegistered.Email)
                || user.AlternateEmail.Equals(userToBeRegistered.AlternateEmail)
                || user.PhoneNumber.Equals(userToBeRegistered.PhoneNumber));
            }
            else
            {
                retrievedUser = FindFirstOccurence(user => user.Email.Equals(userToBeRegistered.Email)
                || user.PhoneNumber.Equals(userToBeRegistered.PhoneNumber));
            }

            return retrievedUser != null;
        }
    }
}
