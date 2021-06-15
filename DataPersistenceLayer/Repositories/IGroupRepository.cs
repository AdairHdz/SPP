using DataPersistenceLayer.Entities;
using System.Collections.Generic;

namespace DataPersistenceLayer.Repositories
{
    public interface IGroupRepository : IRepository<Group>
    {
        bool GroupIsAlreadyRegistered(Group group);
        int GroupId();
        Teacher GetTeacherAssigned(string staffNumber);
        bool GroupCanBeModify(Group group);
        bool GetIfThisTeacherHaveActiveGroups(string staffNumber);
        IList<Group> GetActiveGroupsForThisTeacher(string staffNumber);
        IList<Activity> GetActivitiesForThisGroup(int idGroup);

    }
}
