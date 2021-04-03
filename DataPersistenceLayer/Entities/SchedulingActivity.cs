using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class SchedulingActivity
    {
        [Key]
        public int IdSchedulingActivity { get; set; }

        [Required]
        [MaxLength(300)]
        public string Activity { get; set; }

        [Required]
        [MaxLength(50)]
        public string Month { get; set; }

        public int IdProject { get; set; }

        [ForeignKey("IdProject")]
        public virtual Project Project { get; set; }
    }
}
