using System.ComponentModel.DataAnnotations;

namespace DataPersistenceLayer.Entities
{
    public class Sector
    {
        [Key]
        public int IdSector { get; set; }

        [MaxLength(50)]
        public string NameSector { get; set; }
    }
}
