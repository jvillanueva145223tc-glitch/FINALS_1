using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.DataAccess
{
    /* FacultyLoad-specific repository interface */
    public interface IFacultyLoadRepository : IRepository<FacultyLoad>
    {
        FacultyLoad GetByFacultyAndTerm(int facultyId, int termId);
        bool UpdateLoad(int facultyId, int termId, decimal units);
    }
}
