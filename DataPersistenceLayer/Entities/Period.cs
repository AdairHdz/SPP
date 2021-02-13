using System.ComponentModel.DataAnnotations;

namespace DataPersistenceLayer.Entities
{
    public class Period
    {
        [Key]
        public int IdPeriod { get; set; }

        [Required]
        [MaxLength(25)]
        public string PeriodName { get; set; }
    }
}
