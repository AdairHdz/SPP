using System.ComponentModel.DataAnnotations;

namespace DataPersistenceLayer.Entities
{
    public class ProjectStatus
    {
        [Key]
        public int IdProjectStatus { get; set; }

        [Required]
        [MaxLength(30)]
        public string Status { get; set; }
    }
}
