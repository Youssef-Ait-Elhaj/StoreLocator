using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace store_locator.Models
{
    public class StoreContext
    {
        public string ConnectionString { get; set; }

        public StoreContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<Store> getStores(string address)
        {
            List<Store> stores = new List<Store>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Stores", connection);
                // cmd.Parameters.Add(new MySqlParameter("@latitude", ))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stores.Add(new Store()
                        {
                            id = Convert.ToInt32(reader["id"]),
                            address = reader["features__properties__Location__Address"].ToString(),
                            latitude = Convert.ToDouble(reader["features__properties__Location__Geo Coordinates__Latitude"]),
                            longtitude = Convert.ToDouble(reader["features__properties__Location__Geo Coordinates__Longitude"])
                        });
                    }
                }
            }

            return stores;
        }
    }
}