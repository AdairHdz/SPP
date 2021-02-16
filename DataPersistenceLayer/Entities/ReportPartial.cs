using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class ReportPartial
    {
        [Key]
        public int IdReportParcial { get; set; }

        [Required]
        public int NumberReport { get; set; }

        [Required]
        [MaxLength(254)]
        public string ResultsObtained { get; set; }

        [Required]
        public int HoursCovered { get; set; }

        [MaxLength(254)]
        public string Observations { get; set; }

        [Required]
        public int IdReport { get; set; }
        
        [ForeignKey("IdReport")]
        public virtual Report Report { get; set; }
    }
}
