using System.ComponentModel.DataAnnotations;

namespace DataPersistenceLayer.Entities
{
    public class Gender
    {

        [Key]
        public int IdGender { get; set; }

        [Required]
        [MaxLength(25)]
        public string GenderName { get; set; }
    }
}
