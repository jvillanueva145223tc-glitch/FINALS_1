using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Models
{
    //registrar class
    public class Registrar : User
    {
        public Registrar()
        {
            Role = "Registrar";
        }
        public override string GetDashboardMessage()
        {
            return $"Welcome {FullName}! Manage schedules and room assignments.";
        }
    }
}
