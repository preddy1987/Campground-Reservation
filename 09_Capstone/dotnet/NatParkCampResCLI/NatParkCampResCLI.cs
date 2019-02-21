using System;
using System.Collections.Generic;
using NatParkCampRes.DAL;
using NatParkCampRes;
using NatParkCampRes.Models;
using System.Collections.Generic;
using System.Text;

namespace NatParkCampResCLI
{
    public class NatParkCampResCLI
    {
        private string connectionString { get; set; }
        

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
                IList<Park> parkList = parkSqlDal.GetAllParks();

                foreach(var park in parkList)
                {
                    Console.WriteLine($"{park.Id}) {park.Name}");
                }

                Console.WriteLine("4) Quit");

                Console.WriteLine();

                int selection = CLIHelper.GetSingleInteger("Select an option...", 1, 5);

                if (selection == 1)
                {
                    //PlayerMenu();
                }
                else if (selection == 2)
                {
                   // DisplayLeaderBoard();
                }
                else if (selection == 3)
                {
                   // PlayGame();
                }
                else if (selection == 4)
                {
                    quit = true;
                }
            }
        }



        private void PrintHeader()
        {
            Console.Clear();

            Console.WriteLine("****************************************************************");
            Console.WriteLine("*               National Park Camp Reservation                 *");
            Console.WriteLine("****************************************************************");

            Console.WriteLine();
        }
    }
}
