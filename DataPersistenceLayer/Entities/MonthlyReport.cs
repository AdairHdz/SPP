using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataPersistenceLayer.Entities
{
    public class MonthlyReport
    {
        [Key]
        public int IdMonthlyReport { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string PerformedActivities { get; set; }
        public string resultsObtained { get; set; }

        public string Enrollment { get; set; }
        
        [ForeignKey("Enrollment")]
        public Practicioner Practicioner { get; set; }

        public int IdProject { get; set; }
        [ForeignKey("IdProject")]
        public Project Project { get; set; }
    }
}
