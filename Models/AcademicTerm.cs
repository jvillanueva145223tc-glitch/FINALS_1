using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Models
{

    //academic term class
    public class AcademicTerm
    {
        public int TermId { get; set; }
        public int Year { get; set; }
        public string Semester { get; set; } //1st, 2nd, summer
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public override string ToString()
        {
            return $"{Semester} Semester {Year}";
        }
    }
}
