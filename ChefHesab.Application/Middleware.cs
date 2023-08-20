using AutoMapper;
using ChefHesab.Application.services;
using ChefHesab.Data.Presentition.Reositories;
using ChefHesab.Domain.Peresentition.IRepositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Application
{
    public static class Middleware
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(op => op.AddMaps(typeof(Middleware)));

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddAutoMapper(typeof(Middleware).Assembly);
         
            services.AddSingleton(mapper);
            services.AddScoped<IChefHesabUnitOfWork, ChefHesabUnitOfWork>();
            return services;
        }

    }
}
