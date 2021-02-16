using System.ComponentModel.DataAnnotations;

namespace DataPersistenceLayer.Entities
{
    public class UserStatus
    {
        [Key]
        public int UserStatusId { get; set; }

        [Required]
        [MaxLength(30)]
        public string Status { get; set; }
    }
}
