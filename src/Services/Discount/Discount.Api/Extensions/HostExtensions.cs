using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;

namespace Discount.Api.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();

                try
                {
                    logger.LogInformation("Database starting...");

                    using var connection = new NpgsqlConnection
                        (configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();

                    using var command = new NpgsqlCommand
                    {
                        Connection = connection
                    };

                    command.CommandText = "DROP TABLE IF EXISTS coupon";
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY,
                                            ProductName VARCHAR(24) NOT NULL,
                                            Description TEXT,
                                            Amount INT
                                            )";

                    command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone X','IPhone Discount', 150);";
                    command.ExecuteNonQuery();

                    logger.LogInformation("migrated to database");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "an error occureed while migrating");

                    if (retryForAvailability > 50)
                    {
                        retryForAvailability++;
                        System.Threading.Thread.Sleep(2000);
                        host.MigrateDatabase<TContext>(retryForAvailability);
                    }
                    throw;
                }

                return host;
            }
        }
    }
}
