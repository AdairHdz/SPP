using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class LinkedOrganization
    {      
        [Key]
        public int IdLinkedOrganization { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int DirectUsers { get; set; }

        [Required]
        public int IndirectUsers { get; set; }

        [Required]
        [MaxLength(254)]
        public string Email { get; set; }

        [Required]
        public virtual List<Phone> PhoneNumbers { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Address { get; set; }

        [Required]
        public int IdCity { get; set; }
        
        [ForeignKey("IdCity")]
        public virtual City City { get; set; }

        [Required]
        public int IdState { get; set; }
        
        [ForeignKey("IdState")]
        public virtual State State { get; set; }

        [Required]
        public int IdSector { get; set; }
        
        [ForeignKey("IdSector")]
        public virtual Sector Sector { get; set; }

        public virtual LinkedOrganizationStatus LinkedOrganizationStatus { get; set; }

        public virtual List<Project> Projects { get; set; }

    }
}
