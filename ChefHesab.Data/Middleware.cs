using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories;
using ChefHesab.Domain.Peresentition.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Data;

public static class Middleware
{
    public static IServiceCollection AddData(this IServiceCollection services)
    {
        var configurations = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        services.AddScoped<DbContext, ChefHesabContext>();
        services.AddDbContextFactory<ChefHesabContext>(option =>
        {
            option.UseSqlServer(configurations.GetConnectionString("ChefHesabContext"));

        });


        ServiceTool.Create(services);

        return services;
    }


   
}
public class ServiceTool
{
    private static IServiceProvider? ServiceProvider { get; set; }
    public static IServiceCollection Create(IServiceCollection service)
    {
        ServiceProvider = service.BuildServiceProvider();
        return service;
    }
    public static T Resolve<T>()
    {
        return ServiceProvider!.GetService<T>()!;
    }
}