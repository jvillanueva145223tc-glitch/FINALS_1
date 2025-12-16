using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.DataAccess
{
    /* Generic repository interface */
    public interface IRepository<T> where T : class
    {
        bool Add(T entity);
        bool Update(T entity);
        bool Delete(int id);
        T GetById(int id);
        List<T> GetAll();
    }
}
