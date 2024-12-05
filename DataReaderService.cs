using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CurrencyExchangeSystem
{
    public class DataReaderService
    {
        private readonly string _connectionString;

        public DataReaderService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<CurrencyPair> GetAllCurrencyPairs()
        {
            var pairs = new List<CurrencyPair>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT PairName, ExchangeRate, UpdateTime FROM CurrencyPairs", connection);
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        pairs.Add(new CurrencyPair
                        {
                            PairName = reader.GetString(0),
                            ExchangeRate = reader.GetDecimal(1),
                            UpdateTime = reader.GetDateTime(2)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading data: {ex.Message}");
            }

            return pairs;
        }
    }
}
