using System;
using System.Collections.Generic;
using System.Text;
using NatParkCampRes.Models;
using System.Data.SqlClient;

namespace NatParkCampRes.DAL
{
    class ParkSiteSearchSqlDAL
    {
        #region Constants
        private const string SqlSelectAllParkSites = "SELECT campground.name,campground.daily_fee,site.site_id,site.site_number," +
                                                        "site.max_occupancy,site.accessible,site.max_rv_length,site.utilities " +
                                                        " FROM campground JOIN site ON campground.campground_id = site.campground_id " +
                                                        "WHERE campground.park_id = @ParkId ;";
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
        public ParkSiteSearchSqlDAL(string connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<SiteReservation> GetAllSitesInPark(Park park)
        {
            List<SiteReservation> output = new List<SiteReservation>();

            //Always wrap connection to a database in a try-catch block
            try
            {
                //Create a SqlConnection to our database
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(SqlSelectAllParkSites, connection);
                    cmd.Parameters.AddWithValue("@ParkId", park.ParkId);

                    // Execute the query to the database
                    SqlDataReader reader = cmd.ExecuteReader();

                    // The results come back as a SqlDataReader. Loop through each of the rows
                    // and add to the output list
                    while (reader.Read())
                    {
                        // Read in the value from the reader
                        // Reference by index or by column_name
                        SiteReservation siteRes = new SiteReservation();
//                        siteRes.R = Convert.ToString(reader["campground_name"]);
//                        siteRes.DailyFee = Convert.ToDecimal(reader["daily_fee"]);
                        siteRes.SiteId = Convert.ToInt32(reader["site_id"]);
                        siteRes.SiteNumber = Convert.ToInt32(reader["site_number"]);
                        siteRes.MaxOccupants = Convert.ToInt32(reader["max_occupancy"]);
                        siteRes.IsAccessible = Convert.ToBoolean(reader["accessible"]);
                        siteRes.MaxRvLength = Convert.ToInt32(reader["max_rv_length"]);
                        siteRes.HasUtilities = Convert.ToBoolean(reader["utilities"]);

                        // Add the department to the output list                       
                        output.Add(siteRes);
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
