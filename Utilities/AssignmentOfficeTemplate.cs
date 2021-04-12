using DataPersistenceLayer.Entities;
using System;

namespace Utilities
{
    public class AssignmentOfficeTemplate
    {
        public string OfficeNumber { get; set; }
        public DateTime DateOfGeneration { get; set; }
        public string ResponsibleProjectName { get; set; }
        public string ResponsibleProjectCharge { get; set; }
        public string LinkedOrganizationName { get; set; }
        public string LinkedOrganizationAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PracticionerName { get; set; }
        public string PracticionerEnrollment { get; set; }
        public string ProjectName { get; set; }
        public int ProjectDuration { get; set; }
        public string CoordinatorName { get; set; }

        public void MapData(Project project, Practicioner practicioner, Assignment assignment, Coordinator coordinator)
        {            
            ResponsibleProject responsibleProject = project.ResponsibleProject;
            LinkedOrganization linkedOrganization = project.LinkedOrganization;            

            OfficeNumber = "XX/YY";
            DateOfGeneration = DateTime.Now;
            ResponsibleProjectName = $"{responsibleProject.Name} {responsibleProject.LastName}";
            ResponsibleProjectCharge = responsibleProject.Charge;
            LinkedOrganizationName = linkedOrganization.Name;
            LinkedOrganizationAddress = linkedOrganization.Address;
            City = linkedOrganization.City.NameCity;
            State = linkedOrganization.State.NameState;
            PracticionerName = $"{practicioner.User.Name} {practicioner.User.LastName}";
            PracticionerEnrollment = practicioner.Enrollment;
            ProjectName = project.NameProject;
            ProjectDuration = project.Duration;
            CoordinatorName = coordinator.User.Name;
            
        }

    }    
}
