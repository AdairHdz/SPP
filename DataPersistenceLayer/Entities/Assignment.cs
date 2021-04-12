using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class Assignment
    {
        //clave primaria compuesta: matrícula y id proyecto
        [Key]
        public int IdAssignment { get; set; }
        public string CompletionTerm { get; set; } //Periodo
        public DateTime DateAssignment { get; set; }
        [MaxLength(250)]
        public string RouteSave { get; set; }
        public string StartTerm { get; set; }
        public AssignmentStatus Status { get; set; }        
        public int IdOfficeOfAcceptance { get; set; }

        [ForeignKey("IdOfficeOfAcceptance")]
        public virtual OfficeOfAcceptance OfficeOfAcceptance { get; set; }

        public int IdProject { get; set; }
        [ForeignKey("IdProject")]
        public virtual Project Project { get; set; }

        [MaxLength(10)]
        public string Enrollment { get; set; }
        [ForeignKey("Enrollment")]
        public virtual Practicioner Practicioner { get; set; }
    }
}
