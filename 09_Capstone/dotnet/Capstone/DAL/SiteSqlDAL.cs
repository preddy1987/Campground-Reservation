using System;
using System.Collections.Generic;
using System.Text;
using NatParkCampRes.Models;
using System.Data.SqlClient;
using System.Globalization;

namespace NatParkCampRes.DAL
{
    public class SiteSqlDAL
    {

        #region Constants
        private const string SqlSelectAvailableSitesInCampground = "select TOP 5 * from site LEFT OUTER JOIN( " +
                                                                        "SELECT DISTINCT q.site_number as site_conflict FROM (" +
                                                                        "SELECT site_number, " +
                                                                        "   site.site_id,          " +
                                                                        "   site.campground_id,    " +
                                                                        "   site.max_occupancy,    " +
                                                                        "   site.accessible,       " +
                                                                        "   site.max_rv_length,    " +
                                                                        "   site.utilities,        " +
                                                                        "   reservation.from_date, " +
                                                                        "   reservation.to_date   " +
                                                                        "FROM site JOIN reservation ON site.site_id = reservation.site_id " +
                                                                        "WHERE (campground_id = @CampId " +
                                                                        "  AND " +
                                                                        "   ( "+
                                                                        "       (@Arrive >= reservation.from_date AND @Arrive < reservation.to_date) " +
                                                                        "           OR " +
                                                                        "       (@Depart > reservation.from_date AND  @Depart < reservation.to_date) " +
                                                                        "           OR " +
                                                                        "       (@Arrive < reservation.from_date AND @Depart >= reservation.to_date) " +
                                                                        "   )" +
                                                                        "   ))" +
                                                                        "   as q)" +
                                                                        "   as r " +
                                                                        "ON site.site_number = r.site_conflict " +
                                                                        " WHERE campground_id = @CampId AND r.site_conflict IS NULL; ";
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
        /// Find a list of available site in a campground in a park
        /// </summary>
        /// <param name="campground_id"></param>
        /// <param name="arrival">DateTime Start of requested reservation</param>
        /// <param name="departure">DateTime End of requested reservation</param>
        /// <returns></returns>
        public List<Site> GetAvailalableSitesInCampground(int campground_id, DateTime arrival, DateTime departure)
        {
            List<Site> output = new List<Site>();

            //Always wrap connection to a database in a try-catch block
            try
            {
                //Create a SqlConnection to our database
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(SqlSelectAvailableSitesInCampground, connection);
                    cmd.Parameters.AddWithValue("@CampId", campground_id);
                    cmd.Parameters.AddWithValue("@Arrive", arrival.ToString("d", CultureInfo.CreateSpecificCulture("en-US")));
                    cmd.Parameters.AddWithValue("@Depart", departure.ToString("d", CultureInfo.CreateSpecificCulture("en-US")));
                    //Console.WriteLine($"departure: { arrival.ToString("d", CultureInfo.CreateSpecificCulture("en-US"))}");
                    //Console.WriteLine($"departure: { departure.ToString("d", CultureInfo.CreateSpecificCulture("en-US"))}");
                    //Console.WriteLine($" command text: {cmd.CommandText}");
                    // Execute the query to the database
                    SqlDataReader reader = cmd.ExecuteReader();

                    // The results come back as a SqlDataReader. Loop through each of the rows
                    // and add to the output list
                    while (reader.Read())
                    {
                        // Read in the value from the reader
                        // Reference by index or by column_name
                        Site siteRes = new Site();
                        siteRes.SiteId = Convert.ToInt32(reader["site_id"]);
                        siteRes.CampgroundId = Convert.ToInt32(reader["campground_id"]);
                        siteRes.SiteNumber = Convert.ToInt32(reader["site_number"]);
                        siteRes.MaxOccupants = Convert.ToInt32(reader["max_occupancy"]);
                        siteRes.IsAccessible = Convert.ToBoolean(reader["accessible"]);
                        siteRes.MaxRvLength = Convert.ToInt32(reader["max_rv_length"]);
                        siteRes.HasUtilities = Convert.ToBoolean(reader["utilities"]);
                        if (reader.IsDBNull(reader.GetOrdinal("site_conflict")))
                        {
                            output.Add(siteRes);
                        }
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
