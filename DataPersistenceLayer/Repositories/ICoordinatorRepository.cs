using DataPersistenceLayer.Entities;
using System;
using System.Collections.Generic;

namespace DataPersistenceLayer.Repositories
{
    public interface ICoordinatorRepository : IRepository<Coordinator>
    {
        bool CoordinatorIsAlreadyRegistered(Coordinator coordinator, bool isUpdate);
        Coordinator GetCoordinatorWithUserAndAccountData(string staffNumber);
        Coordinator GetActiveCoordinator();
        void SetCoordinatorStatusAsInactive(string staffNumber);
        IList<Coordinator> GetAllCoordinatorsWithUserData();
        IList<Coordinator> GetCoordinatorsWithUserData(Func<Coordinator, bool> predicate);
    }
}
