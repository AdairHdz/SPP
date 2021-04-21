using DataPersistenceLayer.Entities;
using System.Collections.Generic;

namespace DataPersistenceLayer.Repositories
{
	public interface ITeacherRepository : IRepository<Teacher>
	{
		bool ActiveTeacher();
		IList<Teacher> GetActiveTeachers();
		bool TeacherIsAlreadyRegistered(Teacher teacher, bool isUpdate);
		Teacher GetTeacherWithAllInformation(string staffNumber);
		string GetStaffNumberTeacher(string password, string userName);

	}
}