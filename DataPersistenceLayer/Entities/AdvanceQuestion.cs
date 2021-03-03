using System.ComponentModel.DataAnnotations;

namespace DataPersistenceLayer.Entities
{
    public class AdvanceQuestion
    {
        [Key]
        public int IdAdvanceQuestion { get; set; }

        public string Question { get; set; }
        public string Reasons { get; set; }
        public bool Reply { get; set; }
    }
}
