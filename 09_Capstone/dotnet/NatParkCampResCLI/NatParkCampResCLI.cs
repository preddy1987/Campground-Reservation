using System;
using System.Collections.Generic;
using NatParkCampRes.DAL;
using NatParkCampRes;
using NatParkCampRes.Models;
using System.Text;

namespace NatParkCampResCLI
{
    public class NatParkCampResCLI
    {
        private string connectionString { get; set; }
        List<string> monthNames = new List<string>()
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



        public NatParkCampResCLI(string connectionStringDb)
        {
            connectionString = connectionStringDb;
        }

        public void MainMenu()
        {
            ParksSqlDAL parkSqlDal = new ParksSqlDAL(connectionString);
            bool quit = false;

            while (!quit)
            {
                PrintHeader();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("View Park\n Select a Park for Further Details");
                List<Park> parkList = parkSqlDal.GetAllParks();
                //parkList.Sort();

                for (int i = 1; i <= parkList.Count; i++)
                {
                    Console.WriteLine($"{i}) {parkList[i - 1].Name}");
                }

                Console.WriteLine("Q) Quit");

                Console.WriteLine();

                int selection = CLIHelper.GetSingleIntegerOrQ("Select an option...", 1, parkList.Count);
                if (selection == -1)
                {
                    quit = true;
                }
                else
                {
                    DisplayParkInfo(parkList[selection - 1]);
                }
            }
        }

        private void DisplayParkInfo(Park park)
        {
            PrintHeader();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Park Information");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{park.Name}");
            Console.WriteLine($"Location:         {park.Location}");
            Console.WriteLine($"Established:      {park.EstablishDate}");
            Console.WriteLine($"Area:             {park.Area} sq km");
            Console.WriteLine($"Annual Visitors:  {park.Visitors}");
            Console.WriteLine();
            Console.WriteLine($"{park.Desc}");
            Console.WriteLine();
            Console.WriteLine("Select a Command");
            Console.WriteLine("1) View Campgrounds");
            Console.WriteLine("2) Search for Reservation");
            Console.WriteLine("3) Return to Previous Screen");

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

            }
        }
        private List<Campground> DisplayCampgroundInfo(Park park)
        {
            CampgroundSqlDAL campgroundSqlDAL = new CampgroundSqlDAL(connectionString);

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
                Console.WriteLine($"#{i.ToString().Trim().PadRight(4)}{campList[i - 1].Name.PadRight(35)}{monthNames[campList[i - 1].OpenFromMonth - 1].PadRight(10)}{monthNames[campList[i - 1].OpenToMonth - 1].PadRight(10)} {campList[i - 1].DailyFee.ToString("C2").PadLeft(9)}");
            }
            return campList;

        }
        private void DisplayCampgroundMenu(Park park)
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

            }
        }
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
                    DateTime departDate = CLIHelper.GetDateTime("What is the departure date?");
                    Console.WriteLine();
                    SiteSelectionMenu(campList[selection - 1], arrivalDate, departDate);
                }
            }
        }
        private void SiteSelectionMenu(Campground campground,DateTime arrival,DateTime departure)
        {
            SiteSqlDAL siteSqlDAL = new SiteSqlDAL(connectionString);
            List<Site> siteList = siteSqlDAL.GetAllCampgroundSites(campground.CampgroundId);
            Console.WriteLine("Results Matching Your Search Criteria");
            Console.WriteLine("Site No." + "Max Occup." + "Accessible?" + "Max RV Length" + "Utility" + "Cost");

            TimeSpan interval = departure - arrival;
            decimal cost = interval.Days * campground.DailyFee;

            for (int i = 0; i < siteList.Count; i++)
            {
                string accessbility = "";
                string utilites = "";
                if (siteList[i].HasUtilities)
                {
                    utilites = "Yes";
                }
                else
                {
                    utilites = "N/A";
                }
                if (siteList[i].IsAccessible)
                {
                    accessbility = "Yes";
                }
                else
                {
                    accessbility = "No";
                }
                    Console.WriteLine($"{siteList[i].SiteNumber}{siteList[i].MaxOccupants}{accessbility}{siteList[i].MaxRvLength}{utilites}{cost.ToString("C2")}");
            }
            Console.ReadKey();
        }
        private void PrintHeader()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("****************************************************************");
            Console.WriteLine("*               National Park Camp Reservation                 *");
            Console.WriteLine("****************************************************************");
            Console.WriteLine();
        }
    }
}
