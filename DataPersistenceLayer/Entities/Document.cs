using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public int IdActivityPracticioner { get; set; }

        [ForeignKey("IdActivityPracticioner")]
        public virtual ActivityPracticioner ActivityPracticioner { get; set; }
    }
}
