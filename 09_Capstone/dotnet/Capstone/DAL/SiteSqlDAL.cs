using System;
using System.Collections.Generic;
using System.Text;
using NatParkCampRes.Models;
using System.Data.SqlClient;

namespace NatParkCampRes.DAL
{
    class SiteSqlDAL
    {

        #region Constants
        private const string SqlSelectAllSites = "SELECT * FROM reservation;";
        #endregion

        #region Member Variables
        //private const string _getLastIdSQL = "SELECT CAST(SCOPE_IDENTITY() as int);";
        private string _connectionString;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public SiteSqlDAL(string connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<Site> GetAllSites()
        {
            List<Site> output = new List<Site>();

            //Always wrap connection to a database in a try-catch block
            try
            {
                //Create a SqlConnection to our database
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(SqlSelectAllSites, connection);

                    // Execute the query to the database
                    SqlDataReader reader = cmd.ExecuteReader();

                    // The results come back as a SqlDataReader. Loop through each of the rows
                    // and add to the output list
                    while (reader.Read())
                    {
                        // Read in the value from the reader
                        // Reference by index or by column_name
                        Site site = new Site();
                        site.SiteId = Convert.ToInt32(reader["site_id"]);
                        site.CampgroundId = Convert.ToInt32(reader["campground_id"]);
                        site.SiteNumber = Convert.ToInt32(reader["site_number"]);
                        site.MaxOccupants = Convert.ToInt32(reader["max_occupancy"]);
                        site.IsAccessible = Convert.ToBoolean(reader["accessible"]);
                        site.MaxRvLength = Convert.ToInt32(reader["max_rv_length"]);
                        site.IsAccessible = Convert.ToBoolean(reader["utilities"]);

                        // Add the department to the output list                       
                        output.Add(site);
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
