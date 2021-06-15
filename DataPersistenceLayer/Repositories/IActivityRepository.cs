using DataPersistenceLayer.Entities;
using System.Collections.Generic;

namespace DataPersistenceLayer.Repositories
{
    public interface IActivityRepository : IRepository<Activity>
    {
        void ChangeToFinished();
        IList<Practicioner> GetPracticionersToActivity(int idGroup);
        int GetId();
        int GetIdActivityMonthlyReportPracticioner(string enrollment);
        bool SearchDocument(string enrollment);

    }
}
