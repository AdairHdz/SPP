using DataPersistenceLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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

		public bool ActiveTeacher()
		{
		   IList<Teacher> teacher = _context.Set<Teacher>().Where(Teacher => Teacher.User.UserStatus == UserStatus.ACTIVE).ToList();
		   if (object.ReferenceEquals(null, teacher))
			{
				return false;
			}
			return true;
		}

		public IList<Teacher> GetActiveTeachers()
        {
			return _context.Set<Teacher>().Include(teacher => teacher.User).Where(Teacher => Teacher.User.UserStatus == UserStatus.ACTIVE).ToList();
		}
	}
}
