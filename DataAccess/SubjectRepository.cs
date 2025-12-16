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
    /// Repository implementation for Subject entity
    /// Demonstrates SOLID principles (Single Responsibility, Open/Closed)
    /// </summary>
    public class SubjectRepository : ISubjectRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public SubjectRepository()
        {
            _dbConnection = DatabaseConnection.Instance;
        }

        /// <summary>
        /// Adds a new subject to the database
        /// </summary>
        public bool Add(Subject subject)
        {
            try
            {
                using (var connection = _dbConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"INSERT INTO subjects 
                                    (subject_code, subject_title, units, lecture_hours, 
                                     lab_hours, department_id, program) 
                                    VALUES 
                                    (@code, @title, @units, @lecture, @lab, @dept, @program)";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@code", subject.SubjectCode);
                        cmd.Parameters.AddWithValue("@title", subject.SubjectTitle);
                        cmd.Parameters.AddWithValue("@units", subject.Units);
                        cmd.Parameters.AddWithValue("@lecture", subject.LectureHours);
                        cmd.Parameters.AddWithValue("@lab", subject.LabHours);
                        cmd.Parameters.AddWithValue("@dept", subject.DepartmentId);
                        cmd.Parameters.AddWithValue("@program", subject.Program ?? (object)DBNull.Value);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding subject: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing subject
        /// </summary>
        public bool Update(Subject subject)
        {
            try
            {
                using (var connection = _dbConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"UPDATE subjects SET 
                                    subject_code = @code,
                                    subject_title = @title,
                                    units = @units,
                                    lecture_hours = @lecture,
                                    lab_hours = @lab,
                                    department_id = @dept,
                                    program = @program
                                    WHERE subject_id = @id";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", subject.SubjectId);
                        cmd.Parameters.AddWithValue("@code", subject.SubjectCode);
                        cmd.Parameters.AddWithValue("@title", subject.SubjectTitle);
                        cmd.Parameters.AddWithValue("@units", subject.Units);
                        cmd.Parameters.AddWithValue("@lecture", subject.LectureHours);
                        cmd.Parameters.AddWithValue("@lab", subject.LabHours);
                        cmd.Parameters.AddWithValue("@dept", subject.DepartmentId);
                        cmd.Parameters.AddWithValue("@program", subject.Program ?? (object)DBNull.Value);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating subject: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a subject by ID
        /// </summary>
        public bool Delete(int id)
        {
            try
            {
                using (var connection = _dbConnection.GetConnection())
                {
                    connection.Open();
                    string query = "DELETE FROM subjects WHERE subject_id = @id";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting subject: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets a subject by ID
        /// </summary>
        public Subject GetById(int id)
        {
            try
            {
                using (var connection = _dbConnection.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM subjects WHERE subject_id = @id";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapReaderToSubject(reader);
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting subject: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets all subjects
        /// </summary>
        public List<Subject> GetAll()
        {
            var subjects = new List<Subject>();

            try
            {
                using (var connection = _dbConnection.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM subjects ORDER BY subject_code";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                subjects.Add(MapReaderToSubject(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting all subjects: {ex.Message}");
            }

            return subjects;
        }

        /// <summary>
        /// Gets a subject by subject code
        /// </summary>
        public Subject GetByCode(string subjectCode)
        {
            try
            {
                using (var connection = _dbConnection.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM subjects WHERE subject_code = @code";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@code", subjectCode);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapReaderToSubject(reader);
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting subject by code: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets subjects by department
        /// </summary>
        public List<Subject> GetByDepartment(int departmentId)
        {
            var subjects = new List<Subject>();

            try
            {
                using (var connection = _dbConnection.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM subjects WHERE department_id = @deptId ORDER BY subject_code";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@deptId", departmentId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                subjects.Add(MapReaderToSubject(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting subjects by department: {ex.Message}");
            }

            return subjects;
        }

        /// <summary>
        /// Searches subjects by keyword (code or title)
        /// </summary>
        public List<Subject> Search(string keyword)
        {
            var subjects = new List<Subject>();

            try
            {
                using (var connection = _dbConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"SELECT * FROM subjects 
                                    WHERE subject_code LIKE @keyword 
                                    OR subject_title LIKE @keyword 
                                    ORDER BY subject_code";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@keyword", $"%{keyword}%");

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                subjects.Add(MapReaderToSubject(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching subjects: {ex.Message}");
            }

            return subjects;
        }

        /// <summary>
        /// Helper method to map MySqlDataReader to Subject object
        /// </summary>
        private Subject MapReaderToSubject(MySqlDataReader reader)
        {
            return new Subject
            {
                SubjectId = reader.GetInt32("subject_id"),
                SubjectCode = reader.GetString("subject_code"),
                SubjectTitle = reader.GetString("subject_title"),
                Units = reader.GetDecimal("units"),
                LectureHours = reader.GetInt32("lecture_hours"),
                LabHours = reader.GetInt32("lab_hours"),
                DepartmentId = reader.GetInt32("department_id"),
                Program = reader.IsDBNull(reader.GetOrdinal("program")) ? null : reader.GetString("program"),
                CreatedAt = reader.GetDateTime("created_at")
            };
        }
    }
}
