using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Models
{
<<<<<<< HEAD
=======
    //user base class
>>>>>>> 366ca0d1bac7ac1540849da5b0363a8cad3b4735
    public abstract class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
<<<<<<< HEAD
        public string PasswordHash { get; set; }
        public string Role { get; protected set; }
    }
}
    
=======
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public abstract string GetDashboardMessage();

        public virtual string GetUserInfo()
        {
            return $"{FullName} {Role}";
        }

    }
}
>>>>>>> 366ca0d1bac7ac1540849da5b0363a8cad3b4735
