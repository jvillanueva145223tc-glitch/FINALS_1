using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.DataAccess
{
    /* Department-specific repository interface */
    public interface IDepartmentRepository : IRepository<Department>
    {
        Department GetByCode(string departmentCode);
    }
}
