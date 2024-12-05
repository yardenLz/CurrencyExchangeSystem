using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CurrencyExchangeSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var urls = new List<string>
            {
                "https://finance.yahoo.com/currencies",  // Primary URL
                "https://www.bloomberg.com/markets/currencies" // Backup URL
            };

            const string connectionString = "Your SQL Server Connection String Here" /*"Data Source=DESKTOP-13VE4SB\\SQLEXPRESS;Initial Catalog=CurrencyDB;Integrated Security=True"*/;
            var scraper = new CurrencyScraper(urls);
            var databaseService = new DatabaseService(connectionString);

            var producerConsumer = new ProducerConsumer(scraper, databaseService);
            producerConsumer.Start();

            Console.WriteLine("System started. Press Enter to exit...");
            Console.ReadLine();
        }
    }
}
