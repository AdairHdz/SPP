using System.ComponentModel.DataAnnotations;

namespace DataPersistenceLayer.Entities
{
    public class RequestStatus
    {
        [Key]
        public int IdRequestStatus { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }
    }
}
