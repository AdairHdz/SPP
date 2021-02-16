using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class Practicing
    {
        [Key]
        [MaxLength(10)]
        public string Enrollment { get; set; }

        public int IdTurn { get; set; }

        [ForeignKey("IdTurn")]
        public virtual Turn Turn { get; set; }

        [Required]
        public int IdPeriod { get; set; }
        
        [ForeignKey("IdPeriod")]
        public virtual Period Period { get; set; }

        [Required]
        public int IdUser { get; set; }
        
        [ForeignKey("IdUser")]
        public virtual User User { get; set; }

        public int IdProject { get; set; }

        [ForeignKey("IdProject")]
        public virtual Project Project { get; set; }

        public virtual List<Report> Reports { get; set; }

        public virtual List<Activity> Activities { get; set; }

        public virtual List<Request> Requests { get; set; }
    }
}
