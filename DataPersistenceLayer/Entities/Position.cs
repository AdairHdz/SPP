using System.ComponentModel.DataAnnotations;

namespace DataPersistenceLayer.Entities
{
    public class Position
    {
        [Key]
        public int IdPosition { get; set; }

        [Required]
        [MaxLength(50)]
        public string NamePosition { get; set; }
    }
}
