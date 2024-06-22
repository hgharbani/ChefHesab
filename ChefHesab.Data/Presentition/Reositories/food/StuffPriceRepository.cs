using ChefHesab.Data;
using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories.generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Domain.Peresentition.IRepositories.food
{
    public class StuffPriceRepository : GenericRepository<StuffPrice>, IStuffPriceRepository
    { 
        public StuffPriceRepository(ChefHesabContext chefHesab) : base(chefHesab)
        { 
        }

    }
}