using System;

namespace EncryptionMessengerCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            //var builder = new ConfigurationBuilder()
            //.SetBasePath(Directory.GetCurrentDirectory())
            // .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            //IConfigurationRoot configuration = builder.Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");
            var db = new MessengerDBService(connectionString);
            //var log = new LogFileService();
            VendingMachine vm = new VendingMachine(db, log);
            VndrCLI cli = new VndrCLI(vm);
            cli.Run();
        }
    }
}
