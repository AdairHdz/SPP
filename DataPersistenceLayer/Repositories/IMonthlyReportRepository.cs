using DataPersistenceLayer.Entities;

namespace DataPersistenceLayer.Repositories
{
    public interface IMonthlyReportRepository : IRepository<MonthlyReport>
    {
        int GetId();

    }
}
