using DataPersistenceLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataPersistenceLayer.Repositories
{
	public class ActivityRepository : Repository<Activity>, IActivityRepository
	{
		public ProfessionalPracticesContext ProfessionalPracticesContext
		{
			get
			{
				return _context as ProfessionalPracticesContext;
			}
		}

		public ActivityRepository(DbContext context) : base(context) { }

		public void ChangeToFinished()
        {
			string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			DateTime dateNow = Convert.ToDateTime(date);
			IList<Activity> activities = _context.Set<Activity>().Where(activity => activity.FinishDate <= dateNow).ToList();
			
			foreach (Activity activity in activities)
            {
				activity.ActivityStatus = ActivityStatus.FINISHED;
            }
		
		}

		public IList<Practicioner> GetPracticionersToActivity(int idGroup)
		{
			return _context.Set<Practicioner>().Where(practicioner => practicioner.IdGroup == idGroup).ToList();

		}

		public int GetId()
		{
			Activity activity = _context.Set<Activity>().OrderByDescending(activity => activity.IdActivity).FirstOrDefault();
			return activity.IdActivity;
		}
	}
}
