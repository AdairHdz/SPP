using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class PartialReport
    {
        [Key]
        public int IdPartialReport { get; set; }

        [Required]
        [MaxLength(20)]
        public string NumberReport { get; set; }

        [Required]
        [MaxLength(500)]
        public string ResultsObtained { get; set; }

        [Required]
        public int HoursCovered { get; set; }

        [MaxLength(500)]
        public string Observations { get; set; }

        [Required]
        public DateTime? DeliveryDate { get; set; }

        [Required]
        public int IdProject { get; set; }
        
        [ForeignKey("IdProject")]
        public Project Project { get; set; }

        [Required]
        public string Enrollment { get; set; }
        [ForeignKey("Enrollment")]
        public Practicioner Practicioner { get; set; }
    }
}
