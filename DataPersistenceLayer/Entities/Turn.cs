using System.ComponentModel.DataAnnotations;

namespace DataPersistenceLayer.Entities
{
    public class Turn
    {
        [Key]
        public int IdTurn { get; set; }

        [Required]
        [MaxLength(15)]
        public string TurnName { get; set; }
    }
}
