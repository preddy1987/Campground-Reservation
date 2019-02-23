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
        private const string SqlSelectAllReservations = "SELECT * FROM reservation;";
        private const string SqlInsertReservation = "INSERT INTO reservation (site_id,name,from_date,to_date,create_date)" +
                                                    " VALUES (@SiteId,@Name,@FromDate,@ToDate,@CreateDate);";
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
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Reservation> GetAllReservations()
        {
            List<Reservation> output = new List<Reservation>();

            //Always wrap connection to a database in a try-catch block
            try
            {
                //Create a SqlConnection to our database
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(SqlSelectAllReservations, connection);

                    // Execute the query to the database
                    SqlDataReader reader = cmd.ExecuteReader();

                    // The results come back as a SqlDataReader. Loop through each of the rows
                    // and add to the output list
                    while (reader.Read())
                    {
                        // Read in the value from the reader
                        // Reference by index or by column_name
                        Reservation reservation = new Reservation();
                        reservation.ReservationId = Convert.ToInt32(reader["reservation_id"]);
                        reservation.SiteId = Convert.ToInt32(reader["site_id"]);
                        reservation.Name = Convert.ToString(reader["name"]);
                        reservation.FromDate = Convert.ToDateTime(reader["from_date"]);
                        reservation.ToDate = Convert.ToDateTime(reader["to_date"]);
                        reservation.CreateDate = Convert.ToDateTime(reader["create_date"]);

                        // Add the department to the output list                       
                        output.Add(reservation);
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
        /// add new reservation
        /// </summary>
        /// <returns>boolean; true if add succeeded,  false if add failed</returns>
        public Reservation AddReservation(Reservation reserve)
        {
            bool result = false;
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
                    cmd.ExecuteNonQuery();
                    //int rows = cmd.ExecuteNonQuery();
                    //if ( rows != 0)
                    //{
                    //    result = true;
                    //}
                    //else
                    //{
                    //    result = false;
                    //}
                    cmd = new SqlCommand(SqlGetLastReservationId, connection);
                    reserve.ReservationId = Convert.ToInt32(cmd.ExecuteScalar());
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
