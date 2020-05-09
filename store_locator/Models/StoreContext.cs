using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace store_locator.Models
{
    public class StoreContext
    {
        private static readonly HttpClient client = new HttpClient();
        public string ConnectionString { get; set; }

        public StoreContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<Store> getStores(double lat, double lng)
        {
            List<Store> stores = new List<Store>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                // request to return nearby stores
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Stores WHERE ABS(features__geometry__coordinates__002 - @latitude) < 0.006 AND ABS(features__geometry__coordinates__001 - @longtitude) < 0.006", connection);
                cmd.Parameters.Add(new MySqlParameter("@latitude", lat));
                cmd.Parameters.Add(new MySqlParameter("@longtitude", lng));
                // MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
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