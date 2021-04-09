using DataPersistenceLayer.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace DataPersistenceLayer.Repositories
{
	class ProjectRepository : Repository<Project>, IProjectRepository
	{
		public ProfessionalPracticesContext ProfessionalPracticesContext
		{
			get
			{
				return _context as ProfessionalPracticesContext;
			}
		}

		public ProjectRepository(DbContext context) : base(context) { }

		public IList<Project> GetProjectsAvailableToRequest(string enrollment)
		{
			List<Project> projects = _context.Set<Project>().Where(Project => Project.Status == ProjectStatus.ACTIVE).ToList();
			List<RequestProject> requestProjects = _context.Set<RequestProject>().Where(Request => Request.Enrollment == enrollment).ToList();
			List<Project> projectsAvailableForThisPracticioner = projects;

			for (int index = 0; index < projects.Count; index++)
			{
				foreach (RequestProject requestProject in requestProjects)
				{
					if (projects[index].IdProject == requestProject.IdProject)
					{
						projectsAvailableForThisPracticioner.Remove(projects[index]);
					}
				}
			}

			return projectsAvailableForThisPracticioner;
		}
	}
}
