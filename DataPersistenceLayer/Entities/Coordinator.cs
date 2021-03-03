using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class Coordinator
    {
        [Key]
        [MaxLength(20)]
        public string StaffNumber { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        
        public DateTime? DischargeDate { get; set; }

        public int IdUser { get; set; }
        
        [ForeignKey("IdUser")]
        public User User { get; set; }
        
    }
}
