using DataPersistenceLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataPersistenceLayer.Repositories
{
    public class PracticionerRepository : Repository<Practicioner>, IPracticionerRepository
    {
        public ProfessionalPracticesContext ProfessionalPracticesContext
        {
            get
            {
                return _context as ProfessionalPracticesContext;
            }
        }

        public PracticionerRepository(DbContext context) : base(context) { }

        public bool PracticionerIsAlreadyRegistered(Practicioner practicioner)
        {
            Practicioner practicionerWithSameEnrollment = Get(practicioner.Enrollment);
            if (practicionerWithSameEnrollment != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool PracticionerHasActiveProject(string enrollment)
        {
            Assignment assignment = _context.Set<Assignment>().SingleOrDefault(Assignment => Assignment.Enrollment == enrollment
            && (Assignment.Project.Status == ProjectStatus.FILLED || Assignment.Project.Status == ProjectStatus.ACTIVE));
            if (!object.ReferenceEquals(null, assignment))
            {
                return true;
            }
            return false;
        }

        public void SetPracticionerStatusAsInactive(string enrollment)
        {
            Practicioner practicioner = _context.Set<Practicioner>().SingleOrDefault(Practicioner => Practicioner.Enrollment == enrollment);
            if (!object.ReferenceEquals(null, practicioner))
            {
                practicioner.User.UserStatus = UserStatus.INACTIVE;
            }
        }

        public bool PracticionerIsAlreadyRegistered(Practicioner practicioner, bool isUpdate)
        {
            User practicionerUser = practicioner.User;
            int practicionerThatMatch = 0;
            try
            {
                if (isUpdate)
                {
                    practicionerThatMatch = _context.Set<Practicioner>().Include(UserPracticioner => UserPracticioner.User)
                    .Where(PracticionerFound => PracticionerFound.User.IdUser != practicionerUser.IdUser
                    && (PracticionerFound.User.AlternateEmail.Equals(practicionerUser.AlternateEmail)
                    || PracticionerFound.User.Email.Equals(practicionerUser.Email)
                    || PracticionerFound.User.Email.Equals(practicionerUser.AlternateEmail)
                    || PracticionerFound.User.AlternateEmail.Equals(practicionerUser.Email)
                    || PracticionerFound.User.PhoneNumber.Equals(practicionerUser.PhoneNumber))).Count();
                }
                else
                {
                    practicionerThatMatch = _context.Set<Practicioner>()
                    .Where(PracticionerFound => PracticionerFound.Enrollment.Equals(practicioner.Enrollment)
                    || PracticionerFound.User.AlternateEmail.Equals(practicionerUser.AlternateEmail)
                    || PracticionerFound.User.Email.Equals(practicionerUser.Email)
                    || PracticionerFound.User.Email.Equals(practicionerUser.AlternateEmail)
                    || PracticionerFound.User.AlternateEmail.Equals(practicionerUser.Email)
                    || PracticionerFound.User.PhoneNumber.Equals(practicionerUser.PhoneNumber)).Include(UserPracticioner => UserPracticioner.User).Count();
                }

                if (practicionerThatMatch != 0)
                {
                    return true;
                }
                return false;
            }
            catch (ArgumentNullException)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public Practicioner GetAllInformationPracticioner(string enrrolment)
        {
            Practicioner practicioner = _context.Set<Practicioner>().Include(PracticionerUser => PracticionerUser.User.Account).Where(PracticionerSearch => PracticionerSearch.Enrollment == enrrolment).First();
            return practicioner;
        }

        public bool RequiredPracticionersToGroup()
        {
            IList<Practicioner> practicioner = _context.Set<Practicioner>().Where(Practicioner => Practicioner.IdGroup == null
            && Practicioner.User.UserStatus == UserStatus.ACTIVE).ToList();
            if (practicioner.Count < 5)
            {
                return false;
            }
            return true;
        }

        public IList<Practicioner> PracticionersToGroup()
        {
            return _context.Set<Practicioner>().Include(UserPracticioner => UserPracticioner.User).Where(Practicioner => Practicioner.IdGroup == null
            && Practicioner.User.UserStatus == UserStatus.ACTIVE).ToList();  
        }

        public void AddGroup(IList<Practicioner> practicionersSelected, int idGroup)
        {
            foreach (Practicioner practicionerInList in practicionersSelected)
            {
                Practicioner practicioner = _context.Set<Practicioner>().Where(Practicioner => Practicioner.Enrollment == practicionerInList.Enrollment).FirstOrDefault();
                practicioner.IdGroup = idGroup;
            }
        }

        public IList<Practicioner> GetAllPracticionersWithUserData()
        {
            return _context.Set<Practicioner>().Include(UserPracticioner => UserPracticioner.User).ToList();
        }

        public IList<Practicioner> GetPracticionersWithUserData(Func<Practicioner, bool> predicate)
        {
            return _context.Set<Practicioner>().Include(UserPracticioner => UserPracticioner.User).Where(predicate).ToList();
        }

        public IList<Practicioner> GetPracticionersInThisGroup(int idGroup)
        {
            return _context.Set<Practicioner>().Include(UserPracticioner => UserPracticioner.User).Where(Practicioner => Practicioner.IdGroup == idGroup).ToList();
        }
        public IList<Practicioner> GetPracticionerActiveNotInThisGroup(int idGroup)
        {
            return _context.Set<Practicioner>().Include(UserPracticioner => UserPracticioner.User).Where(Practicioner => Practicioner.IdGroup != idGroup
            && Practicioner.User.UserStatus == UserStatus.ACTIVE).ToList();
        }

        public void RemoveGroup(IList<Practicioner> practicionersToRemove)
        {
            foreach (Practicioner practicionerInList in practicionersToRemove)
            {
                Practicioner practicioner = _context.Set<Practicioner>().Where(Practicioner => Practicioner.Enrollment == practicionerInList.Enrollment).FirstOrDefault();
                practicioner.IdGroup = null;
            }
        }

    }
}
