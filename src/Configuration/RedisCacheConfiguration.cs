using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;

namespace ScalingSignalR.Configuration
{
    public static class RedisCacheConfiguration
    {
        public static ISignalRServerBuilder ConfigureRedis(this ISignalRServerBuilder builder)
        {
            return builder.AddStackExchangeRedis(o =>
            {
                o.ConnectionFactory = async writer =>
                {
                    var config = new ConfigurationOptions
                    {
                        AbortOnConnectFail = false
                    };

                    // This is the name of the redis container. See container_name on docker-compose.yml
                    // Please: Dont hardcode your connection string in your code. Use appsettings.json instead
                    config.EndPoints.Add("rediscache"); 

                    var connection = await ConnectionMultiplexer.ConnectAsync(config, writer);
                    connection.ConnectionFailed += (_, e) =>
                    {
                        Console.WriteLine("Connection to Redis failed.");
                    };

                    connection.ErrorMessage += (sender, args) =>
                    {
                        Console.WriteLine($"Endpoint: {args.EndPoint} Message: {args.Message}");
                    };

                    if (!connection.IsConnected)
                    {
                        Console.WriteLine("Did not connect to Redis.");                        
                    }
                    return connection;
                };

                o.Configuration.ClientName = "SignalRWithRedis";
            });
        }
    }
}
