﻿using Employee_Management_System.DAL;
using Employee_Management_System.Model;
using Employee_Management_System.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(enviroment == "Development" ? $"appSettings.Development.json" : $"appSettings.json").Build();
string connectionSting = config.GetConnectionString("DefaultConnection");
var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddLogging(se => se.AddLog4Net())
                .AddSingleton(new AppDbContext(connectionSting))
                .AddSingleton<IDUsers, DUsers>()
                .AddSingleton<IDEmployees, DEmployees>()
                .AddSingleton<IDDepartments, DDepartments>()
                .AddSingleton<IDSalary, DSalary>()
                .AddSingleton<IDAttendance, DAttendance>()
                .AddSingleton<IDLeave, DLeave>()
                .AddSingleton<IDCommitDBChanges, DCommitDBChanges>()
                .AddSingleton<IAdminServices, AdminServices>()
                .AddSingleton<IManagerServices, ManagerServices>()
                .AddSingleton<IEmployeeServices, EmployeeServices>()
                .AddSingleton<IEmployeeManagmentServices, EmployeeManagmentServices>();
            })
            .Build();

host.Services.GetRequiredService<IEmployeeManagmentServices>().RenderApp();