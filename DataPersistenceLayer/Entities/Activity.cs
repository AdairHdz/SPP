using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class Activity
    {
        [Key]
        public int IdActivity{ get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [Required]
        public ActivityType ActivityType { get; set; }

        [Required]
        public ActivityStatus ActivityStatus { get; set; } = ActivityStatus.ACTIVE;

        [MaxLength(255)]
        public string Description { get; set; }

        [Required]
        public double ValueActivity { get; set; } = 0;

        [Required]
        public DateTime? StartDate { get; set; }

        [Required]
        public DateTime? FinishDate { get; set; }

        [Required]
        public string StaffNumberTeacher { get; set; }

        [ForeignKey("StaffNumberTeacher")]
        public virtual Teacher teacher { get; set; } 
    }
}
