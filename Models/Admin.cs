using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Models
{
    // admin class
    public class Admin : User
    {
        public Admin()
        {
            Role = "Admin";
        }
        public override string GetDashboardMessage()
        {
            return "Welcome Admin! You have full system access.";
        }
        public override string GetUserInfo()
        {
            return $"Administrator: {FullName}";
        }
    }
}
