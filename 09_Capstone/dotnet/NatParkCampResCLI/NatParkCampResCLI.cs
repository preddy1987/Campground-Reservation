using System;
using NatParkCampRes.DAL;
using NatParkCampRes;
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
                Console.WriteLine("1) Player Management");
                Console.WriteLine("2) Leader Board");
                Console.WriteLine("3) Start Game");
                Console.WriteLine("4) Change Font");
                Console.WriteLine("5) Quit");
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
                   // FontMenu();
                }
                else if (selection == 5)
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
