using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class Report
    {
        [Key]
        public int IdReport { get; set; }

        [Required]        
        public string Activities { get; set; }
        
        public int Score { get; set; }

        public DateTime? CompletionDate { get; set; }

        public DateTime? DeliverDate { get; set; }

        public string Document { get; set; }

        [Required]
        public string EnrollmentPracticing { get; set; }
          
        [ForeignKey("EnrollmentPracticing")]
        public virtual Practicing Practicing { get; set; }
    }
}
