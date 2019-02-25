using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace NatParkCampResCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the connection string from the appsettings.json file
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            string connectionString = configuration.GetConnectionString("Project");
            try
            {
                NatParkCampResCLI cli = new NatParkCampResCLI(connectionString);
                cli.MainMenu();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.Write("\nPress any key to exit.");
                Console.ReadKey();
            }
        }
    }
}
