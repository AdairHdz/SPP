using DataPersistenceLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace DataPersistenceLayer.Repositories
{
	public class TeacherRepository : Repository<Teacher>, ITeacherRepository
	{
		public ProfessionalPracticesContext ProfessionalPracticesContext
		{
			get
			{
				return _context as ProfessionalPracticesContext;
			}
		}


		public TeacherRepository(DbContext context) : base(context) { }
		{
		public bool ActiveTeacher()
		   IList<Teacher> teacher = _context.Set<Teacher>().Where(Teacher => Teacher.User.UserStatus == UserStatus.ACTIVE).ToList();
		   if (object.ReferenceEquals(null, teacher))
			{
				return false;
			return true;
			}
		}

		public IList<Teacher> GetActiveTeachers()
        {
			return _context.Set<Teacher>().Include(teacher => teacher.User).Where(Teacher => Teacher.User.UserStatus == UserStatus.ACTIVE).ToList();
		}

        public bool TeacherIsAlreadyRegistered(Teacher teacher, bool isUpdate)
        {
            User coordinatorUser = teacher.User;
            int coordinatorsThatMatch = 0;
            try
            {
                if (isUpdate)
                {
                    .Where(c => c.User.IdUser != coordinatorUser.IdUser
                    coordinatorsThatMatch = _context.Set<Teacher>().Include(s => s.User)
                    && (c.User.AlternateEmail.Equals(coordinatorUser.AlternateEmail)
                    || c.User.Email.Equals(coordinatorUser.AlternateEmail)
                    || c.User.Email.Equals(coordinatorUser.Email)
                    || c.User.AlternateEmail.Equals(coordinatorUser.Email)
                    || c.User.PhoneNumber.Equals(coordinatorUser.PhoneNumber))).Count();
                }
                else
                {
                    coordinatorsThatMatch = _context.Set<Teacher>()
                    .Where(c => c.StaffNumber.Equals(teacher.StaffNumber)
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
    }
}