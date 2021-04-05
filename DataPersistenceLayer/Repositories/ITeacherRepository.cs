using DataPersistenceLayer.Entities;
using System.Collections.Generic;

namespace DataPersistenceLayer.Repositories
{
    public interface ITeacherRepository : IRepository<Teacher>
    {
        bool ActiveTeacher();
        IList<Teacher> GetActiveTeachers();
        
    }
}
