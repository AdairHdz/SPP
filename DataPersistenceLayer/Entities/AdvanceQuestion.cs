using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class AdvanceQuestion
    {
        [Key]
        public int IdAdvanceQuestion { get; set; }

        public string Question { get; set; }
        public string Reasons { get; set; }
        public bool Reply { get; set; }
        public int IdMonthlyReport { get; set; }
        [ForeignKey("IdMonthlyReport ")]
        public MonthlyReport MonthlyReport { get; set; }
    }
}
