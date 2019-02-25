using System;
using System.Collections.Generic;
using System.Text;
using NatParkCampRes.Models;
using System.Data.SqlClient;
using System.Globalization;

namespace NatParkCampRes.DAL
{
    public class ReservationSqlDAL
    {
        #region Constants
        private const string SqlInsertReservation = "INSERT INTO reservation (site_id,name,from_date,to_date,create_date)" +
                                                    " VALUES (@SiteId,@Name,@FromDate,@ToDate,@CreateDate); " +
                                                    "SELECT CAST(SCOPE_IDENTITY() as int);";
        private const string SqlGetLastReservationId = "SELECT MAX(reservation_id) FROM reservation;";
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
        public ReservationSqlDAL(string connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion

        #region Methods
        /// <summary>
        /// add new reservation
        /// </summary>
        /// <returns>boolean; true if add succeeded,  false if add failed</returns>
        public Reservation AddReservation(Reservation reserve)
        {
            try
            {
                //Create a SqlConnection to our database
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(SqlInsertReservation, connection);
                    cmd.Parameters.AddWithValue("@SiteId", reserve.SiteId);
                    cmd.Parameters.AddWithValue("@Name", reserve.Name);
                    cmd.Parameters.AddWithValue("@FromDate", reserve.FromDate.ToString("d", CultureInfo.CreateSpecificCulture("en-US")));
                    cmd.Parameters.AddWithValue("@ToDate", reserve.ToDate.ToString("d", CultureInfo.CreateSpecificCulture("en-US")));
                    cmd.Parameters.AddWithValue("@CreateDate", reserve.CreateDate);
                    reserve.ReservationId = (int)cmd.ExecuteScalar();
                }
                
            }
            catch (SqlException ex)
            {
                // A SQL Exception Occurred. Log and throw to our application!!
                throw;
            }
            return reserve;
        }
        #endregion
    }
}
