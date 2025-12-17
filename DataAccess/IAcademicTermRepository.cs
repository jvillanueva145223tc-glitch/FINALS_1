using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.DataAccess
{
    /* AcademicTerm-specific repository interface */
    public interface IAcademicTermRepository : IRepository<AcademicTerm>
    {
        AcademicTerm GetActiveTerm();
        List<AcademicTerm> GetByYear(int year);
    }
}
