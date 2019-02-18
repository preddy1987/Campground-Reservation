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
        private const string SqlSelectAllDepartments = "SELECT * FROM department;";
        private const string SqlGetLastDepartmentId = "SELECT MAX(department_id) FROM department;";
        private const string SqlUpdateDepartment = "UPDATE department SET name = @name WHERE department_id = @id;";
        private const string SqlInsertDepartment = "INSERT INTO department VALUES (@name);";

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

            //Always wrap connection to a database in a try-catch block
            try
            {
                //Create a SqlConnection to our database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();                   
                    SqlCommand cmd = new SqlCommand(SqlSelectAllDepartments, connection);

                    // Execute the query to the database
                    SqlDataReader reader = cmd.ExecuteReader();

                    // The results come back as a SqlDataReader. Loop through each of the rows
                    // and add to the output list
                    while (reader.Read())
                    {
                        // Read in the value from the reader
                        // Reference by index or by column_name
                        Department department = new Department();
                        department.Id = Convert.ToInt32(reader["department_id"]);
                        department.Name = Convert.ToString(reader["name"]);

                        // Add the department to the output list                       
                        output.Add(department);
                    }
                }
            }
            catch (SqlException ex)
            {
                // A SQL Exception Occurred. Log and throw to our application!!
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
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(SqlInsertDepartment, connection);
                    cmd.Parameters.AddWithValue("@name", newDepartment.Name);
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand(SqlGetLastDepartmentId, connection);
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
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //private const string SqlUpdateDepartment = "UPDATE department SET name = @name WHERE department_id = @id;";
                    SqlCommand cmd = new SqlCommand(SqlUpdateDepartment, connection);
                    cmd.Parameters.AddWithValue("@name", updatedDepartment.Name);
                    cmd.Parameters.AddWithValue("@id", updatedDepartment.Id);


                    cmd.ExecuteNonQuery();

                    //cmd = new SqlCommand(SqlGetLastDepartmentId, connection);
                    //int newId = Convert.ToInt32(cmd.ExecuteScalar());

                    return true;
                }
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

    }
}
