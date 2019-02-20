using System;
using System.Collections.Generic;
using System.Text;
using MessengerService.Models;
using MessengerService;

namespace EncryptionMessengerCLI
{
    class EncryptionMessengerCLI
    {
        private const string Option_LoginVendingMachine = "1";
        private const string Option_RegisterVendingMachine = "2";
        private const string Option_LogoutVendingMachine = "3";
        private const string Option_DisplayVendingMachine = "1";
        private const string Option_DisplayPurchaseMenu = "2";
        private const string Option_InsertMoney = "1";
        private const string Option_MakeSelection = "2";
        private const string Option_ReturnChange = "3";
        private const string Option_ReturnToPreviousMenu = "r";
        private const string Option_Quit = "q";

        private EncryptionMessenger _em;

        public EncryptionMessengerCLI(EncryptionMessenger em)
        {
            _em = em;
        }

        public void Run()
        {
            MainMenu();
        }

        private void MainMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                PrintTitle();

                Console.WriteLine(" (1) Login");
                Console.WriteLine(" (2) Register");
                Console.WriteLine(" (Q) Quit");
                Console.Write(" Please make a choice: ");

                string choice = Console.ReadLine().ToLower();

                Console.WriteLine();

                if (choice == Option_LoginVendingMachine)
                {
                    DisplayLogin();
                }
                else if (choice == Option_RegisterVendingMachine)
                {
                    DisplayRegister();
                }
                else if (choice == Option_Quit)
                {
                    exit = true;
                }
                else
                {
                    DisplayInvalidOption();
                    Console.ReadKey();
                }
            }
        }

        private void DisplayLogin()
        {
            Console.Clear();
            Console.WriteLine("Enter username: ");
            string username = Console.ReadLine();
            Console.WriteLine("Enter password: ");
            string password = Console.ReadLine();
            try
            {
                _em.LoginUser(username, password);
                Console.WriteLine($"Welcome {_em.CurrentUser.Username}");
                Console.ReadKey();
                VendingMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

        private void DisplayRegister()
        {
            Console.Clear();

            User user = new User();
            Console.WriteLine("Enter username: ");
            user.Username = Console.ReadLine();
            Console.WriteLine("Enter password: ");
            user.Password = Console.ReadLine();
            Console.WriteLine("Enter password again: ");
            user.ConfirmPassword = Console.ReadLine();

            try
            {
                _em.RegisterUser(user);
                Console.WriteLine($"Welcome {_em.CurrentUser.Username}");
                Console.ReadKey();
                VendingMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

        private void VendingMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();

                PrintTitle();

                Console.WriteLine(" (1) Display vending machine items");
                Console.WriteLine(" (2) Purchase");
                Console.WriteLine(" (3) Logout");
                Console.Write(" Please make a choice: ");

                string choice = Console.ReadLine().ToLower();

                Console.WriteLine();

                if (choice == Option_DisplayVendingMachine)
                {
                    //DisplayInventory();
                    Console.ReadKey();
                }
                else if (choice == Option_DisplayPurchaseMenu)
                {
                    //DisplayPurchaseMenu();
                }
                else if (choice == Option_LogoutVendingMachine)
                {
                    _em.LogoutUser();
                    exit = true;
                }
                else
                {
                    DisplayInvalidOption();
                    Console.ReadKey();
                }
            }
        }
        private void PrintTitle()
        {
            Console.Clear();

            Console.WriteLine("****************************************************************");
            Console.WriteLine("*                     Encryption Messenger                     *");
            Console.WriteLine("****************************************************************");

            Console.WriteLine();
        }
        private void DisplayInvalidOption()
        {
            Console.WriteLine(" The option you selected is not a valid option.");
            Console.WriteLine();
        }
    }
}
