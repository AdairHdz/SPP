using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class Request
    {
        [Key]
        public int IdRequest { get; set; }

        public string Enrollment { get; set; }

        [ForeignKey("Enrollment")]
        public virtual Practicing Practicing { get; set; }
        
        public int IdProject { get; set; }

        [ForeignKey("IdProject")]
        public virtual Project Project { get; set; }

        public int Status { get; set; }

        [ForeignKey("Status")]
        public virtual RequestStatus RequestStatus { get; set; }
    }
}
