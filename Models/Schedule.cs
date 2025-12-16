using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Models
{
    //schedule class
    public class Schedule
    {
        public int ScheduleId { get; set; }
        public int SubjectId { get; set; }
        public int FacultyId { get; set; }
        public int RoomId { get; set; }
        public int TimeSlotId { get; set; }
        public int TermId { get; set; }
        public string Section { get; set; }
        public int StudentCount { get; set; }
        public string ScheduleType { get; set; } // Regular, Lab, etc.
        public DateTime CreatedAt { get; set; }

        //navigation prop. (display purposes)

        public string SubjectCode { get; set; }
        public string SubjectTitle { get; set; }
        public string FacultyName { get; set; }
        public string RoomNumber { get; set; }
        public string TimeSlotDisplay { get; set; }
    }
}
