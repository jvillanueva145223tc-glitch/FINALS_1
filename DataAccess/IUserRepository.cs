using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.DataAccess
{
    /* User-specific repository interface */
    public interface IUserRepository : IRepository<User>
    {
        User Authenticate(string username, string password);
        User GetByUsername(string username);
        List<User> GetByRole(string role);
    }
}
