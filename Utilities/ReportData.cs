using System.Collections.Generic;

namespace Utilities
{
    public class ReportData
    {
        public string SectorName { get; set; }
        public List<StudentData> Students { get; set; } = new List<StudentData>();
    }
}