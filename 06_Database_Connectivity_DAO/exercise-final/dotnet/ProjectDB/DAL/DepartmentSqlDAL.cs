using ProjectDB.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDB.DAL
{
    public class DepartmentSqlDAL
    {
        private string connectionString;
        private const string SQL_SelectAllDepartments = "SELECT * FROM department;";
        private const string SQL_GetLastDepartmentId = "SELECT MAX(department_id) FROM department;";
        private const string SQL_UpdateDepartment = "UPDATE department SET name = @name WHERE department_id = @id;";
        private const string SQL_InsertDepartment = "INSERT INTO department VALUES (@name);";


        // Single Parameter Constructor
        public DepartmentSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns a list of all of the departments.
        /// </summary>
        /// <returns></returns>
        public IList<Department> GetDepartments()
        {
            List<Department> output = new List<Department>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_SelectAllDepartments, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Department d = GetDepartmentFromReader(reader);
                        output.Add(d);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }

            return output;
        }

        /// <summary>
        /// Creates a new department.
        /// </summary>
        /// <param name="newDepartment">The department object.</param>
        /// <returns>The id of the new department (if successful).</returns>
        public int CreateDepartment(Department newDepartment)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_InsertDepartment, conn);
                    cmd.Parameters.AddWithValue("@name", newDepartment.Name);
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand(SQL_GetLastDepartmentId, conn);                    
                    int newId = Convert.ToInt32(cmd.ExecuteScalar());

                    return newId;
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Updates an existing department.
        /// </summary>
        /// <param name="updatedDepartment">The department object.</param>
        /// <returns>True, if successful.</returns>
        public bool UpdateDepartment(Department updatedDepartment)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_UpdateDepartment, conn);
                    cmd.Parameters.AddWithValue("@name", updatedDepartment.Name);
                    cmd.Parameters.AddWithValue("@id", updatedDepartment.Id);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return (rowsAffected > 0);
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
        }

        private Department GetDepartmentFromReader(SqlDataReader reader)
        {
            Department d = new Department();
            d.Id = Convert.ToInt32(reader["department_id"]);
            d.Name = Convert.ToString(reader["name"]);

            return d;
        }
    }
}
