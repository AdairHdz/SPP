
using System.Collections.Generic;

namespace Utilities
{
    public class PartialReportTemplate
    {
        public string Career { get; set; }
        public string Techaer { get; set; }
        public string NRC { get; set; }
        public string Term { get; set; }
        public string Practicioner { get; set; }
        public string Project { get; set; }
        public string LinkedOrganization { get; set; }
        public string Date { get; set; }
        public string Hours { get; set; }
        public string Number { get; set; }
        public string GeneralObjective { get; set; }
        public string Methodology { get; set; }
        public string ActivityOne { get; set; }
        public string ActivityTwo { get; set; }
        public string ActivityThree { get; set; }
        public string ActivityFour { get; set; }
        public string ActivityFive { get; set; }
        public string Observations { get; set; }
        public string Result { get; set; }

        public List<CheckListItem> WeekPlan1 { get; set; }
        public List<CheckListItem> WeekPlan2 { get; set; }
        public List<CheckListItem> WeekPlan3 { get; set; }
        public List<CheckListItem> WeekPlan4 { get; set; }
        public List<CheckListItem> WeekPlan5 { get; set; }
        public List<CheckListItem> WeekReal1 { get; set; }
        public List<CheckListItem> WeekReal2 { get; set; }
        public List<CheckListItem> WeekReal3 { get; set; }
        public List<CheckListItem> WeekReal4 { get; set; }
        public List<CheckListItem> WeekReal5 { get; set; }
    }
}
