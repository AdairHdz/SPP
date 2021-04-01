using DataPersistenceLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataPersistenceLayer.Repositories
{
    public class StateRepository : Repository<State>, IStateRepository
    {
        public ProfessionalPracticesContext ProfessionalPracticesContext
        {
            get
            {
                return _context as ProfessionalPracticesContext;
            }
        }

        public StateRepository(DbContext context) : base(context) { }

        public List<State> GetStatesWithCities()
        {
            IQueryable<State> statesWithCities = _context.Set<State>().Include(s => s.Cities);
            try
            {
                List<State> statesList = statesWithCities.ToList();
                return statesList;
            }
            catch (ArgumentNullException)
            {
                return null;
            }
        }
    }
}
