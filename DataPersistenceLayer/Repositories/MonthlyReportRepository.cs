using DataPersistenceLayer.Entities;
using System.Data.Entity;
using System.Linq;

namespace DataPersistenceLayer.Repositories
{
	public class MonthlyReportRepository : Repository<MonthlyReport>, IMonthlyReportRepository
	{
		public ProfessionalPracticesContext ProfessionalPracticesContext
		{
			get
			{
				return _context as ProfessionalPracticesContext;
			}
		}

		public MonthlyReportRepository(DbContext context) : base(context) { }

		public int GetId()
		{
			MonthlyReport monthlyReport = _context.Set<MonthlyReport>().OrderByDescending(monthlyReport => monthlyReport.IdMonthlyReport).FirstOrDefault();
			return monthlyReport.IdMonthlyReport;
		}
	}
}
