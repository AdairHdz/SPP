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




    }
}
