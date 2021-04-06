using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class Group
    {
        [Key]
        public int IdGroup { get; set; }

        [Required]
        public string Nrc { get; set; }
        public virtual GroupStatus GroupStatus { get; set; }
        public string Term { get; set; }

        [Required]
        public string StaffNumber { get; set; }

        [ForeignKey("StaffNumber")]
        public virtual Teacher Teacher { get; set; }
    }
}
