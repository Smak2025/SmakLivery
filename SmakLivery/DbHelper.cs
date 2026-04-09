using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SmakLivery
{
    public static class DbHelper
    {
        private static string connStr = "Host=localhost;Username=postgres;Password=postgres;Database=smaklivery";
        public static async Task CreateDbAsync()
        {
            var query = "" +
                "CREATE TYPE order_status AS ENUM(" +
                "'Принят', " +
                "'Готовится', " +
                "'Передан курьеру', " +
                "'Получен', " +
                "'Отменен' " +
                ");\n" +
                "CREATE TABLE IF NOT EXISTS \"order\"(" +
                "id BIGINT GENERATED ALWAYS AS IDENTITY, " +
                "address VARCHAR(255) NOT NULL, " +
                "status order_status NOT NULL DEFAULT 'Принят', " +
                "courier_id BIGINT, " +
                "PRIMARY KEY (id)" +
                ")";
            await using var conn = new NpgsqlConnection(connStr);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(query, conn);
            await cmd.ExecuteNonQueryAsync();
        }

        private static OrderStatus GetOrderStatusFrom(string statusName)
        {
            return statusName switch
            {
                "Отменен" => OrderStatus.Cancelled,
                "Готовится" => OrderStatus.Prepearing,
                "Передан курьеру" => OrderStatus.OnTheWay,
                "Получен" => OrderStatus.Delivered,
                _ => OrderStatus.Accepted
            };
        }

        private static string GetStatusNameFrom(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Cancelled => "Отменен",
                OrderStatus.Prepearing => "Готовится",
                OrderStatus.OnTheWay => "Передан курьеру",
                OrderStatus.Delivered => "Получен",
                _ => "Принят"
            };
        }

        public static async Task<List<Order>> GetOrders()
        {
            var orders = new List<Order>();
            var query = "SELECT * FROM \"order\";";
            await using var conn = new NpgsqlConnection(connStr);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(query, conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                orders.Add(new Order()
                {
                    Id = reader.GetInt64(0),
                    Address = reader.GetString(1),
                    Status = GetOrderStatusFrom(reader.GetString(2)),
                    CourierId = await reader.IsDBNullAsync(3) ? null : reader.GetInt64(3)
                });
            }
            return orders;
        }
    }
}
