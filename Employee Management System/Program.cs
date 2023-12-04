using Employee_Management_System.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
string connectionSting = configuration.GetConnectionString("DefaultConnection");
var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddLogging(se => se.AddLog4Net())
                .AddSingleton(new AppDbContext(connectionSting));
            })
            .Build();