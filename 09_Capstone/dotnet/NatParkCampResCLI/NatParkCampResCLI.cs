using System;
using NatParkCampRes.DAL;
using NatParkCampRes;
using NatParkCampRes.Models;
using System.Collections.Generic;
using System.Text;

namespace NatParkCampResCLI
{
    public class NatParkCampResCLI
    {

        public void MainMenu()
        {
            
            bool quit = false;
            
            while (!quit)
            {
                PrintHeader();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("View Park\n Select a Park for Further Details");
                Console.WriteLine($"1) Park 1");
                Console.WriteLine($"2) Park 2");
                Console.WriteLine($"3) Park 3");
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
