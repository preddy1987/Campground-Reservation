using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using NatParkCampRes.Models;

namespace NatParkCampRes.DAL
{
    class ParksSqlDAL
    {
        private string connectionString;
        private const string SqlSelectAllParks = "SELECT * FROM park;";
        //private const string SqlGetLastDepartmentId = "SELECT MAX(department_id) FROM department;";
        //private const string SqlUpdateDepartment = "UPDATE department SET name = @name WHERE department_id = @id;";
        //private const string SqlInsertDepartment = "INSERT INTO department VALUES (@name);";
        //private const string SqlGetADepartment = "Select * from department where department_id = @id;";

        // Single Parameter Constructor
        public ParksSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        /// <summary>
        /// Returns a list of all of the departments.
        /// </summary>
        /// <returns></returns>
        public IList<Park> GetDepartments()
        {
            List<Park> output = new List<Park>();

            //Always wrap connection to a database in a try-catch block
            try
            {
                //Create a SqlConnection to our database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(SqlSelectAllParks, connection);

                    // Execute the query to the database
                    SqlDataReader reader = cmd.ExecuteReader();

                    // The results come back as a SqlDataReader. Loop through each of the rows
                    // and add to the output list
                    while (reader.Read())
                    {
                        // Read in the value from the reader
                        // Reference by index or by column_name
                        Park park = new Park();
                        park.Id = Convert.ToInt32(reader["park_id"]);
                        park.Name = Convert.ToString(reader["name"]);
                        park.Location = Convert.ToString(reader["location"]);
                        park.EstablishDate = Convert.ToDateTime(reader["establish_date"]);
                        park.Area = Convert.ToInt32(reader["area"]);
                        park.Visitors = Convert.ToInt32(reader["visitors"]);
                        park.Desc = Convert.ToString(reader["description"]);

                        // Add the department to the output list                       
                        output.Add(park);
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
    }
}
