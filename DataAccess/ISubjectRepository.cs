using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.DataAccess
{
    public interface ISubjectRepository : IRepository<Subject>
    {
        Subject GetByCode(string subjectCode);
        List<Subject> GetByDepartment(int departmentId);
        List<Subject> Search(string keyword);
    }
}
