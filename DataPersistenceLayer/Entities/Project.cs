using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class Project
    {
        [Key]
        public int IdProject { get; set; }

        [Required]
        [MaxLength(150)]
        public string NameProject { get; set; }

        [Required]
        [MaxLength(100)]      
        public string DaysHours { get; set; }       
      
        [MaxLength(300)]
        public string Description { get; set; }

        [Required]
        [MaxLength(300)]
        public string ObjectiveGeneral { get; set; }

        [Required]
        [MaxLength(300)]
        public string ObjectiveImmediate { get; set; }

        [Required]
        [MaxLength(300)]
        public string ObjectiveMediate { get; set; }

        [Required]
        [MaxLength(300)]
        public string Methodology { get; set; }

        [Required]
        [MaxLength(300)]
        public string Resources { get; set; }
        public virtual ProjectStatus Status { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        [MaxLength(300)]
        public string Activities { get; set; }

        [Required]
        [MaxLength(300)]
        public string Responsibilities { get; set; }

        [Required]
        public int QuantityPracticing { get; set; }

        [Required]
        public int QuantityPracticingAssing { get; set; } = 0;

        [Required]
        [MaxLength(50)]
        public string Term { get; set; }

        [Required]
        public int IdLinkedOrganization { get; set; }
        
        [ForeignKey("IdLinkedOrganization")]
        public virtual LinkedOrganization LinkedOrganization { get; set; }

        [Required]
        public string StaffNumberCoordinator { get; set; }
        
        [ForeignKey("StaffNumberCoordinator")]
        public virtual Coordinator Coordinator { get; set; }

        [Required]
        public int IdResponsibleProject { get; set; }
        
        [ForeignKey("IdResponsibleProject")]
        public virtual ResponsibleProject ResponsibleProject { get; set; }

        public virtual List<SchedulingActivity> SchedulingActivities { get; set; }
    }
}
