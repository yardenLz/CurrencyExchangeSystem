using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CurrencyExchangeSystem
{
    public class DatabaseService
    {
        public readonly string ConnectionString;

        public DatabaseService(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void SaveCurrencyPair(CurrencyPair pair)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    var command = new SqlCommand(
                        "INSERT INTO CurrencyPairs (PairName, ExchangeRate, UpdateTime) VALUES (@PairName, @ExchangeRate, @UpdateTime)",
                        connection);

                    command.Parameters.AddWithValue("@PairName", pair.PairName);
                    command.Parameters.AddWithValue("@ExchangeRate", pair.ExchangeRate);
                    command.Parameters.AddWithValue("@UpdateTime", pair.UpdateTime);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving currency pair {pair.PairName}: {ex.Message}");
            }
        }
    }
}
