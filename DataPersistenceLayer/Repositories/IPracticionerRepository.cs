using DataPersistenceLayer.Entities;

namespace DataPersistenceLayer.Repositories
{
    public interface IPracticionerRepository : IRepository<Practicioner>
    {
        bool PracticionerIsAlreadyRegistered(Practicioner practicioner);
        bool PracticionerHasActiveProject(string enrollment);
        void SetPracticionerStatusAsInactive(string enrollment);
        bool PracticionerIsAlreadyRegistered(Practicioner practicioner, bool isUpdate);
        Practicioner GetAllInformationPracticioner(string enrollment);
    }
}
