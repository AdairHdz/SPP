using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class Teacher
    {
        [Key]
        [MaxLength(20)]
        public string StaffNumber { get; set; }
        
        public DateTime? RegistrationDate { get; set; }
        
        public DateTime? DischargeDate { get; set; }

        public int IdUser { get; set; }

        [ForeignKey("IdUser")]
        public virtual User User { get; set; }
    }
}
