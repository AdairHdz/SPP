using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class Phone
    {
        [Key]
        public int IdPhoneNumber { get; set; }

        [MaxLength(3)]
        public string Extension { get; set; }
        
        [MaxLength(10)]
        public string PhoneNumber { get; set; }

        public int IdLinkedOrganization { get; set; }
        [ForeignKey("IdLinkedOrganization")]
        public virtual LinkedOrganization LinkedOrganization { get; set; }
    }
}
