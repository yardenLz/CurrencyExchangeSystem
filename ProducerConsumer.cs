using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CurrencyExchangeSystem
{
    public class ProducerConsumer
    {
        private readonly CurrencyScraper _scraper;
        private readonly DatabaseService _databaseService;
        private readonly Thread _producerThread;
        private readonly Thread _consumerThread;

        public ProducerConsumer(CurrencyScraper scraper, DatabaseService databaseService)
        {
            _scraper = scraper;
            _databaseService = databaseService;

            _producerThread = new Thread(Produce);
            _consumerThread = new Thread(Consume);
        }

        public void Start()
        {
            _producerThread.Start();
            _consumerThread.Start();
        }

        private void Produce()
        {
            while (true)
            {
                var pairs = new[] { "USD/ILS", "GBP/EUR", "EUR/JPY", "EUR/USD" };

                foreach (var pair in pairs)
                {
                    try
                    {
                        var data = _scraper.GetCurrencyPair(pair);
                        if (data != null)
                        {
                            _databaseService.SaveCurrencyPair(data);
                            Console.WriteLine($"Saved: {data.PairName} - {data.ExchangeRate} at {data.UpdateTime}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in producer for pair {pair}: {ex.Message}");
                    }
                }

                Thread.Sleep(60000); // Wait for one minute
            }
        }

        private void Consume()
        {
            while (true)
            {
                try
                {
                    var readerService = new DataReaderService(_databaseService.ConnectionString);
                    var pairs = readerService.GetAllCurrencyPairs();

                    Console.WriteLine("Current data:");
                    foreach (var pair in pairs)
                    {
                        Console.WriteLine($"{pair.PairName}: {pair.ExchangeRate} (Updated: {pair.UpdateTime})");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in consumer: {ex.Message}");
                }

                Thread.Sleep(60000); // המתנה של דקה
            }
        }
    }
}
