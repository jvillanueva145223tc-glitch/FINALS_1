using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Models
{
    //room class
    public class Room
    {
        private int _capacity;

        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public int Capacity
        {
            get { return _capacity; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Capacity must be greater than zero.");
                _capacity = value;
            }
        }
        public string RoomType { get; set; } // Lecture, Laboratory, Both
        public string Building { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }

        public override string ToString()
        {
            return $"{RoomNumber} ({RoomType}) - Capacity: {Capacity}";
        }
    }
}