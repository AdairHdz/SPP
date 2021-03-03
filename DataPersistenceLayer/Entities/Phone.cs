using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class Phone
    {
        [Key]
        public int IdPhoneNumber { get; set; }
        public string Extension { get; set; }        
        public string PhoneNumber { get; set; }

        public int IdLinkedOrganization { get; set; }
        [ForeignKey("IdLinkedOrganization")]
        public virtual LinkedOrganization LinkedOrganization { get; set; }
    }
}
