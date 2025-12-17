using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Models
{
    /// <summary>
    /// Schedule Conflict entity
    /// </summary>
    public class ScheduleConflict
    {
        public int ConflictId { get; set; }
        public int? ScheduleId { get; set; }
        public string ConflictType { get; set; } // Room, Faculty, Student
        public string ConflictDescription { get; set; }
        public DateTime DetectedAt { get; set; }
        public bool Resolved { get; set; }

        public override string ToString()
        {
            return $"{ConflictType} Conflict: {ConflictDescription}";
        }
    }
}
