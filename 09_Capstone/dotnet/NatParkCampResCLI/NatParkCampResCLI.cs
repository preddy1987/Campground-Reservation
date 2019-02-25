using System;
using System.Collections.Generic;
using NatParkCampRes.DAL;
using NatParkCampRes;
using NatParkCampRes.Models;
using System.Text;
using System.Globalization;

namespace NatParkCampResCLI
{
    public class NatParkCampResCLI
    {
        #region Member Variables
        private string _connectionString { get; set; }
        List<string> _monthNames = new List<string>()
        {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December",
        };
        #endregion

        #region Constructor
        public NatParkCampResCLI(string connectionStringDb)
        {
            _connectionString = connectionStringDb;
        }
        #endregion

        #region public Methods
        /// <summary>
        /// Main Menu 
        /// </summary>
        public void MainMenu()
        {
            ParksSqlDAL parkSqlDal = new ParksSqlDAL(_connectionString);
            bool quit = false;

            while (!quit)
            {
                List<Park> parkList = parkSqlDal.GetAllParks();      // get a list of all parks
                DisplayParkList(parkList);                          // Write list of parks to console

                int selection = CLIHelper.GetSingleIntegerOrQ("Select an option...", 1, parkList.Count);
                if (selection == -1)
                {
                    quit = true;
                }
                else
                {
                    ParkInfoMenu(parkList[selection - 1]);
                }
            }
        }
        #endregion

        #region private Methods
        /// <summary>
        /// Park selection menu
        /// </summary>
        /// <param name="park">Park object</param>
        private void ParkInfoMenu(Park park)
        {
            bool quit = false;
            while (!quit)
            {
                DisplayParkInfo(park);
                int selection = CLIHelper.GetSingleInteger("Select an option...", 1, 3);

                if (selection == 1)
                {
                    DisplayCampgroundMenu(park);
                }
                else if (selection == 2)
                {
                    DisplayReservationMenu(park);
                }
                else if (selection == 3)
                {
                    quit = true;
                }
            }
        }

        /// <summary>
        /// Campground selection menu
        /// </summary>
        /// <param name="park">Park object</param>
        private void DisplayCampgroundMenu(Park park)
        {
            bool quit = false;
            while (!quit)
            {
                List<Campground> campList = DisplayCampgroundInfo(park);

                Console.WriteLine();
                Console.WriteLine("Select a Command");
                Console.WriteLine("1) Search for Available Reservation");
                Console.WriteLine("2) Return to Previous Screen");

                int selection = CLIHelper.GetSingleInteger("Select an option...", 1, 2);
                if (selection == 1)
                {
                    DisplayReservationMenu(park);
                }
                else if (selection == 2)
                {
                    quit = true;
                }
            }
        }

