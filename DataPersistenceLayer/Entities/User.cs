using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }

        public virtual Gender Gender { get; set; }

        public virtual UserStatus UserStatus { get; set; } = UserStatus.ACTIVE;

        [Required]
        [MaxLength(254)]
        public string Email { get; set; }
        
        [MaxLength(254)]
        public string AlternateEmail { get; set; }

        [MaxLength(10)]
        public string PhoneNumber { get; set; }

        public virtual UserType UserType { get; set; }

        public int IdAccount { get; set; }

        [ForeignKey("IdAccount")]
        public virtual Account Account { get; set; }
    }
}
