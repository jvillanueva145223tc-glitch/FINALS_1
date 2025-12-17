using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.DataAccess
{
    /* Room-specific repository interface */
    public interface IRoomRepository : IRepository<Room>
    {
        Room GetByRoomNumber(string roomNumber);
        List<Room> GetAvailableRooms();
        List<Room> GetByType(string roomType);
    }
}
