using System;
using System.ComponentModel.DataAnnotations;

namespace DataPersistenceLayer.Entities
{
    public class OfficeOfAcceptance
    {
        [Key]
        public int IdOfAcceptance { get; set; }
        public DateTime DateOfAcceptance { get; set; }
        public string RouteSave { get; set; }
    }
}
