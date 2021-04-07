using DataPersistenceLayer.Entities;
using System;
using System.Collections.Generic;

namespace DataPersistenceLayer.Repositories
{
    public interface IPracticionerRepository : IRepository<Practicioner>
    {
        bool PracticionerIsAlreadyRegistered(Practicioner practicioner);
        bool PracticionerHasActiveProject(string enrollment);
        void SetPracticionerStatusAsInactive(string enrollment);
        bool PracticionerIsAlreadyRegistered(Practicioner practicioner, bool isUpdate);
        Practicioner GetAllInformationPracticioner(string enrollment);
        bool RequiredPracticionersToGroup();
        IList<Practicioner> PracticionersToGroup();
        void AddGroup(IList<Practicioner> practicionersSelected, int idGroup);
        IList<Practicioner> GetAllPracticionersWithUserData();
        IList<Practicioner> GetPracticionersWithUserData(Func<Practicioner, bool> predicate);
        IList<Practicioner> GetPracticionersInThisGroup(int idGroup);
        IList<Practicioner> GetPracticionerActiveNotInThisGroup(int idGroup);
        void RemoveGroup(IList<Practicioner> practicionersToRemove);
    }
}
