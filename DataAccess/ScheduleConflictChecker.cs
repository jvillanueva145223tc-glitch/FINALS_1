using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.DataAccess
{
    /// <summary>
    /// Service class for checking schedule conflicts
    /// Demonstrates Strategy Pattern and Single Responsibility Principle
    /// </summary>
    public class ScheduleConflictChecker
    {
        private readonly DatabaseConnection _dbConnection;

        public ScheduleConflictChecker()
        {
            _dbConnection = DatabaseConnection.Instance;
        }

        /// <summary>
        /// Checks for all types of conflicts for a given schedule
        /// </summary>
        /// <param name="schedule">Schedule to check</param>
        /// <param name="conflictMessages">Output list of conflict messages</param>
        /// <returns>True if conflicts exist, false otherwise</returns>
        public bool HasConflicts(Schedule schedule, out List<string> conflictMessages)
        {
            conflictMessages = new List<string>();
            bool hasConflict = false;

            // Check room conflict
            if (HasRoomConflict(schedule))
            {
                conflictMessages.Add($"Room conflict: Room {schedule.RoomNumber} is already occupied at this time slot.");
                hasConflict = true;
            }

            // Check faculty conflict
            if (HasFacultyConflict(schedule))
            {
                conflictMessages.Add($"Faculty conflict: This faculty member is already assigned to another class at this time slot.");
                hasConflict = true;
            }

            // Check room capacity
            if (HasCapacityIssue(schedule))
            {
                conflictMessages.Add($"Capacity issue: Student count exceeds room capacity.");
                hasConflict = true;
            }

            // Check room type compatibility
            if (!IsRoomTypeCompatible(schedule))
            {
                conflictMessages.Add($"Room type mismatch: Schedule type ({schedule.ScheduleType}) doesn't match room type.");
                hasConflict = true;
            }

            return hasConflict;
        }

        /// <summary>
        /// Checks if a room is already occupied at the given time slot
        /// </summary>
        private bool HasRoomConflict(Schedule schedule)
        {
            try
            {
                using (var connection = _dbConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"SELECT COUNT(*) FROM schedules 
                                    WHERE room_id = @roomId 
                                    AND time_slot_id = @timeSlotId 
                                    AND term_id = @termId 
                                    AND schedule_id != @scheduleId";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@roomId", schedule.RoomId);
                        cmd.Parameters.AddWithValue("@timeSlotId", schedule.TimeSlotId);
                        cmd.Parameters.AddWithValue("@termId", schedule.TermId);
                        cmd.Parameters.AddWithValue("@scheduleId", schedule.ScheduleId);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking room conflict: {ex.Message}");
            }
        }

        /// <summary>
        /// Checks if a faculty is already teaching at the given time slot
        /// </summary>
        private bool HasFacultyConflict(Schedule schedule)
        {
            try
            {
                using (var connection = _dbConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"SELECT COUNT(*) FROM schedules 
                                    WHERE faculty_id = @facultyId 
                                    AND time_slot_id = @timeSlotId 
                                    AND term_id = @termId 
                                    AND schedule_id != @scheduleId";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@facultyId", schedule.FacultyId);
                        cmd.Parameters.AddWithValue("@timeSlotId", schedule.TimeSlotId);
                        cmd.Parameters.AddWithValue("@termId", schedule.TermId);
                        cmd.Parameters.AddWithValue("@scheduleId", schedule.ScheduleId);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking faculty conflict: {ex.Message}");
            }
        }

        /// <summary>
        /// Checks if student count exceeds room capacity
        /// </summary>
        private bool HasCapacityIssue(Schedule schedule)
        {
            try
            {
                using (var connection = _dbConnection.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT capacity FROM rooms WHERE room_id = @roomId";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@roomId", schedule.RoomId);

                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            int capacity = Convert.ToInt32(result);
                            return schedule.StudentCount > capacity;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking capacity: {ex.Message}");
            }
        }

        /// <summary>
        /// Checks if the room type is compatible with the schedule type
        /// </summary>
        private bool IsRoomTypeCompatible(Schedule schedule)
        {
            try
            {
                using (var connection = _dbConnection.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT room_type FROM rooms WHERE room_id = @roomId";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@roomId", schedule.RoomId);

                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            string roomType = result.ToString();

                            // Room type "Both" is compatible with any schedule type
                            if (roomType == "Both")
                                return true;

                            // Otherwise, room type must match schedule type
                            return roomType == schedule.ScheduleType;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking room type: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets detailed conflict information for display
        /// </summary>
        public List<ScheduleConflict> GetConflictDetails(Schedule schedule)
        {
            var conflicts = new List<ScheduleConflict>();

            try
            {
                using (var connection = _dbConnection.GetConnection())
                {
                    connection.Open();

                    // Check room conflicts with details
                    string roomQuery = @"SELECT s.schedule_id, sub.subject_code, sub.subject_title, 
                                        f.full_name as faculty_name, r.room_number, ts.day_of_week, 
                                        ts.start_time, ts.end_time
                                        FROM schedules s
                                        INNER JOIN subjects sub ON s.subject_id = sub.subject_id
                                        INNER JOIN users f ON s.faculty_id = f.user_id
                                        INNER JOIN rooms r ON s.room_id = r.room_id
                                        INNER JOIN time_slots ts ON s.time_slot_id = ts.time_slot_id
                                        WHERE s.room_id = @roomId 
                                        AND s.time_slot_id = @timeSlotId 
                                        AND s.term_id = @termId 
                                        AND s.schedule_id != @scheduleId";

                    using (var cmd = new MySqlCommand(roomQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@roomId", schedule.RoomId);
                        cmd.Parameters.AddWithValue("@timeSlotId", schedule.TimeSlotId);
                        cmd.Parameters.AddWithValue("@termId", schedule.TermId);
                        cmd.Parameters.AddWithValue("@scheduleId", schedule.ScheduleId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                conflicts.Add(new ScheduleConflict
                                {
                                    ConflictType = "Room",
                                    ConflictDescription = $"Room {reader.GetString("room_number")} is already occupied by " +
                                                        $"{reader.GetString("subject_code")} ({reader.GetString("faculty_name")}) " +
                                                        $"on {reader.GetString("day_of_week")} {reader.GetTimeSpan("start_time")}-{reader.GetTimeSpan("end_time")}",
                                    DetectedAt = DateTime.Now
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting conflict details: {ex.Message}");
            }

            return conflicts;
        }

        /// <summary>
        /// Logs a conflict to the database
        /// </summary>
        public bool LogConflict(ScheduleConflict conflict)
        {
            try
            {
                using (var connection = _dbConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"INSERT INTO schedule_conflicts 
                                    (schedule_id, conflict_type, conflict_description) 
                                    VALUES 
                                    (@scheduleId, @type, @description)";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@scheduleId", conflict.ScheduleId ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@type", conflict.ConflictType);
                        cmd.Parameters.AddWithValue("@description", conflict.ConflictDescription);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error logging conflict: {ex.Message}");
            }
        }
    }
}