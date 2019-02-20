using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using MessengerService;
using MessengerService.Database;
//using EncryptionMessenger.File;


namespace EncryptionMessengerCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");
            var db = new MessengerDBService(connectionString);
            //var log = new LogFileService();
            EncryptionMessenger em = new EncryptionMessenger(db);
            EncryptionMessengerCLI cli = new EncryptionMessengerCLI(em);
            cli.Run();
        }
    }
}
