using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.DataAccess
{
    /* Faculty-specific repository interface */
    public interface IFacultyRepository : IRepository<Faculty>
    {
        Faculty GetByUserId(int userId);
        List<Faculty> GetByDepartment(int departmentId);
        decimal GetTotalUnits(int facultyId, int termId);
    }
}
