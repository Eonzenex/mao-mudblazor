using System;
using System.IO;
using mao.backend;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace mao.frontend;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            CreateHostBuilder(args).Build().Run();
        }
        catch (IOException exception)
        {
            Utils.Log(exception.Message, LogLevel.Error);
            Utils.Log("If the above was a port issue, close the other program using the port or change the port in `appsettings.json`.", LogLevel.Warn);

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
