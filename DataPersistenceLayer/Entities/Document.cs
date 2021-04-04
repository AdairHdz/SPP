using System;
using System.ComponentModel.DataAnnotations;

namespace DataPersistenceLayer.Entities
{
    public class Document
    {
        [Key]
        public int IdDocument { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public DateTime? DeliveryDate { get; set; }

        [MaxLength(50)]
        public string TypeDocument { get; set; }

        public string RouteSave { get; set; } 
    }
}
