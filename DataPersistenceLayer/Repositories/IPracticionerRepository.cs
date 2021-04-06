using DataPersistenceLayer.Entities;
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
       
    }
}
