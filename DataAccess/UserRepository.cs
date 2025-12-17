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
    /// Repository implementation for User entity
    /// Demonstrates Factory Pattern for creating different user types
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public UserRepository()
        {
            _dbConnection = DatabaseConnection.Instance;
        }

        /// <summary>
        /// Authenticates a user
        /// </summary>
        public User Authenticate(string username, string password)
        {
            try
            {
                using (var connection = _dbConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"SELECT * FROM users 
                                    WHERE username = @username 
                                    AND password = @password 
                                    AND is_active = 1";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return CreateUserByRole(reader);
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error authenticating user: {ex.Message}");
            }
        }

        public bool Add(User user)
        {
            try
            {
                using (var connection = _dbConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"INSERT INTO users 
                                    (username, password, full_name, email, role, is_active) 
                                    VALUES 
                                    (@username, @password, @fullname, @email, @role, @active)";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@username", user.Username);
                        cmd.Parameters.AddWithValue("@password", user.Password);
                        cmd.Parameters.AddWithValue("@fullname", user.FullName);
                        cmd.Parameters.AddWithValue("@email", user.Email ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@role", user.Role);
                        cmd.Parameters.AddWithValue("@active", user.IsActive);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding user: {ex.Message}");
            }
        }

        public bool Update(User user)
        {
            try
            {
                using (var connection = _dbConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"UPDATE users SET 
                                    username = @username,
                                    full_name = @fullname,
                                    email = @email,
                                    role = @role,
                                    is_active = @active
                                    WHERE user_id = @id";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", user.UserId);
                        cmd.Parameters.AddWithValue("@username", user.Username);
                        cmd.Parameters.AddWithValue("@fullname", user.FullName);
                        cmd.Parameters.AddWithValue("@email", user.Email ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@role", user.Role);
                        cmd.Parameters.AddWithValue("@active", user.IsActive);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating user: {ex.Message}");
            }
        }

        public bool Delete(int id)
        {
            try
            {
                using (var connection = _dbConnection.GetConnection())
                {
                    connection.Open();
                    string query = "DELETE FROM users WHERE user_id = @id";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting user: {ex.Message}");
            }
        }

        public User GetById(int id)
        {
            try
            {
                using (var connection = _dbConnection.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM users WHERE user_id = @id";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return CreateUserByRole(reader);
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting user: {ex.Message}");
            }
        }

        public User GetByUsername(string username)
        {
            try
            {
                using (var connection = _dbConnection.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM users WHERE username = @username";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@username", username);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return CreateUserByRole(reader);
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting user by username: {ex.Message}");
            }
        }

        public List<User> GetAll()
        {
            var users = new List<User>();

            try
            {
                using (var connection = _dbConnection.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM users ORDER BY full_name";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(CreateUserByRole(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting all users: {ex.Message}");
            }

            return users;
        }

        public List<User> GetByRole(string role)
        {
            var users = new List<User>();

            try
            {
                using (var connection = _dbConnection.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM users WHERE role = @role ORDER BY full_name";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@role", role);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(CreateUserByRole(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting users by role: {ex.Message}");
            }

            return users;
        }

        /// <summary>
        /// Factory method to create appropriate User subclass based on role
        /// Demonstrates Factory Pattern and Polymorphism
        /// </summary>
        private User CreateUserByRole(MySqlDataReader reader)
        {
            string role = reader.GetString("role");
            User user;

            // Factory Pattern - create appropriate user type
            switch (role)
            {
                case "Admin":
                    user = new Admin();
                    break;
                case "Faculty":
                    user = new Faculty();
                    break;
                case "Registrar":
                    user = new Registrar();
                    break;
                default:
                    throw new Exception($"Unknown user role: {role}");
            }

            // Populate common properties
            user.UserId = reader.GetInt32("user_id");
            user.Username = reader.GetString("username");
            user.Password = reader.GetString("password");
            user.FullName = reader.GetString("full_name");
            user.Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString("email");
            user.Role = role;
            user.IsActive = reader.GetBoolean("is_active");
            user.CreatedAt = reader.GetDateTime("created_at");

            return user;
        }
    }
}
