using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class City
    {
        [Key]
        public int IdCity { get; set; }

        [MaxLength(25)]
        public string NameCity { get; set; }    

    }
}
