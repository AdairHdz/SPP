using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class Practicioner
    {
        [Key]
        [MaxLength(10)]
        public string Enrollment { get; set; }

        public string Term { get; set; }

        public int Credits { get; set; }

        [Required]
        public int IdUser { get; set; }
        
        [ForeignKey("IdUser")]
        public virtual User User { get; set; }

        public string Nrc { get; set; }

        [ForeignKey("Nrc")]
        public virtual Group Group { get; set; }

        public virtual List<RequestProject> Requests { get; set; }
    }
}
