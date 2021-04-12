using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class RequestProject
    {
        [Key]
        public int IdRequestProject { get; set; }

        public DateTime RequestDate { get; set; }

        public int IdProject { get; set; }
        [ForeignKey("IdProject")]
        public virtual Project Project { get; set; }

        public string Enrollment { get; set; }
        [ForeignKey("Enrollment")]
        public Practicioner Practicioner { get; set; }

        public virtual RequestStatus RequestStatus { get; set; }
    }
}
