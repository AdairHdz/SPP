using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class ResponsibleProject
    {
        [Key]
        public int IdResponsibleProject { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(254)]
        public string EmailAddress { get; set; }

        [Required]
        [MaxLength(50)]
        public string Charge { get; set; }

        public virtual ResponsibleProjectStatus ResponsibleProjectStatus { get; set; } = ResponsibleProjectStatus.ACTIVE;
    }
}
