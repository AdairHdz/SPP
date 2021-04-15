using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class ActivityMade
    {
        [Key]
        public int IdActivity { get; set; }

        [Required]
        [MaxLength(300)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string PlannedWeek { get; set; }

        [Required]
        [MaxLength(100)]
        public string RealWeek { get; set; }
        public int IdPartialReport { get; set; }
        
        [ForeignKey("IdPartialReport")]
        public virtual PartialReport PartialReport { get; set; }
    }
}
