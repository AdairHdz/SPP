using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class PartialReport
    {
        [Key]
        public int IdParcialReport { get; set; }

        [Required]
        public int NumberReport { get; set; }

        [Required]
        [MaxLength(254)]
        public string ResultsObtained { get; set; }

        [Required]
        public int HoursCovered { get; set; }

        [MaxLength(254)]
        public string Observations { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public int IdProject { get; set; }
        
        [ForeignKey("IdProject")]
        public Project Project { get; set; }

        public string Enrollment { get; set; }
        [ForeignKey("Enrollment")]
        public Practicioner Practicioner { get; set; }
    }
}
