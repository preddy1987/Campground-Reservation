using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using NatParkCampRes.Models;

namespace NatParkCampRes.DAL
{
   public class ParksSqlDAL
    {
        #region Constants
        private const string SqlSelectAllParks = "SELECT * FROM park;";
        #endregion

        #region Member Variables
        private string _connectionString;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbConnectionString"></param>
        public ParksSqlDAL(string dbConnectionString)
        {
            _connectionString = dbConnectionString;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a list of all of the parks.
        /// </summary>
        /// <returns></returns>
        public IList<Park> GetAllParks()
        {
            List<Park> output = new List<Park>();

            //Always wrap connection to a database in a try-catch block
            try
            {
                //Create a SqlConnection to our database
                using (SqlConnection connection = new SqlConnection(_connectionString))
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
        /// <summary>
        /// Returns a list of all of the parks.
        /// </summary>
        /// <returns></returns>
        public Park GetaPark()
        {
            Park output = new Park();

            //Always wrap connection to a database in a try-catch block
            try
            {
                //Create a SqlConnection to our database
                using (SqlConnection connection = new SqlConnection(_connectionString))
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
                        //output.Add(park);
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
        #endregion
    }
}
