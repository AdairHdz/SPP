using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class Document
    {
        [Key]
        public int IdDocument { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public DateTime? DeliveryDate { get; set; }

        [MaxLength(50)]
        public string TypeDocument { get; set; }

        public string RouteSave { get; set; }

        [MaxLength(9)]
        public string Enrollment { get; set; }

        [ForeignKey("Enrollment")]        
        public virtual Practicioner Practicing { get; set; }
    }
}
