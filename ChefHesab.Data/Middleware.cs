using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories;
using ChefHesab.Domain.Peresentition.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Data
{
    public static class Middleware
    {
        public static IServiceCollection AddData(this IServiceCollection services)
        {
            var configurations = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            services.AddDbContext<ChefHesabContext>(config =>
            {
                config.UseSqlServer(configurations.GetConnectionString("ChefHesabContext"), providerOptions => providerOptions.EnableRetryOnFailure());
            });
         
            return services;
        }
    }
}
