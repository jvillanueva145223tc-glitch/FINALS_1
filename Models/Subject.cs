using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Models
{
    //subject class - encapsulation
    public class Subject
    {
        private decimal _units;

        public int SubjectId { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectTitle { get; set; }

        public decimal Units
        {
            get { return _units; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Units must be greater than zero.");
                _units = value;
            }
        }

        public int LectureHours { get; set; }
        public int LabHours { get; set; }
        public int DepartmentId { get; set; }
        public string Program { get; set; }
        public DateTime CreatedAt { get; set; }

        //calculated property
        public int TotalHours => LectureHours + LabHours;

        public override string ToString()
        {
            return $"{SubjectCode} - {SubjectTitle}";
        }
    }
}
