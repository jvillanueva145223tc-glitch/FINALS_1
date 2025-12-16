using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.DataAccess
{
    /* Schedule-specific repository interface */
    public interface IScheduleRepository : IRepository<Schedule>
    {
        List<Schedule> GetByTerm(int termId);
        List<Schedule> GetByFaculty(int facultyId, int termId);
        List<Schedule> GetByRoom(int roomId, int termId);
        List<Schedule> GetByTimeSlot(int timeSlotId, int termId);
        bool HasConflict(Schedule schedule);
    }
}
