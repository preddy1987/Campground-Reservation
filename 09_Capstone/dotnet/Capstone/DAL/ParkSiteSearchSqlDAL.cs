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
        private const string SqlSelectAllParkSites = "SELECT campground.name,site.site_id,site.site_number,site.max_occupancy," +
                                                        "site.accessible,site.max_rv_length,site.utilities "+
                                                        " FROM campground JOIN site ON campground.campground_id = site.campground_id;";
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
        public IList<Reservation> GetAllSitesInPark(Park park)
        {
            List<Reservation> output = new List<Reservation>();

            //Always wrap connection to a database in a try-catch block
            try
            {
                //Create a SqlConnection to our database
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(SqlSelectAllParkSites, connection);
                    cmd.ExecuteNonQuery();
                    // Execute the query to the database
                    SqlDataReader reader = cmd.ExecuteReader();

                    }
                }
            }
            catch (SqlException ex)
            {
                // A SQL Exception Occurred. Log and throw to our application!!
                throw;
            }
        }

        /// <summary>
        /// add new reservation
        /// </summary>
        /// <returns>boolean; true if add succeeded,  false if add failed</returns>
        public bool AddReservation(Reservation reserve)
        {
            bool result = true;
            try
            {
                //Create a SqlConnection to our database
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(SqlInsertReservation, connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                // A SQL Exception Occurred. Log and throw to our application!!
                throw;
            }
            return result;
        }
        #endregion
    }
}
