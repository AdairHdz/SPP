using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class Activity
    {
        [Key]
        public int IdActivity { get; set; }

        [Required]
        public int Value { get; set; }
                
        [MaxLength(100)]
        public string Description { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public DateTime DeliverDate { get; set; }

        [MaxLength(255)]
        public string Document { get; set; }

        [Required]
        public string EnrollmentPracticing { get; set; }
         
        [ForeignKey("EnrollmentPracticing")]
        public virtual Practicing Practicing { get; set; }
                        
        public string IdStaffNumberTeacher { get; set; }

        [ForeignKey("IdStaffNumberTeacher")]
        public virtual Teacher Teacher { get; set; }
    }
}
