using System.ComponentModel.DataAnnotations;

namespace DataPersistenceLayer.Entities
{
    public class Account
    {
        [Key]
        public int IdAccount { get; set; }

        [MaxLength(50)]
        public string Username { get; set; }

        [MaxLength(120)]
        public string Password { get; set; }

        public bool FirstLogin { get; set; } = true;
    }
}
