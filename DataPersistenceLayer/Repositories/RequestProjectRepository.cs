using DataPersistenceLayer.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataPersistenceLayer.Repositories
{
	public class RequestProjectRepository : Repository<RequestProject>, IRequestProjectRepository
	{
		public ProfessionalPracticesContext ProfessionalPracticesContext
		{
			get
			{
				return _context as ProfessionalPracticesContext;
			}
		}

		public RequestProjectRepository(DbContext context) : base(context) { }

		public int GetPracticionerRequest(string enrollment)
        {
			List<RequestProject> requestProjects = _context.Set<RequestProject>().Where(Request => Request.Enrollment == enrollment).ToList();
			return requestProjects.Count;
		}

		public List<RequestProject> GetListPracticionerRequest(string enrollment)
        {
			return _context.Set<RequestProject>().Where(Request => Request.Enrollment == enrollment).ToList();
		}



	}
}