        /// <summary>
        /// Display sites in campground in park, accept reservation start and end dates,
        /// validate dates.
        /// </summary>
        /// <param name="park">Park object</param>
        private void DisplayReservationMenu(Park park)
        {
            bool quit = false;
            while (!quit)
            {
                PrintHeader();
                List<Campground> campList = DisplayCampgroundInfo(park);
                Console.WriteLine();
                int selection = CLIHelper.GetSingleInteger("Which campground (enter 0 to cancel)?", 0, campList.Count);
                if (selection == 0)
                {
                    quit = true;
                }
                else
                {
                    DateTime arrivalDate = CLIHelper.GetDateTime("\nWhat is the arrival date?");
                    if ( ValidArrivalDate(arrivalDate) )
                    {
                        DateTime departDate = CLIHelper.GetDateTime("What is the departure date?");
                        if ( ValidDepartureDate(departDate) )
                        {
                            if ( ValidReservationDates(arrivalDate, departDate) )
                            {
                                SiteSelectionMenu(campList[selection - 1], arrivalDate, departDate);
                                quit = true;
                            }
                            else {Console.WriteLine("\n Departure date must be greater that arrival date.");}                            
                        }
                        else {Console.WriteLine("\n Invalid departure Date. Must be in the future.");}
                    }
                    else { Console.WriteLine("\n Invalid arrival date.  Must be today or in the future.");}
                }
                Console.Write("\n Press any key to continue.");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Select from available sites in selected campground
        /// </summary>
        /// <param name="campground"></param>
        /// <param name="arrival"></param>
        /// <param name="departure"></param>
        private void SiteSelectionMenu(Campground campground, DateTime arrival, DateTime departure)
        {
            Reservation reserve = new Reservation();
            reserve.FromDate = arrival;
            reserve.ToDate = departure;
            bool quit = false;
            while (!quit)
            {
                SiteSqlDAL siteSqlDAL = new SiteSqlDAL(_connectionString);
                List<Site> resList = siteSqlDAL.GetAvailalableSitesInCampground(campground.CampgroundId, arrival, departure);
                TimeSpan interval = reserve.ToDate - reserve.FromDate;
                decimal cost = interval.Days * campground.DailyFee;

                if (resList.Count == 0)
                {
                    Console.WriteLine("\nNo sites available for the dates provided.");
                }
                else
                {
                    DisplayAvailableSites(resList, cost);

                    reserve.SiteId = 0;
                    int selection = CLIHelper.GetInteger("Which site should be reserved (enter 0 to cancel)?");
                    if (selection < 0)
                    {
                        Console.WriteLine(" Invalid selection. Please try again.");
                    }
                    else if (selection == 0)
                    {
                        quit = true;
                    }
                    else
                    {
                        for (int i = 0; i < resList.Count; i++)
                        {
                            if (resList[i].SiteNumber == selection)
                            {
                                reserve.SiteId = resList[i].SiteId;
                            }
                        }
                        if (reserve.SiteId == 0)
                        {
                            Console.WriteLine(" Invalid selection. Please try again.");
                        }
                    }
                    Console.WriteLine("What name Should the reservation be made under?");
                    reserve.Name = Console.ReadLine();
                    ReservationSqlDAL reservationSqlDAL = new ReservationSqlDAL(_connectionString);
                    reserve = reservationSqlDAL.AddReservation(reserve);
                    Console.WriteLine($"The reservation has been made and the confirmation id is {reserve.ReservationId}");

                    quit = true;
                }
            }
        }

        /// <summary>
        /// Print Screen header helper 
        /// </summary>
        private void PrintHeader()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("****************************************************************");
            Console.WriteLine("*               National Park Camp Reservation                 *");
            Console.WriteLine("****************************************************************");
            Console.WriteLine();
        }

        /// <summary>
        /// Display the list of parks to the Console
        /// </summary>
        /// <param name="parklist"></param>
        private void DisplayParkList(List<Park> parklist)
        {
            PrintHeader();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("View Park\n Select a Park for Further Details");
            for (int i = 1; i <= parklist.Count; i++)
            {
                Console.WriteLine($"{i}) {parklist[i - 1].Name}");
            }
     
            Console.WriteLine("Q) Quit\n");
        }

        /// <summary>
        /// Display the Park information block
        /// </summary>
        /// <param name="park"></param>
        private void DisplayParkInfo(Park park)
        {
            PrintHeader();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Park Information");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{park.Name}");
            Console.WriteLine($"Location:         {park.Location}");
            Console.WriteLine($"Established:      {park.EstablishDate.ToString("d", CultureInfo.CreateSpecificCulture("en-US"))}");
            Console.WriteLine($"Area:             {park.Area} sq km");
            Console.WriteLine($"Annual Visitors:  {park.Visitors}");
            Console.WriteLine();
            Console.WriteLine($"{park.Desc}");
            Console.WriteLine();
            Console.WriteLine("Select a Command");
            Console.WriteLine("1) View Campgrounds");
            Console.WriteLine("2) Search for Reservation");
            Console.WriteLine("3) Return to Previous Screen");
        }

