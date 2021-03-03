using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class ActivityMade
    {
        [Key]
        public int IdActivity { get; set; }
        public string Name { get; set; }
        public string PlannedWeek { get; set; }
        public string PlannedMonth { get; set; }
        public string RealMonth { get; set; }
        public string RealWeek { get; set; }
        public int IdPartialReport { get; set; }
        
        [ForeignKey("IdPartialReport")]
        public virtual PartialReport PartialReport { get; set; }
    }
}
