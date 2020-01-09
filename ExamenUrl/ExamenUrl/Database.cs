using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace ExamenUrl
{
    public class Database
    {
        static string connString { get; set; }
        = "Host=localhost;Username=postgres;Password=8841ytrewq;Database=sites";

        public static void Save(List<string> data)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    foreach (var link in data)
                    {
                        cmd.CommandText = $"INSERT INTO sitesurl VALUES ('{link}', 'body');";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static bool Exists(string link)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT link FROM links", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        if (link == (string)reader.GetValue(0))
                            return true;
            }
            return false;
        }
    }
}
