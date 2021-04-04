using DataPersistenceLayer.Entities;

namespace DataPersistenceLayer.Repositories
{
    public interface ITeacherRepository : IRepository<Teacher>
    {
        bool TeacherIsAlreadyRegistered(Teacher teacher, bool isUpdate);
    }
}
