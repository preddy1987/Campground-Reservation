using System;
using System.Collections.Generic;
using System.Text;
using NatParkCampRes.Models;
using System.Data.SqlClient;

namespace NatParkCampRes.DAL
{
    public class CampgroundSqlDAL
    {
        #region Constants
        private const string SqlSelectAllCampgrounds = "SELECT * FROM campground;";
        private const string SqlSelectCampgroundsInPark = "Select * FROM campground where campground.park_id = @ParkId;";
        #endregion

        #region Member Variables
        //private const string _getLastIdSQL = "SELECT CAST(SCOPE_IDENTITY() as int);";
        private string _connectionString;
        #endregion

        #region Constructor
        /// <summary>
        /// Campground DAL Constructor
        /// </summary>
        /// <remarks> </remarks>
        /// <param name="connectionString"></param>
        public CampgroundSqlDAL(string connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Find all campgrounds within a park.
        /// </summary>
        /// <param name="park">Park object</param>
        /// <returns>List of Campground objects in Park</returns>
        public List<Campground> GetCampgroundsInPark(Park park)
        {
            List<Campground> output = new List<Campground>();

            //Always wrap connection to a database in a try-catch block
            try
            {
                //Create a SqlConnection to our database
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(SqlSelectCampgroundsInPark, connection);
                    cmd.Parameters.AddWithValue("@ParkId", park.ParkId);
                    // Execute the query to the database
                    SqlDataReader reader = cmd.ExecuteReader();
                    

                    // The results come back as a SqlDataReader. Loop through each of the rows
                    // and add to the output list
                    while (reader.Read())
                    {
                        // Read in the value from the reader
                        // Reference by index or by column_name
                        Campground campground = new Campground();
                        campground.CampgroundId = Convert.ToInt32(reader["campground_id"]);
                        campground.ParkId = Convert.ToInt32(reader["park_id"]);
                        campground.Name = Convert.ToString(reader["name"]);
                        campground.ParkId = Convert.ToInt32(reader["park_id"]);
                        campground.OpenFromMonth = Convert.ToInt32(reader["open_from_mm"]);
                        campground.OpenToMonth = Convert.ToInt32(reader["open_to_mm"]);
                        campground.DailyFee = Convert.ToDecimal(reader["daily_fee"]);


                        // Add the department to the output list                       
                        output.Add(campground);
                    }
                }
                return output;
            }
            catch (SqlException ex)
            {
                // A SQL Exception Occurred. Log and throw to our application!!
                throw;
            }
        }
        #endregion
    }
}
