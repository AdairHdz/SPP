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

        [Required]
        public int IdGender { get; set; }
        
        [ForeignKey("IdGender")]
        public virtual Gender Gender { get; set; }

        [Required]
        public int IdStatus { get; set; }
        
        [ForeignKey("IdStatus")]
        public virtual UserStatus UserStatus { get; set; }

        [Required]
        [MaxLength(254)]
        public string Email { get; set; }
        
        [MaxLength(254)]
        public string AlternateEmail { get; set; }

        [MaxLength(10)]
        public string PhoneNumber { get; set; }


    }
}
