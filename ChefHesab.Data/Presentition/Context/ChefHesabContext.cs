using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Dalir.common.Context;
using ChefHesab.Domain;
using ChefHesab.Data.Configurations;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;

namespace ChefHesab.Data.Presentition.Context
{
  
        public class ChefHesabContext : DbContext
    {
       
            public ChefHesabContext(DbContextOptions<ChefHesabContext> options) : base(options)
            {
                ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            }

        public ChefHesabContext()
        {
                
        }

        public virtual DbSet<AdditionalCost> AdditionalCosts { get; set; }
        public virtual DbSet<AdditionalCostFood> AdditionalCostFoods { get; set; }
        public virtual DbSet<Authenticate> Authenticates { get; set; }
        public virtual DbSet<ContractingCompany> ContractingCompanies { get; set; }
        public virtual DbSet<FoodCategory> FoodCategories { get; set; }
        public virtual DbSet<FoodProvider> FoodProviders { get; set; }
        public virtual DbSet<FoodStuff> FoodStuffs { get; set; }
        public virtual DbSet<IngredinsFood> IngredinsFoods { get; set; }
        public virtual DbSet<Personal> Personals { get; set; }
        public virtual DbSet<StuffPrice> StuffPrices { get; set; }
       

        //public virtual DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AdditionalCostConfiguration());
            modelBuilder.ApplyConfiguration(new AdditionalCostFoodConfiguration());
            modelBuilder.ApplyConfiguration(new AuthenticateConfiguration());
            modelBuilder.ApplyConfiguration(new ContractingCompanyConfiguration());
            modelBuilder.ApplyConfiguration(new FoodCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new FoodProviderConfiguration());
            modelBuilder.ApplyConfiguration(new FoodStuffConfiguration());
            modelBuilder.ApplyConfiguration(new IngredinsFoodConfiguration());
            modelBuilder.ApplyConfiguration(new PersonalConfiguration());
            modelBuilder.ApplyConfiguration(new StuffPriceConfiguration());

            base.OnModelCreating(modelBuilder);
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        IConfigurationRoot configuration = new ConfigurationBuilder()
        //           .SetBasePath(Directory.GetCurrentDirectory())
        //           .AddJsonFile("appsettings.json")
        //           .Build();
        //        var connectionString = configuration.GetConnectionString("ChefHesabContext");
        //        optionsBuilder.UseSqlServer(connectionString, providerOptions => providerOptions.EnableRetryOnFailure());
        //    }
        //}

    }
}
