using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Models
{
    //faculty load class
    public class FacultyLoad
    {
        public int LoadId { get; set; }
        public int FacultyId { get; set; }
        public int TermId { get; set; }
        public decimal TotalUnits { get; set; }
        public bool IsOverload { get; set; }
        public bool OverloadApproved { get; set; }
        public DateTime CreatedAt { get; set; }

        //checks if faculty can take more units
        public bool CanAddUnits(decimal additionalUnits, decimal maxUnits)
        {
            return (TotalUnits + additionalUnits) <= maxUnits;
        }
    }
}