        /// <summary>
        /// Display Campground list for selected Park
        /// </summary>
        /// <param name="park">Park object</param>
        /// <returns>List of campground objects</returns>
        private List<Campground> DisplayCampgroundInfo(Park park)
        {
            CampgroundSqlDAL campgroundSqlDAL = new CampgroundSqlDAL(_connectionString);

            PrintHeader();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Park Campgrounds");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{park.Name} Campgrounds");
            Console.WriteLine();
            Console.WriteLine("     Name".PadRight(40) + "Open".PadRight(10) + "Close".PadRight(10) + " Daily Fee");
            List<Campground> campList = campgroundSqlDAL.GetCampgroundsInPark(park);

            for (int i = 1; i <= campList.Count; i++)
            {
                Console.WriteLine($"#{i.ToString().Trim().PadRight(4)}{campList[i - 1].Name.PadRight(35)}{_monthNames[campList[i - 1].OpenFromMonth - 1].PadRight(10)}{_monthNames[campList[i - 1].OpenToMonth - 1].PadRight(10)} {campList[i - 1].DailyFee.ToString("C2").PadLeft(9)}");
            }
            return campList;
        }

        /// <summary>
        /// Display list of available sites 
        /// </summary>
        /// <param name="sitelist">List of Site objects</param>
        /// <param name="cost">Total cost of site for requested dates</param>
        private void DisplayAvailableSites(List<Site> sitelist, decimal cost)
        {
            Console.WriteLine("\nResults Matching Your Search Criteria");
            Console.WriteLine("Site No.".PadRight(12) +
                                "Max Occup.".PadRight(12) +
                                "Accessible?".PadRight(15) +
                                "Max RV Length".PadRight(15) +
                                "Utility".PadRight(12) +
                                "Cost");

            for (int i = 0; i < sitelist.Count; i++)
            {
                string utilities = sitelist[i].HasUtilities ? "Yes" : "N/A";
                string accessability = sitelist[i].HasUtilities ? "Yes" : "No";
                string rvStatus = sitelist[i].MaxRvLength == 0 ? "N/A" : sitelist[i].MaxRvLength.ToString();
                Console.WriteLine($"{sitelist[i].SiteNumber.ToString().PadRight(12)}" +
                                    $"{sitelist[i].MaxOccupants.ToString().PadRight(12)}" +
                                    $"{accessability}".PadRight(15) +
                                    $"{rvStatus.PadRight(15)}" +
                                    $"{utilities}".PadRight(12) +
                                    $"{cost.ToString("C2")}");
            }
        }

        /// <summary>
        /// Check arrival date is today or in future
        /// </summary>
        /// <param name="arrival">Arrival DateTime</param>
        /// <returns>true if date is today or later, false otherwise</returns>
        private bool ValidArrivalDate(DateTime arrival)
        {
            bool result = false;

            if ( DateTime.Parse(arrival.ToShortDateString()) >= DateTime.Parse(DateTime.Now.ToShortDateString()) )
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Check departure date is in future
        /// </summary>
        /// <param name="departure">Departure DateTime</param>
        /// <returns>true if date is in future, false otherwise</returns>
        private bool ValidDepartureDate(DateTime departure)
        {
            bool result = false;

            if ( DateTime.Parse(departure.ToShortDateString()) > DateTime.Parse(DateTime.Now.ToShortDateString()) )
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Check departure date is greater than arrival date
        /// </summary>
        /// <param name="arrival">Date of arrival</param>
        /// <param name="departure">Date of departure</param>
        /// <returns>true if departure date later than arrival date</returns>
        private bool ValidReservationDates(DateTime arrival, DateTime departure)
        {
            bool result = false;

            if ( DateTime.Parse(departure.ToShortDateString()) > DateTime.Parse(arrival.ToShortDateString()) )
            {
                result = true;
            }

            return result;
        }
        #endregion
    }
}
