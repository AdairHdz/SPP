using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class Manager 
    {
        [Key]
        public string StaffNumber { get; set; }
        public int IdUser { get; set; }

        [ForeignKey("IdUser")]
        public User User { get; set; }
    }
}
