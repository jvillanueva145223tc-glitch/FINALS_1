using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.DataAccess
{
    /* TimeSlot-specific repository interface */
    public interface ITimeSlotRepository : IRepository<TimeSlot>
    {
        List<TimeSlot> GetByDay(string dayOfWeek);
        bool HasOverlap(TimeSlot timeSlot);
    }

}
