using DataPersistenceLayer.Entities;
using System;

namespace Utilities
{
    public class AcceptanceOfficeTemplate
    {
        public int Day { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public string CoordinatorName { get; set; }
        public string PracticionerName { get; set; }
        public string PracticionerEnrollment { get; set; }
        public string LinkedOrganizationName { get; set; }
        public string StartingDate { get; set; }
        public int ProjectDuration { get; set; }
        public string MondaySchedule { get; set; }
        public string TuesdaySchedule { get; set; }
        public string WednesdaySchedule { get; set; }
        public string ThursdaySchedule { get; set; }
        public string FridaySchedule { get; set; }
        public string CoordinatorEmail { get; set; }
        public string CoordinatorPhoneNumber { get; set; }
        public string CoordinatorCharge { get; set; }        

        public void MapData(Project project, Practicioner practicioner, Assignment assignment, Coordinator coordinator)
        {            
            Day = DateTime.Now.Day;
            Month = "Abril";
            Year = DateTime.Now.Year;
            CoordinatorName = $"{coordinator.User.Name} {coordinator.User.LastName}";
            PracticionerName = practicioner.User.Name;
            PracticionerEnrollment = practicioner.Enrollment;
            LinkedOrganizationName = project.LinkedOrganization.Name;
            StartingDate = assignment.StartTerm;
            ProjectDuration = project.Duration;
            MondaySchedule = "";
            TuesdaySchedule = "";
            WednesdaySchedule = "";
            ThursdaySchedule = "";
            FridaySchedule = "";
            CoordinatorEmail = coordinator.User.Email;
            CoordinatorPhoneNumber = coordinator.User.PhoneNumber;            
            CoordinatorCharge = "Coordinador de prácticas profesionales";
        }

    }
}
