﻿using DataPersistenceLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataPersistenceLayer.Repositories
{
    public class TeacherRepository : Repository<Teacher>, ITeacherRepository
    {
        public ProfessionalPracticesContext ProfessionalPracticesContext
        {
            get
            {
                return _context as ProfessionalPracticesContext;
            }
        }

        public TeacherRepository(DbContext context) : base(context) { }

        public bool TeacherIsAlreadyRegistered(Teacher teacher, bool isUpdate)
        {
            User coordinatorUser = teacher.User;
            int coordinatorsThatMatch = 0;
            try
            {
                if (isUpdate)
                {
                    coordinatorsThatMatch = _context.Set<Teacher>().Include(s => s.User)
                    .Where(c => c.User.IdUser != coordinatorUser.IdUser
                    && (c.User.AlternateEmail.Equals(coordinatorUser.AlternateEmail)
                    || c.User.Email.Equals(coordinatorUser.Email)
                    || c.User.Email.Equals(coordinatorUser.AlternateEmail)
                    || c.User.AlternateEmail.Equals(coordinatorUser.Email)
                    || c.User.PhoneNumber.Equals(coordinatorUser.PhoneNumber))).Count();
                }
                else
                {
                    coordinatorsThatMatch = _context.Set<Teacher>()
                    .Where(c => c.StaffNumber.Equals(teacher.StaffNumber)
                    || c.User.AlternateEmail.Equals(coordinatorUser.AlternateEmail)
                    || c.User.Email.Equals(coordinatorUser.Email)
                    || c.User.Email.Equals(coordinatorUser.AlternateEmail)
                    || c.User.AlternateEmail.Equals(coordinatorUser.Email)
                    || c.User.PhoneNumber.Equals(coordinatorUser.PhoneNumber)).Include(c => c.User).Count();
                }

                if (coordinatorsThatMatch != 0)
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

        public bool ActiveTeacher()
        {
            IList<Teacher> teacher = _context.Set<Teacher>().Where(Teacher => Teacher.User.UserStatus == UserStatus.ACTIVE).ToList();
            if (object.ReferenceEquals(null, teacher))
            {
                return false;
            }
            return true;
        }

        public IList<Teacher> GetActiveTeachers()
        {
            return _context.Set<Teacher>().Include(teacher => teacher.User).Where(Teacher => Teacher.User.UserStatus == UserStatus.ACTIVE).ToList();
        }

        public Teacher GetTeacherWithAllInformation(string staffNumber)
        {
            return _context.Set<Teacher>().Include(s => s.User.Account).Where(Teacher => Teacher.StaffNumber == staffNumber).First();
        }

        public string GetStaffNumberTeacher(string password, string userName)
        {
            Teacher teacher = _context.Set<Teacher>().Where(Teacher => Teacher.User.Account.Password == password 
            && Teacher.User.Account.Username == userName).First();
            return teacher.StaffNumber;
        }
    }
}