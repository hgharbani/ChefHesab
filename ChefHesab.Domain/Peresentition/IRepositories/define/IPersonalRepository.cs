

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChefHesab.Domain.Peresentition.IRepositories.IGenericRepository;

namespace ChefHesab.Domain.Peresentition.IRepositories
{
    public interface IPersonalRepository : IGenericRepository<Personal>
    {
        Task<List<Personal>> GetAllAsync();
    }
}
