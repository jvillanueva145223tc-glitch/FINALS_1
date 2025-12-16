using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Models
{
    // faculty class
    public class Faculty : User
    {
        public int FacultyId { get; set; }
        public string EmployeeId { get; set; }
        public int DepartmentId { get; set; }
        public decimal MaxUnits { get; set; }
        public string Specialization { get; set; }

        public Faculty()
        {
            Role = "Faculty";
            MaxUnits = 26.0m;
        }
        public override string GetDashboardMessage()
        {
            return $"Welcome {FullName}! View your teaching schedule and assignments.";
        }
        public override string GetUserInfo()
        {
            return $"Faculty: {FullName} - {Specialization}";
        }
    }
}
