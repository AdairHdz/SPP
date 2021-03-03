using System.ComponentModel.DataAnnotations;

namespace DataPersistenceLayer.Entities
{
    public class Group
    {
        [Key]
        [MaxLength(5)]
        public string Nrc { get; set; }
        public virtual GroupStatus GroupStatus { get; set; }
        public string Term { get; set; }
    }
}
