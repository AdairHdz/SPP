using DataPersistenceLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataPersistenceLayer.Repositories
{
	public class GroupRepository : Repository<Group>, IGroupRepository
	{
		public ProfessionalPracticesContext ProfessionalPracticesContext
		{
			get
			{
				return _context as ProfessionalPracticesContext;
			}
		}

		public GroupRepository(DbContext context) : base(context) { }

		public bool GroupIsAlreadyRegistered(Group group)
		{
			Group groupWithSameInformation = _context.Set<Group>().SingleOrDefault(Group => Group.Term == group.Term
			&& Group.Nrc == group.Nrc);
			if (groupWithSameInformation != null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool GroupCanBeModify(Group group)
		{
			Group groupWithSameInformation = _context.Set<Group>().SingleOrDefault(Group => Group.IdGroup != group.IdGroup
			&& Group.Nrc == group.Nrc && Group.Term == group.Term);
			if (groupWithSameInformation == null)
			{
				return true;
			}
			else
			{
                
				return false;
			}
		}

		public int GroupId()
		{
			Group groupSearch = _context.Set<Group>().OrderByDescending(group => group.IdGroup).FirstOrDefault();
			return groupSearch.IdGroup;
		}

		public Teacher GetTeacherAssigned(string staffNumber)
        {
			return _context.Set<Teacher>().Include(UserTeacher => UserTeacher.User).SingleOrDefault(Group => Group.StaffNumber == staffNumber);

		}

		public bool GetIfThisTeacherHaveActiveGroups(string staffNumber)
        {
			IList<Group> groupSearch = _context.Set<Group>().Where(group => group.StaffNumber == staffNumber && group.GroupStatus == GroupStatus.ACTIVE).ToList();
			if (groupSearch.Count != 0)
            {
				return true;
            }
			return false; 
		}

		public IList<Group> GetActiveGroupsForThisTeacher(string staffNumber)
		{
			return _context.Set<Group>().Where(group => group.StaffNumber == staffNumber && group.GroupStatus == GroupStatus.ACTIVE).ToList();
		}

		public IList<Activity> GetActivitiesForThisGroup(int idGroup)
        {
			return _context.Set<Activity>().Where(activity => activity.IdGroup == idGroup).ToList();
		}
	}
}
