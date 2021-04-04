using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class ActivityPracticioner
    {
        [Key]
        public int IdActivityPracticioner { get; set; }

        public double Qualification { get; set; } = 0;

        [MaxLength(255)]
        public string Observation { get; set; }

        [MaxLength(255)]
        public string Answer { get; set; }

        public ActivityPracticionerStatus ActivityPracticionerStatus { get; set; } = ActivityPracticionerStatus.NOTQUALIFIED;

        [MaxLength(10)]
        public string Enrollment { get; set; }

        [ForeignKey("Enrollment")]
        public virtual Practicioner Practicioner { get; set; }
        public int IdActivity { get; set; }

        [ForeignKey("IdActivity")]
        public virtual Activity Activity { get; set; }
        public int IdDocument { get; set; }

        [ForeignKey("IdDocument")]
        public virtual Document Document { get; set; }
    }
}
