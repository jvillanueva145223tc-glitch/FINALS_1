using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Models
{
    //time slot class
    public class TimeSlot
    {
        public int TimeSlotId { get; set; }
        public string DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime CreatedAt { get; set; }

        public override string ToString()
        {
            return $"{DayOfWeek} {StartTime:hh\\:mm} - {EndTime:hh\\:mm}";
        }
    }
}